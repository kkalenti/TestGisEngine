using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        private LoadingState _state = LoadingState.Success;

        #endregion

        #region Commands

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
            else
            {
                //TODO: Подумать над сообщением об ошибке
                _state = LoadingState.Failed;
            }
        }

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
                    MessageField = "Произошла ошибка";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Load(object parameter = null)
        {
            ReadFile();
            UpdateMessage();
        }

        //TODO: сделать все проверки
        private void ReadFile()
        {
            try
            {
                using (var sr = new StreamReader(FilePath))
                {
                    string line;
                    var tempGeometries = new ObservableCollection<IGeometry>();

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

                        if(failFlag) continue;

                        if (pointList.Count == 1)
                        {
                            tempGeometries.Add(new PointModel()
                            {
                                GeometryPoints = pointList
                            });
                        }
                        else if (pointList.Count == 2)
                        {
                            tempGeometries.Add(new LineModel()
                            {
                                GeometryPoints = pointList
                            });
                        }
                        else if (pointList.Count >= 3)
                        {
                            tempGeometries.Add(new PolygonModel()
                            {
                                GeometryPoints = pointList
                            });
                        }
                        else
                        {
                            _state = LoadingState.Partial;
                            break;
                        }
                    }

                    Geometries = tempGeometries;
                }
            }
            catch (Exception e)
            {
                _state = LoadingState.Failed;
            }
        }

        private ICommand _getFilePathCommand;

        public ICommand GetFilePathCommand => _getFilePathCommand;

        private ICommand _loadingCommand;

        public ICommand LoadingCommand => _loadingCommand;

        #endregion

    }

    public enum LoadingState
    {
        Success,
        Partial,
        Failed
    }

    public class GeometrySelector : DataTemplateSelector
    {
        public DataTemplate PointTemplate { get; set; }

        public DataTemplate LineTemplate { get; set; }

        public DataTemplate PolygonTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            switch (item)
            {
                case PointModel _:
                    return PointTemplate;
                case LineModel _:
                    return LineTemplate;
                case PolygonModel _:
                    return PolygonTemplate;
                default:
                    return base.SelectTemplate(item, container);
            }
        }
    }
}