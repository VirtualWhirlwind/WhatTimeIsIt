using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WhatTimeIsIt.ViewModels;

namespace WhatTimeIsIt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties
        public static MainWindow instance { get; private set; }

        public MainWindowViewModel ViewModel
        {
            get
            {
                return this.DataContext as MainWindowViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }
        #endregion

        #region Construct / Destruct
        public MainWindow() : this(viewModel: null)
        {
        }

        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel ?? new MainWindowViewModel();

            SetUp();
        }
        #endregion

        #region Events
        private void Settings_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ToggleSettings();
        }
        #endregion

        #region Methods
        protected void SetUp()
        {
            instance = this;

            SettingsHolder.Navigate(ViewModel.SettingsPage);
        }

        public void ToggleSettings()
        {
            ViewModel.IsSettingsVisible = !ViewModel.IsSettingsVisible;
        }
        #endregion
    }
}
