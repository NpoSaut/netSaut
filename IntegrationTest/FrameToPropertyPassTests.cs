using System;
using System.Collections.Generic;
using System.Linq;
using BlokFrames;
using Communications;
using Communications.Can;
using Geographics;
using Microsoft.Practices.Unity;
using Modules;
using NUnit.Framework;
using Rhino.Mocks;
using Saut.Communication.Modules;
using Saut.StateModel.Exceptions;
using Saut.StateModel.Modules;
using Saut.StateModel.StateProperties;

namespace IntegrationTest
{
    [TestFixture]
    public class FrameToPropertyPassTests
    {
        private readonly MyTestBootstrapper _bootstrapper;
        private DateTime _t0;

        private class MyTestBootstrapper : BootstrapperBase
        {
            private readonly Func<ISocketSource<ICanSocket>> _socketFactory;
            public MyTestBootstrapper(Func<ISocketSource<ICanSocket>> SocketFactory) { _socketFactory = SocketFactory; }

            protected override IEnumerable<IModule> EnumerateModules()
            {
                return new IModule[]
                       {
                           new BlokFrameProcessorsModule(),
                           new StateModelModule(),
                           new CommonPropertiesModule(),
                           new MessageProcessingModule(),
                           new MyTestSocketSourceModule(_socketFactory),
                           new DecoderModule()
                       };
            }
        }

        private ICanSocket GetSocketMock(IEnumerable<CanFrame> MockedFrames)
        {
            var socketMock = MockRepository.GenerateMock<ICanSocket>();
            socketMock
                .Expect(s => s.Read())
                .IgnoreArguments()
                .Return(MockedFrames);
            return socketMock;
        }

        public FrameToPropertyPassTests()
        {
            _t0 = DateTime.Today;
            IEnumerable<CanFrame> frames =
                new BlokFrame[]
                {
                    new MmAltLongFrame { Time = _t0.AddSeconds(0.0), Latitude = 60.0, Longitude = 50.0, Reliable = false },
                    new MmAltLongFrame { Time = _t0.AddSeconds(0.5), Latitude = 60.0, Longitude = 50.0, Reliable = true },
                    new MmAltLongFrame { Time = _t0.AddSeconds(1.0), Latitude = 70.0, Longitude = 40.0, Reliable = true },
                    new IpdState { Time = _t0.AddSeconds(0.2), Speed = 30, FrameHalfset = HalfsetKind.SetA },
                    new IpdState { Time = _t0.AddSeconds(0.7), Speed = 35, FrameHalfset = HalfsetKind.SetA },
                    new IpdState { Time = _t0.AddSeconds(1.2), Speed = 45, FrameHalfset = HalfsetKind.SetA }
                }.OrderBy(m => m.Time).Select(bf => bf.GetCanFrame());

            _bootstrapper = new MyTestBootstrapper(
                () =>
                {
                    var socketSourceMock = MockRepository.GenerateMock<ISocketSource<ICanSocket>>();
                    socketSourceMock
                        .Expect(ss => ss.OpenSocket())
                        .Message("Ожидается, что будет однократно запрошено открытие сокета")
                        .Return(GetSocketMock(frames));
                    return socketSourceMock;
                });
            _bootstrapper.Initialize();

            _bootstrapper.Run();
        }

        [Test, Description("Проверяет линейную интерполяцию GPS-позиции")]
        public void PositionProcessingTest()
        {
            var positionProperty = _bootstrapper.Container.Resolve<GpsPositionProperty>();

            AssertPosition(new EarthPoint(60.0, 50.0), positionProperty.GetValue(_t0.AddSeconds(0.0)), "Не верно обработано значение {0} в ключевой точке");
            AssertPosition(new EarthPoint(60.0, 50.0), positionProperty.GetValue(_t0.AddSeconds(0.5)), "Не верно обработано значение {0} в ключевой точке");
            AssertPosition(new EarthPoint(70.0, 40.0), positionProperty.GetValue(_t0.AddSeconds(1.0)), "Не верно обработано значение {0} в ключевой точке");
            AssertPosition(new EarthPoint(60.0, 50.0), positionProperty.GetValue(_t0.AddSeconds(0.3)), "Не верно обработано значение {0} в промежуточной точке");
            AssertPosition(new EarthPoint(64.0, 46.0), positionProperty.GetValue(_t0.AddSeconds(0.7)), "Не верно обработано значение {0} в промежуточной точке");
        }

