using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlokFrames;
using Communications;
using Communications.Can;
using Geographics;
using Microsoft.Practices.Unity;
using Modules;
using NUnit.Framework;
using Rhino.Mocks;
using Saut.Communication.Modules;
using Saut.StateModel;
using Saut.StateModel.Modules;
using Saut.StateModel.StateProperties;

namespace IntegrationTest
{
    [TestFixture]
    public class FrameToPropertyPassTests
    {
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

        [Test]
        public void PassFrameToPropertyTest()
        {
            var t0 = DateTime.Today;
            IEnumerable<CanFrame> frames =
                new BlokFrame[]
                {
                    new MmAltLongFrame { Time = t0.AddSeconds(0.0), Latitude = 60.0, Longitude = 50.0, Reliable = false },
                    new MmAltLongFrame { Time = t0.AddSeconds(0.5), Latitude = 60.0, Longitude = 50.0, Reliable = true },
                    new MmAltLongFrame { Time = t0.AddSeconds(1.0), Latitude = 70.0, Longitude = 40.0, Reliable = true },
                    new IpdState { Time = t0.AddSeconds(0.2), Speed = 30 },
                    new IpdState { Time = t0.AddSeconds(0.7), Speed = 35 },
                    new IpdState { Time = t0.AddSeconds(1.2), Speed = 45 },
                }.OrderBy(m => m.Time).Select(bf => bf.GetCanFrame());

            var bootstrapper = new MyTestBootstrapper(
                () =>
                {
                    var socketSourceMock = MockRepository.GenerateMock<ISocketSource<ICanSocket>>();
                    socketSourceMock
                        .Expect(ss => ss.OpenSocket())
                        .Message("Ожидается, что будет однократно запрошено открытие сокета")
                        .Return(GetSocketMock(frames));
                    return socketSourceMock;
                });
            bootstrapper.Initialize();

            bootstrapper.Run();

            var speedProperty = bootstrapper.Container.Resolve<SpeedProperty>();
            var positionProperty = bootstrapper.Container.Resolve<GpsPositionProperty>();
            var reliabilityProperty = bootstrapper.Container.Resolve<GpsReliabilityProperty>();

            var states = Enumerable.Range(0, 15)
                             .Select(i => t0.AddMilliseconds(i * 100))
                             .Select(t =>
                                     new {
                                         Speed = speedProperty.GetValue(t),
                                         Position = positionProperty.GetValue(t),
                                         Reliability = reliabilityProperty.GetValue(t)
                                     })
                             .ToList();

            //var bootstrapperThread = new Thread(bootstrapper.Run);
            //bootstrapperThread.Start();
        }
    }
}
