using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using AnterealTest.Interfaces;

namespace AnterealTest.Models
{
    public class LineModel : IGeometry
    {
        public List<Point> GeometryPoints { get; set; }

        public Point FirstPoint
        {
            get
            {
                if (GeometryPoints.Count != 2) throw new InvalidDataException();

                return GeometryPoints.FirstOrDefault();
            }
        }

        public Point SecondPoint
        {
            get
            {
                if (GeometryPoints.Count != 2) throw new InvalidDataException();

                return GeometryPoints.LastOrDefault();
            }
        }

    }
}