using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AnterealTest.Interfaces
{
    /// <summary>
    /// Базовый класс, представляет базовую структуру для
    /// геометрий, состоящих из точек
    /// </summary>
    public abstract class GeometryBaseModel
    {
        /// <summary>
        /// Конструктор класса, инициализирует список точек фигуры
        /// </summary>
        /// <param name="geometryPoints"></param>
        protected GeometryBaseModel(List<Point> geometryPoints)
        {
            GeometryPoints = geometryPoints;
        }

        /// <summary>
        /// Набор точек геометрии
        /// </summary>
        public List<Point> GeometryPoints { get; }

        /// <summary>
        /// Получение набора точек в формате для сохранения
        /// </summary>
        /// <returns>Набор точек для сохранения</returns>
        public string GetStringToSave()
        {
            var resultString = GeometryPoints.Aggregate("", (current, point) => current + $"{point.X} {point.Y} ");
            resultString = resultString.Remove(resultString.Length - 1, 1);

            return resultString;
        }
    }
}