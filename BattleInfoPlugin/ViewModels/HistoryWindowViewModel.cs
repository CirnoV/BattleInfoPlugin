using System;
using System.Linq;
using settings = BattleInfoPlugin.Properties.Settings;
using BattleInfoPlugin.Models;
using BattleInfoPlugin.Models.Notifiers;
using Livet;
using Livet.EventListeners;
using Livet.Messaging;

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
		}

		public void Initialize()
		{

		}
	}
}
