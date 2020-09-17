using System.Windows.Input;
using AnterealTest.ViewModels.Base;
using Microsoft.Win32;

namespace AnterealTest.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            _getFilePathCommand = new RelayCommand(GetFilePath);
            _loadFileCommand = new RelayCommand(LoadFile);
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

            //TODO: Подумать над сообщением об ошибке
            MessageField = "Ошибка загрузки файла";
        }

        private void LoadFile(object parameter = null)
        {

        }

        private ICommand _getFilePathCommand;
        public ICommand GetFilePathCommand => _getFilePathCommand;

        private ICommand _loadFileCommand;
        public ICommand LoadFileCommand => _loadFileCommand;

        #endregion

    }
}