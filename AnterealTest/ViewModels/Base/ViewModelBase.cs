using System;
using System.ComponentModel;

namespace AnterealTest.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected void RaisePropertyChanged(string p)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}