using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WhatTimeIsIt
{
    public class BaseNotify : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string propChanged = null)
        {
            if (PropertyChanged != null && !String.IsNullOrEmpty(propChanged))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propChanged));
            }
        }
        #endregion
    }
}
