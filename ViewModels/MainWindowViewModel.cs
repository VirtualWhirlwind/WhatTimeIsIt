using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WhatTimeIsIt.ViewModels
{
    public class MainWindowViewModel : BaseNotify
    {
        #region Fields
        private bool _IsSettingsVisible;
        #endregion

        #region Properties
        public bool IsSettingsVisible
        {
            get { return _IsSettingsVisible; }
            set
            {
                _IsSettingsVisible = value;
                OnPropertyChanged();
                OnPropertyChanged("SettingsVisibility");
            }
        }

        public Visibility SettingsVisibility
        {
            get
            {
                return IsSettingsVisible ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public Pages.Settings SettingsPage { get; set; }
        #endregion

        #region Construct / Destruct
        public MainWindowViewModel()
        {
            SettingsPage = new Pages.Settings(SettingsViewModel.Load());
        }
        #endregion
    }
}
