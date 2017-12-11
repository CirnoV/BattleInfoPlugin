using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin.Models
{
	public enum Rank
	{
		에러 = -1,
		없음 = 0,

		완전승리S = 1,
		S승리 = 2,
		A승리 = 3,
		B승리 = 4,
		C패배 = 5,
		D패배 = 6,
		E패배 = 7,

		공습전 = 8
	}

	public static class RankExtension
	{
		public static Rank ConvertRank(int rank)
		{
			switch (rank)
			{
				case 0: return Rank.E패배;
				case 1: return Rank.D패배;
				case 2: return Rank.C패배;
				case 3: return Rank.B승리;
				case 4: return Rank.A승리;
				case 5: return Rank.S승리;
				case 6: return Rank.완전승리S;
			}
			return Rank.없음;
		}
		public static Rank ConvertRank(string rank)
		{
			switch (rank)
			{
				case "S": return Rank.S승리;
				case "A": return Rank.A승리;
				case "B": return Rank.B승리;
				case "C": return Rank.C패배;
				case "D": return Rank.D패배;
				case "E": return Rank.E패배;
			}
			return Rank.에러;
		}
	}
}