using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AnterealTest.ViewModels;

namespace AnterealTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();
        }

        //Point? lastDragPoint;
        //private double _scaleRate = 0.03;

        //private void GeometryCanvas_OnMouseWheel(object sender, MouseWheelEventArgs e)
        //{
        //    var canvas = (sender as Canvas);
        //    if (canvas == null) return;

        //    var scaleTransformation = canvas.RenderTransform as ScaleTransform;

        //    if (e.Delta > 0)
        //    {
        //        //scaleTransformation.CenterX = e.GetPosition(canvas).X;
        //        //scaleTransformation.CenterY = e.GetPosition(canvas).Y;
        //        scaleTransformation.ScaleX += _scaleRate;
        //        scaleTransformation.ScaleY += _scaleRate;
        //    }
        //    else if(e.Delta < 0)
        //    {
        //        //scaleTransformation.CenterX = e.GetPosition(canvas).X;
        //        //scaleTransformation.CenterY = e.GetPosition(canvas).Y;
        //        scaleTransformation.ScaleX -= _scaleRate;
        //        scaleTransformation.ScaleY -= _scaleRate;
        //    }
        //}

        //private void GeometryCanvas_OnPreviewMouseMove(object sender, MouseEventArgs e)
        //{
        //    if (lastDragPoint.HasValue)
        //    {
        //        var canvas = (sender as Canvas);
        //        if (canvas == null) return;

        //        var scaleTransformation = canvas.RenderTransform as ScaleTransform;

        //        Point posNow = e.GetPosition(canvas);

        //        double dX = posNow.X - lastDragPoint.Value.X;
        //        double dY = posNow.Y - lastDragPoint.Value.Y;

        //        lastDragPoint = posNow;

        //        scaleTransformation.CenterX -= dX;
        //        scaleTransformation.CenterY -= dY;
        //    }
        //}

        //void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    var canvas = (sender as Canvas);
        //    if (canvas == null) return;

        //    var mousePos = e.GetPosition(canvas);

        //    canvas.Cursor = Cursors.SizeAll;
        //        lastDragPoint = mousePos;
        //        //Mouse.Capture(canvas);
        //}

        //void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    var canvas = (sender as Canvas);
        //    if (canvas == null) return;

        //    canvas.Cursor = Cursors.Arrow;
        //    lastDragPoint = null;
        //}
    }


}
