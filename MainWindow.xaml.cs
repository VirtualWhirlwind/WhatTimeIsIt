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
using System.Windows.Threading;
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

        protected DispatcherTimer UpdateDisplay { get; set; }

        protected List<TextBlock> Times { get; set; }

        protected List<TextBlock> Dates { get; set; }
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
                case "Clocks": CreateClocks(); UpdateClocks(); break;
                case "Conversions": UpdateConversions(); break;
            }
        }
        #endregion

        #region Methods
        protected void SetUp()
        {
            instance = this;

            Times = new List<TextBlock>();
            Dates = new List<TextBlock>();

            SettingsHolder.Navigate(ViewModel.SettingsPage);

            ViewModel.PropertyChanged += (o, e) => PropertyChanged(e.PropertyName);
            ViewModel.TriggerUpdate();

            UpdateDisplay = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 2) };
            UpdateDisplay.Tick += (o, e) => UpdateClocks();
            UpdateDisplay.Start();
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

        protected void CreateClocks()
        {
            ClocksHolder.Children.Clear();
            Times.Clear();
            Dates.Clear();

            foreach (string OneClock in ViewModel.Clocks)
            {
                var TmpClock = CreateOneClock(OneClock);
                if (TmpClock != null) { ClocksHolder.Children.Add(TmpClock); }
            }
        }

        protected UIElement CreateOneClock(string timezone)
        {
            if (!ViewModel.TimezonesAvailable.ContainsKey(timezone)) { return null; }

            Grid Ret = new Grid() { Width = 180, Height = 88, Margin = new Thickness(5, 5, 0, 0), Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEEEEE")) };

            Ret.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(24) });
            Ret.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            Ret.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(24) });

            #region Row 0
            TextBlock TmpText = new TextBlock() { Text = timezone, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 5, 0, 0) };
            Viewbox VB = new Viewbox() { Margin = new Thickness(2) };
            VB.SetValue(Grid.RowProperty, 0);
            VB.Child = TmpText;
            Ret.Children.Add(VB);
            #endregion

            #region Row 1
            TmpText = new TextBlock() { Text = "", FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "Resources/Fonts/#Orbitron"), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Tag = timezone };
            VB = new Viewbox();
            VB.SetValue(Grid.RowProperty, 1);
            VB.Child = TmpText;
            Ret.Children.Add(VB);
            Times.Add(TmpText);
            #endregion

            #region Row 2
            TmpText = new TextBlock() { Text = "", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 0, 5), Tag = timezone };
            TmpText.SetValue(Grid.RowProperty, 2);
            Ret.Children.Add(TmpText);
            Dates.Add(TmpText);
            #endregion

            return Ret;
        }

        protected void UpdateClocks()
        {
            Times.ForEach(e => {
                DateTime Now = TimeZoneInfo.ConvertTime(DateTime.Now, ViewModel.TimezonesAvailable[(string)e.Tag]);
                e.Text = Now.ToString(ViewModel.TimeFormat);
            });
            Dates.ForEach(e => {
                DateTime Now = TimeZoneInfo.ConvertTime(DateTime.Now, ViewModel.TimezonesAvailable[(string)e.Tag]);
                e.Text = Now.ToString(ViewModel.DateFormat);
            });
        }

        protected void UpdateConversions()
        {

        }
        #endregion
    }
}
