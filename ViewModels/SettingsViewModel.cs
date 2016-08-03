using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatTimeIsIt.ViewModels
{
    public class SettingsViewModel : BaseNotify
    {
        #region Constants
        public const string SETTINGS_FOLDER = "WhatTimeIsIt";
        public const string SETTINGS_FILE = "WhatTimeIsIt.jset";

        public const string DATE_FORMAT = "M/d/yyyy";
        public const string TIME_FORMAT = "h:mm tt";
        #endregion

        #region Fields
        protected string _DateFormat = DATE_FORMAT;
        protected Dictionary<string, string> _DateOptions = null;
        protected string _TimeFormat = TIME_FORMAT;
        protected Dictionary<string, string> _TimeOptions = null;

        protected ObservableCollection<string> _Clocks = new ObservableCollection<string>();
        protected ObservableCollection<string> _TimeConversions = new ObservableCollection<string>();
        #endregion

        #region Properties
        public static SettingsViewModel instance { get; private set; }

        public string DateFormat
        {
            get
            {
                return _DateFormat;
            }
            set
            {
                _DateFormat = string.IsNullOrWhiteSpace(value) ? DATE_FORMAT : value;
                OnPropertyChanged();
            }
        }

        public Dictionary<string, string> DateOptions
        {
            get
            {
                if (_DateOptions == null || _DateOptions.Count == 0)
                {
                    DateOptions = new Dictionary<string, string>()
                    {
                        { DATE_FORMAT ,"1/25/2016" },
                        { "yyyy-MM-dd" ,"2016-01-25" },
                        { "d/m/yyyy", "25/1/2016" }
                    };
                }
                return _DateOptions;
            }
            set
            {
                _DateOptions = value;
                OnPropertyChanged();
            }
        }

        public string TimeFormat
        {
            get
            {
                return _TimeFormat;
            }
            set
            {
                _TimeFormat = string.IsNullOrWhiteSpace(value) ? TIME_FORMAT : value;
                OnPropertyChanged();
            }
        }

        public Dictionary<string, string> TimeOptions
        {
            get
            {
                if (_TimeOptions == null || _TimeOptions.Count == 0)
                {
                    TimeOptions = new Dictionary<string, string>()
                    {
                        { TIME_FORMAT, "1:05 PM" },
                        { "HH:mm", "13:05" }
                    };
                }
                return _TimeOptions;
            }
            set
            {
                _TimeOptions = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Clocks
        {
            get
            {
                return _Clocks;
            }
            set
            {
                _Clocks = value ?? new ObservableCollection<string>();
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> TimeConversions
        {
            get
            {
                return _TimeConversions;
            }
            set
            {
                _TimeConversions = value ?? new ObservableCollection<string>();
                OnPropertyChanged();
            }
        }
        #endregion

        #region Construct / Destruct
        public SettingsViewModel()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                throw new Exception("Settings already defined, this shouldnt happen.");
            }
        }
        #endregion

        #region Methods
        public static SettingsViewModel Load()
        {
            string SettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), SETTINGS_FOLDER, SETTINGS_FILE);
            try
            {
                if (File.Exists(SettingsPath))
                {
                    return JsonConvert.DeserializeObject<SettingsViewModel>(File.ReadAllText(SettingsPath));
                }
            }
            catch //(Exception ex)
            {
                // TODO: Handle error
            }
            return new SettingsViewModel();
        }

        protected void Save()
        {
            string SettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), SETTINGS_FOLDER);
            try
            {
                if (!Directory.Exists(SettingsPath))
                {
                    Directory.CreateDirectory(SettingsPath);
                }

                SettingsPath = Path.Combine(SettingsPath, SETTINGS_FILE);
                File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(this, Formatting.Indented));
            }
            catch //(Exception ex)
            {
                // TODO: Handle error
            }
        }
        #endregion
    }
}
