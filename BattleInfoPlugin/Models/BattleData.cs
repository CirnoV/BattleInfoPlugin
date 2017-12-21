using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using BattleInfoPlugin.Models.Raw;
using Grabacr07.KanColleWrapper;
using Livet;
using Grabacr07.KanColleWrapper.Models;
using BattleInfoPlugin.Properties;
using System.Text;
using System.Diagnostics;
using System.Windows;
using kcsapi_port = Grabacr07.KanColleWrapper.Models.Raw.kcsapi_port;

#region Alias
using practice_battle = BattleInfoPlugin.Models.Raw.sortie_battle;
using battle_midnight_sp_midnight = BattleInfoPlugin.Models.Raw.sortie_battle_midnight;
using practice_midnight_battle = BattleInfoPlugin.Models.Raw.sortie_battle_midnight;
using sortie_ld_airbattle = BattleInfoPlugin.Models.Raw.sortie_airbattle;
using combined_battle_battle_water = BattleInfoPlugin.Models.Raw.combined_battle;
using combined_battle_ld_airbattle = BattleInfoPlugin.Models.Raw.combined_airbattle;
using combined_battle_sp_midnight = BattleInfoPlugin.Models.Raw.combined_battle_midnight;
#endregion

namespace BattleInfoPlugin.Models
{
	public class BattleData : NotificationObject
	{
		#region Properties

		#region BattleName 변경통지 프로퍼티
		private string _BattleName;

