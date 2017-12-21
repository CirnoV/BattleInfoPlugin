using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using BattleInfoPlugin.Models;
using Livet;

namespace BattleInfoPlugin.ViewModels
{
	public class HistoryWindowViewModel : ViewModel
	{
		public string BattleName { get; }
		public string UpdatedTime { get; }
		public string BattleSituation { get; }
		public string FriendAirSupremacy { get; }

		public BaseNodeData CurrentNode { get; }
		public BattleFlags CurrentBattleFlag { get; }

		public string RankResult { get; }
		public string AirRankResult { get; }
		public bool AirRankAvailable { get; }

		public AirCombatResult[] AirCombatResults { get; }
		public string DropShipName { get; }

		public FleetViewModel AliasFirst { get; }
		public FleetViewModel AliasSecond { get; }
		public FleetViewModel EnemySecond { get; }
		public FleetViewModel EnemyFirst { get; }

		public BattleCalculator BattleCalculator { get; }

		// public string DetailBattleLog { get; }
		public RefinedDamageLog[] DetailBattleLog { get; }

		public HistoryWindowViewModel(BaseNodeData nodeData)
		{
			var BattleHistory = nodeData?.BattleData;

			this.UpdatedTime = nodeData?.NodeTime != default(DateTimeOffset)
					? nodeData?.NodeTime.ToString("HH:mm:ss") // yyyy/MM/dd
					: "No Data";

			this.BattleName = BattleHistory?.BattleName ?? "";
			this.BattleSituation = BattleHistory != null && BattleHistory.BattleSituation != Models.BattleSituation.없음
					? BattleHistory.BattleSituation.ToString()
					: "";
			this.FriendAirSupremacy = BattleHistory != null && BattleHistory.FriendAirSupremacy != AirSupremacy.항공전없음
					? BattleHistory.FriendAirSupremacy.ToString()
					: "";

			this.CurrentNode = nodeData ?? new BaseNodeData();
			this.CurrentBattleFlag = nodeData?.BattleFlags ?? new BattleFlags();

			this.RankResult = BattleHistory?.RankResult.ToString() ?? "";
			this.AirRankResult = BattleHistory?.AirRankResult.ToString() ?? "";

			this.AirRankAvailable = BattleHistory?.RankResult == Rank.공습전;

			this.DropShipName = BattleHistory?.DropShipName ?? "";

			this.AirCombatResults = BattleHistory?.AirCombatResults ?? new AirCombatResult[0];

			this.AliasFirst = new FleetViewModel("기본함대");
			this.AliasSecond = new FleetViewModel("호위함대");
			this.EnemySecond = new FleetViewModel("적호위함대");
			this.EnemyFirst = new FleetViewModel("적함대");
			this.AliasFirst.Fleet = nodeData?.AliasFirst;
			this.AliasSecond.Fleet = nodeData?.AliasSecond;
			this.EnemyFirst.Fleet = nodeData?.EnemyFirst;
			this.EnemySecond.Fleet = nodeData?.EnemySecond;

			this.AliasFirst.AirCombatResults = this.AirCombatResults.Select(x => new AirCombatResultViewModel(x, FleetType.AliasFirst)).ToArray();
			this.AliasSecond.AirCombatResults = this.AirCombatResults.Select(x => new AirCombatResultViewModel(x, FleetType.AliasSecond)).ToArray();
			this.EnemySecond.AirCombatResults = this.AirCombatResults.Select(x => new AirCombatResultViewModel(x, FleetType.EnemySecond)).ToArray();
			this.EnemyFirst.AirCombatResults = this.AirCombatResults.Select(x => new AirCombatResultViewModel(x, FleetType.EnemyFirst)).ToArray();

			this.BattleCalculator = BattleHistory?.BattleCalculator ?? new BattleCalculator();

			if (this.BattleCalculator != null)
			{
				bool aliasCombined = false, enemyCombined = false;
				var aliasLogs = new BattleCalculator.DamageLog[0];
				var enemyLogs = new BattleCalculator.DamageLog[0];

				if (this.BattleCalculator.AliasFirstShips != null)
					aliasLogs = aliasLogs.Concat(
						this.BattleCalculator.AliasFirstShips
							.Where(x => x != null)
							.SelectMany(x => x.DamageList)
					).ToArray();
				if (this.BattleCalculator.AliasSecondShips != null) {
					aliasCombined = true;
					aliasLogs = aliasLogs.Concat(
						this.BattleCalculator.AliasSecondShips
							.Where(x => x != null)
							.SelectMany(x => x.DamageList)
					).ToArray();
				}

				if (this.BattleCalculator.EnemySecondShips != null)
					enemyCombined = true;

				/*
				if (this.BattleCalculator.EnemyFirstShips != null)
					enemyLogs = enemyLogs.Concat(
						this.BattleCalculator.EnemyFirstShips
							.Where(x => x != null)
							.SelectMany(x => x.DamageList)
						).ToArray();
				if (this.BattleCalculator.EnemySecondShips != null) {
					enemyCombined = true;
					enemyLogs = enemyLogs.Concat(
						this.BattleCalculator.EnemySecondShips
							.Where(x => x != null)
							.SelectMany(x => x.DamageList)
					).ToArray();
				}
				*/

				var logOutputs = new List<RefinedDamageLog>();

				var logs = this.BattleCalculator.AllDamageLog.Where(x => !x.IsDealt);
				var phase = BattleCalculator.BattlePhase.none;
				foreach (var log in logs)
				{
					if (phase != log.Phase)
					{
						phase = log.Phase;

						var phaseText = "";
						switch (phase)
						{
							case BattleCalculator.BattlePhase.airbase_injection:
								phaseText = "기지항공대 분식 항공전";
								break;
							case BattleCalculator.BattlePhase.injection_kouku:
								phaseText = "분식 항공전";
								break;
							case BattleCalculator.BattlePhase.airbase_attack:
								phaseText = "기지항공대 항공전";
								break;
							case BattleCalculator.BattlePhase.kouku:
								phaseText = "항공전";
								break;
							case BattleCalculator.BattlePhase.kouku2:
								phaseText = "2차 항공전";
								break;
							case BattleCalculator.BattlePhase.support:
								phaseText = "지원함대";
								break;
							case BattleCalculator.BattlePhase.opening_taisen:
								phaseText = "선제 대잠";
								break;
							case BattleCalculator.BattlePhase.opening_atack:
								phaseText = "개막 뇌격";
								break;
							case BattleCalculator.BattlePhase.hougeki:
								phaseText = "야간 포격전";
								break;
							case BattleCalculator.BattlePhase.n_hougeki1:
								phaseText = "야간 1차 포격전";
								break;
							case BattleCalculator.BattlePhase.n_hougeki2:
								phaseText = "야간 2차 포격전";
								break;
							case BattleCalculator.BattlePhase.hougeki1:
								phaseText = "1차 포격전";
								break;
							case BattleCalculator.BattlePhase.hougeki2:
								phaseText = "2차 포격전";
								break;
							case BattleCalculator.BattlePhase.hougeki3:
								phaseText = "3차 포격전";
								break;
							case BattleCalculator.BattlePhase.raigeki:
								phaseText = "폐막 뇌격";
								break;
							default:
								phaseText = "알 수 없음";
								break;
						}

						logOutputs.Add(new RefinedDamageLog
						{
							IsTitle = true,
							TitleText = string.Format("= {0} 시작 =", phaseText)
						});
					}

					BattleCalculator.ShipBattleInfo fromShip = null;
					BattleCalculator.ShipBattleInfo targetShip = null;
					bool fromCombined = false, targetCombined = false;
					if (aliasLogs.Contains(log)) // 아군이 맞은 피해
					{
						fromCombined = (log.Index >= 6 && enemyCombined);
						targetCombined = (log.Target >= 6 && aliasCombined);

						if (log.Target >= 0)
							fromShip = fromCombined
								? this.BattleCalculator.EnemySecondShips[log.Target]
								: this.BattleCalculator.EnemyFirstShips[log.Target];
						else
							fromShip = null;

						if (log.Index >= 0)
							targetShip = targetCombined
								? this.BattleCalculator.AliasSecondShips[log.Index]
								: this.BattleCalculator.AliasFirstShips[log.Index];
						else
							targetShip = null;
					}
					else
					{
						fromCombined = (log.Index >= 6 && aliasCombined);
						targetCombined = (log.Target >= 6 && enemyCombined);

						if (log.Target >= 0)
							fromShip = fromCombined
								? this.BattleCalculator.AliasSecondShips[log.Target]
								: this.BattleCalculator.AliasFirstShips[log.Target];
						else
							fromShip = null;

						if (log.Index >= 0)
							targetShip = targetCombined
								? this.BattleCalculator.EnemySecondShips[log.Index]
								: this.BattleCalculator.EnemyFirstShips[log.Index];
						else
							targetShip = null;
					}

					var typeText = "";
					switch (log.Type)
					{
						case BattleCalculator.DamageType.Normal:
							typeText = "통상";
							break;
						case BattleCalculator.DamageType.Twice:
							typeText = "연격";
							break;
						case BattleCalculator.DamageType.MainGun_SubGun_Cutin:
							typeText = "주포/부포 컷인";
							break;
						case BattleCalculator.DamageType.MainGun_Radar_Cutin:
							typeText = "주포/전탐 컷인";
							break;
						case BattleCalculator.DamageType.MainGun_Shell_Cutin:
							typeText = "주포/철갑탄 컷인";
							break;
						case BattleCalculator.DamageType.MainGun_MainGun_Cutin:
							typeText = "주포/주포 컷인";
							break;
						case BattleCalculator.DamageType.Airstrike_Cutin:
							typeText = "항공모함 컷인";
							break;
						case BattleCalculator.DamageType.MainGun_Torpedo_Cutin:
							typeText = "주포/어뢰 컷인";
							break;
						case BattleCalculator.DamageType.Torpedo_Torpedo_Cutin:
							typeText = "어뢰/어뢰 컷인";
							break;
						case BattleCalculator.DamageType.MainGun_MainGun_SubGun_Cutin:
							typeText = "주포/주포/부포 컷인";
							break;
						case BattleCalculator.DamageType.MainGun_MainGun_MainGun_Cutin:
							typeText = "주포/주포/주포 컷인";
							break;
						case BattleCalculator.DamageType.MainGun_Torpedo_Radar_Cutin:
							typeText = "주포/어뢰/전탐 컷인";
							break;
						case BattleCalculator.DamageType.Torpedo_Lookouts_Radar_Cutin:
							typeText = "어뢰/견시원/전탐 컷인";
							break;
						default:
							typeText = "알 수 없음";
							break;
					}

					switch (phase)
					{
						case BattleCalculator.BattlePhase.airbase_injection:
						case BattleCalculator.BattlePhase.injection_kouku:
						case BattleCalculator.BattlePhase.airbase_attack:
						case BattleCalculator.BattlePhase.kouku:
						case BattleCalculator.BattlePhase.kouku2:
							if (log.Damage == 0)
								continue;
							break;
					}

					var ToRight = !aliasLogs.Contains(log);
					var fromAdd = fromShip?.Source?.AdditionalName == "-"
						? "" : (fromShip?.Source?.AdditionalName) ?? "";
					var targetAdd = targetShip?.Source?.AdditionalName == "-"
						? "" : (targetShip?.Source?.AdditionalName) ?? "";

					string LeftHP = "", RightHP = "";
					if (ToRight)
					{
						if (log.TargetShip != null) LeftHP = string.Format("{0}/{1}", log.TargetShip.NowHP, log.TargetShip.MaxHP);
						if (log.SourceShip != null) RightHP = string.Format("{0}/{1}", log.SourceShip.NowHP, log.SourceShip.MaxHP);
					}
					else
					{
						if (log.SourceShip != null) LeftHP = string.Format("{0}/{1}", log.SourceShip.NowHP, log.SourceShip.MaxHP);
						if (log.TargetShip != null) RightHP = string.Format("{0}/{1}", log.TargetShip.NowHP, log.TargetShip.MaxHP);
					}

					logOutputs.Add(new RefinedDamageLog {
						IsTitle = false,
						TitleText = "",

						ToRight = ToRight,

						IsMiss = log.Damage == 0,
						Damage = log.Damage,

						LeftName = (ToRight ? fromShip?.Source?.Name : targetShip?.Source?.Name) ?? "아군함대",
						RightName = (ToRight ? $"[{targetShip?.Index}] {targetShip?.Source?.Name}" : $"[{fromShip?.Index}] {fromShip?.Source?.Name}") ?? "적군함대",

						LeftHP = LeftHP,
						RightHP = RightHP,

						LeftAdditional = (ToRight ? fromAdd : targetAdd),
						RightAdditional = (ToRight ? targetAdd : fromAdd),

						TypeText = typeText,
						IsDameconUsed = log.DameconUsed
						/*
						string.Format(
							"{0}  -- {2} -->  {1}, {3} 공격{4}",

							fromShip != null
								? string.Format(
									"{2} {0} {1}",
									fromShip.Source.Name,
									fromShip.Source.AdditionalName == "-" ? "" : fromShip.Source.AdditionalName,
									fromCombined ? "2함대" : "1함대"
								).Trim()
								: (aliasLogs.Contains(log) ? "적군함대" : "아군함대"),
							targetShip != null
								? string.Format(
									"{2} {0} {1}",
									targetShip.Source.Name,
									targetShip.Source.AdditionalName == "-" ? "" : targetShip.Source.AdditionalName,
									targetCombined ? "2함대" : "1함대"
								).Trim()
								: (aliasLogs.Contains(log) ? "아군함대" : "적군함대"),
							log.Damage > 0 ? $"{log.Damage} dmg" : "miss",
							typeText,
							log.DameconUsed ? " (다메콘 발동)" : ""
						)
						*/
					});
				}

				/*
				this.DetailBattleLog = string.Join(
					Environment.NewLine,
					logTexts.ToArray()
				).Trim() + Environment.NewLine;
				*/
				this.DetailBattleLog = logOutputs.ToArray();
			}
		}

		public void Initialize()
		{

		}
	}

	public class RefinedDamageLog
	{
		public bool IsTitle { get; set; }
		public string TitleText { get; set; }

		public bool ToRight { get; set; }

		public string LeftName { get; set; }
		public string LeftAdditional { get; set; }
		public string LeftHP { get; set; }

		public string RightName { get; set; }
		public string RightAdditional { get; set; }
		public string RightHP { get; set; }

		public bool IsMiss { get; set; }
		public int Damage { get; set; }

		public string TypeText { get; set; }
		public bool IsDameconUsed { get; set; }
	}
}
