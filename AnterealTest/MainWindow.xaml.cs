using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AnterealTest.Interfaces;
using AnterealTest.ViewModels;

namespace AnterealTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Коэффициент масштабирования
        /// </summary>
        private double _scaleRate = 1.05;

        /// <summary>
        /// Точка для контроля координат нажатия на канвас
        /// </summary>
        private Point? _lastDragPoint;

        /// <summary>
        /// Точка для контроля последней фиксированной координаты для фигур
        /// </summary>
        private Point? _lastGeomPoint;

        /// <summary>
        /// Фигура, которую могут выбрать(При нажатии левой кнопкой мыши она запоминается,
        /// при ButtonUp она выделяется)
        /// </summary>
        private Shape _preSelectedItem;

        /// <summary>
        /// Возвращает true, если при последнем клике мышкой была выделена фигура
        /// </summary>
        private bool _selectFlag;

        /// <summary>
        /// Словарь для хранения цветов для отображения
        /// </summary>
        private Dictionary<string, Color> _controlColorsDictionary = new Dictionary<string, Color>()
        {
            {"LineStroke", Colors.DarkGray},
            {"EllipseStroke", Colors.CadetBlue},
            {"EllipseFill", Colors.Aquamarine},
            {"PolygonStroke", Colors.Chocolate},
            {"PolygonFill", Colors.Coral},
            {"SelectedStroke", Colors.Gold},
            {"SelectedFill", Colors.Yellow},
        };

        /// <summary>
        /// Лист для хранения выделенных фигур
        /// </summary>
        private List<Shape> _selectedShapeList = new List<Shape>();

        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();
        }

        private void GeometryCanvas_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var canvas = (sender as Canvas);
            if (canvas == null) return;

            var geomTransformValue = ((MainWindowViewModel)DataContext).TransformValue;

            if (e.Delta > 0)
            {
                ((MainWindowViewModel)DataContext).CenterValue = new Point(e.GetPosition(canvas).X - geomTransformValue.X,
                    e.GetPosition(canvas).Y - geomTransformValue.Y);
                ((MainWindowViewModel)DataContext).ScaleValue *= _scaleRate;
            }
            else if(e.Delta < 0)
            {
                ((MainWindowViewModel)DataContext).CenterValue = new Point(e.GetPosition(canvas).X - geomTransformValue.X,
                    e.GetPosition(canvas).Y - geomTransformValue.Y);
                ((MainWindowViewModel)DataContext).ScaleValue /= _scaleRate;
            }
        }

        private void GeometryCanvas_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_lastDragPoint.HasValue)
            {
                _preSelectedItem = null; //Если происходит движение мыши при нажатии на объект, то выделение объекта отменяется
                _selectFlag = false;

                var canvas = (sender as Canvas);
                if (canvas == null) return;

                var newPos = e.GetPosition(canvas) - _lastDragPoint;
                var geomTransformValue = ((MainWindowViewModel)DataContext).TransformValue;

                ((MainWindowViewModel)DataContext).TransformValue = new Point(_lastGeomPoint.Value.X + newPos.Value.X,
                    _lastGeomPoint.Value.Y + newPos.Value.Y);
            }
        }

        private void GeometryCanvas_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = (sender as Canvas);
            if (canvas == null) return;

            _lastGeomPoint = ((MainWindowViewModel) DataContext).TransformValue;
            _lastDragPoint = e.GetPosition(canvas);
        }

        private void GeometryCanvas_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_selectFlag) //Если ButtonDown левой кнопки мыши произошло на фигуре, то все впорядке
            {
                _selectFlag = false;
            }
            else //Если ButtonDown на канвасе, то выбранные фигуры сбрасываются
            {
                if (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl))
                    DropSelectedFigures();
            }

            var canvas = (sender as Canvas);
            if (canvas == null) return;

            _lastGeomPoint = null;
            _lastDragPoint = null;
        }

        private void GeometryCanvas_OnLostFocus(object sender, RoutedEventArgs e)
        {
            _lastDragPoint = null;
        }

        private void Shape_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _preSelectedItem = sender as Shape;
        }

        private void Shape_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(_preSelectedItem == null) return;

            _selectFlag = true;

            if (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl))
                DropSelectedFigures();

            _selectedShapeList.Add(_preSelectedItem);

            _preSelectedItem.Stroke = new SolidColorBrush(_controlColorsDictionary["SelectedStroke"]);
            _preSelectedItem.Fill = new SolidColorBrush(_controlColorsDictionary["SelectedFill"]);
        }

        private void Window_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                ((MainWindowViewModel)DataContext).DeleteGeometries(_selectedShapeList
                    .Select(shape => shape.DataContext as GeometryBaseModel).ToList());

                _selectedShapeList.Clear();
            }

            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.S)
            {
                ((MainWindowViewModel)DataContext).SaveChanges();
            }
        }

        /// <summary>
        /// Сброс выделенных фигур
        /// </summary>
        private void DropSelectedFigures()
        {
            foreach (var shape in _selectedShapeList)
            {
                switch (shape)
                {
                    case Line lineShape:
                        lineShape.Stroke = new SolidColorBrush(_controlColorsDictionary["LineStroke"]);
                        lineShape.Fill = new SolidColorBrush(_controlColorsDictionary["LineStroke"]);
                        break;

                    case Polygon polygonShape:
                        polygonShape.Stroke = new SolidColorBrush(_controlColorsDictionary["PolygonStroke"]);
                        polygonShape.Fill = new SolidColorBrush(_controlColorsDictionary["PolygonFill"]);
                        break;

                    case Path ellipseShape:
                        ellipseShape.Stroke = new SolidColorBrush(_controlColorsDictionary["EllipseStroke"]);
                        ellipseShape.Fill = new SolidColorBrush(_controlColorsDictionary["EllipseFill"]);
                        break;
                }
            }
            
            _selectedShapeList.Clear();
        }
    }


}
