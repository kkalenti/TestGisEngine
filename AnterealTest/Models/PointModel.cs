using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using AnterealTest.Interfaces;

namespace AnterealTest.Models
{
    /// <summary>
    /// Модель, представляет собой точку
    /// </summary>
    public class PointModel : GeometryBaseModel
    {
        /// <summary>
        /// Конструктор класса, инициализирует список точек фигуры
        /// </summary>
        /// <param name="pointList"></param>
        public PointModel(List<Point> pointList) : base(pointList)
        {

        }

        /// <summary>
        /// X координата точки
        /// </summary>
        public double XPosition => GeometryPoints.FirstOrDefault().X;

        /// <summary>
        /// Y координата точки
        /// </summary>
        public double YPosition => GeometryPoints.FirstOrDefault().Y;

        /// <summary>
        /// Формирование строки точек для передачи во View формы
        /// </summary>
        public string StringOfPointsToView => $"{XPosition},{YPosition}";
    }
}