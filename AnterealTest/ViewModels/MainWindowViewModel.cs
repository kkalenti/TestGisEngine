using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using AnterealTest.Helper.Enums;
using AnterealTest.Interfaces;
using AnterealTest.Models;
using AnterealTest.ViewModels.Base;
using Microsoft.Win32;

namespace AnterealTest.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            _getFilePathCommand = new RelayCommand(GetFilePath);
            _loadingCommand = new RelayCommand(Load);
        }

        #region BindedProperties

        private string _filePath;
        /// <summary>
        /// Путь к файлу для обработки
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; RaisePropertyChanged("FilePath"); }
        }

        private string _messageField;
        /// <summary>
        /// Сообщение для пользователя о состоянии программы
        /// </summary>
        public string MessageField
        {
            get { return _messageField; }
            set { _messageField = value; RaisePropertyChanged("MessageField"); }
        }

        private ObservableCollection<GeometryBaseModel> _geometries;
        /// <summary>
        /// Список для хранения геометрий
        /// </summary>
        public ObservableCollection<GeometryBaseModel> Geometries
        {
            get { return _geometries; }
            set { _geometries = value; RaisePropertyChanged("Geometries"); }
        }

        private double _scaleValue = 1;
        /// <summary>
        /// Свойство хранит информацию о масштабе карты
        /// </summary>
        public double ScaleValue
        {
            get { return _scaleValue;}
            set { _scaleValue = value; RaisePropertyChanged("ScaleValue"); }
        }

        private Point _transformValue = new Point(0,0);
        /// <summary>
        /// Свойство хранит информацию о положении области видимости
        /// </summary>
        public Point TransformValue
        {
            get { return _transformValue; }
            set { _transformValue = value; RaisePropertyChanged("TransformValue"); }
        }

        private Point _centerValue = new Point(0, 0);
        /// <summary>
        /// Переменная хранит информацию о центре масштабирования карты
        /// </summary>
        public Point CenterValue
        {
            get { return _centerValue; }
            set { _centerValue = value; RaisePropertyChanged("CenterValue"); }
        }

        /// <summary>
        /// Состояние загрузки файла
        /// </summary>
        private LoadingState _state = LoadingState.Success;

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        private string _message = "";

        #endregion

        #region Commands
        
        /// <summary>
        /// Открытие диалогового окна для выбора файла
        /// </summary>
        /// <param name="parameter"></param>
        private void GetFilePath(object parameter = null)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
            }
        }

        /// <summary>
        /// Загрузка выбранного файла
        /// </summary>
        /// <param name="parameter"></param>
        private void Load(object parameter = null)
        {
            ReadFile();
            UpdateMessage();
        }

        private ICommand _getFilePathCommand;
        /// <summary>
        /// Комманда для выбора файла и сохранения его пути
        /// </summary>
        public ICommand GetFilePathCommand => _getFilePathCommand;

        private ICommand _loadingCommand;
        /// <summary>
        /// Комманда для загрузки информации из файла
        /// </summary>
        public ICommand LoadingCommand => _loadingCommand;

        #endregion

        /// <summary>
        /// Удаление геометрий
        /// </summary>
        /// <param name="geometriesToDelete">Геометрии на удаление</param>
        public void DeleteGeometries(List<GeometryBaseModel> geometriesToDelete)
        {
            geometriesToDelete.ForEach(x => Geometries.Remove(x));
        }

        /// <summary>
        /// Сохранение файла
        /// </summary>
        public void SaveChanges()
        {
            if (Geometries == null || Geometries.Count == 0) return;

            var saveFileDialog = new SaveFileDialog()
            {
                Filter = "Text files (*.txt)|*.txt"
            };

            var stringsToSave = Geometries.Select(geom => geom.GetStringToSave());

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllLines(saveFileDialog.FileName, stringsToSave);
            }
        }

        /// <summary>
        /// Метод для чтения текстого файла
        /// </summary>
        private void ReadFile()
        {
            try
            {
                if (string.IsNullOrEmpty(FilePath))
                {
                    _state = LoadingState.Failed;
                    _message = "Вы не выбрали файл";
                    return;
                }

                using (var sr = new StreamReader(FilePath))
                {
                    string line;
                    Geometries = new ObservableCollection<GeometryBaseModel>();
                    while ((line = sr.ReadLine()) != null)
                    {
                        var numberLine = line.Split(' ');
                        var pointList = new List<Point>();
                        var failFlag = false;

                        for (var index = 0; index < numberLine.Length; index += 2)
                        {
                            if (!int.TryParse(numberLine[index], out var xNumber))
                            {
                                _state = LoadingState.Partial;
                                failFlag = true;
                                break;
                            }

                            if (!int.TryParse(numberLine[index + 1], out var yNumber))
                            {
                                _state = LoadingState.Partial;
                                failFlag = true;
                                break;
                            }

                            pointList.Add(new Point(xNumber, yNumber));
                        }

                        if (failFlag) continue;

                        Geometries.Add(SpawnGeometry(pointList));
                    }
                }

                ScaleValue = 1;
                TransformValue = new Point(0,0);
                CenterValue = new Point(0, 0);
            }
            catch (FileNotFoundException)
            {
                _state = LoadingState.Failed;
                _message = "Запрашиваемый файл не найден";
            }
            catch (UnauthorizedAccessException)
            {
                _state = LoadingState.Failed;
                _message = "У вас нет доступа к этому файлу";
            }
            catch (Exception e)
            {
                _state = LoadingState.Failed;
                _message = "Произошла неизвестная ошибка";
            }
        }

        /// <summary>
        /// Метод создает геометрическую сущность в зависимости от количества точек
        /// </summary>
        /// <returns>Геометрический объект</returns>
        private GeometryBaseModel SpawnGeometry(List<Point> pointList)
        {
            if (pointList.Count == 1)
            {
                return new PointModel(pointList);
            }
            else if (pointList.Count == 2)
            {
                return new LineModel(pointList);
            }
            else if (pointList.Count >= 3)
            {
                return new PolygonModel(pointList);
            }
            else
            {
                _state = LoadingState.Partial;
                return null;
            }
        }

        /// <summary>
        /// Метод для обновления сообщения об ошибке (или её отсутствие)
        /// </summary>
        private void UpdateMessage()
        {
            switch (_state)
            {
                case LoadingState.Success:
                    MessageField = "Документ прочитан без ошибок";
                    break;
                case LoadingState.Partial:
                    MessageField = "Данные были прочитаны частично";
                    break;
                case LoadingState.Failed:
                    MessageField = $"Произошла ошибка: {_message}";
                    break;
            }
        }
    }
}