		public string BattleName
		{
			get
			{ return this._BattleName; }
			set
			{
				if (this._BattleName == value)
					return;
				this._BattleName = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region UpdatedTime 변경통지 프로퍼티
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


		#region CurrentSortie 변경통지 프로퍼티
		public SortieInfo CurrentSortie
		{
			get { return this._CurrentSortie; }
			set
			{
				if (this._CurrentSortie != value)
				{
					this._CurrentSortie = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private SortieInfo _CurrentSortie;
		#endregion

		#region CurrentBattleFlag 변경통지 프로퍼티
		public BattleFlags CurrentBattleFlag
		{
			get { return this._CurrentBattleFlag; }
			set
			{
				if (this._CurrentBattleFlag != value)
				{
					this._CurrentBattleFlag = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private BattleFlags _CurrentBattleFlag;
		#endregion


		#region BattleSituation 변경통지 프로퍼티
		public BattleSituation BattleSituation
		{
			get { return this._BattleSituation; }
			set
			{
				if (this._BattleSituation != value)
				{
					this._BattleSituation = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private BattleSituation _BattleSituation;
		#endregion


		#region AliasFirst 변경통지 프로퍼티
		private FleetData _AliasFirst;

		public FleetData AliasFirst
		{
			get
			{ return this._AliasFirst; }
			set
			{
				if (this._AliasFirst == value)
					return;
				this._AliasFirst = value;
				this._AliasFirst.FleetType = FleetType.AliasFirst;
				Settings.Default.FirstIsCritical = AliasFirst.CriticalCheck();
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region AliasSecond 변경통지 프로퍼티
		private FleetData _AliasSecond;

		public FleetData AliasSecond
		{
			get
			{ return this._AliasSecond; }
			set
			{
				if (this._AliasSecond == value)
					return;
				this._AliasSecond = value;
				this._AliasSecond.FleetType = FleetType.AliasSecond;
				Settings.Default.SecondIsCritical = AliasSecond.CriticalCheck();
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region EnemyFirst 변경통지 프로퍼티
		private FleetData _EnemyFirst;

		public FleetData EnemyFirst
		{
			get
			{ return this._EnemyFirst; }
			set
			{
				if (this._EnemyFirst == value)
					return;
				this._EnemyFirst = value;
				this._EnemyFirst.FleetType = FleetType.EnemyFirst;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region EnemySecond 변경통지 프로퍼티
		private FleetData _EnemySecond;

		public FleetData EnemySecond
		{
			get
			{ return this._EnemySecond; }
			set
			{
				if (this._EnemySecond == value)
					return;
				this._EnemySecond = value;
				this._EnemySecond.FleetType = FleetType.EnemySecond;
				this.RaisePropertyChanged();
			}
		}
		#endregion


		#region RankResult 변경통지 프로퍼티
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

		#region AirRankResult 변경통지 프로퍼티
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


		#region FriendAirSupremacy 변경통지 프로퍼티
		public AirSupremacy FriendAirSupremacy
		{
			get { return this._FriendAirSupremacy; }
			set
			{
				if (this._FriendAirSupremacy != value)
				{
					this._FriendAirSupremacy = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private AirSupremacy _FriendAirSupremacy = AirSupremacy.항공전없음;
		#endregion

		#region AirCombatResults 변경통지 프로퍼티
		public AirCombatResult[] AirCombatResults
		{
			get { return this._AirCombatResults; }
			set
			{
				if (!this._AirCombatResults.Equals(value))
				{
					this._AirCombatResults = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private AirCombatResult[] _AirCombatResults = new AirCombatResult[0];
		#endregion

		#region DropShipName 변경통지 프로퍼티
		public string DropShipName
		{
			get { return this._DropShipName; }
			set
			{
				if (this._DropShipName != value)
				{
					this._DropShipName = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private string _DropShipName;
		#endregion

		#endregion

		public BattleCalculator battleCalculator { get; }
		private MapDifficulty[] EventMapDifficulty = new MapDifficulty[10];

		private int CurrentDeckId { get; set; }
		private bool IsInSortie = false;

		public BattleData()
		{
			this.CurrentBattleFlag = new BattleFlags();

			this.CurrentSortie = new SortieInfo();
			this.CurrentSortie.Updated += (s, e) => this.RaisePropertyChanged(nameof(this.CurrentSortie));

			this.battleCalculator = new BattleCalculator();
			this.battleCalculator.Updated += (s, e) => this.RaisePropertyChanged(nameof(battleCalculator));

			var proxy = KanColleClient.Current.Proxy;

			#region Start / Next / Port
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_map/start")
				.Subscribe(x => this.ProcessStartNext(x));

			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_map/next")
				.Subscribe(x => this.ProcessStartNext(x, true));

			proxy.api_port.TryParse<kcsapi_port>().Subscribe(x => this.ReturnHomeport(x.Data));
			#endregion

			#region 통상 - 주간전 / 연습 - 주간전
			proxy.api_req_sortie_battle
				.TryParse<sortie_battle>().Subscribe(x => this.Update(x.Data, ApiTypes.sortie_battle));

			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_practice/battle")
				.TryParse<practice_battle>().Subscribe(x => this.Update(x.Data, ApiTypes.practice_battle));
			#endregion

			#region 통상 - 야전 / 통상 - 개막야전 / 연습 - 야전
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_battle_midnight/battle")
				.TryParse<sortie_battle_midnight>().Subscribe(x => this.Update(x.Data, ApiTypes.sortie_battle_midnight));

			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_battle_midnight/sp_midnight")
				.TryParse<battle_midnight_sp_midnight>().Subscribe(x => this.Update(x.Data, ApiTypes.sortie_battle_midnight_sp));

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
				.TryParse<combined_battle>().Subscribe(x => this.Update(x.Data, ApiTypes.combined_battle));

			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/battle_water")
				.TryParse<combined_battle_battle_water>().Subscribe(x => this.Update(x.Data, ApiTypes.combined_battle_water));
			#endregion

			#region 연합vs연합 - 주간전
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/ec_battle")
				.TryParse<combined_battle_each>().Subscribe(x => this.Update(x.Data, ApiTypes.combined_battle_ec));

			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/each_battle")
				.TryParse<combined_battle_each>().Subscribe(x => this.Update(x.Data, ApiTypes.combined_battle_each));

			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/each_battle_water")
				.TryParse<combined_battle_each>().Subscribe(x => this.Update(x.Data, ApiTypes.combined_battle_each_water));
			#endregion

			#region 연합함대 - 항공전 / 공습전
			proxy.api_req_combined_battle_airbattle
				.TryParse<combined_airbattle>().Subscribe(x => this.Update(x.Data, ApiTypes.combined_airbattle));

			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/ld_airbattle")
				.TryParse<combined_battle_ld_airbattle>().Subscribe(x => this.Update(x.Data, ApiTypes.combined_ld_airbattle));
			#endregion

			#region 연합함대 - 야전
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/midnight_battle")
				.TryParse<combined_battle_midnight>().Subscribe(x => this.Update(x.Data, ApiTypes.combined_battle_midnight));

			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/sp_midnight")
				.TryParse<combined_battle_sp_midnight>().Subscribe(x => this.Update(x.Data, ApiTypes.combined_battle_midnight_sp));
			#endregion

			#region vs 연합 - 야전
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/ec_midnight_battle")
				.TryParse<combined_battle_midnight_ec>().Subscribe(x => this.Update(x.Data, ApiTypes.combined_battle_midnight_ec));
			#endregion

			#region vs 연합 - 야전>주간전
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/ec_night_to_day")
				.TryParse<combined_battle_ec_nighttoday>().Subscribe(x => this.Update(x.Data, ApiTypes.combined_battle_ec_nighttoday));
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
					this.BattleName = "통상 - 주간전";
					break;
				case ApiTypes.practice_battle:
					this.BattleName = "연습 - 주간전";

					this.ClearBattleInfo();
					this.CurrentSortie.Practice(this);
					break;
			}

			// 적, 아군 함대 정보 갱신
			this.UpdateFleets(data.api_deck_id, data, data.api_formation);

			// 체력 갱신
			this.UpdateMaxHP(data.api_f_maxhps, data.api_e_maxhps);
			this.UpdateBefHP(data.api_f_nowhps, data.api_e_nowhps);
			this.UpdateNowHP(data.api_f_nowhps, data.api_e_nowhps);

			// 대공컷인, 지원함대 갱신
			this.CurrentBattleFlag.UpdateAntiAirFire(data.api_kouku?.api_stage2?.api_air_fire);
			this.CurrentBattleFlag.UpdateSupport(data.api_support_flag);

			// 전투 계산
			battleCalculator
				.Initialize(
					this.AliasFirst, this.AliasSecond,
					this.EnemyFirst, this.EnemySecond
				)
				.Update(data, (ApiTypes_Sortie)apiType);

			// 대파 체크
			Settings.Default.FirstIsCritical = AliasFirst.CriticalCheck();
			Settings.Default.SecondIsCritical = AliasSecond.CriticalCheck();

			// MVP 예상
			UpdateMVP(battleCalculator.MVP_First, battleCalculator.MVP_Second);

			this.FriendAirSupremacy = data.api_kouku.GetAirSupremacy();
			this.AirCombatResults =
				data.api_air_base_injection.ToResult()
					.Concat(data.api_injection_kouku.ToResult("분식 "))
					.Concat(data.api_air_base_attack.ToResult())
					.Concat(data.api_kouku.ToResult()).ToArray();

			this.RankResult = this.CalcRank();

			// 현재 노드에 전투 결과를 반영 (임시)
			this.CurrentSortie.UpdateResult(this);
		}
		private void Update(sortie_battle_midnight data, ApiTypes apiType)
		{
			AutoSelectTab();

			switch (apiType)
			{
				case ApiTypes.sortie_battle_midnight:
					this.BattleName = "통상 - 야전";
					break;
				case ApiTypes.sortie_battle_midnight_sp:
					this.BattleName = "통상 - 개막야전";
					this.FriendAirSupremacy = AirSupremacy.항공전없음;
					break;
				case ApiTypes.practice_midnight_battle:
					this.BattleName = "연습 - 야전";
					break;
			}

			if (apiType == ApiTypes.sortie_battle_midnight_sp)
			{
				// 적, 아군 함대 정보 갱신
				this.UpdateFleets(data.api_deck_id, data, data.api_formation);

				// 체력 갱신
				this.UpdateMaxHP(data.api_f_maxhps, data.api_e_maxhps);
				this.UpdateBefHP(data.api_f_nowhps, data.api_e_nowhps);
			}

			// 체력 갱신
			this.UpdateNowHP(data.api_f_nowhps, data.api_e_nowhps);

			// 지원함대, 조명탄, 야간정찰 갱신
			this.CurrentBattleFlag.UpdateSupport(0, data.api_n_support_flag);
			this.CurrentBattleFlag.UpdateFlare(data.api_flare_pos);
			this.CurrentBattleFlag.UpdateNightRecon(data.api_touch_plane);

			// 전투 계산
			if (apiType == ApiTypes.sortie_battle_midnight_sp)
				battleCalculator
					.Initialize(
						this.AliasFirst, this.AliasSecond,
						this.EnemyFirst, this.EnemySecond
					);

			battleCalculator.Update(data, (ApiTypes_SortieMidnight)apiType);

			// 대파 체크
			Settings.Default.FirstIsCritical = AliasFirst.CriticalCheck();
			Settings.Default.SecondIsCritical = AliasSecond.CriticalCheck();

			// MVP 예상
			UpdateMVP(battleCalculator.MVP_First, battleCalculator.MVP_Second);

			if (apiType == ApiTypes.sortie_battle_midnight_sp)
				this.RankResult = this.CalcRank();
			else
				this.RankResult = this.CalcRank(false, false);

			// 현재 노드에 전투 결과를 반영 (임시)
			this.CurrentSortie.UpdateResult(this);
		}
		private void Update(sortie_airbattle data, ApiTypes apiType)
		{
			AutoSelectTab();

			switch (apiType)
			{
				case ApiTypes.sortie_airbattle:
					this.BattleName = "항공전 - 주간전";
					break;
				case ApiTypes.sortie_ld_airbattle:
					this.BattleName = "공습전 - 주간전";
					break;
			}

			// 적, 아군 함대 정보 갱신
			this.UpdateFleets(data.api_deck_id, data, data.api_formation);

			// 체력 갱신
			this.UpdateMaxHP(data.api_f_maxhps, data.api_e_maxhps);
			this.UpdateBefHP(data.api_f_nowhps, data.api_e_nowhps);
			this.UpdateNowHP(data.api_f_nowhps, data.api_e_nowhps);

			// 대공컷인, 지원함대 갱신
			if (apiType == ApiTypes.sortie_airbattle)
				this.CurrentBattleFlag.UpdateAntiAirFire(
					data.api_kouku?.api_stage2?.api_air_fire,
					data.api_kouku2?.api_stage2?.api_air_fire
				);
			else
				this.CurrentBattleFlag.UpdateAntiAirFire(data.api_kouku?.api_stage2?.api_air_fire);

			this.CurrentBattleFlag.UpdateSupport(data.api_support_flag);

			// 전투 계산
			battleCalculator
				.Initialize(
					this.AliasFirst, this.AliasSecond,
					this.EnemyFirst, this.EnemySecond
				)
				.Update(data, (ApiTypes_SortieAirBattle)apiType);

			// 대파 체크
			Settings.Default.FirstIsCritical = AliasFirst.CriticalCheck();
			Settings.Default.SecondIsCritical = AliasSecond.CriticalCheck();

			// MVP 예상
			UpdateMVP(battleCalculator.MVP_First, battleCalculator.MVP_Second);

			this.FriendAirSupremacy = data.api_kouku.GetAirSupremacy();
			this.AirCombatResults =
				data.api_air_base_injection.ToResult()
					.Concat(data.api_injection_kouku.ToResult("분식 "))
					.Concat(data.api_air_base_attack.ToResult())
					.Concat(data.api_kouku.ToResult()).ToArray();

			if (apiType == ApiTypes.sortie_airbattle)
			{
				this.AirCombatResults =
					data.api_air_base_injection.ToResult()
						.Concat(data.api_injection_kouku.ToResult("분식 "))
						.Concat(data.api_air_base_attack.ToResult())
						.Concat(data.api_kouku.ToResult("1회차/"))
						.Concat(data.api_kouku2.ToResult("2회차/"))
						.ToArray();

				this.RankResult = this.CalcRank();
			}
			else
			{
				this.AirCombatResults =
					data.api_air_base_attack.ToResult()
						.Concat(data.api_kouku.ToResult()).ToArray();

				this.AirRankResult = this.CalcLDAirRank();
				this.RankResult = Rank.공습전;
			}

			// 현재 노드에 전투 결과를 반영 (임시)
			this.CurrentSortie.UpdateResult(this);
		}

		private void Update(combined_battle data, ApiTypes apiType)
		{
			AutoSelectTab();

			switch (apiType)
			{
				case ApiTypes.combined_battle:
					this.BattleName = "연합함대 - 기동부대 - 주간전";
					break;
				case ApiTypes.combined_battle_water:
					this.BattleName = "연합함대 - 수상부대 - 주간전";
					break;
			}

			// 적, 아군 함대 정보 갱신
			this.UpdateFleets(data.api_deck_id, data, data.api_formation);

			// 체력 갱신
			this.UpdateMaxHP(data.api_f_maxhps, data.api_e_maxhps, data.api_f_maxhps_combined);
			this.UpdateBefHP(data.api_f_nowhps, data.api_e_nowhps, data.api_f_maxhps_combined);
			this.UpdateNowHP(data.api_f_nowhps, data.api_e_nowhps, data.api_f_nowhps_combined);

			// 대공컷인, 지원함대 갱신
			this.CurrentBattleFlag.UpdateAntiAirFire(data.api_kouku?.api_stage2?.api_air_fire);
			this.CurrentBattleFlag.UpdateSupport(data.api_support_flag);

			// 전투 계산
			battleCalculator
				.Initialize(
					this.AliasFirst, this.AliasSecond,
					this.EnemyFirst, this.EnemySecond
				)
				.Update(data, (ApiTypes_CombinedBattle)apiType);

			// 대파 체크
			Settings.Default.FirstIsCritical = AliasFirst.CriticalCheck();
			Settings.Default.SecondIsCritical = AliasSecond.CriticalCheck();

			// MVP 예상
			UpdateMVP(battleCalculator.MVP_First, battleCalculator.MVP_Second);

			this.FriendAirSupremacy = data.api_kouku.GetAirSupremacy();
			this.AirCombatResults =
				data.api_air_base_injection.ToResult()
					.Concat(data.api_injection_kouku.ToResult("분식 "))
					.Concat(data.api_air_base_attack.ToResult())
					.Concat(data.api_kouku.ToResult()).ToArray();

			this.RankResult = this.CalcRank(true);

			// 현재 노드에 전투 결과를 반영 (임시)
			this.CurrentSortie.UpdateResult(this);
		}
		private void Update(combined_battle_each data, ApiTypes apiType)
		{
			AutoSelectTab();

			switch (apiType)
			{
				case ApiTypes.combined_battle_ec:
					this.BattleName = "통상함대vs심해연합 - 주간전";
					break;
				case ApiTypes.combined_battle_each:
					this.BattleName = "기동부대vs심해연합 - 주간전";
					break;
				case ApiTypes.combined_battle_each_water:
					this.BattleName = "수상부대vs심해연합 - 주간전";
					break;
			}

			// 적, 아군 함대 정보 갱신
			this.UpdateFleets(data.api_deck_id, data, data.api_formation);

			// 체력 갱신
			this.UpdateMaxHP(data.api_f_maxhps, data.api_e_maxhps, data.api_f_maxhps_combined, data.api_e_maxhps_combined);
			this.UpdateBefHP(data.api_f_nowhps, data.api_e_nowhps, data.api_f_maxhps_combined, data.api_e_nowhps_combined);
			this.UpdateNowHP(data.api_f_nowhps, data.api_e_nowhps, data.api_f_nowhps_combined, data.api_e_nowhps_combined);

			// 대공컷인, 지원함대 갱신
			this.CurrentBattleFlag.UpdateAntiAirFire(data.api_kouku?.api_stage2?.api_air_fire);
			this.CurrentBattleFlag.UpdateSupport(data.api_support_flag);

			// 전투 계산
			battleCalculator
				.Initialize(
					this.AliasFirst, this.AliasSecond,
					this.EnemyFirst, this.EnemySecond
				)
				.Update(data, (ApiTypes_CombinedBattleEC)apiType);

			// 대파 체크
			Settings.Default.FirstIsCritical = AliasFirst.CriticalCheck();
			Settings.Default.SecondIsCritical = AliasSecond.CriticalCheck();

			// MVP 예상
			UpdateMVP(battleCalculator.MVP_First, battleCalculator.MVP_Second);

			this.FriendAirSupremacy = data.api_kouku.GetAirSupremacy();
			this.AirCombatResults =
				data.api_air_base_injection.ToResult()
					.Concat(data.api_injection_kouku.ToResult("분식 "))
					.Concat(data.api_air_base_attack.ToResult())
					.Concat(data.api_kouku.ToResult()).ToArray();

			this.RankResult = this.CalcRank(true, true);

			// 현재 노드에 전투 결과를 반영 (임시)
			this.CurrentSortie.UpdateResult(this);
		}
		private void Update(combined_airbattle data, ApiTypes apiType)
		{
			AutoSelectTab();

			switch (apiType)
			{
				case ApiTypes.combined_airbattle:
					this.BattleName = "연합함대 - 항공전 - 주간";
					break;
				case ApiTypes.combined_ld_airbattle:
					this.BattleName = "연합함대 - 공습전 - 주간";
					break;
			}

			// 적, 아군 함대 정보 갱신
			this.UpdateFleets(data.api_deck_id, data, data.api_formation);

			// 체력 갱신
			this.UpdateMaxHP(data.api_f_maxhps, data.api_e_maxhps, data.api_f_maxhps_combined);
			this.UpdateBefHP(data.api_f_nowhps, data.api_e_nowhps, data.api_f_maxhps_combined);
			this.UpdateNowHP(data.api_f_nowhps, data.api_e_nowhps, data.api_f_nowhps_combined);

			// 대공컷인, 지원함대 갱신
			if (apiType == ApiTypes.combined_airbattle)
				this.CurrentBattleFlag.UpdateAntiAirFire(
					data.api_kouku?.api_stage2?.api_air_fire,
					data.api_kouku2?.api_stage2?.api_air_fire
				);
			else
				this.CurrentBattleFlag.UpdateAntiAirFire(data.api_kouku?.api_stage2?.api_air_fire);

			this.CurrentBattleFlag.UpdateSupport(data.api_support_flag);

			// 전투 계산
			battleCalculator
				.Initialize(
					this.AliasFirst, this.AliasSecond,
					this.EnemyFirst, this.EnemySecond
				)
				.Update(data, (ApiTypes_CombinedAirBattle)apiType);

			// 대파 체크
			Settings.Default.FirstIsCritical = AliasFirst.CriticalCheck();
			Settings.Default.SecondIsCritical = AliasSecond.CriticalCheck();

			// MVP 예상
			UpdateMVP(battleCalculator.MVP_First, battleCalculator.MVP_Second);

			this.FriendAirSupremacy = data.api_kouku.GetAirSupremacy();

			switch (apiType)
			{
				case ApiTypes.combined_airbattle:
					this.AirCombatResults =
						data.api_air_base_injection.ToResult()
							.Concat(data.api_injection_kouku.ToResult("분식 "))
							.Concat(data.api_air_base_attack.ToResult())
							.Concat(data.api_kouku.ToResult("1회차/"))
							.Concat(data.api_kouku2.ToResult("2회차/"))
							.ToArray();
					this.RankResult = this.CalcRank(true);
					break;

				case ApiTypes.combined_ld_airbattle:
					this.AirCombatResults =
						data.api_air_base_injection.ToResult()
							.Concat(data.api_injection_kouku.ToResult("분식 "))
							.Concat(data.api_air_base_attack.ToResult())
							.Concat(data.api_kouku.ToResult()).ToArray();

					this.AirRankResult = this.CalcLDAirRank(true);
					this.RankResult = Rank.공습전;
					break;
			}

			// 현재 노드에 전투 결과를 반영 (임시)
			this.CurrentSortie.UpdateResult(this);
		}

		private void Update(combined_battle_midnight data, ApiTypes apiType)
		{
			AutoSelectTab();

			switch (apiType)
			{
				case ApiTypes.combined_battle_midnight:
					this.BattleName = "연합함대 - 야전";
					break;
				case ApiTypes.combined_battle_midnight_sp:
					this.BattleName = "연합함대 - 개막야전";
					this.FriendAirSupremacy = AirSupremacy.항공전없음;
					break;
			}

			if (apiType == ApiTypes.combined_battle_midnight_sp)
			{
				// 적, 아군 함대 정보 갱신
				this.UpdateFleets(data.api_deck_id, data, data.api_formation);

				// 체력 갱신
				this.UpdateMaxHP(data.api_f_maxhps, data.api_e_maxhps, data.api_f_maxhps_combined);
				this.UpdateBefHP(data.api_f_nowhps, data.api_e_nowhps, data.api_f_maxhps_combined);
			}

			// 체력 갱신
			this.UpdateNowHP(data.api_f_nowhps, data.api_e_nowhps, data.api_f_nowhps_combined);

			// 지원함대, 조명탄, 야간정찰 갱신
			this.CurrentBattleFlag.UpdateSupport(0, data.api_n_support_flag);
			this.CurrentBattleFlag.UpdateFlare(data.api_flare_pos);
			this.CurrentBattleFlag.UpdateNightRecon(data.api_touch_plane);

			if (apiType == ApiTypes.combined_battle_midnight_sp)
				battleCalculator
					.Initialize(
						this.AliasFirst, this.AliasSecond,
						this.EnemyFirst, this.EnemySecond
					);

			battleCalculator.Update(data, (ApiTypes_CombinedMidnight)apiType);

			// 대파 체크
			Settings.Default.FirstIsCritical = AliasFirst.CriticalCheck();
			Settings.Default.SecondIsCritical = AliasSecond.CriticalCheck();

			// MVP 예상
			UpdateMVP(battleCalculator.MVP_First, battleCalculator.MVP_Second);

			switch (apiType)
			{
				case ApiTypes.combined_battle_midnight:
					this.RankResult = this.CalcRank(true, true);
					break;
				case ApiTypes.combined_battle_midnight_sp:
					this.RankResult = this.CalcRank(true);
					break;
			}

			// 현재 노드에 전투 결과를 반영 (임시)
			this.CurrentSortie.UpdateResult(this);
		}
		private void Update(combined_battle_midnight_ec data, ApiTypes apiType)
		{
			AutoSelectTab();
			this.BattleName = "vs심해연합 - 야전";

			// 적, 아군 함대 정보 갱신
			this.UpdateFleetsCombinedEnemy(data.api_deck_id, data);

			// 체력 갱신
			this.UpdateNowHP(data.api_f_nowhps, data.api_e_nowhps, data.api_f_nowhps_combined, data.api_e_nowhps_combined);

			// 지원함대, 조명탄, 야간정찰 갱신
			this.CurrentBattleFlag.UpdateSupport(0, data.api_n_support_flag);
			this.CurrentBattleFlag.UpdateFlare(data.api_flare_pos);
			this.CurrentBattleFlag.UpdateNightRecon(data.api_touch_plane);

			// 전투 계산
			battleCalculator.Update(data);

			// 대파 체크
			Settings.Default.FirstIsCritical = AliasFirst.CriticalCheck();
			Settings.Default.SecondIsCritical = AliasSecond.CriticalCheck();

			// MVP 예상
			UpdateMVP(battleCalculator.MVP_First, battleCalculator.MVP_Second);

			this.RankResult = this.CalcRank(true, true);

			// 현재 노드에 전투 결과를 반영 (임시)
			this.CurrentSortie.UpdateResult(this);
		}
		private void Update(combined_battle_ec_nighttoday data, ApiTypes apiType)
		{
			AutoSelectTab();
			this.BattleName = "vs심해연합 - 야전>주간전";

			// 적, 아군 함대 정보 갱신
			this.UpdateFleetsCombinedEnemy(data.api_deck_id, data);

			// 체력 갱신
			this.UpdateMaxHP(data.api_f_maxhps, data.api_e_maxhps, data.api_f_maxhps_combined, data.api_e_maxhps_combined);
			this.UpdateBefHP(data.api_f_nowhps, data.api_e_nowhps, data.api_f_maxhps_combined, data.api_e_nowhps_combined);
			this.UpdateNowHP(data.api_f_nowhps, data.api_e_nowhps, data.api_f_nowhps_combined, data.api_e_nowhps_combined);

			// 대공컷인, 지원함대, 조명탄, 야간정찰 갱신
			this.CurrentBattleFlag.UpdateAntiAirFire(data.api_kouku?.api_stage2?.api_air_fire);
			this.CurrentBattleFlag.UpdateSupport(data.api_support_flag, data.api_n_support_flag);
			this.CurrentBattleFlag.UpdateFlare(data.api_flare_pos);
			this.CurrentBattleFlag.UpdateNightRecon(data.api_touch_plane);

			// 전투 계산
			battleCalculator.Update(data);

			// 대파 체크
			Settings.Default.FirstIsCritical = AliasFirst.CriticalCheck();
			Settings.Default.SecondIsCritical = AliasSecond.CriticalCheck();

			// MVP 예상
			UpdateMVP(battleCalculator.MVP_First, battleCalculator.MVP_Second);

			this.FriendAirSupremacy = data.api_kouku.GetAirSupremacy();
			this.AirCombatResults =
				data.api_air_base_injection.ToResult()
					.Concat(data.api_injection_kouku.ToResult("분식 "))
					.Concat(data.api_air_base_attack.ToResult())
					.Concat(data.api_kouku.ToResult()).ToArray();

			this.RankResult = this.CalcRank(true, true);

			// 현재 노드에 전투 결과를 반영 (임시)
			this.CurrentSortie.UpdateResult(this);
		}

		#endregion

		public void Update(battle_result data)
		{
			//this.DropShipName = KanColleClient.Current.Translations.GetTranslation(data.api_get_ship?.api_ship_name, TranslationType.Ships, true);
			this.DropShipName = KanColleClient.Current.Master.Ships
				.SingleOrDefault(x => x.Value.Id == data.api_get_ship?.api_ship_id).Value
				?.Name;

			if (this.DropShipName == null)
				this.DropShipName = "(없음)";

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
				this.CurrentBattleFlag.MapExtended = (data.api_m1.Value == 1);

			// 현재 노드에 전투 결과를 반영
			this.CurrentSortie.UpdateResult(this);

			// MVP 갱신
			UpdateMVP(data.api_mvp, data.api_mvp_combined ?? 0);
		}

		/// <summary>
		/// 계산 혹은 전달된 MVP 를 지정
		/// </summary>
		/// <param name="mvp1"></param>
		/// <param name="mvp2"></param>
		private void UpdateMVP(int mvp1 = 0, int mvp2 = 0)
		{
			var firstMvp = new bool[6];
			var secondMvp = new bool[6];

			if (mvp1 > 0) firstMvp[(mvp1 - 1) % 6] = true;
			if (AliasFirst != null) AliasFirst.Ships.SetValues(firstMvp, (s, v) => s.IsMvp = v);

			if (mvp2 > 0) secondMvp[(mvp2 - 1) % 6] = true;
			if (AliasSecond != null) AliasSecond.Ships.SetValues(secondMvp, (s, v) => s.IsMvp = v);
		}

		/// <summary>
		/// 출격 혹은 진격 데이터 처리
		/// </summary>
		/// <param name="session">Raw 세션</param>
		/// <param name="isNext">진격에 의한 호출인지 여부</param>
		private void ProcessStartNext(Nekoxy.Session session, bool isNext = false)
		{
			SvData<map_start_next> data;
			if (!SvData.TryParse<map_start_next>(session, out data)) return;

			var startNext = data.Data;
			if (!isNext) this.CurrentDeckId = int.Parse(data.Request["api_deck_id"]);
			if (this.CurrentDeckId < 1) return;

			// 아군 함대 정보 갱신
			this.UpdateFriendFleets(this.CurrentDeckId);

			// 출격중
			this.IsInSortie = true;

			// 출격시 맵 정보 초기화
			if (!isNext)
			{
				CurrentSortie.Initialize(
					data.Data.api_maparea_id,
					data.Data.api_mapinfo_no,
					data.Data.api_eventmap != null ? EventMapDifficulty[data.Data.api_mapinfo_no] : MapDifficulty.None
				);
			}

			// 전투 관련 정보들 초기화
			this.ClearBattleInfo();

			// 노드 방문만으로 맵이 확장되는 경우
			if (data.Data.api_m1.HasValue)
				this.CurrentBattleFlag.MapExtended = (data.Data.api_m1.Value == 1);

			// 노드 방문
			CurrentSortie.Update(
				data.Data.api_no,
				data.Data.api_event_id,
				data.Data.api_event_kind,
				session,
				this
			);

			// 탭 자동 선택
			AutoSelectTab();
		}

		/// <summary>
		/// 적 함대, 전투 이름, MVP 초기화 등 처리 전 작업
		/// </summary>
		private void ClearBattleInfo()
		{
			// Reset enemy fleet
			this.EnemyFirst = new FleetData();
			this.EnemySecond = new FleetData();
			if (this.AliasFirst != null) this.AliasFirst.Formation = Formation.없음;

			#region 전투 정보 초기화
			// 시간 갱신, 전투명 및 드롭 초기화
			this.UpdatedTime = DateTimeOffset.Now;
			this.BattleName = "";
			this.DropShipName = null;

			// MVP 초기화
			if (this.AliasFirst != null) AliasFirst.Ships.SetValues(new bool[6], (s, v) => s.IsMvp = v);
			if (this.AliasSecond != null) AliasSecond.Ships.SetValues(new bool[6], (s, v) => s.IsMvp = v);

			// 각종 플래그들 초기화 (조명탄, 야정 등)
			this.CurrentBattleFlag.Clear();

			// 환경 초기화
			this.BattleSituation = BattleSituation.없음;
			this.FriendAirSupremacy = AirSupremacy.항공전없음;
			this.AirCombatResults = new AirCombatResult[0];

			// 결과 초기화
			this.RankResult = Rank.없음;
			this.AirRankResult = Rank.없음;
			#endregion
		}

		/// <summary>
		/// 적 및 아군 함대 정보 갱신 (단일함대)
		/// </summary>
		/// <param name="api_deck_id">아군 함대 번호</param>
		/// <param name="data">결과 데이터</param>
		/// <param name="api_formation">아군, 적 진형과 전투형태 정보</param>
		private void UpdateFleets(int api_deck_id, ICommonBattleMembers data, int[] api_formation = null)
		{
			this.UpdatedTime = DateTimeOffset.Now;

			// 아군 정보 모항 기준으로 갱신
			this.UpdateFriendFleets(api_deck_id);

			this.EnemyFirst = new FleetData(
				data.ToMastersShipDataArray(),
				this.EnemyFirst?.Formation ?? Formation.없음,
				this.EnemyFirst?.Name ?? "",
				FleetType.EnemyFirst
			);

			// 제 2함대는 없음
			this.EnemySecond = new FleetData(
				new MembersShipData[0],
				Formation.없음,
				"",
				FleetType.EnemySecond
			);

			// 진형과 전투형태 존재하면
			if (api_formation != null)
			{
				this.BattleSituation = (BattleSituation)api_formation[2];

				if (this.AliasFirst != null)
					this.AliasFirst.Formation = (Formation)api_formation[0];

				if (this.EnemyFirst != null)
					this.EnemyFirst.Formation = (Formation)api_formation[1];
			}

			// 출격중인 함대 번호 갱신
			this.CurrentDeckId = api_deck_id;
		}

		/// <summary>
		/// 적 및 아군 함대 정보 갱신 (연합함대)
		/// </summary>
		/// <param name="api_deck_id">아군 함대 번호</param>
		/// <param name="data">결과 데이터</param>
		/// <param name="api_formation">아군, 적 진형과 전투형태 정보</param>
		private void UpdateFleetsCombinedEnemy(int api_deck_id, ICommonEachBattleMembers data, int[] api_formation = null)
		{
			this.UpdatedTime = DateTimeOffset.Now;

			// 아군 정보 모항 기준으로 갱신
			this.UpdateFriendFleets(api_deck_id);

			this.EnemyFirst = new FleetData(
				data.ToMastersShipDataArray(),
				this.EnemyFirst?.Formation ?? Formation.없음,
				this.EnemyFirst?.Name ?? "",
				FleetType.EnemyFirst
			);

			this.EnemySecond = new FleetData(
				data.ToMastersSecondShipDataArray(),
				this.EnemySecond?.Formation ?? Formation.없음,
				this.EnemySecond?.Name ?? "",
				FleetType.EnemySecond
			);

			// 진형과 전투형태 존재하면
			if (api_formation != null)
			{
				this.BattleSituation = (BattleSituation)api_formation[2];

				if (this.AliasFirst != null)
					this.AliasFirst.Formation = (Formation)api_formation[0];

				if (this.EnemyFirst != null)
					this.EnemyFirst.Formation = (Formation)api_formation[1];
			}

			// 출격중인 함대 번호 갱신
			this.CurrentDeckId = api_deck_id;
		}

		/// <summary>
		/// 아군 정보 갱신 (모항 데이터 기준)
		/// </summary>
		/// <param name="deckID"></param>
		private void UpdateFriendFleets(int deckID)
		{
			var organization = KanColleClient.Current.Homeport.Organization;

			this.AliasFirst = new FleetData(
				organization.Fleets[deckID].Ships.Select(s => new MembersShipData(s)).ToArray(),
				this.AliasFirst?.Formation ?? Formation.없음,
				organization.Fleets[deckID].Name,
				FleetType.AliasFirst
			);
			this.AliasSecond = new FleetData(
				organization.Combined && deckID == 1
					? organization.Fleets[2].Ships.Select(s => new MembersShipData(s)).ToArray()
					: new MembersShipData[0],
				this.AliasSecond?.Formation ?? Formation.없음,
				organization.Fleets[2].Name,
				FleetType.AliasSecond
			);
		}

		/// <summary>
		/// 최대 HP 갱신
		/// </summary>
		/// <param name="api_f_maxhps">아군 1함대</param>
		/// <param name="api_e_maxhps">적군 1함대</param>
		/// <param name="api_f_maxhps_combined">아군 2함대</param>
		/// <param name="api_e_maxhps_combined">적군 2함대</param>
		private void UpdateMaxHP(int[] api_f_maxhps, int[] api_e_maxhps, int[] api_f_maxhps_combined = null, int[] api_e_maxhps_combined = null)
		{
			this.AliasFirst.Ships.SetValues(api_f_maxhps, (s, v) => s.MaxHP = v);
			if (api_f_maxhps_combined != null)
				this.AliasSecond.Ships.SetValues(api_f_maxhps_combined, (s, v) => s.MaxHP = v);

			this.EnemyFirst.Ships.SetValues(api_e_maxhps, (s, v) => s.MaxHP = v);
			if (api_e_maxhps_combined != null)
				this.EnemySecond.Ships.SetValues(api_e_maxhps_combined, (s, v) => s.MaxHP = v);
		}

		/// <summary>
		/// 현재 HP 갱신
		/// </summary>
		/// <param name="api_f_nowhps">아군 1함대</param>
		/// <param name="api_e_nowhps">적군 1함대</param>
		/// <param name="api_f_nowhps_combined">아군 2함대</param>
		/// <param name="api_e_nowhps_combined">적군 2함대</param>
		private void UpdateNowHP(int[] api_f_nowhps, int[] api_e_nowhps, int[] api_f_nowhps_combined = null, int[] api_e_nowhps_combined = null)
		{
			this.AliasFirst.Ships.SetValues(api_f_nowhps, (s, v) => s.NowHP = v);
			if (api_f_nowhps_combined != null)
				this.AliasSecond.Ships.SetValues(api_f_nowhps_combined, (s, v) => s.NowHP = v);

			this.EnemyFirst.Ships.SetValues(api_e_nowhps, (s, v) => s.NowHP = v);
			if (api_e_nowhps_combined != null)
				this.EnemySecond.Ships.SetValues(api_e_nowhps_combined, (s, v) => s.NowHP = v);
		}

		/// <summary>
		/// 현재 HP 갱신
		/// </summary>
		/// <param name="api_f_nowhps">아군 1함대</param>
		/// <param name="api_e_nowhps">적군 1함대</param>
		/// <param name="api_f_nowhps_combined">아군 2함대</param>
		/// <param name="api_e_nowhps_combined">적군 2함대</param>
		private void UpdateBefHP(int[] api_f_nowhps, int[] api_e_nowhps, int[] api_f_nowhps_combined = null, int[] api_e_nowhps_combined = null)
		{
			this.AliasFirst.Ships.SetValues(api_f_nowhps, (s, v) => s.BeforeNowHP = v);
			if (api_f_nowhps_combined != null)
				this.AliasSecond.Ships.SetValues(api_f_nowhps_combined, (s, v) => s.BeforeNowHP = v);

			this.EnemyFirst.Ships.SetValues(api_e_nowhps, (s, v) => s.BeforeNowHP = v);
			if (api_e_nowhps_combined != null)
				this.EnemySecond.Ships.SetValues(api_e_nowhps_combined, (s, v) => s.BeforeNowHP = v);
		}

		/// <summary>
		/// 모항으로 복귀했을 경우
		/// </summary>
		/// <param name="port"></param>
		private void ReturnHomeport(kcsapi_port port)
		{
			if (this.IsInSortie) AutoBackTab();
			this.IsInSortie = false;

			this.CurrentBattleFlag.MechanismOn = (port.api_event_object?.api_m_flag2 == 1);
		}

		private Rank CalcRank(bool IsCombined = false, bool IsEnemyCombined = false)
		{
			try
			{
				var AliasFirstShips = this.AliasFirst.Ships
					.Where(x => !x.Situation.HasFlag(ShipSituation.Tow) && !x.Situation.HasFlag(ShipSituation.Evacuation));
				var ShipCount = AliasFirstShips.Count();
				var SinkCount = battleCalculator.AliasFirstShips.Count(x => x?.Source.NowHP <= 0);
				var AliasMax = AliasFirstShips.Sum(x => x?.BeforeNowHP ?? 0);
				var AliasDamaged = battleCalculator.AliasFirstShips.Sum(x => x?.HPChangedValue ?? 0);

				var EnemyFirstShips = this.EnemyFirst.Ships;
				var EnemyShipCount = EnemyFirstShips.Count();
				var EnemySinkCount = battleCalculator.EnemyFirstShips.Count(x => x?.Source.NowHP <= 0);
				var EnemyMax = EnemyFirstShips.Sum(x => x?.BeforeNowHP ?? 0);
				var EnemyDamaged = battleCalculator.EnemyFirstShips.Sum(x => x?.HPChangedValue ?? 0);

				if (IsCombined)
				{
					var AliasSecondShips = this.AliasSecond.Ships
						.Where(x => !x.Situation.HasFlag(ShipSituation.Tow) && !x.Situation.HasFlag(ShipSituation.Evacuation));
					ShipCount += AliasSecondShips.Count();
					SinkCount += battleCalculator.AliasSecondShips.Count(x => x?.Source.NowHP <= 0);
					AliasMax += AliasSecondShips.Sum(x => x?.BeforeNowHP ?? 0);
					AliasDamaged += battleCalculator.AliasSecondShips.Sum(x => x?.HPChangedValue ?? 0);
				}
				if (IsEnemyCombined)
				{
					var EnemySecondShips = this.EnemySecond.Ships;
					EnemyShipCount += EnemySecondShips.Count();
					EnemySinkCount += battleCalculator.EnemySecondShips.Count(x => x?.Source.NowHP <= 0);
					EnemyMax += EnemySecondShips.Sum(x => x?.BeforeNowHP ?? 0);
					EnemyDamaged += battleCalculator.EnemySecondShips.Sum(x => x?.HPChangedValue ?? 0);
				}

				var IsShipSink = SinkCount > 0;
				var flagshipSink = this.EnemyFirst.Ships.First().NowHP < 0;

				decimal AliasDamagedPercent = AliasDamaged / (decimal)AliasMax; // 아군이 받은 총 데미지
				decimal EnemyDamagedPercent = EnemyDamaged / (decimal)EnemyMax; // 적군이 받은 총 데미지

				decimal DamageRate = AliasDamagedPercent == 0
					? -1 // same with x2.5
					: (decimal)EnemyDamagedPercent / AliasDamagedPercent;

				this.AliasFirst.AttackGauge = this.MakeGaugeText(EnemyDamaged, EnemyMax, EnemyDamagedPercent);
				this.EnemyFirst.AttackGauge = this.MakeGaugeText(AliasDamaged, AliasMax, AliasDamagedPercent);

				if (!IsShipSink)
				{
					if (EnemySinkCount == EnemyShipCount)
					{
						if (AliasDamaged == 0)
							return Rank.완전승리S;
						else
							return Rank.S승리;
					}
					else if (EnemyShipCount > 1 && (EnemySinkCount >= (int)Math.Floor(0.7 * EnemyShipCount)))
						return Rank.A승리;
				}
				if (flagshipSink && SinkCount < EnemySinkCount)
					return Rank.B승리;
				else if (ShipCount == 1 && AliasDamagedPercent > 0.75m)
					return Rank.D패배;
				else if (EnemyDamagedPercent * 2 > AliasDamagedPercent * 5)
					return Rank.B승리;
				else if (EnemyDamagedPercent * 10 > AliasDamagedPercent * 9)
					return Rank.C패배;
				else if (SinkCount > 0 && (ShipCount - SinkCount) == 1)
					return Rank.E패배;
				else
					return Rank.D패배;
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
				var AliasFirstShips = this.AliasFirst.Ships
					.Where(x => !x.Situation.HasFlag(ShipSituation.Tow) && !x.Situation.HasFlag(ShipSituation.Evacuation));
				var SinkCount = battleCalculator.AliasFirstShips.Count(x => x?.Source.NowHP < 0);
				var AliasMax = AliasFirstShips.Sum(x => x?.BeforeNowHP ?? 0);
				var AliasDamaged = battleCalculator.AliasFirstShips.Sum(x => x?.TotalDamaged ?? 0);

				if (IsCombined)
				{
					var AliasSecondShips = this.AliasSecond.Ships
						.Where(x => !x.Situation.HasFlag(ShipSituation.Tow) && !x.Situation.HasFlag(ShipSituation.Evacuation));

					SinkCount += battleCalculator.AliasSecondShips.Count(x => x.Source.NowHP < 0);

					AliasMax += AliasSecondShips.Sum(x => x.BeforeNowHP);
					AliasDamaged += battleCalculator.AliasSecondShips.Sum(x => x.TotalDamaged);
				}

				var IsShipSink = SinkCount > 0;
				decimal AliasDamagedPercent = AliasDamaged / (decimal)AliasMax; // 아군이 받은 총 데미지

				this.AliasFirst.AttackGauge = ""; // 항공전/공습전은 받은 데미지로만 계산됨
				this.EnemyFirst.AttackGauge = this.MakeGaugeText(AliasDamaged, AliasMax, AliasDamagedPercent);

				if (AliasDamaged > 0)
				{
					int AliasValue = (int)decimal.Floor(AliasDamagedPercent * 100); // 아군 피격 데미지 비율 (소숫점 제외)

					if (SinkCount > 0) return Rank.E패배;
					else if (AliasValue >= 50) return Rank.D패배;
					else if (AliasValue >= 20) return Rank.C패배;
					else if (AliasValue >= 10) return Rank.B승리;
					else return Rank.A승리;
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

		private string MakeGaugeText(int current, int max, decimal percent)
			=> string.Format(
				"({0}/{1}) {2}%",
				current,
				max,
				Math.Round(percent * 100, 2, MidpointRounding.AwayFromZero)
			);

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
