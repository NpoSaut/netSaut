using System;
using System.Windows;
using System.Windows.Media;
using GraphVisualizing.GraphProviders;
using GraphVisualizing.Projection;

namespace PropertyVisualizer
{
    /// <summary>Логика взаимодействия для MainWindow.xaml</summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ((ScaleShiftProjector<Double, Double>)GraphView.Projector).ScaleX = 30;
            ((ScaleShiftProjector<Double, Double>)GraphView.Projector).ScaleY = 30;
            ((ScaleShiftProjector<Double, Double>)GraphView.Projector).X0 = 0;
            ((ScaleShiftProjector<Double, Double>)GraphView.Projector).Y0 = 2;
            GraphView.GraphProvider = new GP(Math.Sin, new Pen(new SolidColorBrush(Colors.Red), 3));
        }

        private void GraphView_OnLoaded(object Sender, RoutedEventArgs E)
        {
            GraphView.Refresh();
        }
    }
}
