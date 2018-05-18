using System;
using System.Collections.Generic;
using System.Linq;
using Grabacr07.KanColleWrapper.Models;
using Livet;

namespace BattleInfoPlugin.Models
{
	public class ShipData : NotificationObject
	{
		#region Id 변경통지 프로퍼티
		private int _Id;
		public int Id
		{
			get { return this._Id; }
			set
			{
				if (this._Id != value)
				{
					this._Id = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region MasterId 변경통지 프로퍼티
		private int _MasterId;
		public int MasterId
		{
			get { return this._MasterId; }
			set
			{
				if (this._MasterId != value)
				{
					this._MasterId = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region Name 변경통지 프로퍼티
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

		#region AdditionalName 변경통지 프로퍼티
		private string _AdditionalName;
		public string AdditionalName
		{
			get
			{ return this._AdditionalName; }
			set
			{ 
				if (this._AdditionalName == value)
					return;
				this._AdditionalName = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region ShipSpeed 변경통지 프로퍼티
		private ShipSpeed _ShipSpeed;
		public ShipSpeed ShipSpeed
		{
			get { return this._ShipSpeed; }
			set
			{
				if (this._ShipSpeed != value)
				{
					this._ShipSpeed = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region ShipType 변경통지 프로퍼티
		private int _ShipType;
		public int ShipType
		{
			get { return this._ShipType; }
			set
			{
				if (this._ShipType != value)
				{
					this._ShipType = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region TypeName 변경통지 프로퍼티
		private string _TypeName;
		public string TypeName
		{
			get
			{ return this._TypeName; }
			set
			{ 
				if (this._TypeName == value)
					return;
				this._TypeName = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region Level 변경통지 프로퍼티
		private int _Level;
		public int Level
		{
			get { return this._Level; }
			set
			{
				if (this._Level == value)
					return;
				this._Level = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region Situation 변경통지 프로퍼티
		private ShipSituation _Situation;
		public ShipSituation Situation
		{
			get
			{ return _Situation; }
			set
			{ 
				if (_Situation == value)
					return;
				_Situation = value;
				RaisePropertyChanged();
			}
		}
		#endregion

		#region MaxHP 변경통지 프로퍼티
		private int _MaxHP;
		public int MaxHP
		{
			get { return this._MaxHP; }
			set
			{
				if (this._MaxHP == value)
					return;
				this._MaxHP = value;
				this.RaisePropertyChanged();
				this.RaisePropertyChanged(() => this.HP);

				if (this.IsHeavilyDamage())
					this.Situation |= ShipSituation.HeavilyDamaged;
				else
					this.Situation &= ~ShipSituation.HeavilyDamaged;
			}
		}
		#endregion

		#region NowHP 변경통지 프로퍼티
		private int _NowHP;
		public int NowHP
		{
			get { return this._NowHP; }
			set
			{
				if (this._NowHP == value)
					return;
				this._NowHP = value;
				this.RaisePropertyChanged();
				this.RaisePropertyChanged(() => this.HP);

				if (this.IsHeavilyDamage())
					this.Situation |= ShipSituation.HeavilyDamaged;
				else
					this.Situation &= ~ShipSituation.HeavilyDamaged;
			}
		}
		#endregion

		#region BeforeNowHP 변경통지 프로퍼티
		private int _BeforeNowHP;
		public int BeforeNowHP
		{
			get { return this._BeforeNowHP; }
			set
			{
				if (this._BeforeNowHP == value)
					return;
				this._BeforeNowHP = value;
			}
		}
		#endregion

		#region Firepower 변경통지 프로퍼티
		/// <summary>
		/// 화력 스테이터스
		/// </summary>
		public int Firepower
		{
			get { return this._Firepower; }
			set
			{
				if (this._Firepower != value)
				{
					this._Firepower = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private int _Firepower;
		#endregion

		#region Torpedo 변경통지 프로퍼티
		/// <summary>
		/// 뇌장 스테이터스
		/// </summary>
		public int Torpedo
		{
			get { return this._Torpedo; }
			set
			{
				if (this._Torpedo != value)
				{
					this._Torpedo = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private int _Torpedo;
		#endregion

		#region AA 변경통지 프로퍼티
		/// <summary>
		/// 대공 스테이터스
		/// </summary>
		public int AA
		{
			get { return this._AA; }
			set
			{
				if (this._AA != value)
				{
					this._AA = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private int _AA;
		#endregion

		#region Armor 변경통지 프로퍼티
		/// <summary>
		/// 장갑 스테이터스
		/// </summary>
		public int Armor
		{
			get { return this._Armor; }
			set
			{
				if (this._Armor != value)
				{
					this._Armor = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private int _Armor;
		#endregion

		#region Luck 변경통지 프로퍼티
		/// <summary>
		/// 운 스테이터스
		/// </summary>
		public int Luck
		{
			get { return this._Luck; }
			set
			{
				if (this._Luck != value)
				{
					this._Luck = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private int _Luck;
		#endregion

		#region ASW 변경통지 프로퍼티
		/// <summary>
		/// 대잠 스테이터스
		/// </summary>
		public int ASW
		{
			get { return this._ASW; }
			set
			{
				if (this._ASW != value)
				{
					this._ASW = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private int _ASW;
		#endregion

		#region Evade 변경통지 프로퍼티
		/// <summary>
		/// 회피 스테이터스
		/// </summary>
		public int Evade
		{
			get { return this._Evade; }
			set
			{
				if (this._Evade != value)
				{
					this._Evade = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private int _Evade;
		#endregion

		#region Slots 변경통지 프로퍼티
		public IEnumerable<ShipSlotData> Slots
		{
			get { return this._Slots; }
			set
			{
				if (this._Slots != value)
				{
					this._Slots = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private IEnumerable<ShipSlotData> _Slots;
		#endregion

		#region ExSlot 변경통지 프로퍼티
		public ShipSlotData ExSlot
		{
			get { return this._ExSlot; }
			set
			{
				if (this._ExSlot != value)
				{
					this._ExSlot = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private ShipSlotData _ExSlot;
		#endregion

		#region IsUsedDamecon 변경통지 프로퍼티
		/// <summary>
		/// 다메콘 아이템 사용 여부
		/// </summary>
		public bool IsUsedDamecon
		{
			get { return this._IsUsedDamecon; }
			set
			{
				if (this._IsUsedDamecon != value)
				{
					this._IsUsedDamecon = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private bool _IsUsedDamecon;
		#endregion

		#region Condition 변경통지 프로퍼티
		public int Condition
		{
			get { return this._Condition; }
			set
			{
				if (this._Condition != value)
				{
					this._Condition = value;
					this.RaisePropertyChanged();
					this.RaisePropertyChanged(nameof(this.ConditionType));
				}
			}
		}
		private int _Condition;

		public ConditionType ConditionType
			=> ConditionTypeHelper.ToConditionType(this.Condition);
		#endregion

		#region IsMvp 변경통지 프로퍼티
		/// <summary>
		/// 이 함선이 MVP 인지 여부
		/// </summary>
		public bool IsMvp
		{
			get { return this._IsMvp; }
			set
			{
				if (this._IsMvp != value)
				{
					this._IsMvp = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private bool _IsMvp;
		#endregion


		#region 장비 수치 합계
		public int EquipFirepower => this.Slots.Sum(x => x.Firepower) + (this.ExSlot?.Firepower ?? 0);
		public int EquipTorpedo => this.Slots.Sum(x => x.Torpedo) + (this.ExSlot?.Torpedo ?? 0);
		public int EquipAA => this.Slots.Sum(x => x.AA) + (this.ExSlot?.AA ?? 0);
		public int EquipArmor => this.Slots.Sum(x => x.Armor) + (this.ExSlot?.Armor ?? 0);
		public int EquipASW => this.Slots.Sum(x => x.ASW) + (this.ExSlot?.ASW ?? 0);
		public int EquipAccuracy => this.Slots.Sum(x => x.Accuracy) + (this.ExSlot?.Accuracy ?? 0);
		public int EquipEvade => this.Slots.Sum(x => x.Evade) + (this.ExSlot?.Evade ?? 0);
		#endregion

		#region 최종 수치
		public int TotalFirepower => 0 < this.Firepower ? this.Firepower + this.EquipFirepower : this.Firepower;
		public int TotalTorpedo => 0 < this.Torpedo ? this.Torpedo + this.EquipTorpedo : this.Torpedo;
		public int TotalAA => 0 < this.AA ? this.AA + this.EquipAA : this.AA;
		public int TotalArmor => 0 < this.Armor ? this.Armor + this.EquipArmor : this.Armor;
		public int TotalASW => this.ASW + this.EquipASW;
		public int TotalEvade => this.Evade; // + this.SlotsEvade;
		#endregion

		// 전달되는 값이 합산값이므로 함선 고유 회피를 구하려면 장비값을 빼야함
		public int ShipEvade => this.Evade - this.EquipEvade;

		// 선제대잠 가능 여부
		public bool OpeningASW
			=> this.MasterId == 141 ? true // 이스즈改2
				: this.ShipType == 1 ? TotalASW >= 60 // 해방함
				: this.ShipType == 7 && this.ShipSpeed == ShipSpeed.Slow ? TotalASW >= 65 // 저속 경공모
				: TotalASW >= 100;

		public LimitedValue HP => new LimitedValue(this.NowHP, this.MaxHP, 0, this.BeforeNowHP);

		#region TODO 나중에
		public AttackType DayAttackType
			=> this.HasScout() && this.Count(SlotitemCategoryType.主砲) == 2 && this.Count(SlotitemCategoryType.対艦強化弾) == 1 ? AttackType.주주컷인
			: this.HasScout() && this.Count(SlotitemCategoryType.主砲) == 1 && this.Count(SlotitemCategoryType.副砲) == 1 && this.Count(SlotitemCategoryType.対艦強化弾) == 1 ? AttackType.주철컷인
			: this.HasScout() && this.Count(SlotitemCategoryType.主砲) == 1 && this.Count(SlotitemCategoryType.副砲) == 1 && this.Count(SlotitemCategoryType.電探) == 1 ? AttackType.주전컷인
			: this.HasScout() && this.Count(SlotitemCategoryType.主砲) >= 1 && this.Count(SlotitemCategoryType.副砲) >= 1 ? AttackType.주부컷인
			: this.HasScout() && this.Count(SlotitemCategoryType.主砲) >= 2 ? AttackType.연격
			: AttackType.통상;

		public AttackType NightAttackType
			=> this.SubmarineRaderCount() >= 1 && this.LateModelTorpedoCount() >= 1 ? AttackType.후기어뢰전탐컷인
			: this.LateModelTorpedoCount() >= 2 ? AttackType.후기어뢰컷인
			: this.Count(SlotitemCategoryType.魚雷) >= 2 ? AttackType.뇌격컷인
			: this.Count(SlotitemCategoryType.主砲) >= 3 ? AttackType.주주주컷인
			: this.Count(SlotitemCategoryType.主砲) == 2 && this.Count(SlotitemCategoryType.副砲) >= 1 ? AttackType.주주부컷인
			: this.Count(SlotitemCategoryType.主砲) == 2 && this.Count(SlotitemCategoryType.副砲) == 0 && this.Count(SlotitemCategoryType.魚雷) == 1 ? AttackType.주뢰컷인
			: this.Count(SlotitemCategoryType.主砲) == 1 && this.Count(SlotitemCategoryType.魚雷) == 1 ? AttackType.주뢰컷인
			: this.Count(SlotitemCategoryType.主砲) == 2 && this.Count(SlotitemCategoryType.副砲) == 0 && this.Count(SlotitemCategoryType.魚雷) == 0 ? AttackType.연격
			: this.Count(SlotitemCategoryType.主砲) == 1 && this.Count(SlotitemCategoryType.副砲) >= 1 && this.Count(SlotitemCategoryType.魚雷) == 0 ? AttackType.연격
			: this.Count(SlotitemCategoryType.主砲) == 0 && this.Count(SlotitemCategoryType.副砲) >= 2 && this.Count(SlotitemCategoryType.魚雷) <= 1 ? AttackType.연격
			: AttackType.통상;
		#endregion

		public ShipData()
		{
			this._Name = "？？？";
			this._AdditionalName = "";
			this._TypeName = "？？？";
			this._ShipType = 0;
			this._Situation = ShipSituation.None;
			this._Slots = new ShipSlotData[0];
			this._ShipSpeed = ShipSpeed.Immovable;
		}

		public virtual ShipData Clone()
		{
			return new ShipData
			{
				Id = this.Id,
				MasterId = this.MasterId,
				Name = this.Name,
				AdditionalName = this.AdditionalName,
				ShipSpeed = this.ShipSpeed,
				ShipType = this.ShipType,
				TypeName = this.TypeName,
				Level = this.Level,
				Situation = this.Situation,
				MaxHP = this.MaxHP,
				NowHP = this.NowHP,
				BeforeNowHP = this.BeforeNowHP,
				Firepower = this.Firepower,
				Torpedo = this.Torpedo,
				AA = this.AA,
				Armor = this.Armor,
				Luck = this.Luck,
				ASW = this.ASW,
				Evade = this.Evade,
				Slots = this.Slots,
				ExSlot = this.ExSlot,
				IsUsedDamecon = this.IsUsedDamecon,
				Condition = this.Condition,
				IsMvp = this.IsMvp
			};
		}
	}
	public static class ShipDataExtensions
	{
		public static int Count(this ShipData data, SlotitemCategoryType type2)
		{
			return data.Slots.Count(x => x.CategoryType == type2)
				+ (data.ExSlot?.CategoryType == type2 ? 1 : 0);
		}

		public static bool HasScout(this ShipData data)
		{
			return data.Slots
				.Where(x => x.Source.Type == SlotItemType.水上偵察機
							|| x.Source.Type == SlotItemType.水上爆撃機)
				.Any(x => 0 < x.Current);
		}

		public static int SubmarineRaderCount(this ShipData data)
		{
			var SubmarineRaders = new int[]
			{
				210, // 潜水艦搭載電探&水防式望遠鏡
				211, // 潜水艦搭載電探&逆探(E27)
			};
			return data.Slots.Count(x => SubmarineRaders.Contains(x.Source.Id))
				+ (SubmarineRaders.Contains(data.ExSlot?.Source.Id ?? 0) ? 1 : 0);
		}
		public static int LateModelTorpedoCount(this ShipData data)
		{
			var LateModelTorpedos = new int[]
			{
				213, // 後期型艦首魚雷(6門)
				214, // 熟練聴音員+後期型艦首魚雷(6門)
			};
			return data.Slots.Count(x => LateModelTorpedos.Contains(x.Source.Id))
				+ (LateModelTorpedos.Contains(data.ExSlot?.Source.Id ?? 0) ? 1 : 0);
		}

		public static bool IsHeavilyDamage(this ShipData ship)
		{
			return (ship.NowHP / (double)ship.MaxHP) <= 0.25;
		}
	}

	public class MembersShipData : ShipData
	{
		public MembersShipData()
		{
		}
		public MembersShipData(Ship ship) : this()
		{
			this.Id = ship.Id;
			this.MasterId = ship.Info.Id;

			this.Name = ship.Info.Name;
			this.Level = ship.Level;
			this.Situation = ship.Situation;

			this.NowHP = ship.HP.Current;
			this.MaxHP = ship.HP.Maximum;

			this.ShipSpeed = ship.Speed;
			this.ShipType = ship.Info.ShipType.Id;
			this.TypeName = ship.Speed == ShipSpeed.Immovable
				? "육상기지"
				: ship.Info.ShipType.Name;

			this.Slots = ship.Slots
				.Where(s => s != null)
				.Where(s => s.Equipped)
				.Select(s => new ShipSlotData(s))
				.ToArray();
			this.ExSlot =
				ship.ExSlotExists && ship.ExSlot.Equipped
				? new ShipSlotData(ship.ExSlot)
				: null;

			this.Condition = ship.Condition;

			this.Firepower = ship.Firepower.Current;
			this.Torpedo = ship.Torpedo.Current;
			this.AA = ship.AA.Current;
			this.Armor = ship.Armer.Current;
			this.Luck = ship.Luck.Current;
			this.ASW = ship.ASW.Current;
			this.Evade = ship.RawData.api_kaihi[0];
		}
	}
	public class MastersShipData : ShipData
	{
		public MastersShipData()
		{
		}

		public MastersShipData(ShipInfo info) : this()
		{
			this.Id = info.Id;
			this.Name = info.Name;

			this.Condition = -1;

			this.AdditionalName = info?.Id > 1500 ? info?.RawData.api_yomi : "";

			this.ShipSpeed = info?.Speed ?? ShipSpeed.Immovable;
			this.TypeName = info?.Speed == ShipSpeed.Immovable
				? "육상기지"
				: info?.ShipType.Name;
		}
	}
}
