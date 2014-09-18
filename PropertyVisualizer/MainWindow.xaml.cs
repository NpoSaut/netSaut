using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using BlokFrames;
using Communications;
using Communications.Can;
using GraphVisualizing.GraphProviders;
using GraphVisualizing.Projection;
using Microsoft.Practices.Unity;
using Rhino.Mocks;
using Saut.StateModel.StateProperties;

namespace PropertyVisualizer
{
    /// <summary>Логика взаимодействия для MainWindow.xaml</summary>
    public partial class MainWindow : Window
    {
        private MyTestBootstrapper _bootstrapper;
        private DateTime _t0;

        public MainWindow()
        {
            InitializeBootstrapper();
            InitializeComponent();
            ((ScaleShiftProjector<DateTime, Double>)GraphView.Projector).ScaleX = (100.0 / TimeSpan.FromSeconds(2).TotalSeconds);
            ((ScaleShiftProjector<DateTime, Double>)GraphView.Projector).ScaleY = 2;
            ((ScaleShiftProjector<DateTime, Double>)GraphView.Projector).X0 = _t0.AddSeconds(-3);
            ((ScaleShiftProjector<DateTime, Double>)GraphView.Projector).Y0 = 100;
            GraphView.GraphProvider = new GP<DateTime, Double>(GetSpeedValue, new Pen(new SolidColorBrush(Colors.Red), 3));
        }

        private double GetSpeedValue(DateTime Time)
        {
            var sp = _bootstrapper.Container.Resolve<SpeedProperty>();
            var val = sp.HaveValue(Time) ? sp.GetValue(Time) : -10;
            return val;
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

        private void InitializeBootstrapper()
        {
            _t0 = DateTime.Today;
            IEnumerable<CanFrame> frames =
                new BlokFrame[]
                {
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

        private void GraphView_OnLoaded(object Sender, RoutedEventArgs E)
        {
            GraphView.Refresh();
        }
    }
}
