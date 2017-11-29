using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleInfoPlugin.Models.Raw;
using SlotItemType = Grabacr07.KanColleWrapper.Models.SlotItemType;

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
	public class MVPOracle
	{
		public struct DamageContainer
		{
			public int Index { get; set; }

			public int at_injection { get; set; }
			public int at_kouku { get; set; }
			public int at_opening_taisen { get; set; }
			public int at_opening_raigeki { get; set; }
			public int at_hougeki1 { get; set; }
			public int at_hougeki2 { get; set; }
			public int at_hougeki3 { get; set; }
			public int at_raigeki { get; set; }

			public int GetTotal()
				=> at_injection + at_kouku
				+ at_opening_taisen + at_opening_raigeki
				+ at_hougeki1 + at_hougeki2 + at_hougeki3 + at_raigeki;
		}
		private class CommonBattleData
		{
			public Api_Kouku api_injection_kouku { get; set; }
			public Api_Kouku api_kouku { get; set; }
			public Api_Kouku api_kouku2 { get; set; }
			public Hougeki api_opening_taisen { get; set; }
			public Raigeki api_opening_atack { get; set; }
			public Midnight_Hougeki api_hougeki { get; set; }
			public Hougeki api_hougeki1 { get; set; }
			public Hougeki api_hougeki2 { get; set; }
			public Hougeki api_hougeki3 { get; set; }
			public Raigeki api_raigeki { get; set; }
		}

		public DamageContainer[] AliasDamages { get; private set; }
		public int MVP1 { get; private set; }
		public int MVP2 { get; private set; }

		private IEnumerable<ShipData> shipData { get; set; }

		public MVPOracle Initialize(params FleetData[] shipDatas)
		{
			this.AliasDamages = new DamageContainer[14]; // 7 + 7
			for (var i = 0; i < this.AliasDamages.Length; i++)
				this.AliasDamages[i].Index = i + 1;

			this.shipData = shipDatas.Where(x => x != null)
				.Select(x => x.Ships)
				.SelectMany(x => x);
			return this;
		}
		private MVPOracle Update(CommonBattleData data, bool isCombined)
		{
			var ships = this.shipData.ToArray();

			#region kouku
			if (data.api_kouku != null && data.api_kouku.api_stage3 != null)
			{
				for (var i = 0; i < ships.Length; i++)
				{
					foreach (var slot in ships[i].Slots)
					{
						if (!slot.Source.IsNumerable) continue;
						if (slot.Current == 0) continue;

						double value = 0, rate = 0;
						switch (slot.Source.Type)
						{
							case SlotItemType.艦上爆撃機:
							case SlotItemType.噴式戦闘爆撃機:
								value = slot.Bomb;
								rate = 1;
								break;
							case SlotItemType.艦上攻撃機:
							case SlotItemType.噴式攻撃機:
							case SlotItemType.陸上攻撃機:
								value = slot.Torpedo;
								rate = 1.15;
								break;
						}
						if (rate == 0) continue;

						value = (25 + value * Math.Sqrt(slot.Current)) * rate;
						value = value > 150 ? (150 + Math.Sqrt(value - 150)) : value; // 150 cap

						// skip Critical
						// skip Contact multiplier
						// skip Enemy armor
						// skip Ammo modifier

						this.AliasDamages[i + 1].at_kouku += (int)value;
					}
				}
			}
			#endregion

			#region kouku2
			if (data.api_kouku2 != null && data.api_kouku2.api_stage3 != null)
			{
				for (var i = 0; i < ships.Length; i++)
				{
					foreach (var slot in ships[i].Slots)
					{
						if (!slot.Source.IsNumerable) continue;
						if (slot.Current == 0) continue;

						double value = 0, rate = 0;
						switch (slot.Source.Type)
						{
							case SlotItemType.艦上爆撃機:
							case SlotItemType.噴式戦闘爆撃機:
								value = slot.Bomb;
								rate = 1;
								break;
							case SlotItemType.艦上攻撃機:
							case SlotItemType.噴式攻撃機:
							case SlotItemType.陸上攻撃機:
								value = slot.Torpedo;
								rate = 1.15;
								break;
						}
						if (rate == 0) continue;

						value = (25 + value * Math.Sqrt(slot.Current)) * rate;
						value = value > 150 ? (150 + Math.Sqrt(value - 150)) : value; // 150 cap

						// skip Critical
						// skip Contact multiplier
						// skip Enemy armor
						// skip Ammo modifier

						this.AliasDamages[i + 1].at_kouku += (int)value;
					}
				}
			}
			#endregion

			#region injection
			if (data.api_injection_kouku != null && data.api_injection_kouku.api_stage3 != null)
			{
				for (var i = 0; i < ships.Length; i++)
				{
					foreach (var slot in ships[i].Slots)
					{
						if (!slot.Source.IsNumerable) continue;
						if (slot.Current == 0) continue;

						double value = 0, rate = 0;
						switch (slot.Source.Type)
						{
							case SlotItemType.噴式戦闘爆撃機:
								value = slot.Bomb;
								rate = 1;
								break;
							case SlotItemType.噴式攻撃機:
								value = slot.Torpedo;
								rate = 1.15;
								break;
						}
						if (rate == 0) continue;

						value = (25 + value * Math.Sqrt(slot.Current)) * rate;
						value = value > 150 ? (150 + Math.Sqrt(value - 150)) : value; // 150 cap

						// skip Critical
						// skip Contact multiplier
						// skip Enemy armor
						// skip Ammo modifier

						this.AliasDamages[i + 1].at_kouku += (int)value;
					}
				}
			}
			#endregion

			#region opening_taisen
			if (data.api_opening_taisen != null)
			{
				var src = data.api_opening_taisen;
                for (int i = 0; i < src.api_damage.Length; i++)
                {
                    var from = src.api_at_list[i];
                    if (!isCombined && src.api_at_eflag[i] == 1) continue; // from enemy

                    var dmg = (src.api_damage[i] is Array)
						? (src.api_damage[i] as object[]).Select(Convert.ToInt32).Sum(x => (int)x)
						: (int)src.api_damage[i];

					this.AliasDamages[from].at_opening_taisen += (int)dmg;
				}
			}
			#endregion

			#region opening_atack
			if (data.api_opening_atack != null)
			{
				var src = data.api_opening_atack;
				for (int i = 1; i < src.api_fydam.Length; i++)
					this.AliasDamages[i].at_opening_raigeki += (int)src.api_fydam[i];
			}
			#endregion

			#region hougeki
			if (data.api_hougeki != null)
			{
				var src = data.api_hougeki;
                for (int i = 0; i < src.api_damage.Length; i++)
                {
                    var from = src.api_at_list[i];
                    if (!isCombined && src.api_at_eflag[i] == 1) continue; // from enemy

                    var dmg = (src.api_damage[i] is Array)
						? (src.api_damage[i] as object[]).Select(Convert.ToInt32).Sum(x => (int)x)
						: (int)src.api_damage[i];

					this.AliasDamages[from].at_hougeki1 += (int)dmg;
				}
			}
			#endregion

			#region hougeki1
			if (data.api_hougeki1 != null)
			{
				var src = data.api_hougeki1;
				for (int i = 0; i < src.api_damage.Length; i++)
				{
					var from = src.api_at_list[i];
                    if (!isCombined && src.api_at_eflag[i] == 1) continue; // from enemy

                    var dmg = (src.api_damage[i] is Array)
						? (src.api_damage[i] as object[]).Select(Convert.ToInt32).Sum(x => (int)x)
						: (int)src.api_damage[i];

					this.AliasDamages[from].at_hougeki1 += (int)dmg;
				}
			}
			#endregion

			#region hougeki2
			if (data.api_hougeki2 != null)
			{
				var src = data.api_hougeki2;
                for (int i = 0; i < src.api_damage.Length; i++)
                {
                    var from = src.api_at_list[i];
                    if (!isCombined && src.api_at_eflag[i] == 1) continue; // from enemy

                    var dmg = (src.api_damage[i] is Array)
						? (src.api_damage[i] as object[]).Select(Convert.ToInt32).Sum(x => (int)x)
						: (int)src.api_damage[i];

					this.AliasDamages[from].at_hougeki2 += (int)dmg;
				}
			}
			#endregion

			#region hougeki3
			if (data.api_hougeki3 != null)
			{
				var src = data.api_hougeki3;
                for (int i = 0; i < src.api_damage.Length; i++)
                {
                    var from = src.api_at_list[i];
                    if (!isCombined && src.api_at_eflag[i] == 1) continue; // from enemy

                    var dmg = (src.api_damage[i] is Array)
						? (src.api_damage[i] as object[]).Select(Convert.ToInt32).Sum(x => (int)x)
						: (int)src.api_damage[i];

					this.AliasDamages[from].at_hougeki3 += (int)dmg;
				}
			}
			#endregion

			#region raigeki
			if (data.api_raigeki != null)
			{
				var src = data.api_raigeki;
				for (int i = 1; i < src.api_fydam.Length; i++)
					this.AliasDamages[i].at_raigeki += (int)src.api_fydam[i];
			}
			#endregion

			this.MVP1 = this.AliasDamages.Where((x, y) => (y >= 0 && y < 6))
				.OrderByDescending(x => x.GetTotal())
				.FirstOrDefault().Index;

			this.MVP2 = this.AliasDamages.Where((x, y) => (y >= 6 && y < 12))
				.OrderByDescending(x => x.GetTotal())
				.FirstOrDefault().Index;
			return this;
		}

		#region Raw Update functions
		public void Update(battle_midnight_battle data, bool isCombined = false)
		{
			this.Update(new CommonBattleData
			{
				api_hougeki = data.api_hougeki
			}, isCombined);
		}
		public void Update(combined_battle_battle data, bool isCombined = false)
		{
			this.Update(new CommonBattleData
			{
				api_injection_kouku = data.api_injection_kouku,
				api_kouku = data.api_kouku,
				api_opening_taisen = data.api_opening_taisen,
				api_opening_atack = data.api_opening_atack,
				api_hougeki1 = data.api_hougeki1,
				api_hougeki2 = data.api_hougeki2,
				api_hougeki3 = data.api_hougeki3,
				api_raigeki = data.api_raigeki
			}, isCombined);
		}
		public void Update(combined_battle_airbattle data, bool isCombined = false)
		{
			this.Update(new CommonBattleData
			{
				api_injection_kouku = data.api_injection_kouku,
				api_kouku = data.api_kouku,
				api_kouku2 = data.api_kouku2
			}, isCombined);
		}
		public void Update(combined_battle_midnight_battle data, bool isCombined = false)
		{
			this.Update(new CommonBattleData
			{
				api_hougeki = data.api_hougeki
			}, isCombined);
		}
		public void Update(sortie_battle data, bool isCombined = false)
		{
			this.Update(new CommonBattleData
			{
				api_injection_kouku = data.api_injection_kouku,
				api_kouku = data.api_kouku,
				api_opening_taisen = data.api_opening_taisen,
				api_opening_atack = data.api_opening_atack,
				api_hougeki1 = data.api_hougeki1,
				api_hougeki2 = data.api_hougeki2,
				api_hougeki3 = data.api_hougeki3,
				api_raigeki = data.api_raigeki
			}, isCombined);
		}
		public void Update(sortie_airbattle data, bool isCombined = false)
		{
			this.Update(new CommonBattleData
			{
				api_injection_kouku = data.api_injection_kouku,
				api_kouku = data.api_kouku,
				api_kouku2 = data.api_kouku2
			}, isCombined);
		}
		public void Update(combined_battle_each_battle data, bool isCombined)
		{
			this.Update(new CommonBattleData
			{
				api_injection_kouku = data.api_injection_kouku,
				api_kouku = data.api_kouku,
				api_opening_taisen = data.api_opening_taisen,
				api_opening_atack = data.api_opening_atack,
				api_hougeki1 = data.api_hougeki1,
				api_hougeki2 = data.api_hougeki2,
				api_hougeki3 = data.api_hougeki3,
				api_raigeki = data.api_raigeki
			}, isCombined);
		}
		public void Update(combined_battle_ec_midnight_battle data, bool isCombined = false)
		{
			this.Update(new CommonBattleData
			{
				api_hougeki = data.api_hougeki
			}, isCombined);
		}
		#endregion
	}
}
