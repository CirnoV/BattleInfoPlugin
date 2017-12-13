using System;
using System.Collections.Generic;
using System.Linq;
using BattleInfoPlugin.Properties;

namespace BattleInfoPlugin.Models.Raw
{
	public static class CommonTypeExtensions
	{
		public static AirSupremacy GetAirSupremacy(this Api_Kouku kouku)
			=> (AirSupremacy)(kouku?.api_stage1?.api_disp_seiku ?? (int)AirSupremacy.항공전없음);

		public static AirCombatResult[] ToResult(this Api_Kouku kouku, string prefixName = "")
		{
			return kouku != null
				? new[]
				{
					kouku.api_stage1.ToResult($"{prefixName}공대공"),
					kouku.api_stage2.ToResult($"{prefixName}공대함")
				}
				: new AirCombatResult[0];
		}
		public static AirCombatResult[] ToResult(this Api_Air_Base_Attack[] attacks)
		{
			return attacks != null && Settings.Default.DetailKouku
				? attacks.SelectMany(x => new[] {
					x.api_stage1.ToResult($"제 {x.api_base_id}기항대 공대공"),
					x.api_stage2.ToResult($"제 {x.api_base_id}기항대 공대함")
				}).ToArray()
				: new AirCombatResult[0];
		}
		public static AirCombatResult[] ToResult(this Api_Air_Base_Injection attacks)
		{
			return attacks != null && Settings.Default.DetailKouku
				? new[] {
					attacks.api_stage1.ToResult($"제 {attacks.api_base_id}기항대 분식 공대공"),
					attacks.api_stage2.ToResult($"제 {attacks.api_base_id}기항대 분식 공대함")
				}.ToArray()
				: new AirCombatResult[0];
		}

		public static AirCombatResult ToResult(this Api_Stage1 stage1, string name)
			=> stage1 == null ? new AirCombatResult(name)
			: new AirCombatResult(name, stage1.api_f_count, stage1.api_f_lostcount, stage1.api_e_count, stage1.api_e_lostcount);

		public static AirCombatResult ToResult(this Api_Stage2 stage2, string name)
			=> stage2 == null ? new AirCombatResult(name)
			: new AirCombatResult(name, stage2.api_f_count, stage2.api_f_lostcount, stage2.api_e_count, stage2.api_e_lostcount);
	}
}
