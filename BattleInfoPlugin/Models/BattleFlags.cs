using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using BattleInfoPlugin.Models.Raw;

namespace BattleInfoPlugin.Models
{
	public class BattleFlags : NotificationObject
	{
		#region Definitions
		/// <summary>
		/// 대공컷인 데이터
		/// </summary>
		[Flags]
		public enum AntiAirFireFlag
		{
			Unset = 0,
			Unused = 1,

			Used = 2,

			First = 4,
			Second = 8,
			Both = First | Second,
		}

		/// <summary>
		/// 무엇인가 사용된 플래그 (조명탄, 야간정찰기 등)
		/// </summary>
		[Flags]
		public enum UsedFlag
		{
			Unset = 0,
			Unused = 1,

			Alias = 2,
			Enemy = 3,
			Both = Alias | Enemy
		}

		/// <summary>
		/// 지원함대 종류
		/// </summary>
		public enum SupportType
		{
			Unset = 0,
			Unused = 1,

			/// <summary>
			/// 포격지원
			/// </summary>
			GunFire = 2,

			/// <summary>
			/// 항공지원
			/// </summary>
			Airstrike = 3,

			/// <summary>
			/// 뇌격지원
			/// </summary>
			Torpedo = 4,
		}
		#endregion


		#region FlareUsed 변경통지 프로퍼티
		public UsedFlag FlareUsed
		{
			get { return this._FlareUsed; }
			private set
			{
				if (this._FlareUsed != value)
				{
					this._FlareUsed = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private UsedFlag _FlareUsed;
		#endregion

		#region NightReconScouted 변경통지 프로퍼티
		public UsedFlag NightReconScouted
		{
			get { return this._NightReconScouted; }
			private set
			{
				if (this._NightReconScouted != value)
				{
					this._NightReconScouted = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private UsedFlag _NightReconScouted;
		#endregion

		#region AntiAirFired 변경통지 프로퍼티
		public AntiAirFireFlag AntiAirFired
		{
			get { return this._AntiAirFired; }
			private set
			{
				if (this._AntiAirFired != value)
				{
					this._AntiAirFired = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private AntiAirFireFlag _AntiAirFired;

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
		private string _AntiAirFiredDetail;
		#endregion

		#region SupportUsed 변경통지 프로퍼티
		public SupportType SupportUsed
		{
			get { return this._SupportUsed; }
			private set
			{
				if (this._SupportUsed != value)
				{
					this._SupportUsed = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private SupportType _SupportUsed;
		#endregion

		#region MechanismOn 변경통지 프로퍼티
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
		private bool _MechanismOn;
		#endregion

		#region MapExtended 변경통지 프로퍼티
		public bool MapExtended
		{
			get { return this._MapExtended; }
			set
			{
				if (this._MapExtended != value)
				{
					this._MapExtended = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private bool _MapExtended;
		#endregion


		/// <summary>
		/// 모든 정보를 초기화
		/// </summary>
		public void Clear()
		{
			this.FlareUsed = UsedFlag.Unset;
			this.NightReconScouted = UsedFlag.Unset;
			this.AntiAirFired = AntiAirFireFlag.Unset;
			this.SupportUsed = SupportType.Unset;
			this.MechanismOn = this.MapExtended = false;
		}

		/// <summary>
		/// 조명탄 정보 갱신
		/// </summary>
		public void UpdateFlare(int[] FlareData)
		{
			this.FlareUsed = UsedFlag.Unset;
			try
			{
				if (FlareData[0] == -1 && FlareData[1] == -1)
					this.FlareUsed = UsedFlag.Unused;

				else if (FlareData[0] != -1)
					this.FlareUsed = UsedFlag.Alias;

				else if (FlareData[1] != -1)
					this.FlareUsed = UsedFlag.Enemy;

				else
					this.FlareUsed = UsedFlag.Both;
			}
			catch { }
		}

		/// <summary>
		/// 야간정찰기 정보 갱신
		/// </summary>
		public void UpdateNightRecon(int[] NightReconData)
		{
			this.NightReconScouted = UsedFlag.Unset;
			try
			{
				if (NightReconData[0] == -1 && NightReconData[1] == -1)
					this.NightReconScouted = UsedFlag.Unused;

				else if (NightReconData[0] != -1)
					this.NightReconScouted = UsedFlag.Alias;

				else if (NightReconData[1] != -1)
					this.NightReconScouted = UsedFlag.Enemy;

				else
					this.NightReconScouted = UsedFlag.Both;
			}
			catch { }
		}

		/// <summary>
		/// 대공컷인 정보 갱신 (항공전이 아닌 경우)
		/// </summary>
		public void UpdateAntiAirFire(Api_Air_Fire data)
		{
			this.AntiAirFired = AntiAirFireFlag.Unset;
			try
			{
				this.AntiAirFired = (data == null ? AntiAirFireFlag.Unused : AntiAirFireFlag.Used);
			}
			catch { }
		}

		/// <summary>
		/// 대공컷인 정보 갱신 (항공전)
		/// </summary>
		public void UpdateAntiAirFire(Api_Air_Fire data1, Api_Air_Fire data2)
		{
			this.AntiAirFired = AntiAirFireFlag.Unset;
			try
			{
				if (data1 == null && data2 == null)
					this.AntiAirFired = AntiAirFireFlag.Unused;

				else if (data2 == null)
					this.AntiAirFired = AntiAirFireFlag.First;

				else if (data1 == null)
					this.AntiAirFired = AntiAirFireFlag.Second;

				else
					this.AntiAirFired = AntiAirFireFlag.Both;
			}
			catch { }
		}

		/// <summary>
		/// 지원함대 정보 갱신
		/// </summary>
		/// <param name="support_flag">주간 지원함대 종류</param>
		/// <param name="n_support_flag">야간 지원함대 종류</param>
		public void UpdateSupport(int support_flag, int n_support_flag = 0)
		{
			this.SupportUsed = SupportType.Unset;
			try
			{
				int flag = 0;

				if (support_flag != 0)
					flag = support_flag;

				else if (n_support_flag != 0)
					flag = n_support_flag;

				switch (flag)
				{
					case 1:
						this.SupportUsed = SupportType.Airstrike;
						break;

					case 2:
						this.SupportUsed = SupportType.GunFire;
						break;

					case 3:
						this.SupportUsed = SupportType.Torpedo;
						break;
				}
			}
			catch { }
		}
	}
}
