using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        private ObservableCollection<IGeometry> _geometries;
        /// <summary>
        /// Список для хранения геометрий
        /// </summary>
        public ObservableCollection<IGeometry> Geometries
        {
            get { return _geometries; }
            set { _geometries = value; RaisePropertyChanged("Geometries"); }
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

        public ICommand GetFilePathCommand => _getFilePathCommand;

        private ICommand _loadingCommand;

        public ICommand LoadingCommand => _loadingCommand;

        #endregion

        /// <summary>
        /// Метод для чтения текстого файла
        /// </summary>
        private void ReadFile()
        {
            try
            {
                using (var sr = new StreamReader(FilePath))
                {
                    string line;
                    Geometries = new ObservableCollection<IGeometry>();
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
            catch (Exception)
            {
                _state = LoadingState.Failed;
                _message = "Произошла неизвестная ошибка";
            }
        }

        /// <summary>
        /// Метод создает геометрическую сущность в зависимости от количества точек
        /// </summary>
        /// <returns>Геометрический объект</returns>
        private IGeometry SpawnGeometry(List<Point> pointList)
        {
            if (pointList.Count == 1)
            {
                return new PointModel()
                {
                    GeometryPoints = pointList
                };
            }
            else if (pointList.Count == 2)
            {
                return new LineModel()
                {
                    GeometryPoints = pointList
                };
            }
            else if (pointList.Count >= 3)
            {
                return new PolygonModel()
                {
                    GeometryPoints = pointList
                };
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