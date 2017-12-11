using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin.Models.Static
{
	public static class NodeItemInfo
	{
		private static Dictionary<int, string> NodeItemList
			= new Dictionary<int, string>
			{
				{  0, "연료" },
				{  1, "탄약" },
				{  2, "강재" },
				{  3, "보크사이트" },
				{  4, "고속건조재" },
				{  5, "고속수복재" },
				{  6, "개발자재" },
				{  7, "개수자재" },
				{  9, "가구함(소)" },
				{ 10, "가구함(중)" },
				{ 11, "가구함(대)" },
			};

		/// <summary>
		/// 주어진 id에 대한 값이 존재하는지 검사
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static bool Exists(int id)
			=> NodeItemList.ContainsKey(id);

		/// <summary>
		/// 주어진 id에 대한 값을 가져옴. 없는 경우 ??? 를 반환
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static string Get(int id)
			=> Exists(id) ? NodeItemList[id] : "???";
	}
}
