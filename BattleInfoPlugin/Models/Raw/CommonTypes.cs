namespace BattleInfoPlugin.Models.Raw
{
	#region 항공전
	// 항공전 / 분식 항공전
	// Api_Kouku, Api_Injection_Kouku
	public class Api_Kouku
	{
		public int[][] api_plane_from { get; set; }
		public Api_Stage1 api_stage1 { get; set; }
		public Api_Stage2 api_stage2 { get; set; }
		public Api_Stage3 api_stage3 { get; set; }
		public Api_Stage3 api_stage3_combined { get; set; }
	}

	public class Api_Stage1
	{
		public int api_f_count { get; set; }
		public int api_f_lostcount { get; set; }
		public int api_e_count { get; set; }
		public int api_e_lostcount { get; set; }
		public int api_disp_seiku { get; set; }
		public int[] api_touch_plane { get; set; }
	}

	public class Api_Stage2
	{
		public int api_f_count { get; set; }
		public int api_f_lostcount { get; set; }
		public int api_e_count { get; set; }
		public int api_e_lostcount { get; set; }
		public Api_Air_Fire api_air_fire { get; set; }
	}

	public class Api_Air_Fire
	{
		public int api_idx { get; set; }
		public int api_kind { get; set; }
		public int[] api_use_items { get; set; }
	}

	public class Api_Stage3
	{
		public int?[] api_frai_flag { get; set; }
		public int[] api_erai_flag { get; set; }
		public int?[] api_fbak_flag { get; set; }
		public int[] api_ebak_flag { get; set; }
		public int[] api_fcl_flag { get; set; }
		public int[] api_ecl_flag { get; set; }
		public decimal[] api_fdam { get; set; }
		public decimal[] api_edam { get; set; }
	}
	#endregion

	#region 지원함대
	public class Api_Support_Info
	{
		public Api_Support_Airatack api_support_airatack { get; set; }
		public Api_Support_Hourai api_support_hourai { get; set; }
	}

	public class Api_Support_Airatack : Api_Kouku
	{
		public int api_deck_id { get; set; }
		public int[] api_ship_id { get; set; }
		public int[] api_undressing_flag { get; set; }
		public int[] api_stage_flag { get; set; }
	}

	public class Api_Support_Hourai
	{
		public int api_deck_id { get; set; }
		public int[] api_ship_id { get; set; }
		public int[] api_undressing_flag { get; set; }
		public int[] api_cl_list { get; set; }
		public decimal[] api_damage { get; set; }
		public decimal[] api_damage_combined { get; set; }
	}
	#endregion

	#region 선제뇌격 / 뇌격전
	public class Raigeki
	{
		public int[] api_frai { get; set; }
		public int[] api_erai { get; set; }
		public decimal[] api_fdam { get; set; }
		public decimal[] api_edam { get; set; }
		public decimal[] api_fydam { get; set; }
		public decimal[] api_eydam { get; set; }
		public int[] api_fcl { get; set; }
		public int[] api_ecl { get; set; }
	}
	#endregion

	#region 포격전
	public class Hougeki
	{
		public int[] api_at_eflag { get; set; }
		public int[] api_at_list { get; set; }
		public int[] api_at_type { get; set; }
		public object[] api_df_list { get; set; }
		public object[] api_si_list { get; set; }
		public object[] api_cl_list { get; set; }
		public object[] api_damage { get; set; }
	}

	public class Midnight_Hougeki
	{
		public int[] api_at_eflag { get; set; }
		public int[] api_at_list { get; set; }
		public object[] api_df_list { get; set; }
		public object[] api_si_list { get; set; }
		public object[] api_cl_list { get; set; }
		public int[] api_sp_list { get; set; }
		public object[] api_damage { get; set; }
	}
	#endregion

	#region 기지항공대
	// 기지항공대 항공전
	public class Api_Air_Base_Attack : Api_Kouku
	{
		public int api_base_id { get; set; }
		public int[] api_stage_flag { get; set; }
		public Api_Squadron_Plane[] api_squadron_plane { get; set; }
	}
	public class Api_Squadron_Plane
	{
		public int api_mst_id { get; set; }
		public int api_count { get; set; }
	}

	// 기지항공대 분식 항공전
	public class Api_Air_Base_Injection : Api_Kouku
	{
		public Api_Squadron_Plane[] api_air_base_data { get; set; }
	}
	#endregion
}
