using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public string DateFormat
        {
            get
            {
                return SettingsPage.ViewModel.DateFormat;
            }
        }

        public string TimeFormat
        {
            get
            {
                return SettingsPage.ViewModel.TimeFormat;
            }
        }

        public string DateTimeFormat
        {
            get
            {
                return DateFormat + " " + TimeFormat;
            }
        }

        public ObservableCollection<string> Clocks
        {
            get
            {
                return SettingsPage.ViewModel.Clocks;
            }
        }

        public ObservableCollection<string> Conversions
        {
            get
            {
                return SettingsPage.ViewModel.Conversions;
            }
        }
        #endregion

        #region Construct / Destruct
        public MainWindowViewModel()
        {
            SettingsPage = new Pages.Settings();
            SettingsPage.ViewModel.PropertyChanged += (o, e) => { if (!IsSettingsVisible) { OnPropertyChanged(e.PropertyName); } };
        }
        #endregion

        #region Methods
        public void TriggerUpdate()
        {
            OnPropertyChanged("Clocks");
            OnPropertyChanged("Conversions");
        }
        #endregion
    }
}
