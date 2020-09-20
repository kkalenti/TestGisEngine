using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using AnterealTest.Interfaces;

namespace AnterealTest.Models
{
    public class PointModel : IGeometry
    {
        public List<Point> GeometryPoints { get; set; }

        public double XPosition => GeometryPoints.FirstOrDefault().X;

        public double YPosition => GeometryPoints.FirstOrDefault().Y;

        public string StringOfPoints => $"{XPosition},{YPosition}";
    }
}