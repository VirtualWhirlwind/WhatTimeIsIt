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

            ViewModel = viewModel ?? new SettingsViewModel().Load();

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

        #region Clocks
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
            if (DestinationClocks.SelectedItems.Count > 0)
            {
                List<string> ToRemove = new List<string>();
                foreach (var OneItem in DestinationClocks.SelectedItems)
                {
                    ToRemove.Add(OneItem.ToString());
                }

                ToRemove.ForEach(item => ViewModel.Clocks.Remove(item));
                ViewModel.Setup();
            }
        }

        private void DestinationClocks_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            ViewModel.ClockRemoveEnabled = ViewModel.ClockUpEnabled = ViewModel.ClockDownEnabled = ((ListView)e.Source).SelectedItems.Count > 0;
        }

        private void ClockUp_Click(object sender, RoutedEventArgs e)
        {
            if (DestinationClocks.SelectedItems.Count > 0)
            {
                List<string> ToMove = new List<string>();
                foreach (var OneItem in DestinationClocks.SelectedItems)
                {
                    ToMove.Add(OneItem.ToString());
                }

                ToMove.ForEach(item =>
                {
                    int Index = ViewModel.Clocks.IndexOf(item);
                    if (Index > 0)
                    {
                        ViewModel.Clocks.Move(Index, Index - 1);
                    }
                });
            }
        }

        private void ClockDown_Click(object sender, RoutedEventArgs e)
        {
            if (DestinationClocks.SelectedItems.Count > 0)
            {
                List<string> ToMove = new List<string>();
                foreach (var OneItem in DestinationClocks.SelectedItems)
                {
                    ToMove.Add(OneItem.ToString());
                }

                ToMove.ForEach(item =>
                {
                    int Index = ViewModel.Clocks.IndexOf(item);
                    if (Index < ViewModel.Clocks.Count - 1)
                    {
                        ViewModel.Clocks.Move(Index, Index + 1);
                    }
                });
            }
        }
        #endregion

        #region Conversions
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
            if (DestinationConversions.SelectedItems.Count > 0)
            {
                List<string> ToRemove = new List<string>();
                foreach (var OneItem in DestinationConversions.SelectedItems)
                {
                    ToRemove.Add(OneItem.ToString());
                }

                ToRemove.ForEach(item => ViewModel.Conversions.Remove(item));
                ViewModel.Setup();
            }
        }

        private void DestinationConversions_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            ViewModel.ConversionRemoveEnabled = ViewModel.ConversionUpEnabled = ViewModel.ConversionDownEnabled = ((ListView)e.Source).SelectedItems.Count > 0;
        }

        private void ConversionUp_Click(object sender, RoutedEventArgs e)
        {
            if (DestinationConversions.SelectedItems.Count > 0)
            {
                List<string> ToMove = new List<string>();
                foreach (var OneItem in DestinationConversions.SelectedItems)
                {
                    ToMove.Add(OneItem.ToString());
                }

                ToMove.ForEach(item =>
                {
                    int Index = ViewModel.Conversions.IndexOf(item);
                    if (Index > 0)
                    {
                        ViewModel.Conversions.Move(Index, Index - 1);
                    }
                });
            }
        }

        private void ConversionDown_Click(object sender, RoutedEventArgs e)
        {
            if (DestinationConversions.SelectedItems.Count > 0)
            {
                List<string> ToMove = new List<string>();
                foreach (var OneItem in DestinationConversions.SelectedItems)
                {
                    ToMove.Add(OneItem.ToString());
                }

                ToMove.ForEach(item =>
                {
                    int Index = ViewModel.Conversions.IndexOf(item);
                    if (Index < ViewModel.Conversions.Count - 1)
                    {
                        ViewModel.Conversions.Move(Index, Index + 1);
                    }
                });
            }
        }
        #endregion
        #endregion

        #region Methods
        public void Setup()
        {
            ViewModel.Setup();
        }
        #endregion
    }
}
