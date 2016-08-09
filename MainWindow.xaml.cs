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

        private void PropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case "Clocks": UpdateClocks(); break;
                case "Conversions": UpdateConversions(); break;
            }
        }
        #endregion

        #region Methods
        protected void SetUp()
        {
            instance = this;

            SettingsHolder.Navigate(ViewModel.SettingsPage);

            ViewModel.PropertyChanged += (o, e) => PropertyChanged(e.PropertyName);
            ViewModel.TriggerUpdate();
        }

        public void ToggleSettings()
        {
            ViewModel.IsSettingsVisible = !ViewModel.IsSettingsVisible;
            if (!ViewModel.IsSettingsVisible)
            {
                ViewModel.SettingsPage.ViewModel.Load();
                ViewModel.TriggerUpdate();
            }
        }

        protected void UpdateClocks()
        {
            ClocksHolder.Children.Clear();
            foreach (string OneClock in ViewModel.Clocks)
            {
                ClocksHolder.Children.Add(CreateOneClock(OneClock));
            }
        }

        protected UIElement CreateOneClock(string timezone)
        {
            Grid Ret = new Grid() { Width = 150, Height = 80, Margin = new Thickness(5), Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEEEEE")) };

            Ret.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(20) });
            Ret.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            Ret.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(20) });

            TextBlock TmpText = new TextBlock() { Text = timezone, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 5, 0, 0) };
            TmpText.SetValue(Grid.RowProperty, 0);
            Ret.Children.Add(TmpText);

            TmpText = new TextBlock() { Text = "10:00 AM", FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "Resources/Fonts/#Orbitron"), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Tag = timezone };
            Viewbox VB = new Viewbox();
            VB.SetValue(Grid.RowProperty, 1);
            VB.Child = TmpText;
            Ret.Children.Add(VB);

            TmpText = new TextBlock() { Text = "2016-08-01", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 0, 5), Tag = timezone };
            TmpText.SetValue(Grid.RowProperty, 2);
            Ret.Children.Add(TmpText);

            return Ret;
        }

        protected void UpdateConversions()
        {

        }
        #endregion
    }
}
