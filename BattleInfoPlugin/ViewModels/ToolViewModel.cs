using System;
using System.Linq;
using settings=BattleInfoPlugin.Properties.Settings;
using BattleInfoPlugin.Models;
using BattleInfoPlugin.Models.Notifiers;
using Livet;
using Livet.EventListeners;
using Livet.Messaging;

namespace BattleInfoPlugin.ViewModels
{
	public class ToolViewModel : ViewModel
	{
		private readonly BattleEndNotifier notifier;

		private BattleData BattleData { get; } = new BattleData();

		public string BattleName
			=> this.BattleData?.BattleName ?? "";

		public string UpdatedTime
			=> this.BattleData != null && this.BattleData.UpdatedTime != default(DateTimeOffset)
				? this.BattleData.UpdatedTime.ToString("HH:mm:ss") // yyyy/MM/dd
				: "No Data";

		public string BattleSituation
			=> this.BattleData != null && this.BattleData.BattleSituation != Models.BattleSituation.없음
				? this.BattleData.BattleSituation.ToString()
				: "";

		public string FriendAirSupremacy
			=> this.BattleData != null && this.BattleData.FriendAirSupremacy != AirSupremacy.항공전없음
				? this.BattleData.FriendAirSupremacy.ToString()
				: "";

		public CellData[] Cells => this.BattleData?.Cells?.ToArray();
		public string CurrentMap => this.BattleData?.CurrentMap ?? "";

		public string RankResult
			=> this.BattleData.RankResult.ToString();

		public string AirRankResult
			=> this.BattleData.AirRankResult.ToString();

		public bool AirRankAvailable
			=> this.BattleData.RankResult == Rank.공습전;

		public string DropShipName
			=> this.BattleData?.DropShipName;

		public AirCombatResult[] AirCombatResults
			=> this.BattleData?.AirCombatResults ?? new AirCombatResult[0];

		public string FlareUsed
			=> (this.BattleData?.FlareUsed ?? UsedFlag.Unset).ToReadableString();

		public string NightReconScouted
			=> (this.BattleData?.NightReconScouted ?? UsedFlag.Unset).ToReadableString();

		public string AntiAirFired
			=> (this.BattleData?.AntiAirFired ?? AirFireFlag.Unset).ToReadableString();

        public string AntiAirFiredDetail
            => this.BattleData?.AntiAirFiredDetail ?? "";

		public string SupportUsed
			=> (this.BattleData?.SupportUsed ?? UsedSupport.Unset).ToReadableString();


		public bool MechanismOn
			=> this.BattleData?.MechanismOn ?? false;

        public bool MapExtended
            => this.BattleData?.MapExtended ?? false;

