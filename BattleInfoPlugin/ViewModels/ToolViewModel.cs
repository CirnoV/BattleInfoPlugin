using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using settings = BattleInfoPlugin.Properties.Settings;
using BattleInfoPlugin.Models;
using BattleInfoPlugin.Models.Notifiers;
using MetroTrilithon.Mvvm;
using Livet;
using Livet.EventListeners;
using Livet.Messaging;
using System.Windows.Input;

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

		public SortieInfo CurrentSortie => this.BattleData?.CurrentSortie;
		public BattleFlags CurrentBattleFlag => this.BattleData?.CurrentBattleFlag;

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

		#region AliasFirst変更通知プロパティ
		private FleetViewModel _AliasFirst;
		public FleetViewModel AliasFirst
		{
			get { return this._AliasFirst; }
			set
			{
				if (this._AliasFirst != value)
				{
					this._AliasFirst = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region AliasSecond変更通知プロパティ
		private FleetViewModel _AliasSecond;
		public FleetViewModel AliasSecond
		{
			get { return this._AliasSecond; }
			set
			{
				if (this._AliasSecond != value)
				{
					this._AliasSecond = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region EnemySecond変更通知プロパティ
		private FleetViewModel _EnemySecond;
		public FleetViewModel EnemySecond
		{
			get { return this._EnemySecond; }
			set
			{
				if (this._EnemySecond != value)
				{
					this._EnemySecond = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region EnemyFirst変更通知プロパティ
		private FleetViewModel _EnemyFirst;
		public FleetViewModel EnemyFirst
		{
			get { return this._EnemyFirst; }
			set
			{
				if (this._EnemyFirst != value)
				{
					this._EnemyFirst = value;
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

		#region DisplayAdditionalName
		public bool DisplayAdditionalName
		{
			get { return settings.Default.DisplayAdditionalName; }
			set
			{
				if (settings.Default.DisplayAdditionalName != value)
				{
					settings.Default.DisplayAdditionalName = value;
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

		#region CriticalActionList, CriticalAction property
		public IReadOnlyCollection<DisplayViewModel<CriticalActionType>> CriticalActionList { get; }
			= new DisplayViewModel<CriticalActionType>[]
			{
				DisplayViewModel.Create(CriticalActionType.Notify, "알림"),
				DisplayViewModel.Create(CriticalActionType.None, "무시"),
				DisplayViewModel.Create(CriticalActionType.ForceRefresh, "새로고침")
			};

		public CriticalActionType CriticalAction
		{
			get
			{
				return CriticalActionTypeExtension.Parse(settings.Default.CriticalAction);
			}
			set
			{
				if (value != this.CriticalAction)
				{
					BattleInfoPlugin.Properties.Settings.Default.CriticalAction = value.ToString();
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		public ToolViewModel(Plugin plugin)
		{
			this.notifier = new BattleEndNotifier(plugin);
			this._AliasFirst = new FleetViewModel("기본함대");
			this._AliasSecond = new FleetViewModel("호위함대");
			this._EnemySecond = new FleetViewModel("적호위함대");
			this._EnemyFirst = new FleetViewModel("적함대");

			this.CompositeDisposable.Add(new PropertyChangedEventListener(this.BattleData)
			{
				{
					nameof(this.BattleData.CurrentSortie),
					(_, __) => this.RaisePropertyChanged(() => this.CurrentSortie)
				},
				{
					nameof(this.BattleData.CurrentBattleFlag),
					(_, __) => this.RaisePropertyChanged(() => this.CurrentBattleFlag)
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
						this.AliasFirst.AirCombatResults = this.AirCombatResults.Select(x => new AirCombatResultViewModel(x, FleetType.AliasFirst)).ToArray();
						this.AliasSecond.AirCombatResults = this.AirCombatResults.Select(x => new AirCombatResultViewModel(x, FleetType.AliasSecond)).ToArray();
						this.EnemySecond.AirCombatResults = this.AirCombatResults.Select(x => new AirCombatResultViewModel(x, FleetType.EnemySecond)).ToArray();
						this.EnemyFirst.AirCombatResults = this.AirCombatResults.Select(x => new AirCombatResultViewModel(x, FleetType.EnemyFirst)).ToArray();
					}
				},
				{
					nameof(this.BattleData.DropShipName),
					(_, __) => this.RaisePropertyChanged(() => this.DropShipName)
				},
				{
					nameof(this.BattleData.AliasFirst),
					(_, __) => this.AliasFirst.Fleet = this.BattleData.AliasFirst
				},
				{
					nameof(this.BattleData.AliasSecond),
					(_, __) => this.AliasSecond.Fleet = this.BattleData.AliasSecond
				},
				{
					nameof(this.BattleData.EnemySecond),
					(_, __) => this.EnemySecond.Fleet = this.BattleData.EnemySecond
				},
				{
					nameof(this.BattleData.EnemyFirst),
					(_, __) => this.EnemyFirst.Fleet = this.BattleData.EnemyFirst
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
				}
			});
		}

		public void OpenHistoryWindow(BaseNodeData param)
		{
			var message = new TransitionMessage("Show/HistoryWindow")
			{
				TransitionViewModel = new HistoryWindowViewModel(param)
			};
			this.Messenger.RaiseAsync(message);
		}
		public void OpenHistoryWindow(SortieNodeData param) => this.OpenHistoryWindow(param as BaseNodeData);
		public void OpenHistoryWindow(PracticeNodeData param) => this.OpenHistoryWindow(param as BaseNodeData);
	}
}
