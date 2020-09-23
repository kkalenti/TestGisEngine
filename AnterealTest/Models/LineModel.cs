using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using AnterealTest.Interfaces;

namespace AnterealTest.Models
{
    /// <summary>
    /// Модель представляет собой линию
    /// </summary>
    public class LineModel : GeometryBaseModel
    {
        /// <summary>
        /// Конструктор класса, инициализирует список точек фигуры
        /// </summary>
        /// <param name="pointList"></param>
        public LineModel(List<Point> pointList) : base(pointList)
        {

        }

        /// <summary>
        /// Первая точка линии
        /// </summary>
        public Point FirstPoint
        {
            get
            {
                if (GeometryPoints.Count != 2) throw new InvalidDataException();

                return GeometryPoints.FirstOrDefault();
            }
        }

        /// <summary>
        /// Вторая точка линии
        /// </summary>
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