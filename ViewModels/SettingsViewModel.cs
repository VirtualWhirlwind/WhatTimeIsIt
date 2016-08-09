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
		protected ObservableCollection<string> _Conversions = new ObservableCollection<string>();

		protected ObservableCollection<TimeZoneInfo> _ClocksAvailable = new ObservableCollection<TimeZoneInfo>();
		protected ObservableCollection<TimeZoneInfo> _ConversionsAvailable = new ObservableCollection<TimeZoneInfo>();

		protected bool _ClockAddEnabled = false;
		protected bool _ClockRemoveEnabled = false;
		protected bool _ClockUpEnabled = false;
		protected bool _ClockDownEnabled = false;
		protected bool _ConversionAddEnabled = false;
		protected bool _ConversionRemoveEnabled = false;
		protected bool _ConversionUpEnabled = false;
		protected bool _ConversionDownEnabled = false;
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

		public ObservableCollection<string> Conversions
		{
			get
			{
				return _Conversions;
			}
			set
			{
				_Conversions = value ?? new ObservableCollection<string>();
				OnPropertyChanged();
			}
		}

		#region UI Bindings
		[JsonIgnore]
		public ReadOnlyCollection<TimeZoneInfo> TimezonesAvailable { get; set; }

		[JsonIgnore]
		public ObservableCollection<TimeZoneInfo> ClocksAvailable
		{
			get
			{
				return _ClocksAvailable;
			}
			set
			{
				if (_ClocksAvailable != value)
				{
					_ClocksAvailable = value ?? new ObservableCollection<TimeZoneInfo>();
					OnPropertyChanged();
				}
			}
		}

		[JsonIgnore]
		public ObservableCollection<TimeZoneInfo> ConversionsAvailable
		{
			get
			{
				return _ConversionsAvailable;
			}
			set
			{
				if (_ConversionsAvailable != value)
				{
					_ConversionsAvailable = value ?? new ObservableCollection<TimeZoneInfo>();
					OnPropertyChanged();
				}
			}
		}

		[JsonIgnore]
		public bool ClockAddEnabled
		{
			get
			{
				return _ClockAddEnabled;
			}
			set
			{
				if (_ClockAddEnabled != value)
				{
					_ClockAddEnabled = value;
					OnPropertyChanged();
				}
			}
		}

		[JsonIgnore]
		public bool ClockRemoveEnabled
		{
			get
			{
				return _ClockRemoveEnabled;
			}
			set
			{
				if (_ClockRemoveEnabled != value)
				{
					_ClockRemoveEnabled = value;
					OnPropertyChanged();
				}
			}
		}

		[JsonIgnore]
		public bool ClockUpEnabled
		{
			get
			{
				return _ClockUpEnabled;
			}
			set
			{
				if (_ClockUpEnabled != value)
				{
					_ClockUpEnabled = value;
					OnPropertyChanged();
				}
			}
		}

		[JsonIgnore]
		public bool ClockDownEnabled
		{
			get
			{
				return _ClockDownEnabled;
			}
			set
			{
				if (_ClockDownEnabled != value)
				{
					_ClockDownEnabled = value;
					OnPropertyChanged();
				}
			}
		}

		[JsonIgnore]
		public bool ConversionAddEnabled
		{
			get
			{
				return _ConversionAddEnabled;
			}
			set
			{
				if (_ConversionAddEnabled != value)
				{
					_ConversionAddEnabled = value;
					OnPropertyChanged();
				}
			}
		}

		[JsonIgnore]
		public bool ConversionRemoveEnabled
		{
			get
			{
				return _ConversionRemoveEnabled;
			}
			set
			{
				if (_ConversionRemoveEnabled != value)
				{
					_ConversionRemoveEnabled = value;
					OnPropertyChanged();
				}
			}
		}

		[JsonIgnore]
		public bool ConversionUpEnabled
		{
			get
			{
				return _ConversionUpEnabled;
			}
			set
			{
				if (_ConversionUpEnabled != value)
				{
					_ConversionUpEnabled = value;
					OnPropertyChanged();
				}
			}
		}

		[JsonIgnore]
		public bool ConversionDownEnabled
		{
			get
			{
				return _ConversionDownEnabled;
			}
			set
			{
				if (_ConversionDownEnabled != value)
				{
					_ConversionDownEnabled = value;
					OnPropertyChanged();
				}
			}
		}
		#endregion
		#endregion

		#region Construct / Destruct
		public SettingsViewModel()
		{
			TimezonesAvailable = TimeZoneInfo.GetSystemTimeZones();

			if (instance == null)
			{
				instance = this;
			}
		}
		#endregion

		#region Methods
		public void Setup()
		{
			ClocksAvailable.Clear();
			TimezonesAvailable.ToList().ForEach(e => { if (!Clocks.Contains(e.ToString())) { ClocksAvailable.Add(e); } });

			ConversionsAvailable.Clear();
			TimezonesAvailable.ToList().ForEach(e => { if (!Conversions.Contains(e.ToString())) { ConversionsAvailable.Add(e); } });
		}

		public SettingsViewModel Load()
		{
			string SettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), SETTINGS_FOLDER, SETTINGS_FILE);
			try
			{
				if (File.Exists(SettingsPath))
				{
					SettingsViewModel TmpData = JsonConvert.DeserializeObject<SettingsViewModel>(File.ReadAllText(SettingsPath));
                    this.DateFormat = TmpData.DateFormat;
                    this.DateOptions = TmpData.DateOptions;
                    this.TimeFormat = TmpData.TimeFormat;
                    this.TimeOptions = TmpData.TimeOptions;
                    this.Clocks = TmpData.Clocks;
                    this.Conversions = TmpData.Conversions;
				}
			}
			catch //(Exception ex)
			{
				// TODO: Handle error
			}

            return this;
		}

		public void Save()
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
