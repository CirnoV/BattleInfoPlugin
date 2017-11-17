using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using BattleInfoPlugin.Models.Raw;
using BattleInfoPlugin.Models.Repositories;
using Grabacr07.KanColleWrapper;
using Livet;
using Grabacr07.KanColleWrapper.Models;
using BattleInfoPlugin.Properties;
using System.Text;
using System.Diagnostics;
using System.Windows;
using kcsapi_port = BattleInfoPlugin.Models.Raw.kcsapi_port;

#region Alias
using practice_battle = BattleInfoPlugin.Models.Raw.sortie_battle;
using battle_midnight_sp_midnight = BattleInfoPlugin.Models.Raw.battle_midnight_battle;
using practice_midnight_battle = BattleInfoPlugin.Models.Raw.battle_midnight_battle;
using sortie_ld_airbattle = BattleInfoPlugin.Models.Raw.sortie_airbattle;
using combined_battle_battle_water = BattleInfoPlugin.Models.Raw.combined_battle_battle;
using combined_battle_ld_airbattle = BattleInfoPlugin.Models.Raw.combined_battle_airbattle;
using combined_battle_sp_midnight = BattleInfoPlugin.Models.Raw.combined_battle_midnight_battle;
#endregion

namespace BattleInfoPlugin.Models
{
	public class BattleData : NotificationObject
	{
		#region Properties

		#region Name変更通知プロパティ
		private string _Name;

