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
	// TODO: Add drag and drop capabilities

    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
		#region Properties
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
		#endregion

		#region Construct / Destruct
		public Settings() : this(viewModel: null)
        {
        }

        public Settings(SettingsViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel ?? SettingsViewModel.Load();

            Setup();
        }
		#endregion

		#region Events
		private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.instance.ToggleSettings();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Save();
            MainWindow.instance.ToggleSettings();
        }

		private void ClocksAvailable_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ViewModel.ClockAddEnabled = ((ListView)e.Source).SelectedItems.Count > 0;
		}

		private void ClockAdd_Click(object sender, RoutedEventArgs e)
		{
			if (SourceClocks.SelectedItems.Count > 0)
			{
				foreach (var OneItem in SourceClocks.SelectedItems)
				{
					ViewModel.Clocks.Add(OneItem.ToString());
				}
				ViewModel.Setup();
			}
		}

		private void ClockRemove_Click(object sender, RoutedEventArgs e)
		{

		}

		private void Clocks_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void ConversionsAvailable_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ViewModel.ConversionAddEnabled = ((ListView)e.Source).SelectedItems.Count > 0;
		}

		private void ConversionAdd_Click(object sender, RoutedEventArgs e)
		{
			if (SourceConversions.SelectedItems.Count > 0)
			{
				foreach (var OneItem in SourceConversions.SelectedItems)
				{
					ViewModel.Conversions.Add(OneItem.ToString());
				}
				ViewModel.Setup();
			}
		}

		private void ConversionRemove_Click(object sender, RoutedEventArgs e)
		{

		}

		private void Conversions_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}
		#endregion

		#region Methods
		public void Setup()
        {
            ViewModel.Setup();
        }
		#endregion
	}
}
