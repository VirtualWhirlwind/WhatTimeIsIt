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

        protected List<TextBlock> TimeBlocks { get; set; }

        protected List<TextBlock> DateBlocks { get; set; }

        protected List<TextBlock> ConversionBlocks { get; set; }
        #endregion

        #region Construct / Destruct
        public MainWindow() : this(viewModel: null)
        {
        }

        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();

            SetUp(viewModel);
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
                case "Conversions": CreateConversions(); UpdateConversions(); break;
                case "UsableEnteredDateTime": UpdateConversions(); break;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize everything
        /// </summary>
        /// <param name="viewModel"></param>
        protected void SetUp(MainWindowViewModel viewModel = null)
        {
            instance = this;

            ViewModel = viewModel ?? new MainWindowViewModel();

            // Initialize Properties
            TimeBlocks = new List<TextBlock>();
            DateBlocks = new List<TextBlock>();
            ConversionBlocks = new List<TextBlock>();
            UpdateDisplay = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 2) };

            // Configure Settings
            SettingsHolder.Navigate(ViewModel.SettingsPage);

            // Wire up events
            ViewModel.PropertyChanged += (o, e) => PropertyChanged(e.PropertyName);
            ViewModel.TriggerUpdate();

            UpdateDisplay.Tick += (o, e) => UpdateClocks();
            UpdateDisplay.Start();
        }

        /// <summary>
        /// Show or hide the settings view
        /// </summary>
        public void ToggleSettings()
        {
            ViewModel.IsSettingsVisible = !ViewModel.IsSettingsVisible;
            if (!ViewModel.IsSettingsVisible)
            {
                ViewModel.SettingsPage.ViewModel.Load();
                ViewModel.TriggerUpdate();
            }
        }

        #region Clocks / Top Section
        /// <summary>
        /// Dump and create the top section clocks
        /// </summary>
        protected void CreateClocks()
        {
            ClocksHolder.Children.Clear();
            TimeBlocks.Clear();
            DateBlocks.Clear();

            foreach (string OneClock in ViewModel.Clocks)
            {
                var TmpClock = CreateOneClock(OneClock);
                if (TmpClock != null) { ClocksHolder.Children.Add(TmpClock); }
            }
        }

        /// <summary>
        /// Creates one clock to be displayed in the top section
        /// </summary>
        /// <param name="timezone"></param>
        /// <returns></returns>
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
            TimeBlocks.Add(TmpText);
            #endregion

            #region Row 2
            TmpText = new TextBlock() { Text = "", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 0, 5), Tag = timezone };
            TmpText.SetValue(Grid.RowProperty, 2);
            Ret.Children.Add(TmpText);
            DateBlocks.Add(TmpText);
            #endregion

            return Ret;
        }

        /// <summary>
        /// Using data binding, update the text fields that can change on the clocks
        /// </summary>
        protected void UpdateClocks()
        {
            TimeBlocks.ForEach(e => {
                DateTime Now = TimeZoneInfo.ConvertTime(DateTime.Now, ViewModel.TimezonesAvailable[(string)e.Tag]);
                e.Text = Now.ToString(ViewModel.TimeFormat);
            });
            DateBlocks.ForEach(e => {
                DateTime Now = TimeZoneInfo.ConvertTime(DateTime.Now, ViewModel.TimezonesAvailable[(string)e.Tag]);
                e.Text = Now.ToString(ViewModel.DateFormat);
            });
        }
        #endregion

        #region Conversions / Bottom Section
        /// <summary>
        /// Dump and create the bottom section conversions
        /// </summary>
        protected void CreateConversions()
        {
            ConversionsHolder.Children.Clear();
            ConversionsHolder.RowDefinitions.Clear();
            ConversionBlocks.Clear();

            int RowCount = 0;
            foreach (string OneConversion in ViewModel.Conversions)
            {
                var TmpConversion = CreateOneConversion(OneConversion, RowCount);
                if (TmpConversion != null)
                {
                    ConversionsHolder.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                    TmpConversion.ForEach(e => ConversionsHolder.Children.Add(e));
                    RowCount++;
                }
            }
        }

        /// <summary>
        /// Create one conversion to be displayed in the bottom section
        /// </summary>
        /// <param name="timezone"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        protected List<UIElement> CreateOneConversion(string timezone, int rowCount)
        {
            if (!ViewModel.TimezonesAvailable.ContainsKey(timezone)) { return null; }

            List<UIElement> Ret = new List<UIElement>();

            TextBlock TmpText = new TextBlock() { Text = timezone, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Center };
            TmpText.SetValue(Grid.ColumnProperty, 0);
            TmpText.SetValue(Grid.RowProperty, rowCount);
            Ret.Add(TmpText);

            //TmpText = new TextBlock() { Text = "", FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "Resources/Fonts/#Orbitron"), FontWeight = FontWeights.Bold, Margin = new Thickness(5, 0, 0, 0), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Center, Tag = timezone };
            TmpText = new TextBlock() { Text = "", FontWeight = FontWeights.Bold, Margin = new Thickness(5, 0, 0, 0), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Center, Tag = timezone };
            TmpText.SetValue(Grid.ColumnProperty, 1);
            TmpText.SetValue(Grid.RowProperty, rowCount);
            Ret.Add(TmpText);
            ConversionBlocks.Add(TmpText);

            return Ret;
        }

        /// <summary>
        /// Using data binding, update the text fields that can change on the conversions
        /// </summary>
        protected void UpdateConversions()
        {
            ConversionBlocks.ForEach(e => {
                if (ViewModel.UsableEnteredDateTime.HasValue)
                {
                    DateTime Now = TimeZoneInfo.ConvertTime(ViewModel.UsableEnteredDateTime.Value, ViewModel.TimezonesAvailable[(string)e.Tag]);
                    e.Text = Now.ToString(ViewModel.DateFormat + " " + ViewModel.TimeFormat);
                }
                else
                {
                    e.Text = "";
                }
            });
        }
        #endregion
        #endregion
    }
}
