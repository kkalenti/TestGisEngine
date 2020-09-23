using System.Collections.Generic;
using System.Linq;
using System.Windows;
using AnterealTest.Interfaces;

namespace AnterealTest.Models
{
    /// <summary>
    /// Модель, представляет собой полигон
    /// </summary>
    public class PolygonModel : GeometryBaseModel
    {
        /// <summary>
        /// Конструктор класса, инициализирует список точек фигуры
        /// </summary>
        /// <param name="pointList"></param>
        public PolygonModel(List<Point> pointList) : base(pointList)
        {

        }

        /// <summary>
        /// Формирование строки точек для передачи во View формы
        /// </summary>
        public string StringOfPointsToView
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