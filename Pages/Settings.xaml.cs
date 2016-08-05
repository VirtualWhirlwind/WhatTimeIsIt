using System;
using System.Collections.Generic;
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

namespace WhatTimeIsIt.Pages
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public SettingsViewModel ViewModel
        {
            get
            {
                return this.DataContext as SettingsViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        public Settings() : this(viewModel: null)
        {
        }

        public Settings(SettingsViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel ?? SettingsViewModel.Load();

            Setup();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.instance.ToggleSettings();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Save();
            MainWindow.instance.ToggleSettings();
        }

        public void Setup()
        {
            ViewModel.Setup();
        }
    }
}