		public string Name
		{
			get
			{ return this._Name; }
			set
			{ 
				if (this._Name == value)
					return;
				this._Name = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region UpdatedTime変更通知プロパティ
		private DateTimeOffset _UpdatedTime;

		public DateTimeOffset UpdatedTime
		{
			get
			{ return this._UpdatedTime; }
			set
			{ 
				if (this._UpdatedTime == value)
					return;
				this._UpdatedTime = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region CellEvent変更通知プロパティ
		private int _CellEvent;
		public int CellEvent
		{
			get { return this._CellEvent; }
			set
			{
				if (this._CellEvent != value)
				{
					this._CellEvent = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region Cells変更通知プロパティ
		private List<CellData> _Cells;
		public List<CellData> Cells
		{
			get { return this._Cells; }
			set
			{
				if (this._Cells != value)
				{
					this._Cells = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region CurrentMap変更通知プロパティ
		private string _CurrentMap;
		public string CurrentMap
		{
			get { return this._CurrentMap; }
			set
			{
				if (this._CurrentMap != value)
				{
					this._CurrentMap = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region BattleSituation変更通知プロパティ

		private BattleSituation _BattleSituation;

		public BattleSituation BattleSituation
		{
			get
			{ return this._BattleSituation; }
			set
			{
				if (this._BattleSituation == value)
					return;
				this._BattleSituation = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region FirstFleet変更通知プロパティ
		private FleetData _FirstFleet;

		public FleetData FirstFleet
		{
			get
			{ return this._FirstFleet; }
			set
			{ 
				if (this._FirstFleet == value)
					return;
				this._FirstFleet = value;
				this._FirstFleet.FleetType = FleetType.First;
				Settings.Default.FirstIsCritical = this._FirstFleet.CriticalCheck();
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region SecondFleet変更通知プロパティ
		private FleetData _SecondFleet;

		public FleetData SecondFleet
		{
			get
			{ return this._SecondFleet; }
			set
			{ 
				if (this._SecondFleet == value)
					return;
				this._SecondFleet = value;
				this._SecondFleet.FleetType = FleetType.Second;
				Settings.Default.SecondIsCritical = this._SecondFleet.CriticalCheck();
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region Enemies変更通知プロパティ
		private FleetData _Enemies;

		public FleetData Enemies
		{
			get
			{ return this._Enemies; }
			set
			{ 
				if (this._Enemies == value)
					return;
				this._Enemies = value;
				this._Enemies.FleetType = FleetType.Enemy;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region SecondEnemies変更通知プロパティ
		private FleetData _SecondEnemies;

		public FleetData SecondEnemies
		{
			get
			{ return this._SecondEnemies; }
			set
			{
				if (this._SecondEnemies == value)
					return;
				this._SecondEnemies = value;
				this._SecondEnemies.FleetType = FleetType.SecondEnemy;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region RankResult変更通知プロパティ
		private Rank _RankResult;

		public Rank RankResult
		{
			get
			{ return this._RankResult; }
			set
			{
				if (this._RankResult == value)
					return;
				this._RankResult = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region AirRankResult変更通知プロパティ
		private Rank _AirRankResult;

		public Rank AirRankResult
		{
			get
			{ return this._AirRankResult; }
			set
			{
				if (this._AirRankResult == value) return;
				this._AirRankResult = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region FriendAirSupremacy変更通知プロパティ
		private AirSupremacy _FriendAirSupremacy = AirSupremacy.항공전없음;

		public AirSupremacy FriendAirSupremacy
		{
			get
			{ return this._FriendAirSupremacy; }
			set
			{ 
				if (this._FriendAirSupremacy == value)
					return;
				this._FriendAirSupremacy = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region AirCombatResults変更通知プロパティ
		private AirCombatResult[] _AirCombatResults = new AirCombatResult[0];

		public AirCombatResult[] AirCombatResults
		{
			get
			{ return this._AirCombatResults; }
			set
			{
				if (this._AirCombatResults.Equals(value))
					return;
				this._AirCombatResults = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region DropShipName変更通知プロパティ
		private string _DropShipName;

		public string DropShipName
		{
			get
			{ return this._DropShipName; }
			set
			{
				if (this._DropShipName == value)
					return;
				this._DropShipName = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region FlareUsed変更通知プロパティ
		private UsedFlag _FlareUsed;
		public UsedFlag FlareUsed
		{
			get { return this._FlareUsed; }
			set
			{
				if (this._FlareUsed != value)
				{
					this._FlareUsed = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region NightReconScouted変更通知プロパティ
		private UsedFlag _NightReconScouted;
		public UsedFlag NightReconScouted
		{
			get { return this._NightReconScouted; }
			set
			{
				if (this._NightReconScouted != value)
				{
					this._NightReconScouted = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region AntiAirFired変更通知プロパティ
		private AirFireFlag _AntiAirFired;
		public AirFireFlag AntiAirFired
		{
			get { return this._AntiAirFired; }
			set
			{
				if (this._AntiAirFired != value)
				{
					this._AntiAirFired = value;
					this.RaisePropertyChanged();
				}
			}
		}

		private string _AntiAirFiredDetail;
		public string AntiAirFiredDetail
		{
			get { return this._AntiAirFiredDetail; }
			set
			{
				if (this._AntiAirFiredDetail != value)
				{
					this._AntiAirFiredDetail = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region SupportUsed変更通知プロパティ
		private UsedSupport _SupportUsed;
		public UsedSupport SupportUsed
		{
			get { return this._SupportUsed; }
			set
			{
				if (this._SupportUsed != value)
				{
					this._SupportUsed = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region MechanismOn変更通知プロパティ
		private bool _MechanismOn;
		public bool MechanismOn
		{
			get { return this._MechanismOn; }
			set
			{
				if (this._MechanismOn != value)
				{
					this._MechanismOn = value;
					this.RaisePropertyChanged();
				}
			}
		}
        #endregion

        #region MapExtended変更通知プロパティ
        private bool _MapExtended;
        public bool MapExtended
        {
            get { return this._MapExtended; }
            set
            {
                if (this._MapExtended != value)
                {
                    this._MechanismOn = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion

        #endregion

        /// <summary>
        /// MVP 추적기
        /// </summary>
        private MVPOracle mvpOracle { get; set; }

		private int CurrentDeckId { get; set; }
		private bool IsInSortie = false;

		public BattleData()
		{
			this.Cells = new List<CellData>();
			this.mvpOracle = new MVPOracle();

			var proxy = KanColleClient.Current.Proxy;

			#region Start / Next / Port
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_map/start")
				.Subscribe(x => this.ProcessStartNext(x));

			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_map/next")
				.Subscribe(x => this.ProcessStartNext(x, true));

			proxy.api_port.TryParse<kcsapi_port>().Subscribe(x => this.ResultClear(x.Data));
			#endregion

			#region 통상 - 주간전 / 연습 - 주간전
			proxy.api_req_sortie_battle
				.TryParse<sortie_battle>().Subscribe(x => this.Update(x.Data, ApiTypes.sortie_battle));

			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_practice/battle")
				.TryParse<practice_battle>().Subscribe(x => this.Update(x.Data, ApiTypes.practice_battle));
			#endregion

			#region 통상 - 야전 / 통상 - 개막야전 / 연습 - 야전
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_battle_midnight/battle")
				.TryParse<battle_midnight_battle>().Subscribe(x => this.Update(x.Data, ApiTypes.battle_midnight_battle));

			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_battle_midnight/sp_midnight")
				.TryParse<battle_midnight_sp_midnight>().Subscribe(x => this.Update(x.Data, ApiTypes.battle_midnight_sp_midnight));

			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_practice/midnight_battle")
				.TryParse<practice_midnight_battle>().Subscribe(x => this.Update(x.Data, ApiTypes.practice_midnight_battle));
			#endregion

			#region 항공전 - 주간전 / 공습전 - 주간전
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_sortie/airbattle")
				.TryParse<sortie_airbattle>().Subscribe(x => this.Update(x.Data, ApiTypes.sortie_airbattle));

			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_sortie/ld_airbattle")
				.TryParse<sortie_ld_airbattle>().Subscribe(x => this.Update(x.Data, ApiTypes.sortie_ld_airbattle));
			#endregion

			#region 연합함대 - 주간전
			proxy.api_req_combined_battle_battle
				.TryParse<combined_battle_battle>().Subscribe(x => this.Update(x.Data, ApiTypes.combined_battle_battle));

			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/battle_water")
				.TryParse<combined_battle_battle_water>().Subscribe(x => this.Update(x.Data, ApiTypes.combined_battle_battle_water));
			#endregion

			#region 연합vs연합 - 주간전
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/ec_battle")
				.TryParse<combined_battle_each_battle>().Subscribe(x => this.Update(x.Data, ApiTypes.combined_battle_ec_battle));

			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/each_battle")
				.TryParse<combined_battle_each_battle>().Subscribe(x => this.Update(x.Data, ApiTypes.combined_battle_each_battle));

			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/each_battle_water")
				.TryParse<combined_battle_each_battle>().Subscribe(x => this.Update(x.Data, ApiTypes.combined_battle_each_battle_water));
			#endregion

			#region 연합함대 - 항공전 / 공습전
			proxy.api_req_combined_battle_airbattle
				.TryParse<combined_battle_airbattle>().Subscribe(x => this.Update(x.Data, ApiTypes.combined_battle_airbattle));

			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/ld_airbattle")
				.TryParse<combined_battle_ld_airbattle>().Subscribe(x => this.Update(x.Data, ApiTypes.combined_battle_ld_airbattle));
			#endregion

			#region 연합함대 - 야전
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/midnight_battle")
				.TryParse<combined_battle_midnight_battle>().Subscribe(x => this.Update(x.Data, ApiTypes.combined_battle_midnight_battle));

			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/sp_midnight")
				.TryParse<combined_battle_sp_midnight>().Subscribe(x => this.Update(x.Data, ApiTypes.combined_battle_sp_midnight));
			#endregion

			#region 연합vs연합 - 야전
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/ec_midnight_battle")
				.TryParse<combined_battle_ec_midnight_battle>().Subscribe(x => this.Update(x.Data, ApiTypes.combined_battle_ec_midnight_battle));
			#endregion

			#region 결과
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_practice/battle_result")
				.TryParse<battle_result>().Subscribe(x => this.Update(x.Data));

			proxy.api_req_sortie_battleresult
				.TryParse<battle_result>().Subscribe(x => this.Update(x.Data));

			proxy.api_req_combined_battle_battleresult
				.TryParse<battle_result>().Subscribe(x => this.Update(x.Data));
			#endregion
		}

		private void AutoSelectTab()
		{
			if (Settings.Default.AutoSelectTab)
			{
				var info = Grabacr07.KanColleViewer.WindowService.Current.Information;

				info.SelectedItem.IsSelected = false;
				info.Tools.IsSelected = true;

				info.SelectedItem = info.Tools;

				info.Tools.SelectedTool = info.Tools.Tools
					.FirstOrDefault(x => x.Name == "BattleInfo");
			}
		}
		private void AutoBackTab()
		{
			if (Settings.Default.AutoBackTab)
			{
				var info = Grabacr07.KanColleViewer.WindowService.Current.Information;

				info.SelectedItem.IsSelected = false;
				info.Overview.IsSelected = true;

				info.SelectedItem = info.Overview;
			}
		}

		#region Update From Battle SvData

		private void Update(sortie_battle data, ApiTypes apiType)
		{
			AutoSelectTab();

			switch (apiType)
			{
				case ApiTypes.sortie_battle:
					this.Name = "통상 - 주간전";
					break;
				case ApiTypes.practice_battle:
					this.Name = "연습 - 주간전";

					this.Clear();

					this.CellEvent = (int)CellType.연습전;
					this.Cells.Clear();

					this.Cells.ForEach(x => x.IsOld = true);
					this.Cells.Add(new CellData
					{
						CellName = "",
						CellEvent = this.CellEvent.ToString(),
						CellText = "연습전",
						IsOld = false
					});
					this.RaisePropertyChanged(nameof(this.Cells));

					this.CurrentMap = "";
					break;
			}

			this.UpdateFleets(data.api_deck_id, data, data.api_formation);
			this.UpdateMaxHP(data.api_f_maxhps, data.api_e_maxhps);
			this.UpdateNowHP(data.api_f_nowhps, data.api_e_nowhps);

			this.UpdateUsedFlag(data.api_kouku?.api_stage2?.api_air_fire);
			this.UpdateUsedFlag(data.api_support_info);

			mvpOracle.Initialize(this.FirstFleet, this.SecondFleet).Update(data);
			UpdateMVP(mvpOracle.MVP1, mvpOracle.MVP2);

			if (apiType == ApiTypes.practice_battle)
			{
				this.FirstFleet.CalcPracticeDamages(
					data.api_injection_kouku.GetFirstFleetDamages(),		// Fleet Jet
					data.api_kouku.GetFirstFleetDamages(),				  // Fleet Airstrike
					data.api_opening_taisen.GetFriendDamages(),			 // Opening ASW
					data.api_opening_atack.GetFriendDamages(),			  // Opening Torpedo
					data.api_hougeki1.GetFriendDamages(),				   // Duel 1
					data.api_hougeki2.GetFriendDamages(),				   // Duel 2
					data.api_raigeki.GetFriendDamages()					 // Torpedo
				);
				this.Enemies.CalcPracticeDamages(
					data.api_air_base_injection.GetEnemyDamages(),		  // AirBase Jet
					data.api_injection_kouku.GetEnemyDamages(),			 // Fleet Jet
					data.api_air_base_attack.GetEachFirstEnemyDamages(),	// AirBase Airstrike
					data.api_support_info.GetEnemyDamages(),				// Support-fleet Firestrike
					data.api_kouku.GetEnemyDamages(),					   // Fleet Airstrike
					data.api_opening_taisen.GetEnemyDamages(),			  // Opening ASW
					data.api_opening_atack.GetEnemyDamages(),			   // Opening Torpedo
					data.api_hougeki1.GetEnemyDamages(),					// Duel 1
					data.api_hougeki2.GetEnemyDamages(),					// Duel 2
					data.api_raigeki.GetEnemyDamages()					  // Torpedo
				);
			}
			else
			{
				this.FirstFleet.CalcDamages(
					data.api_injection_kouku.GetFirstFleetDamages(),		// Fleet Jet
					data.api_kouku.GetFirstFleetDamages(),				  // Fleet Airstrike
					data.api_opening_taisen.GetFriendDamages(),			 // Opening ASW
					data.api_opening_atack.GetFriendDamages(),			  // Opening Torpedo
					data.api_hougeki1.GetFriendDamages(),				   // Duel 1
					data.api_hougeki2.GetFriendDamages(),				   // Duel 2
					data.api_raigeki.GetFriendDamages()					 // Torpedo
				);
				this.Enemies.CalcDamages(
					data.api_air_base_injection.GetEnemyDamages(),		  // AirBase Jet
					data.api_injection_kouku.GetEnemyDamages(),			 // Fleet Jet
					data.api_air_base_attack.GetEachFirstEnemyDamages(),	// AirBase Airstrike
					data.api_support_info.GetEnemyDamages(),				// Support-fleet Firestrike
					data.api_kouku.GetEnemyDamages(),					   // Fleet Airstrike
					data.api_opening_taisen.GetEnemyDamages(),			  // Opening ASW
					data.api_opening_atack.GetEnemyDamages(),			   // Opening Torpedo
					data.api_hougeki1.GetEnemyDamages(),					// Duel 1
					data.api_hougeki2.GetEnemyDamages(),					// Duel 2
					data.api_raigeki.GetEnemyDamages()					  // Torpedo
				);
			}

			this.FriendAirSupremacy = data.api_kouku.GetAirSupremacy();
			this.AirCombatResults = data.api_air_base_attack.ToResult().Concat(data.api_kouku.ToResult()).ToArray();
			this.RankResult = this.CalcRank();
		}

		private void Update(battle_midnight_battle data, ApiTypes apiType)
		{
			AutoSelectTab();

			switch (apiType)
			{
				case ApiTypes.battle_midnight_battle:
					this.Name = "통상 - 야전";
					break;
				case ApiTypes.battle_midnight_sp_midnight:
					this.Name = "통상 - 개막야전";
					break;
				case ApiTypes.practice_midnight_battle:
					this.Name = "연습 - 야전";
					break;
			}

			int BeforedayBattleHP = this.FirstFleet.Ships
				.Where(x => !x.Situation.HasFlag(ShipSituation.Tow) && !x.Situation.HasFlag(ShipSituation.Evacuation))
				.Sum(x => x.BeforeNowHP);//리스트 갱신하기전에 아군 HP최대값을 저장
			int EnemyBeforedayBattle = this.Enemies.Ships.Sum(x => x.BeforeNowHP);

			this.UpdateFleets(data.api_deck_id, data, data.api_formation);
            this.UpdateMaxHP(data.api_f_maxhps, data.api_e_maxhps);
            this.UpdateNowHP(data.api_f_nowhps, data.api_e_nowhps);

            this.UpdateUsedFlag(data.api_flare_pos, data.api_touch_plane);

			if (apiType == ApiTypes.battle_midnight_sp_midnight)
				mvpOracle.Initialize(this.FirstFleet, this.SecondFleet);

			mvpOracle.Update(data);
			UpdateMVP(mvpOracle.MVP1, mvpOracle.MVP2);

			if (apiType == ApiTypes.practice_battle)
			{
				this.FirstFleet.CalcPracticeDamages(data.api_hougeki.GetFriendDamages());
				this.Enemies.CalcPracticeDamages(data.api_hougeki.GetEnemyDamages());
			}
			else
			{
				this.FirstFleet.CalcDamages(data.api_hougeki.GetFriendDamages());
				this.Enemies.CalcDamages(data.api_hougeki.GetEnemyDamages());
			}

			if (apiType == ApiTypes.battle_midnight_sp_midnight)
				this.RankResult = this.CalcRank();
			else
				this.RankResult = this.CalcRank(false, false, true, BeforedayBattleHP, EnemyBeforedayBattle);
		}

		private void Update(sortie_airbattle data, ApiTypes apiType)
		{
			AutoSelectTab();

			switch (apiType)
			{
				case ApiTypes.sortie_airbattle:
					this.Name = "항공전 - 주간전";

					this.UpdateUsedFlag(
						data.api_kouku?.api_stage2?.api_air_fire,
						data.api_kouku2?.api_stage2?.api_air_fire
					);
					break;
				case ApiTypes.sortie_ld_airbattle:
					this.Name = "공습전 - 주간전";
					this.UpdateUsedFlag(data.api_kouku?.api_stage2?.api_air_fire);
					break;
			}

			this.UpdateFleets(data.api_deck_id, data, data.api_formation);
            this.UpdateMaxHP(data.api_f_maxhps, data.api_e_maxhps);
            this.UpdateNowHP(data.api_f_nowhps, data.api_e_nowhps);

            this.UpdateUsedFlag(data.api_support_info);

			mvpOracle.Initialize(this.FirstFleet, this.SecondFleet).Update(data);
			UpdateMVP(mvpOracle.MVP1, mvpOracle.MVP2);

			this.FirstFleet.CalcDamages(
				data.api_injection_kouku.GetFirstFleetDamages(),
				data.api_kouku.GetFirstFleetDamages(),
				data.api_kouku2.GetFirstFleetDamages()
			);
			this.Enemies.CalcDamages(
				data.api_air_base_injection.GetEnemyDamages(),
				data.api_injection_kouku.GetEnemyDamages(),
				data.api_air_base_attack.GetEachFirstEnemyDamages(),

				data.api_kouku.GetEnemyDamages(),
				data.api_kouku2.GetEnemyDamages()
			);

			this.FriendAirSupremacy = data.api_kouku.GetAirSupremacy();

			if (apiType == ApiTypes.sortie_airbattle)
			{
				this.AirCombatResults = data.api_air_base_attack.ToResult()
										.Concat(data.api_kouku.ToResult("1회차/"))
										.Concat(data.api_kouku2.ToResult("2회차/"))
										.ToArray();

				this.RankResult = this.CalcRank();
			}
			else
			{
				this.AirCombatResults = data.api_air_base_attack.ToResult().Concat(data.api_kouku.ToResult()).ToArray();

				this.AirRankResult = this.CalcLDAirRank();
				this.RankResult = Rank.공습전;
			}
		}

		private void Update(combined_battle_battle data, ApiTypes apiType)
		{
			AutoSelectTab();

			switch (apiType)
			{
				case ApiTypes.combined_battle_battle:
					this.Name = "연합함대 - 기동부대 - 주간전";
					break;
				case ApiTypes.combined_battle_battle_water:
					this.Name = "연합함대 - 수상부대 - 주간전";
					break;
			}

			this.UpdateFleets(data.api_deck_id, data, data.api_formation);
            this.UpdateMaxHP(data.api_f_maxhps, data.api_e_maxhps, data.api_f_maxhps_combined, data.api_e_maxhps_combined);
            this.UpdateNowHP(data.api_f_nowhps, data.api_e_nowhps, data.api_f_nowhps_combined, data.api_e_nowhps_combined);

            this.UpdateUsedFlag(data.api_kouku?.api_stage2?.api_air_fire);
			this.UpdateUsedFlag(data.api_support_info);

			mvpOracle.Initialize(this.FirstFleet, this.SecondFleet).Update(data, true);
			UpdateMVP(mvpOracle.MVP1, mvpOracle.MVP2);

			switch (apiType)
			{
				case ApiTypes.combined_battle_battle:
					this.FirstFleet.CalcDamages(
						data.api_injection_kouku.GetFirstFleetDamages(),
						data.api_kouku.GetFirstFleetDamages(),
						data.api_hougeki2.GetFriendDamages(),
						data.api_hougeki3.GetFriendDamages()
					);
					this.SecondFleet.CalcDamages(
						data.api_injection_kouku.GetSecondFleetDamages(),
						data.api_kouku.GetSecondFleetDamages(),
						data.api_opening_taisen.GetFriendDamages(),
						data.api_opening_atack.GetFriendDamages(),
						data.api_hougeki1.GetFriendDamages(),
						data.api_raigeki.GetFriendDamages()
					);
					break;

				case ApiTypes.combined_battle_battle_water:
					this.FirstFleet.CalcDamages(
						data.api_injection_kouku.GetFirstFleetDamages(),
						data.api_kouku.GetFirstFleetDamages(),
						data.api_hougeki1.GetFriendDamages(),
						data.api_hougeki2.GetFriendDamages()
					);
					this.SecondFleet.CalcDamages(
						data.api_injection_kouku.GetSecondFleetDamages(),
						data.api_kouku.GetSecondFleetDamages(),
						data.api_opening_taisen.GetFriendDamages(),
						data.api_opening_atack.GetFriendDamages(),
						data.api_hougeki3.GetFriendDamages(),
						data.api_raigeki.GetFriendDamages()
					);
					break;
			}

			this.Enemies.CalcDamages(
				data.api_air_base_injection.GetEnemyDamages(),
				data.api_injection_kouku.GetEnemyDamages(),
				data.api_air_base_attack.GetEachFirstEnemyDamages(),
				data.api_support_info.GetEnemyDamages(),
				data.api_kouku.GetEnemyDamages(),
				data.api_opening_taisen.GetEnemyDamages(),
				data.api_opening_atack.GetEnemyDamages(),
				data.api_hougeki1.GetEnemyDamages(),
				data.api_hougeki2.GetEnemyDamages(),
				data.api_hougeki3.GetEnemyDamages(),
				data.api_raigeki.GetEnemyDamages()
			);

			this.FriendAirSupremacy = data.api_kouku.GetAirSupremacy();
			this.AirCombatResults = data.api_air_base_attack.ToResult().Concat(data.api_kouku.ToResult()).ToArray();
			this.RankResult = this.CalcRank(true);
		}

		private void Update(combined_battle_each_battle data, ApiTypes apiType)
		{
			AutoSelectTab();

			switch (apiType)
			{
				case ApiTypes.combined_battle_ec_battle:
					this.Name = "심해연합 - 주간전";
					break;
				case ApiTypes.combined_battle_each_battle:
					this.Name = "기동부대vs심해연합 - 주간전";
					break;
				case ApiTypes.combined_battle_each_battle_water:
					this.Name = "수상부대vs심해연합 - 주간전";
					break;
			}

			this.UpdateFleetsCombinedEnemy(data.api_deck_id, data, data.api_formation);
            this.UpdateMaxHP(data.api_f_maxhps, data.api_e_maxhps, data.api_f_maxhps_combined, data.api_e_maxhps_combined);
            this.UpdateNowHP(data.api_f_nowhps, data.api_e_nowhps, data.api_f_nowhps_combined, data.api_e_nowhps_combined);

            this.UpdateUsedFlag(data.api_kouku?.api_stage2?.api_air_fire);
			this.UpdateUsedFlag(data.api_support_info);

			mvpOracle.Initialize(this.FirstFleet, this.SecondFleet)
				.Update(data, apiType != ApiTypes.combined_battle_ec_battle);
			UpdateMVP(mvpOracle.MVP1, mvpOracle.MVP2);

			#region 연합 vs 심해연합
			if (apiType != ApiTypes.combined_battle_ec_battle)
			{
				// 기동부대
				if (apiType == ApiTypes.combined_battle_each_battle)
				{
					this.FirstFleet.CalcDamages(
						data.api_injection_kouku.GetFirstFleetDamages(),
						data.api_kouku.GetFirstFleetDamages(),
						data.api_opening_taisen.GetEachFirstFriendDamages(),
						data.api_opening_atack.GetEachFirstFriendDamages(),
						data.api_hougeki1.GetEachFirstFriendDamages(),
						data.api_raigeki.GetEachFirstFriendDamages(),
						data.api_hougeki3.GetEachFirstFriendDamages()
					);
					this.SecondFleet.CalcDamages(
						data.api_injection_kouku.GetSecondFleetDamages(),
						data.api_kouku.GetSecondFleetDamages(),
						data.api_opening_taisen.GetEachSecondFriendDamages(),
						data.api_opening_atack.GetEachSecondFriendDamages(),
						data.api_hougeki2.GetEachSecondFriendDamages(),
						data.api_raigeki.GetEachSecondFriendDamages(),
						data.api_hougeki3.GetEachSecondFriendDamages()
					);

					this.Enemies.CalcDamages(
						data.api_air_base_injection.GetEnemyDamages(),
						data.api_injection_kouku.GetEnemyDamages(),
						data.api_air_base_attack.GetEachFirstEnemyDamages(),
						data.api_kouku.GetEnemyDamages(),
						data.api_support_info.GetEachFirstEnemyDamages(),
						data.api_opening_taisen.GetEachFirstEnemyDamages(),
						data.api_opening_atack.GetEachFirstEnemyDamages(),
						data.api_hougeki1.GetEachFirstEnemyDamages(),
						data.api_raigeki.GetEachFirstEnemyDamages(),
						data.api_hougeki3.GetEachFirstEnemyDamages()
					);
					this.SecondEnemies.CalcDamages(
						data.api_air_base_injection.GetSecondEnemyDamages(),
						data.api_injection_kouku.GetSecondEnemyDamages(),
						data.api_air_base_attack.GetEachSecondEnemyDamages(),
						data.api_kouku.GetSecondEnemyDamages(),
						data.api_support_info.GetEachSecondEnemyDamages(),
						data.api_opening_taisen.GetEachSecondEnemyDamages(),
						data.api_opening_atack.GetEachSecondEnemyDamages(),
						data.api_hougeki2.GetEachSecondEnemyDamages(),
						data.api_raigeki.GetEachSecondEnemyDamages(),
						data.api_hougeki3.GetEachSecondEnemyDamages()
					);
				}

				// 수상부대
				else if (apiType == ApiTypes.combined_battle_each_battle_water)
				{
					this.FirstFleet.CalcDamages(
						data.api_injection_kouku.GetFirstFleetDamages(),
						data.api_kouku.GetFirstFleetDamages(),
						data.api_opening_taisen.GetEachFirstFriendDamages(),
						data.api_opening_atack.GetEachFirstFriendDamages(),
						data.api_hougeki1.GetEachFirstFriendDamages(),
						data.api_hougeki2.GetEachFirstFriendDamages(),
						data.api_raigeki.GetEachFirstFriendDamages()
					);
					this.SecondFleet.CalcDamages(
						data.api_injection_kouku.GetSecondFleetDamages(),
						data.api_kouku.GetSecondFleetDamages(),
						data.api_opening_taisen.GetEachSecondFriendDamages(),
						data.api_opening_atack.GetEachSecondFriendDamages(),
						data.api_hougeki2.GetEachSecondFriendDamages(),
						data.api_hougeki3.GetEachSecondFriendDamages(),
						data.api_raigeki.GetEachSecondFriendDamages()
					);

					this.Enemies.CalcDamages(
						data.api_air_base_injection.GetEnemyDamages(),
						data.api_injection_kouku.GetEnemyDamages(),
						data.api_air_base_attack.GetEachFirstEnemyDamages(),
						data.api_kouku.GetEnemyDamages(),
						data.api_support_info.GetEachFirstEnemyDamages(),
						data.api_opening_taisen.GetEachFirstEnemyDamages(),
						data.api_opening_atack.GetEachFirstEnemyDamages(),
						data.api_hougeki1.GetEachFirstEnemyDamages(),
						data.api_hougeki2.GetEachFirstEnemyDamages(),
						data.api_raigeki.GetEachFirstEnemyDamages()
					);
					this.SecondEnemies.CalcDamages(
						data.api_air_base_injection.GetSecondEnemyDamages(),
						data.api_injection_kouku.GetSecondEnemyDamages(),
						data.api_air_base_attack.GetEachSecondEnemyDamages(),
						data.api_kouku.GetSecondEnemyDamages(),
						data.api_support_info.GetEachSecondEnemyDamages(),
						data.api_opening_taisen.GetEachSecondEnemyDamages(),
						data.api_opening_atack.GetEachSecondEnemyDamages(),
						data.api_hougeki2.GetEachSecondEnemyDamages(),
						data.api_hougeki3.GetEachSecondEnemyDamages(),
						data.api_raigeki.GetEachSecondEnemyDamages()
					);
				}
				// else == combined_battle_ec_battle
			}
			#endregion

			#region 단일 vs 심해연합
			else
			{
				this.FirstFleet.CalcDamages(
					data.api_injection_kouku.GetFirstFleetDamages(),
					data.api_kouku.GetFirstFleetDamages(),
					data.api_opening_taisen.GetEachFirstFriendDamages(),
					data.api_opening_atack.GetEachFirstFriendDamages(),
					data.api_hougeki1.GetEachFirstFriendDamages(),
					data.api_hougeki2.GetEachFirstFriendDamages(),
					data.api_raigeki.GetEachFirstFriendDamages(),
					data.api_hougeki3.GetEachFirstFriendDamages()
				);

				this.Enemies.CalcDamages(
					data.api_air_base_injection.GetEnemyDamages(),
					data.api_injection_kouku.GetEnemyDamages(),
					data.api_air_base_attack.GetEachFirstEnemyDamages(),
					data.api_kouku.GetEnemyDamages(),
					data.api_support_info.GetEachFirstEnemyDamages(),
					data.api_opening_taisen.GetEachFirstEnemyDamages(),
					data.api_opening_atack.GetEachFirstEnemyDamages(),
					data.api_raigeki.GetEachFirstEnemyDamages(),
					data.api_hougeki2.GetEachFirstEnemyDamages(),
					data.api_hougeki3.GetEachFirstEnemyDamages()
				);

				this.SecondEnemies.CalcDamages(
					data.api_air_base_injection.GetSecondEnemyDamages(),
					data.api_injection_kouku.GetSecondEnemyDamages(),
					data.api_air_base_attack.GetEachSecondEnemyDamages(),
					data.api_kouku.GetSecondEnemyDamages(),
					data.api_support_info.GetEachSecondEnemyDamages(),
					data.api_opening_taisen.GetEachSecondEnemyDamages(),
					data.api_opening_atack.GetEachSecondEnemyDamages(),
					data.api_hougeki1.GetEachSecondEnemyDamages(),
					data.api_raigeki.GetEachSecondEnemyDamages(),
					data.api_hougeki3.GetEachSecondEnemyDamages()
				);
			}
			#endregion

			this.FriendAirSupremacy = data.api_kouku.GetAirSupremacy();
			this.AirCombatResults = data.api_air_base_attack.ToResult().Concat(data.api_kouku.ToResult()).ToArray();
			this.RankResult = this.CalcRank(true, true, true);
		}

		private void Update(combined_battle_airbattle data, ApiTypes apiType)
		{
			AutoSelectTab();

			switch (apiType)
			{
				case ApiTypes.combined_battle_airbattle:
					this.Name = "연합함대 - 항공전 - 주간";
					this.UpdateUsedFlag(
						data.api_kouku?.api_stage2?.api_air_fire,
						data.api_kouku2?.api_stage2?.api_air_fire
					);
					break;
				case ApiTypes.combined_battle_ld_airbattle:
					this.Name = "연합함대 - 공습전 - 주간";
					this.UpdateUsedFlag(data.api_kouku?.api_stage2?.api_air_fire);
					break;
			}

			this.UpdateFleets(data.api_deck_id, data, data.api_formation);
            this.UpdateMaxHP(data.api_f_maxhps, data.api_e_maxhps, data.api_f_maxhps_combined, data.api_e_maxhps_combined);
            this.UpdateNowHP(data.api_f_nowhps, data.api_e_nowhps, data.api_f_nowhps_combined, data.api_e_nowhps_combined);

            mvpOracle.Initialize(this.FirstFleet, this.SecondFleet).Update(data, true);
			UpdateMVP(mvpOracle.MVP1, mvpOracle.MVP2);

			this.UpdateUsedFlag(data.api_support_info);

			this.FirstFleet.CalcDamages(
				data.api_air_base_injection.GetFirstFleetDamages(),
				data.api_injection_kouku.GetFirstFleetDamages(),
				data.api_kouku.GetFirstFleetDamages(),
				data.api_kouku2.GetFirstFleetDamages()
			);
			this.SecondFleet.CalcDamages(
				data.api_air_base_injection.GetSecondFleetDamages(),
				data.api_injection_kouku.GetSecondFleetDamages(),
				data.api_kouku.GetSecondFleetDamages(),
				data.api_kouku2.GetSecondFleetDamages()
			);

			this.Enemies.CalcDamages(
				data.api_air_base_injection.GetEnemyDamages(),
				data.api_injection_kouku.GetEnemyDamages(),
				data.api_air_base_attack.GetEachFirstEnemyDamages(),
				data.api_support_info.GetEnemyDamages(),
				data.api_kouku.GetEnemyDamages(),
				data.api_kouku2.GetEnemyDamages()
			);

			this.FriendAirSupremacy = data.api_kouku.GetAirSupremacy();

			switch (apiType)
			{
				case ApiTypes.combined_battle_airbattle:
					this.AirCombatResults = data.api_air_base_attack.ToResult()
							.Concat(data.api_kouku.ToResult("1회차/"))
							.Concat(data.api_kouku2.ToResult("2회차/"))
							.ToArray();
					this.RankResult = this.CalcRank(true);
					break;
				case ApiTypes.combined_battle_ld_airbattle:
					this.AirCombatResults = data.api_air_base_attack.ToResult().Concat(data.api_kouku.ToResult()).ToArray();

					this.AirRankResult = this.CalcLDAirRank(true);
					this.RankResult = Rank.공습전;
					break;
			}
		}

		private void Update(combined_battle_midnight_battle data, ApiTypes apiType)
		{
			AutoSelectTab();

			switch (apiType)
			{
				case ApiTypes.combined_battle_midnight_battle:
					this.Name = "연합함대 - 야전";
					break;
				case ApiTypes.combined_battle_sp_midnight:
					this.Name = "연합함대 - 개막야전";
					this.FriendAirSupremacy = AirSupremacy.항공전없음;
					break;
			}

			int BeforedayBattleHP = this.FirstFleet.Ships
				.Where(x => !x.Situation.HasFlag(ShipSituation.Tow) && !x.Situation.HasFlag(ShipSituation.Evacuation))
				.Sum(x => x.BeforeNowHP);//리스트 갱신하기전에 아군 HP최대값을 저장

			int EnemyBeforedayBattle = this.Enemies.Ships.Sum(x => x.BeforeNowHP);
			BeforedayBattleHP += this.SecondFleet.Ships
				.Where(x => !x.Situation.HasFlag(ShipSituation.Tow) && !x.Situation.HasFlag(ShipSituation.Evacuation))
				.Sum(x => x.BeforeNowHP);//리스트 갱신하기전에 아군 HP최대값을 저장

			// HP가 초기화되는 문제가 있어서..
			if (apiType == ApiTypes.combined_battle_sp_midnight)
			{
				this.UpdateFleets(data.api_deck_id, data);
				this.UpdateMaxHP(data.api_f_maxhps, data.api_e_maxhps, data.api_f_maxhps_combined, data.api_e_maxhps_combined);
				this.UpdateNowHP(data.api_f_nowhps, data.api_e_nowhps, data.api_f_nowhps_combined, data.api_e_nowhps_combined);
			}

			this.UpdateUsedFlag(data.api_flare_pos, data.api_touch_plane);

			if (apiType == ApiTypes.combined_battle_sp_midnight)
				mvpOracle.Initialize(this.FirstFleet, this.SecondFleet);

			mvpOracle.Update(data, true);
			UpdateMVP(mvpOracle.MVP1, mvpOracle.MVP2);

			this.SecondFleet.CalcDamages(data.api_hougeki.GetFriendDamages());
			this.Enemies.CalcDamages(data.api_hougeki.GetEnemyDamages());

			switch (apiType)
			{
				case ApiTypes.combined_battle_midnight_battle:
					this.RankResult = this.CalcRank(true, true, true, BeforedayBattleHP, EnemyBeforedayBattle);
					break;
				case ApiTypes.combined_battle_sp_midnight:
					this.RankResult = this.CalcRank(true);
					break;
			}
		}

		private void Update(combined_battle_ec_midnight_battle data, ApiTypes apiType)
		{
			AutoSelectTab();
			this.Name = "연합vs연합 - 야전";

			int BeforedayBattleHP = this.FirstFleet.Ships
				.Where(x => !x.Situation.HasFlag(ShipSituation.Tow) && !x.Situation.HasFlag(ShipSituation.Evacuation))
				.Sum(x => x.BeforeNowHP); // 리스트 갱신하기전에 아군 HP최대값을 저장
			BeforedayBattleHP += this.SecondFleet.Ships
				.Where(x => !x.Situation.HasFlag(ShipSituation.Tow) && !x.Situation.HasFlag(ShipSituation.Evacuation))
				.Sum(x => x.BeforeNowHP); // 리스트 갱신하기전에 아군 HP최대값을 저장

			int EnemyBeforedayBattle = this.Enemies.Ships.Sum(x => x.BeforeNowHP);
			EnemyBeforedayBattle += this.SecondEnemies.Ships.Sum(x => x.BeforeNowHP);

			this.UpdateFleetsCombinedEnemy(data.api_deck_id, data);
			this.UpdateEnemyMaxHP(data.api_e_maxhps, data.api_e_maxhps_combined);
			this.UpdateNowHP(data.api_f_nowhps, data.api_e_nowhps, data.api_f_nowhps_combined, data.api_e_nowhps_combined);

			this.UpdateUsedFlag(data.api_flare_pos, data.api_touch_plane);

			mvpOracle.Update(data, true);
			UpdateMVP(mvpOracle.MVP1, mvpOracle.MVP2);

			if (data.api_active_deck[0] == 1) this.FirstFleet.CalcDamages(data.api_hougeki.GetFriendDamages());
			else this.SecondFleet.CalcDamages(data.api_hougeki.GetFriendDamages());

			if (data.api_active_deck[1] == 1) this.Enemies.CalcDamages(data.api_hougeki.GetEnemyDamages());
			else this.SecondEnemies.CalcDamages(data.api_hougeki.GetEnemyDamages());

			this.RankResult = this.CalcRank(true, true, true, BeforedayBattleHP, EnemyBeforedayBattle);
		}

		#endregion

		public void Update(battle_result data)
		{
			//this.DropShipName = KanColleClient.Current.Translations.GetTranslation(data.api_get_ship?.api_ship_name, TranslationType.Ships, true);
			this.DropShipName = KanColleClient.Current.Master.Ships
				.SingleOrDefault(x => x.Value.Id == data.api_get_ship?.api_ship_id).Value
				?.Name;

			if (this.RankResult != Rank.완전승리S)
			{
				var rank = RankExtension.ConvertRank(data.api_win_rank);
				if (rank != Rank.에러)
				{
					if (this.RankResult == Rank.공습전)
						this.AirRankResult = rank == Rank.S승리
							? Rank.완전승리S : rank;

					else
						this.RankResult = rank;
				}
			}

            if (data.api_m1.HasValue)
                this.MapExtended = (data.api_m1.Value == 1);

            UpdateMVP(data.api_mvp, data.api_mvp_combined ?? 0);
		}
		private void UpdateMVP(int mvp1 = 0, int mvp2 = 0)
		{
			bool[] firstMvp = new bool[6], secondMvp = new bool[6];

			if (mvp1 > 0) firstMvp[(mvp1 - 1) % 6] = true;
			if (FirstFleet != null) FirstFleet.Ships.SetValues(firstMvp, (s, v) => s.IsMvp = v);

			if (mvp2 > 0) secondMvp[(mvp2 - 1) % 6] = true;
			if (SecondFleet != null) SecondFleet.Ships.SetValues(secondMvp, (s, v) => s.IsMvp = v);
		}

		private void ProcessStartNext(Nekoxy.Session session, bool isNext = false)
		{
			SvData<map_start_next> data;
			if (!SvData.TryParse<map_start_next>(session, out data)) return;

			var startNext = data.Data;
			string api_deck_id = data.Request["api_deck_id"];

			this.IsInSortie = true;
			this.Clear();

			this.CurrentMap = getMapText(startNext);

			this.RankResult = Rank.없음;
			this.AirRankResult = Rank.없음;

			if (!isNext)
			{
				this.CurrentDeckId = int.Parse(api_deck_id);
				this.Cells.Clear();
			}

			#region Cell list
			string Cell = "";
			this.CellEvent = startNext.api_event_id;
			if (startNext.api_no != 0)
				Cell = startNext.api_maparea_id + "-" + startNext.api_mapinfo_no + "-" + startNext.api_no;

			this.Cells.ForEach(x => x.IsOld = true);
			this.Cells.Add(new CellData
			{
				CellName = MapAreaData.MapAreaTable.SingleOrDefault(x => x.Key == Cell).Value ?? (startNext.api_no.ToString()), // Cell,
				CellEvent = this.CellEvent.ToString(),
				CellText = getCellText(startNext, session),
				IsOld = false
			});
			this.RaisePropertyChanged(nameof(this.Cells));
			#endregion

			if (this.CurrentDeckId < 1) return;

			this.UpdateFriendFleets(this.CurrentDeckId);

			if (this.FirstFleet != null) this.FirstFleet.TotalDamaged = 0;
			if (this.SecondFleet != null) this.SecondFleet.TotalDamaged = 0;

			AutoSelectTab();
		}
		private void UpdateFleets(int api_deck_id, ICommonBattleMembers data, int[] api_formation = null)
		{
			this.UpdatedTime = DateTimeOffset.Now;
			this.UpdateFriendFleets(api_deck_id);
			
			var eTotal = 0;
			if (this.Enemies != null) eTotal = this.Enemies.TotalDamaged;
			this.Enemies = new FleetData(
				data.ToMastersShipDataArray(),
				this.Enemies?.Formation ?? Formation.없음,
				this.Enemies?.Name ?? "",
				FleetType.Enemy,
				this.Enemies?.Rank);
			this.Enemies.TotalDamaged = eTotal;

			this.SecondEnemies = new FleetData(
				new MembersShipData[0],
				Formation.없음,
				"",
				FleetType.SecondEnemy,
				this.SecondEnemies?.Rank
			);


			if (api_formation != null)
			{
				this.BattleSituation = (BattleSituation)api_formation[2];
				if (this.FirstFleet != null) this.FirstFleet.Formation = (Formation)api_formation[0];
				if (this.Enemies != null) this.Enemies.Formation = (Formation)api_formation[1];
			}

			this.CurrentDeckId = api_deck_id;
		}
		private void UpdateFleetsCombinedEnemy(int api_deck_id, ICommonEachBattleMembers data, int[] api_formation = null)
		{
			this.UpdatedTime = DateTimeOffset.Now;
			this.UpdateFriendFleets(api_deck_id);

			var eTotal = 0;
			if (this.Enemies != null) eTotal = this.Enemies.TotalDamaged;
			this.Enemies = new FleetData(
				data.ToMastersShipDataArray(),
				this.Enemies?.Formation ?? Formation.없음,
				this.Enemies?.Name ?? "",
				FleetType.Enemy,
				this.Enemies?.Rank);
			this.Enemies.TotalDamaged = eTotal;

			eTotal = 0;
			if (this.SecondEnemies != null) eTotal = this.SecondEnemies.TotalDamaged;
			this.SecondEnemies = new FleetData(
				data.ToMastersSecondShipDataArray(),
				this.SecondEnemies?.Formation ?? Formation.없음,
				this.SecondEnemies?.Name ?? "",
				FleetType.SecondEnemy,
				this.SecondEnemies?.Rank);
			this.SecondEnemies.TotalDamaged = eTotal;

			if (api_formation != null)
			{
				this.BattleSituation = (BattleSituation)api_formation[2];
				if (this.FirstFleet != null) this.FirstFleet.Formation = (Formation)api_formation[0];
				if (this.Enemies != null) this.Enemies.Formation = (Formation)api_formation[1];
			}

			this.CurrentDeckId = api_deck_id;
		}

		/// <summary>
		/// 아군 정보 갱신 (모항 데이터 기준)
		/// </summary>
		/// <param name="deckID"></param>
		private void UpdateFriendFleets(int deckID)
		{
			var organization = KanColleClient.Current.Homeport.Organization;

			int firstTotal = 0;
			int secondTotal = 0;
			if (this.FirstFleet != null) firstTotal = this.FirstFleet.TotalDamaged;
			if (this.SecondFleet != null) secondTotal = this.SecondFleet.TotalDamaged;

			this.FirstFleet = new FleetData(
				organization.Fleets[deckID].Ships.Select(s => new MembersShipData(s)).ToArray(),
				this.FirstFleet?.Formation ?? Formation.없음,
				organization.Fleets[deckID].Name,
				FleetType.First
			);
			this.FirstFleet.TotalDamaged = firstTotal;

			this.SecondFleet = new FleetData(
				organization.Combined && deckID == 1
					? organization.Fleets[2].Ships.Select(s => new MembersShipData(s)).ToArray()
					: new MembersShipData[0],
				this.SecondFleet?.Formation ?? Formation.없음,
				organization.Fleets[2].Name,
				FleetType.Second
			);
			this.SecondFleet.TotalDamaged = secondTotal;
		}

		private void UpdateMaxHP(int[] api_f_maxhps, int[] api_e_maxhps, int[] api_f_maxhps_combined = null, int[] api_e_maxhps_combined = null)
		{
			this.FirstFleet.Ships.SetValues(api_f_maxhps, (s, v) => s.MaxHP = v);
			this.Enemies.Ships.SetValues(api_e_maxhps, (s, v) => s.MaxHP = v);

            if (api_f_maxhps_combined != null)
                this.SecondFleet.Ships.SetValues(api_f_maxhps_combined, (s, v) => s.MaxHP = v);

            if (api_e_maxhps_combined != null)
                this.SecondEnemies.Ships.SetValues(api_e_maxhps_combined, (s, v) => s.MaxHP = v);
		}
		private void UpdateNowHP(int[] api_f_nowhps, int[] api_e_nowhps, int[] api_f_nowhps_combined = null, int[] api_e_nowhps_combined = null)
        {
			this.FirstFleet.Ships.SetValues(api_f_nowhps, (s, v) => s.NowHP = s.BeforeNowHP = v);
			this.Enemies.Ships.SetValues(api_e_nowhps, (s, v) => s.NowHP = s.BeforeNowHP = v);

            if (api_f_nowhps_combined != null)
                this.SecondFleet.Ships.SetValues(api_f_nowhps_combined, (s, v) => s.NowHP = s.BeforeNowHP = v);

            if (api_e_nowhps_combined != null)
                this.SecondEnemies.Ships.SetValues(api_e_nowhps_combined, (s, v) => s.NowHP = s.BeforeNowHP = v);
		}

		private void UpdateEnemyMaxHP(int[] api_maxhps, int[] api_maxhps_combined = null)
		{
			this.Enemies.Ships.SetValues(api_maxhps, (s, v) => s.MaxHP = v);

			if (api_maxhps_combined == null) return;
			this.SecondEnemies.Ships.SetValues(api_maxhps_combined, (s, v) => s.MaxHP = v);
		}

		private void ResultClear(kcsapi_port port)
		{
			if (this.FirstFleet != null) this.FirstFleet.TotalDamaged = 0;
			if (this.SecondFleet != null) this.SecondFleet.TotalDamaged = 0;

			if(this.IsInSortie) AutoBackTab();
			this.IsInSortie = false;

			this.MechanismOn = (port.api_event_object?.api_m_flag2 == 1);
		}
		private void Clear()
		{
			this.UpdatedTime = DateTimeOffset.Now;
			this.Name = "";
			this.DropShipName = null;

			if (this.FirstFleet != null) FirstFleet.Ships.SetValues(new bool[6], (s, v) => s.IsMvp = v);
			if (this.SecondFleet != null) SecondFleet.Ships.SetValues(new bool[6], (s, v) => s.IsMvp = v);

			this.FlareUsed = UsedFlag.Unset;
			this.NightReconScouted = UsedFlag.Unset;
			this.AntiAirFired = AirFireFlag.Unset;
			this.SupportUsed = UsedSupport.Unset;

			this.BattleSituation = BattleSituation.없음;
			this.FriendAirSupremacy = AirSupremacy.항공전없음;
			this.AirCombatResults = new AirCombatResult[0];
			if (this.FirstFleet != null) this.FirstFleet.Formation = Formation.없음;
			this.Enemies = new FleetData();
			this.SecondEnemies = new FleetData();

			this.MechanismOn = false;
            this.MapExtended = false;
		}

		private bool CalcOverKill(int MaxCount, int SinkCount)
		{
			if (MaxCount == 1)
			{
				if (MaxCount == SinkCount) return true;
				else return false;
			}

			// x / 3m * 2m
			return SinkCount >= (int)decimal.Floor(MaxCount / 1.5m);
		}
		private Rank CalcRank(bool IsCombined = false, bool IsEnemyCombined = false, bool IsMidnight = false, int BeforeHP = 0, int EnemyBefore = 0)
		{
			try
			{
				var AliasMax = this.FirstFleet.Ships
					.Where(x => !x.Situation.HasFlag(ShipSituation.Tow) && !x.Situation.HasFlag(ShipSituation.Evacuation))
					.Sum(x => x.BeforeNowHP);
				var EnemyMax = this.Enemies.Ships.Sum(x => x.BeforeNowHP);

				var AliasTotalDamaged = this.FirstFleet.TotalDamaged;
				var EnemyTotalDamaged = this.Enemies.TotalDamaged;


				int SinkCount = this.FirstFleet.SinkCount;
				int EnemySinkCount = this.Enemies.SinkCount;
				ShipData EnemyFlag = this.Enemies.Ships.First();

				int MaxCount = this.FirstFleet.Ships
					.Where(x => !x.Situation.HasFlag(ShipSituation.Tow) && !x.Situation.HasFlag(ShipSituation.Evacuation))
					.Count();
				int EnemyMaxCount = this.Enemies.Ships.Count();


				if (IsCombined)
				{
					AliasTotalDamaged += this.SecondFleet.TotalDamaged;
					AliasMax += this.SecondFleet.Ships.Sum(x => x.BeforeNowHP);

					MaxCount += this.SecondFleet.Ships
						.Where(x => !x.Situation.HasFlag(ShipSituation.Tow) && !x.Situation.HasFlag(ShipSituation.Evacuation))
						.Count();
					SinkCount += this.SecondFleet.SinkCount;
				}
				if (IsEnemyCombined)
				{
					EnemyTotalDamaged += this.SecondEnemies.TotalDamaged;
					EnemyMax += this.SecondEnemies.Ships.Sum(x => x.BeforeNowHP);

					EnemyMax += this.SecondEnemies.Ships.Count();
					EnemySinkCount += this.SecondEnemies.SinkCount;
				}

				if (IsMidnight)
				{
					AliasMax = BeforeHP;
					EnemyMax = EnemyBefore;
				}

				var IsShipSink = SinkCount > 0; // ? true : false;
				decimal EnemyDamagedRate = EnemyTotalDamaged / (decimal)EnemyMax; // 적이 받은 총 데미지
				decimal AliasDamagedRate = AliasTotalDamaged / (decimal)AliasMax; // 아군이 받은 총 데미지

				decimal DamageRate = AliasDamagedRate == 0
					? -1 // same with x2.5
					: (decimal)EnemyDamagedRate / AliasDamagedRate;

				this.FirstFleet.AttackGauge = this.MakeGaugeText(EnemyTotalDamaged, EnemyMax, EnemyDamagedRate);
				this.Enemies.AttackGauge = this.MakeGaugeText(AliasTotalDamaged, AliasMax, AliasDamagedRate);


				var IsOverKill = CalcOverKill(EnemyMaxCount, EnemySinkCount);
				var IsOverKilled = CalcOverKill(MaxCount, SinkCount);

				int damageType = 0;

				if (DamageRate < 0) damageType = 2;
				else if (IsShipSink && DamageRate > 3m) damageType = 2 | 4;
				else if (DamageRate > 2.5m) damageType = 2;
				else if (DamageRate > 1.0m) damageType = 1;
				else damageType = 0;

				if (AliasTotalDamaged == 0 && EnemyTotalDamaged == 0)
					return Rank.D패배;

				else if (EnemyDamagedRate < 0.0005m)
					return Rank.D패배;

				else if (IsShipSink)
				{
					if (EnemyFlag.NowHP <= 0)
					{
						if (IsOverKill) return Rank.B승리;
						else return Rank.D패배;
					}
					else if ((damageType &1)== 1) // Mid Damage
						return Rank.C패배;

					else
					{
						if (IsOverKilled) return Rank.E패배;
						else
						{
							if (!IsOverKill && (damageType & 4) == 4) return Rank.B승리; // x3 damage (With sinked ship)
							if (IsOverKill && (damageType & 2) == 2) return Rank.B승리; // Over damage (x2.5)
							return Rank.C패배;
						}
					}
				}

				else
				{
					if (EnemyFlag.NowHP <= 0)
					{
						if (EnemyMaxCount == this.Enemies.SinkCount)
						{
							if (AliasTotalDamaged > 0) return Rank.S승리;
							else return Rank.완전승리S;
						}
						else
						{
							if (IsOverKill) return Rank.A승리;
							else return Rank.B승리;
						}
					}
					else
					{
						if (IsOverKill) return Rank.A승리;

						if ((damageType & 2) == 2) return Rank.B승리;
						else if ((damageType & 1) == 1) return Rank.C패배;
						else if (damageType == 0) return Rank.D패배;
						else return Rank.D패배;
					}
				}

				#region OldCode
				/*
				bool IsOverKill = CalcOverKill(EnemyMaxCount, this.Enemies.SinkCount);
				bool IsOverKilled = CalcOverKill(MaxCount, SinkCount);

				if (AliasTotalDamaged > 0)
				{
					decimal EnemyValue = decimal.Floor(EnemyDamagedPercent * 100m); // 적군 피격 데미지 비율 (소숫점 제외)
					decimal AliasValue = decimal.Floor(AliasDamagedPercent * 100m); // 아군 피격 데미지 비율 (소숫점 제외)

					if (AliasValue == 0)
					{
						if (EnemyTotalDamaged == 0) IsScratch = true;
						else IsOverDamage = true; // 아군피해 0인 경우
					}
					else
					{
						var CalcPercent = Math.Round(EnemyValue / AliasValue, 2, MidpointRounding.AwayFromZero);
						if (CalcPercent > 2.5m)
							IsOverDamage = true;// 2.5배 초과 데미지
						else if (CalcPercent > 1m)
							IsMidDamage = true;// 1초과 2.5이하
						else
							IsScratch = true;// 1미만

						if (IsShipSink)
						{
							if (CalcPercent > 3m)
							{
								IsThreeTime = true;
								IsOverDamage = true;
								IsMidDamage = false;
								IsScratch = false;
							}
						}
					}
				}
				else if (AliasTotalDamaged == 0)
				{
					if (EnemyTotalDamaged == 0) IsScratch = true;
					else IsOverDamage = true; // 아군피해 0인 경우
				}

				if (AliasTotalDamaged == 0 && EnemyTotalDamaged == 0) return Rank.D패배;//d
				if (EnemyDamagedPercent < 0.0005m) return Rank.D패배;//d
				else if (IsShipSink)
				{
					if (EnemyFlag.NowHP <= 0)
					{
						if (IsOverKill) return Rank.B승리;
						else return Rank.D패배;//d
					}
					else if (IsMidDamage) return Rank.C패배;//c
					else
					{
						if (IsOverKilled) return Rank.E패배;//e
						else
						{
							if (!IsOverKill && IsThreeTime) return Rank.B승리;
							if (IsOverKill && IsOverDamage) return Rank.B승리;
							else return Rank.C패배;//c
						}
					}
				}
				else
				{
					if (EnemyFlag.NowHP <= 0)
					{
						if (EnemyMaxCount == this.Enemies.SinkCount)
						{
							if (AliasTotalDamaged > 0) return Rank.S승리;
							else return Rank.완전승리S;
						}
						else
						{
							if (IsOverKill) return Rank.A승리;
							else return Rank.B승리;
						}
					}
					else
					{
						if (IsOverKill) return Rank.A승리;

						if (IsOverDamage) return Rank.B승리;
						else if (IsMidDamage) return Rank.C패배;//c
						else if (IsScratch) return Rank.D패배;//d
						else return Rank.D패배;//d
					}
				}
				*/
				#endregion
			}
			catch (Exception ex)
			{
				// KanColleClient.Current.CatchedErrorLogWriter.ReportException(ex.Source, ex);
				System.IO.File.AppendAllText("battleinfo_error.log", ex.ToString() + Environment.NewLine);
				Debug.WriteLine(ex);

				return Rank.에러;
			}
		}
		private Rank CalcLDAirRank(bool IsCombined = false)
		{
			try
			{
				var AliasMax = this.FirstFleet.Ships
					.Where(x => !x.Situation.HasFlag(ShipSituation.Tow) && !x.Situation.HasFlag(ShipSituation.Evacuation))
					.Sum(x => x.BeforeNowHP);

				var EnemyTotal = this.Enemies.TotalDamaged;
				var EnemyMax = this.Enemies.Ships.Sum(x => x.BeforeNowHP);

				var AliasTotal = this.FirstFleet.TotalDamaged;
				var IsShipSink = this.FirstFleet.SinkCount > 0; // ? true : false;

				decimal EnemyDamagedPercent = EnemyTotal / (decimal)EnemyMax; // 적이 받은 총 데미지
				decimal AliasDamagedPercent = AliasTotal / (decimal)AliasMax; // 아군이 받은 총 데미지

				int MaxCount = this.FirstFleet.Ships
					.Where(x => !x.Situation.HasFlag(ShipSituation.Tow) && !x.Situation.HasFlag(ShipSituation.Evacuation))
					.Count();
				int SinkCount = this.FirstFleet.SinkCount;

				if (IsCombined)
				{
					AliasTotal += this.SecondFleet.TotalDamaged;
					AliasMax += this.SecondFleet.Ships.Sum(x => x.BeforeNowHP);
					AliasDamagedPercent = AliasTotal / (decimal)AliasMax; // 아군이 받은 총 데미지

					if (!IsShipSink) IsShipSink = this.SecondFleet.SinkCount > 0 ? true : false;
					MaxCount += this.SecondFleet.Ships
						.Where(x => !x.Situation.HasFlag(ShipSituation.Tow) && !x.Situation.HasFlag(ShipSituation.Evacuation))
						.Count();
					SinkCount += this.SecondFleet.SinkCount;
				}

				this.FirstFleet.AttackGauge = this.MakeGaugeText(EnemyTotal, EnemyMax, EnemyDamagedPercent);
				this.Enemies.AttackGauge = this.MakeGaugeText(AliasTotal, AliasMax, AliasDamagedPercent);

				bool IsOverKilled = CalcOverKill(MaxCount, SinkCount);

				if (AliasTotal > 0)
				{
					decimal AliasValue = decimal.Floor(AliasDamagedPercent * 100m); // 아군 피격 데미지 비율 (소숫점 제외)

					if (SinkCount > 0) return Rank.E패배;
					else if (AliasValue == 0) return Rank.A승리;
					else if (AliasValue > 0 && AliasValue < 10) return Rank.A승리;
					else if (AliasValue > 10 && AliasValue < 20) return Rank.B승리;
					else if (AliasValue > 20 && AliasValue < 50) return Rank.C패배;
					else return Rank.D패배;
				}
				else return Rank.완전승리S;
			}
			catch (Exception ex)
			{
				// KanColleClient.Current.CatchedErrorLogWriter.ReportException(ex.Source, ex);
				System.IO.File.AppendAllText("battleinfo_error.log", ex.ToString() + Environment.NewLine);
				Debug.WriteLine(ex);

				return Rank.에러;
			}
		}

		private void UpdateUsedFlag(int[] flare, int[] nightrecon)
		{
			try
			{
				if (flare[0] == -1 && flare[1] == -1)
					this.FlareUsed = UsedFlag.Unused;
				else if (flare[0] != -1)
					this.FlareUsed = UsedFlag.Used;
				else if (flare[1] != -1)
					this.FlareUsed = UsedFlag.EnemyUsed;
				else
					this.FlareUsed = UsedFlag.BothUsed;

				if (nightrecon[0] == -1 && nightrecon[1] == -1)
					this.NightReconScouted = UsedFlag.Unused;
				else if (nightrecon[0] != -1)
					this.NightReconScouted = UsedFlag.Used;
				else if (nightrecon[1] != -1)
					this.NightReconScouted = UsedFlag.EnemyUsed;
				else
					this.NightReconScouted = UsedFlag.BothUsed;
			}
			catch
			{
				this.FlareUsed = UsedFlag.Unset;
				this.NightReconScouted = UsedFlag.Unset;
			}
		}
		private void UpdateUsedFlag(Api_Air_Fire data)
		{
			try
			{
				this.AntiAirFired = data == null ? AirFireFlag.Unused : AirFireFlag.Used;
			}
			catch
			{
				this.AntiAirFired = AirFireFlag.Unset;
			}
		}
		private void UpdateUsedFlag(Api_Air_Fire data1, Api_Air_Fire data2)
		{
			try
			{
				if (data1 == null && data2 == null)
					this.AntiAirFired = AirFireFlag.Unused;
				else if (data2 == null)
					this.AntiAirFired = AirFireFlag.Used1;
				else if (data1 == null)
					this.AntiAirFired = AirFireFlag.Used2;
				else
					this.AntiAirFired = AirFireFlag.UsedAll;
			}
			catch
			{
				this.AntiAirFired = AirFireFlag.Unset;
			}
		}
		private void UpdateUsedFlag(Api_Support_Info data)
		{
			try
			{
				if (data == null)
					this.SupportUsed = UsedSupport.Unused;
				else if (data.api_support_airatack != null)
					this.SupportUsed = UsedSupport.Kouku;
				else if (data.api_support_hourai != null)
					this.SupportUsed = UsedSupport.Hourai;
				else
					this.SupportUsed = UsedSupport.Unset;
			}
			catch
			{
				this.SupportUsed = UsedSupport.Unset;
			}
		}

		private string MakeGaugeText(int current, int max, decimal percent)
		{
			StringBuilder temp = new StringBuilder();

			temp.Append("(" + current + "/" + max + ") ");
			temp.Append(Math.Round(percent * 100, 2, MidpointRounding.AwayFromZero) + "%");
			return temp.ToString();
		}

		private string getCellText(map_start_next data, Nekoxy.Session session)
		{
			Dictionary<int, string> resources = new Dictionary<int, string>()
			{
				{  0, "연료" },
				{  1, "탄약" },
				{  2, "강재" },
				{  3, "보크사이트" },
				{  4, "고속건조재" },
				{  5, "고속수복재" },
				{  6, "개발자재" },
				{  7, "개수자재" },
				{  9, "가구함(소)" },
				{ 10, "가구함(중)" },
				{ 11, "가구함(대)" },
			};

			int eventId = data.api_event_id;

			switch (eventId)
			{
				case 2: // 일반 자원획득
					if (data.api_itemget == null)
						return "자원획득";

					return string.Join(
						" ",
						data.api_itemget
							.Select(x => {
								var resname = resources.ContainsKey(x.api_id - 1)
									? resources[x.api_id - 1]
									: (x.api_name?.Length > 0 ? x.api_name : "???");

								return x.api_getcount > 1
									? string.Format("{0} +{1}", resname, x.api_getcount)
									: resname;
							})
							.ToArray()
					);
				case 3:
					if (data.api_happening == null || data.api_happening.api_count == 0)
						return "소용돌이";

					{
						var resname = resources.ContainsKey(data.api_happening.api_mst_id - 1)
							? resources[data.api_happening.api_mst_id - 1]
							: "???";

						return data.api_happening.api_count > 1
							? string.Format("{0}-{1}", resname, data.api_happening.api_count)
							: resname;
					}

				case 4:
				case 31:
					switch (data.api_event_kind) {
						case 1: return "적군조우";
						case 2: return "개막야전";
						case 3: return "야전>주간전";
						case 4: return "항공전";
						case 5: return "심해연합";
						case 6: return "공습전";
					}
					break;

				case 5:
					switch (data.api_event_kind)
					{
						case 2: return "보스 (개막야전)";
						case 3: return "보스 (야전>주간전)";
						case 5: return "보스 (심해연합)";
					}
					return "보스전";

				case 6:
					if(data.api_select_route == null) return "기분탓";
					return "능동분기";
				case 7: // 항공정찰 자원획득
					if (data.api_event_kind == 0)
					{
						SvData<map_start_next2> svdata;
						if (!SvData.TryParse<map_start_next2>(session, out svdata))
							return "정찰실패";

						var data2 = svdata.Data;
						if (data2.api_itemget == null) return "정찰실패";

						var resname = resources.ContainsKey(data2.api_itemget.api_id - 1)
							? resources[data2.api_itemget.api_id - 1]
							: (data2.api_itemget.api_name?.Length > 0 ? data2.api_itemget.api_name : "???");

						return data2.api_itemget.api_getcount > 1
							? string.Format("{0}+{1}", resname, data2.api_itemget.api_getcount)
							: resname;
					}
					else return "항공전";

				case 8: // EO (1-6)
					{
						var x = data.api_itemget_eo_comment;
						if (x == null)
							return "자원획득";

						var resname = resources.ContainsKey(x.api_id - 1)
							? resources[x.api_id - 1]
							: (x.api_name?.Length > 0 ? x.api_name : "???");

						return x.api_getcount > 1
							? string.Format("{0}+{1}", resname, x.api_getcount)
							: resname;
					}
				case 9: // TP
					return "수송지점";
				case 10:
					return "공습전";
			}
			return "?";
		}
		private string getMapText(map_start_next startNext)
		{
			int maparea_id, mapinfo_no;
			maparea_id = startNext.api_maparea_id;
			mapinfo_no = startNext.api_mapinfo_no;

			return string.Format(
				"{0}-{1}",
				maparea_id > 30 ? "E" : maparea_id.ToString(),
				mapinfo_no
			);
		}
	}
}