        #region FirstFleet変更通知プロパティ
        private FleetViewModel _FirstFleet;
		public FleetViewModel FirstFleet
		{
			get { return this._FirstFleet; }
			set
			{
				if (this._FirstFleet != value)
				{
					this._FirstFleet = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region SecondFleet変更通知プロパティ
		private FleetViewModel _SecondFleet;
		public FleetViewModel SecondFleet
		{
			get { return this._SecondFleet; }
			set
			{
				if (this._SecondFleet != value)
				{
					this._SecondFleet = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region SecondEnemies変更通知プロパティ
		private FleetViewModel _SecondEnemies;
		public FleetViewModel SecondEnemies
		{
			get { return this._SecondEnemies; }
			set
			{
				if (this._SecondEnemies != value)
				{
					this._SecondEnemies = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region Enemies変更通知プロパティ
		private FleetViewModel _Enemies;
		public FleetViewModel Enemies
		{
			get { return this._Enemies; }
			set
			{
				if (this._Enemies != value)
				{
					this._Enemies = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion


		#region IsCriticalNotiEnabled
		// ここ以外で変更しないのでModel変更通知は受け取らない雑対応
		public bool IsCriticalNotiEnabled
		{
			get { return this.notifier.CriticalEnabled; }
			set
			{
				if (this.notifier.CriticalEnabled != value)
				{
					this.notifier.CriticalEnabled = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region IsCriticalAlways
		// ここ以外で変更しないのでModel変更通知は受け取らない雑対応
		public bool IsCriticalAlways
		{
			get { return settings.Default.CriticalAlways; }
			set
			{
				if (settings.Default.CriticalAlways != value)
				{
					settings.Default.CriticalAlways = value;
					settings.Default.Save();
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region IsNotifierEnabled変更通知プロパティ
		// ここ以外で変更しないのでModel変更通知は受け取らない雑対応
		public bool IsNotifierEnabled
		{
			get { return this.notifier.IsEnabled; }
			set
			{
				if (this.notifier.IsEnabled != value)
				{
					this.notifier.IsEnabled = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region IsPursuitEnabled変更通知プロパティ
		public bool IsPursuitEnabled
		{
			get { return this.notifier.IsEnabled; }
			set
			{
				if (this.notifier.IsEnabled != value)
				{
					this.notifier.IsEnabled = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region EnableColorChange
		public bool EnableColorChange
		{
			get { return settings.Default.EnableColorChange; }
			set
			{
				if (settings.Default.EnableColorChange != value)
				{
					settings.Default.EnableColorChange = value;
					settings.Default.Save();
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region UseBrowserOverlay
		public bool UseBrowserOverlay
		{
			get { return settings.Default.UseBrowserOverlay; }
			set
			{
				if (settings.Default.UseBrowserOverlay != value)
				{
					settings.Default.UseBrowserOverlay = value;
					settings.Default.Save();
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region DetailKouku
		public bool DetailKouku
		{
			get { return settings.Default.DetailKouku; }
			set
			{
				if (settings.Default.DetailKouku != value)
				{
					settings.Default.DetailKouku = value;
					settings.Default.Save();
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region AutoSelectTab
		public bool AutoSelectTab
		{
			get { return settings.Default.AutoSelectTab; }
			set
			{
				if (settings.Default.AutoSelectTab != value)
				{
					settings.Default.AutoSelectTab = value;
					settings.Default.Save();
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region AutoBackTab
		public bool AutoBackTab
		{
			get { return settings.Default.AutoBackTab; }
			set
			{
				if (settings.Default.AutoBackTab != value)
				{
					settings.Default.AutoBackTab = value;
					settings.Default.Save();
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region IsNotifyOnlyWhenInactive変更通知プロパティ
		// ここ以外で変更しないのでModel変更通知は受け取らない雑対応
		public bool IsNotifyOnlyWhenInactive
		{
			get { return this.notifier.IsNotifyOnlyWhenInactive; }
			set
			{
				if (this.notifier.IsNotifyOnlyWhenInactive != value)
				{
					this.notifier.IsNotifyOnlyWhenInactive = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion


		public ToolViewModel(Plugin plugin)
		{
			this.notifier = new BattleEndNotifier(plugin);
			this._FirstFleet = new FleetViewModel("기본함대");
			this._SecondFleet = new FleetViewModel("호위함대");
			this._SecondEnemies = new FleetViewModel("적호위함대");
			this._Enemies = new FleetViewModel("적함대");

            this.CompositeDisposable.Add(new PropertyChangedEventListener(this.BattleData)
            {
                {
                    nameof(this.BattleData.Cells),
                    (_, __) => this.RaisePropertyChanged(() => this.Cells)
                },
                {
                    nameof(this.BattleData.CurrentMap),
                    (_, __) => this.RaisePropertyChanged(() => this.CurrentMap)
                },
                {
                    nameof(this.BattleData.BattleName),
                    (_, __) => this.RaisePropertyChanged(nameof(this.BattleName))
                },
                {
                    nameof(this.BattleData.UpdatedTime),
                    (_, __) => this.RaisePropertyChanged(() => this.UpdatedTime)
                },
                {
                    nameof(this.BattleData.BattleSituation),
                    (_, __) => this.RaisePropertyChanged(() => this.BattleSituation)
                },
                {
                    nameof(this.BattleData.FriendAirSupremacy),
                    (_, __) => this.RaisePropertyChanged(() => this.FriendAirSupremacy)
                },
                {
                    nameof(this.BattleData.AirCombatResults),
                    (_, __) =>
                    {
                        this.RaisePropertyChanged(() => this.AirCombatResults);
                        this.FirstFleet.AirCombatResults = this.AirCombatResults.Select(x => new AirCombatResultViewModel(x, FleetType.First)).ToArray();
                        this.SecondFleet.AirCombatResults = this.AirCombatResults.Select(x => new AirCombatResultViewModel(x, FleetType.Second)).ToArray();
                        this.SecondEnemies.AirCombatResults = this.AirCombatResults.Select(x => new AirCombatResultViewModel(x, FleetType.SecondEnemy)).ToArray();
                        this.Enemies.AirCombatResults = this.AirCombatResults.Select(x => new AirCombatResultViewModel(x, FleetType.Enemy)).ToArray();
                    }
                },
                {
                    nameof(this.BattleData.DropShipName),
                    (_, __) => this.RaisePropertyChanged(() => this.DropShipName)
                },
                {
                    nameof(this.BattleData.FirstFleet),
                    (_, __) => this.FirstFleet.Fleet = this.BattleData.FirstFleet
                },
                {
                    nameof(this.BattleData.SecondFleet),
                    (_, __) => this.SecondFleet.Fleet = this.BattleData.SecondFleet
                },
                {
                    nameof(this.BattleData.SecondEnemies),
                    (_, __) => this.SecondEnemies.Fleet = this.BattleData.SecondEnemies
                },
                {
                    nameof(this.BattleData.Enemies),
                    (_, __) => this.Enemies.Fleet = this.BattleData.Enemies
                },
                {
                    nameof(this.BattleData.RankResult),
                    (_, __) => {
                        this.RaisePropertyChanged(nameof(this.RankResult));
                        this.RaisePropertyChanged(nameof(this.AirRankAvailable));
                    }
                },
                {
                    nameof(this.BattleData.AirRankResult),
                    (_, __) => this.RaisePropertyChanged(nameof(this.AirRankResult))
                },
                {
                    nameof(this.BattleData.FlareUsed),
                    (_, __) => this.RaisePropertyChanged(nameof(this.FlareUsed))
                },
                {
                    nameof(this.BattleData.NightReconScouted),
                    (_, __) => this.RaisePropertyChanged(nameof(this.NightReconScouted))
                },
                {
                    nameof(this.BattleData.AntiAirFired),
                    (_, __) => this.RaisePropertyChanged(nameof(this.AntiAirFired))
                },
                {
                    nameof(this.BattleData.SupportUsed),
                    (_, __) => this.RaisePropertyChanged(nameof(this.SupportUsed))
                },
                {
                    nameof(this.BattleData.MechanismOn),
                    (_, __) => this.RaisePropertyChanged(nameof(this.MechanismOn))
                },
                {
                    nameof(this.BattleData.MapExtended),
                    (_, __) => this.RaisePropertyChanged(nameof(this.MapExtended))
                }
            });
		}

		public void OpenEnemyWindow()
		{
			var message = new TransitionMessage("Show/EnemyWindow")
			{
				TransitionViewModel = new EnemyWindowViewModel()
			};
			this.Messenger.RaiseAsync(message);
		}
	}
}
