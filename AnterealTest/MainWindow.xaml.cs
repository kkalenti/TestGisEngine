using System;
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
        /// VM для главного окна
        /// </summary>
        private MainWindowViewModel _mainWindowViewModel;

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
        /// Словарь для хранения соответствий фигур и цветов
        /// </summary>
        private Dictionary<Type, (Color Stroke, Color Fill)> shapeColorDictionary
            = new Dictionary<Type, (Color, Color)>()
            {
                {typeof(Line), (Colors.DarkGray, Colors.DarkGray)},
                {typeof(Polygon), (Colors.Chocolate, Colors.Coral)},
                {typeof(Path), (Colors.CadetBlue, Colors.Aquamarine)},
            };

        /// <summary>
        /// Лист для хранения выделенных фигур
        /// </summary>
        private readonly List<Shape> _selectedShapeList = new List<Shape>();

        public MainWindow()
        {
            _mainWindowViewModel = new MainWindowViewModel();
            DataContext = _mainWindowViewModel;
            InitializeComponent();
        }

        private void GeometryCanvas_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var canvas = (sender as Canvas);
            if (canvas == null) return;

            var geomTransformValue = _mainWindowViewModel.TransformValue;

            if (e.Delta > 0)
            {
                _mainWindowViewModel.CenterValue = new Point(e.GetPosition(canvas).X - geomTransformValue.X,
                    e.GetPosition(canvas).Y - geomTransformValue.Y);
                _mainWindowViewModel.ScaleValue *= _scaleRate;
            }
            else if(e.Delta < 0)
            {
                _mainWindowViewModel.CenterValue = new Point(e.GetPosition(canvas).X - geomTransformValue.X,
                    e.GetPosition(canvas).Y - geomTransformValue.Y);
                _mainWindowViewModel.ScaleValue /= _scaleRate;
            }
        }

        private void GeometryCanvas_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_lastDragPoint.HasValue)
            {
                _preSelectedItem = null;
                _selectFlag = false;

                var canvas = (sender as Canvas);
                if (canvas == null) return;

                var newPos = e.GetPosition(canvas) - _lastDragPoint;
                var geomTransformValue = _mainWindowViewModel.TransformValue;

                _mainWindowViewModel.TransformValue = new Point(_lastGeomPoint.Value.X + newPos.Value.X,
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
            //Если ButtonDown левой кнопки мыши произошло на фигуре, то все впорядке
            //иначе выбранные фигуры сбрасываются
            if (_selectFlag)
            {
                _selectFlag = false;
            }
            else
            {
                if (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    DropSelectedFigures();
                }
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

            _preSelectedItem.Stroke = new SolidColorBrush(Colors.Gold);
            _preSelectedItem.Fill = new SolidColorBrush(Colors.Yellow);
        }

        private void Window_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                _mainWindowViewModel.DeleteGeometries(_selectedShapeList
                    .Select(shape => shape.DataContext as GeometryBaseModel).ToList());

                _selectedShapeList.Clear();
            }

            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.S)
            {
                _mainWindowViewModel.SaveChanges();
            }
        }

        /// <summary>
        /// Сброс выделенных фигур
        /// </summary>
        private void DropSelectedFigures()
        {
            foreach (var shape in _selectedShapeList)
            {
                var shapeStyle = shapeColorDictionary[shape.GetType()];

                shape.Fill = new SolidColorBrush(shapeStyle.Fill);
                shape.Stroke = new SolidColorBrush(shapeStyle.Stroke);
            }
            
            _selectedShapeList.Clear();
        }
    }
}
