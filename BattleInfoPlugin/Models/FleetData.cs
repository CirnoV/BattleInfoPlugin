using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using Grabacr07.KanColleWrapper.Models;
using BattleInfoPlugin.Properties;

namespace BattleInfoPlugin.Models
{
	public class FleetData : NotificationObject
	{
		#region FleetType変更通知プロパティ
		private FleetType _FleetType;
		public FleetType FleetType
		{
			get { return this._FleetType; }
			set
			{
				if (this._FleetType != value)
				{
					this._FleetType = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region Name変更通知プロパティ
		private string _Name;
		public string Name
		{
			get { return this._Name; }
			set
			{
				if (this._Name != value)
				{
					this._Name = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region AttackGauge変更通知プロパティ
		private string _AttackGauge;
		public string AttackGauge
		{
			get { return this._AttackGauge; }
			set
			{
				if (this._AttackGauge != value)
				{
					this._AttackGauge = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region IsCritical変更通知プロパティ
		private bool _IsCritical;
		public bool IsCritical
		{
			get { return this._IsCritical; }
			set
			{
				if (this._IsCritical != value)
				{
					this._IsCritical = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region Ships変更通知プロパティ
		private IEnumerable<ShipData> _Ships;
		public IEnumerable<ShipData> Ships
		{
			get { return this._Ships; }
			set
			{
				if (this._Ships != value)
				{
					this._Ships = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region Formation変更通知プロパティ
		private Formation _Formation;
		public Formation Formation
		{
			get { return this._Formation; }
			set
			{
				if (this._Formation != value)
				{
					this._Formation = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		//#region AirSuperiorityPotential変更通知プロパティ
		//private int _AirSuperiorityPotential;

		//public int AirSuperiorityPotential
		//{
		//	get
		//	{ return this._AirSuperiorityPotential; }
		//	set
		//	{ 
		//		if (this._AirSuperiorityPotential == value)
		//			return;
		//		this._AirSuperiorityPotential = value;
		//		this.RaisePropertyChanged();
		//	}
		//}
		//#endregion

		//public int AirParityRequirements => this.AirSuperiorityPotential * 2 / 3;
		//public int AirSuperiorityRequirements => this.AirSuperiorityPotential * 3 / 2;
		//public int AirSupremacyRequirements => this.AirSuperiorityPotential * 3;

		public FleetData() : this(new ShipData[0], Formation.없음, "", FleetType.EnemyFirst)
		{
		}

		public FleetData(IEnumerable<ShipData> ships, Formation formation, string name, FleetType type)
		{
			this._Ships = ships;
			this._Formation = formation;
			this._Name = name;
			this._FleetType = type;

			if (type == FleetType.EnemyFirst || type == FleetType.EnemySecond) return;
			this.IsCritical = this.Ships?
				.Any(x => (x.NowHP / (double)x.MaxHP <= 0.25) && (x.NowHP / (double)x.MaxHP > 0))
				?? false;

			//this._AirSuperiorityPotential = this._Ships
			//	.SelectMany(s => s.Slots)
			//	.Where(s => s.Source.IsAirSuperiorityFighter)
			//	.Sum(s => (int)(s.AA * Math.Sqrt(s.Current)))
			//	;
		}

		public FleetData Clone()
		{
			return new FleetData
			{
				FleetType = this.FleetType,
				Name = this.Name,
				AttackGauge = this.AttackGauge,
				IsCritical = this.IsCritical,
				Ships = this.Ships.Select(x => x.Clone()).ToArray(),
				Formation = this.Formation
			};
		}
	}

	public static class FleetDataExtensions
	{
		/// <summary>
		/// Actionを使用して値を設定
		/// Zipするので要素数が少ない方に合わせられる
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="source"></param>
		/// <param name="values"></param>
		/// <param name="setter"></param>
		public static void SetValues<TSource, TValue>(
			this IEnumerable<TSource> source,
			IEnumerable<TValue> values,
			Action<TSource, TValue> setter)
		{
			source.Zip(values, (s, v) => new { s, v })
				.ToList()
				.ForEach(x => setter(x.s, x.v));
		}
		public static bool CriticalCheck(this FleetData fleet)
		{
			if (fleet.Ships
					.Where(x => !x.Situation.HasFlag(ShipSituation.DamageControlled))
					.Where(x => !x.Situation.HasFlag(ShipSituation.Evacuation))
					.Where(x => !x.Situation.HasFlag(ShipSituation.Tow))
					.Any(x => x.Situation.HasFlag(ShipSituation.HeavilyDamaged)))
				return true;
			else return false;
		}
	}
}
