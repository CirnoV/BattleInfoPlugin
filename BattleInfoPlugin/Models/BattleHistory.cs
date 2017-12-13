using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin.Models
{
	public class BattleHistory
	{
		public string BattleName { get; }
		public DateTimeOffset UpdatedTime { get; }

		public BattleSituation BattleSituation { get; }
		public AirSupremacy FriendAirSupremacy { get; }

		public BaseNodeData CurrentNode { get; }
		public BattleFlags BattleFlags { get; }

		public Rank RankResult { get; }
		public Rank AirRankResult { get; }

		public string DropShipName { get; }
		public AirCombatResult[] AirCombatResults { get; }

		public FleetData AliasFirst { get; }
		public FleetData AliasSecond { get; }
		public FleetData EnemyFirst { get; }
		public FleetData EnemySecond { get; }

		public BattleCalculator BattleCalculator { get; }

		public BattleHistory(BattleData Source)
		{
			this.BattleName = Source.BattleName;
			this.UpdatedTime = Source.UpdatedTime;

			this.BattleSituation = Source.BattleSituation;
			this.FriendAirSupremacy = Source.FriendAirSupremacy;

			this.CurrentNode = Source.CurrentSortie.CurrentNode;
			this.BattleFlags = Source.CurrentBattleFlag.Clone();

			this.RankResult = Source.RankResult;
			this.AirRankResult = Source.AirRankResult;

			this.DropShipName = Source.DropShipName;
			this.AirCombatResults = Source.AirCombatResults;

			this.AliasFirst = Source.AliasFirst.Clone();
			this.AliasSecond = Source.AliasSecond.Clone();
			this.EnemyFirst = Source.EnemyFirst.Clone();
			this.EnemySecond = Source.EnemySecond.Clone();

			this.BattleCalculator = Source.battleCalculator.Clone();
		}
	}
}
