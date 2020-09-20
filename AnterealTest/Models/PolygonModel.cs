using System.Collections.Generic;
using System.Linq;
using System.Windows;
using AnterealTest.Interfaces;

namespace AnterealTest.Models
{
    public class PolygonModel : IGeometry
    {
        public List<Point> GeometryPoints { get; set; }

        public string StringOfPoints
        {
            get
            {
                var resultString = GeometryPoints.Aggregate("", (current, point) => current + $"{point.X}, {point.Y}, ");

                resultString = resultString.Remove(resultString.Length - 2, 2);

                return resultString;
            }
        }
    }
}