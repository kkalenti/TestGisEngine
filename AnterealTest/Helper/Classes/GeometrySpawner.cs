using System.Collections.Generic;
using System.IO;
using System.Windows;
using AnterealTest.Interfaces;
using AnterealTest.Models;

namespace AnterealTest.Helper.Classes
{
    /// <summary>
    /// Класс спавнер геометрий, создан для разделения логики
    /// VM и создания геометрий
    /// </summary>
    public class GeometrySpawner
    {
        /// <summary>
        /// Создание геометрии по списку точек
        /// </summary>
        /// <param name="pointList">Список точек</param>
        /// <returns>Геометрическая фигура</returns>
        public static GeometryBaseModel Spawn(List<Point> pointList)
        {
            if(pointList == null || pointList.Count == 0) throw new InvalidDataException();

            if (pointList.Count == 1)
            {
                return new PointModel(pointList);
            }
            
            if (pointList.Count == 2)
            {
                return new LineModel(pointList);
            }
            
            if (pointList.Count >= 3)
            {
                return new PolygonModel(pointList);
            }

            throw new InvalidDataException();
        }
    }
}