        private void AssertPosition(EarthPoint ExpectedPosition, EarthPoint ActualPosition, String ErrorMessage)
        {
            Assert.AreEqual(ExpectedPosition.Latitude.Value, ActualPosition.Latitude.Value, 0.0001, string.Format(ErrorMessage, "широты"));
            Assert.AreEqual(ExpectedPosition.Longitude.Value, ActualPosition.Longitude.Value, 0.0001, string.Format(ErrorMessage, "долготы"));
        }

        [Test, Description("Проверяет ступенчатую интерполяцию достоверности GPS")]
        public void ReliabilityProcessingTest()
        {
            var reliabilityProperty = _bootstrapper.Container.Resolve<GpsReliabilityProperty>();
            Assert.AreEqual(false, reliabilityProperty.GetValue(_t0.AddSeconds(0.0)), "Не верно обработано значение в ключевой точке");
            Assert.AreEqual(true, reliabilityProperty.GetValue(_t0.AddSeconds(0.5)), "Не верно обработано значение в ключевой точке");
            Assert.AreEqual(true, reliabilityProperty.GetValue(_t0.AddSeconds(1.0)), "Не верно обработано значение в ключевой точке");
            Assert.AreEqual(false, reliabilityProperty.GetValue(_t0.AddSeconds(0.2)), "Значение не верно интерполировалось");
            Assert.AreEqual(true, reliabilityProperty.GetValue(_t0.AddSeconds(1.2)), "Значение не верно интерполировалось");
        }

        [Test, Description("Проверяет возникновение исключения при попытке получить значение скорости вне диапазона актуальности")]
        [ExpectedException(typeof (PropertyValueUndefinedException))]
        public void SpeedObsoletingTest()
        {
            DateTime t = _t0.AddSeconds(10.0);
            var speedProperty = _bootstrapper.Container.Resolve<SpeedProperty>();
            Assert.AreEqual(false, speedProperty.HaveValue(t));
            speedProperty.GetValue(t);
        }

        [Test, Description("Проверяет линейную интерполяцию скорости")]
        public void SpeedProcessingTest()
        {
            var speedProperty = _bootstrapper.Container.Resolve<SpeedProperty>();
            Assert.AreEqual(30, speedProperty.GetValue(_t0.AddSeconds(0.2)), "Не верно обработано значение в ключевой точке");
            Assert.AreEqual(35, speedProperty.GetValue(_t0.AddSeconds(0.7)), "Не верно обработано значение в ключевой точке");
            Assert.AreEqual(45, speedProperty.GetValue(_t0.AddSeconds(1.2)), "Не верно обработано значение в ключевой точке");
            Assert.AreEqual(28, speedProperty.GetValue(_t0.AddSeconds(0.0)), "Значение не верно экстраполировалось");
            Assert.AreEqual(41, speedProperty.GetValue(_t0.AddSeconds(1.0)), "Значение не верно интерполировалось");
        }

        /*[Test]
        public void PassFrameToPropertyTest()
        {
            var positionProperty = _bootstrapper.Container.Resolve<GpsPositionProperty>();
            var reliabilityProperty = _bootstrapper.Container.Resolve<GpsReliabilityProperty>();

            var states = Enumerable.Range(0, 25)
                                   .Select(i => _t0.AddMilliseconds(i * 100))
                                   .Select(t =>
                                           new
                                           {
                                               Speed = speedProperty.HaveValue(t) ? speedProperty.GetValue(t).ToString() : "undefined",
                                               Position = positionProperty.HaveValue(t) ? positionProperty.GetValue(t).ToString() : "undefined",
                                               Reliability = reliabilityProperty.HaveValue(t) ? reliabilityProperty.GetValue(t).ToString() : "undefined"
                                           })
                                   .ToList();

            //var bootstrapperThread = new Thread(bootstrapper.Run);
            //bootstrapperThread.Start();
        }*/
    }
}
