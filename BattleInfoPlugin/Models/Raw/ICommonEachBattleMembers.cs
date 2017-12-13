using System.Linq;
using Grabacr07.KanColleWrapper;

namespace BattleInfoPlugin.Models.Raw
{
	interface ICommonEachBattleMembers : ICommonBattleMembers
	{
		int[] api_ship_ke_combined { get; set; }
		int[] api_ship_lv_combined { get; set; }
		int[] api_e_nowhps_combined { get; set; }
		int[] api_e_maxhps_combined { get; set; }
		int[][] api_eSlot_combined { get; set; }
		int[][] api_eParam_combined { get; set; }
	}

	static class CommonEachBattleMembersExtensions
	{
		public static MastersShipData[] ToMastersShipDataArray(this ICommonEachBattleMembers data)
		{
			var master = KanColleClient.Current.Master;
			return data.api_ship_ke
				.Where(x => x != -1)
				.Select((x, i) => new MastersShipData(master.Ships[x])
				{
					Level = data.api_ship_lv[i + 1],
					Firepower = data.api_eParam[i][0],
					Torpedo = data.api_eParam[i][1],
					AA = data.api_eParam[i][2],
					Armor = data.api_eParam[i][3],
					Slots = data.api_eSlot[i]
						.Where(s => 0 < s)
						.Select(s => master.SlotItems[s])
						.Select(s => new ShipSlotData(s))
						.ToArray(),
				})
				.ToArray();
		}
		public static MastersShipData[] ToMastersSecondShipDataArray(this ICommonEachBattleMembers data)
		{
			var master = KanColleClient.Current.Master;
			return data.api_ship_ke_combined
				.Where(x => x != -1)
				.Select((x, i) => new MastersShipData(master.Ships[x])
				{
					Level = data.api_ship_lv_combined[i + 1],
					Firepower = data.api_eParam_combined[i][0],
					Torpedo = data.api_eParam_combined[i][1],
					AA = data.api_eParam_combined[i][2],
					Armor = data.api_eParam_combined[i][3],
					Slots = data.api_eSlot_combined[i]
						.Where(s => 0 < s)
						.Select(s => master.SlotItems[s])
						.Select(s => new ShipSlotData(s))
						.ToArray(),
				})
				.ToArray();
		}
	}
}
