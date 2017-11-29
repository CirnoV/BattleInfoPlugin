using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleInfoPlugin.Models
{
	/// <summary>
	/// 1艦隊分のダメージ一覧
	/// </summary>
	public class FleetDamages
	{
		public int Ship1 { get; set; }
		public int Ship2 { get; set; }
		public int Ship3 { get; set; }
		public int Ship4 { get; set; }
		public int Ship5 { get; set; }
		public int Ship6 { get; set; }
        public int Ship7 { get; set; }

        public int[] ToArray()
		{
			return new[]
			{
				this.Ship1,
				this.Ship2,
				this.Ship3,
				this.Ship4,
				this.Ship5,
                this.Ship6,
                this.Ship7,
            };
		}

		public FleetDamages Add(FleetDamages value)
		{
			return Parse(new[]
			{
				this.Ship1 + value.Ship1,
				this.Ship2 + value.Ship2,
				this.Ship3 + value.Ship3,
				this.Ship4 + value.Ship4,
				this.Ship5 + value.Ship5,
                this.Ship6 + value.Ship6,
                this.Ship7 + value.Ship7,
            });
		}

		public static FleetDamages Parse(IEnumerable<int> damages)
		{
			if (damages == null) throw new ArgumentNullException();

			var arr = damages.ToArray();
            var dat = new int[7];
            for (var i = 0; i < arr.Length; i++)
                dat[i] = arr[i];

			// if (arr.Length != 6) throw new ArgumentException("艦隊ダメージ配列の長さは6である必要があります。");

			return new FleetDamages
			{
				Ship1 = dat[0],
				Ship2 = dat[1],
				Ship3 = dat[2],
				Ship4 = dat[3],
				Ship5 = dat[4],
                Ship6 = dat[5],
                Ship7 = dat[6],
            };
		}
	}

	public static class FleetDamagesExtensions
	{
		public static FleetDamages ToFleetDamages(this IEnumerable<int> damages)
		{
			return FleetDamages.Parse(damages);
		}
	}
}
