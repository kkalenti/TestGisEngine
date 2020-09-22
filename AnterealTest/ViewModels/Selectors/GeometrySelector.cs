using System.Windows;
using System.Windows.Controls;
using AnterealTest.Models;

namespace AnterealTest.ViewModels.Selectors
{
    public class GeometrySelector : DataTemplateSelector
    {
        public DataTemplate PointTemplate { get; set; }

        public DataTemplate LineTemplate { get; set; }

        public DataTemplate PolygonTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item)
            {
                case PointModel _:
                    return PointTemplate;
                case LineModel _:
                    return LineTemplate;
                case PolygonModel _:
                    return PolygonTemplate;
                default:
                    return base.SelectTemplate(item, container);
            }
        }
    }
}