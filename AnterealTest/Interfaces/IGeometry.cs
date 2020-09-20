using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace AnterealTest.Interfaces
{
    public interface IGeometry
    {
        List<Point> GeometryPoints { get; set; }
    }
}