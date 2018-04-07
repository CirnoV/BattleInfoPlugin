using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleInfoPlugin.Models
{
	/// <summary>
	/// 대파 진격했을 때 플러그인이 수행할 행동
	/// </summary>
	public enum CriticalActionType
	{
		/// <summary>
		/// 알림
		/// </summary>
		Notify,

		/// <summary>
		/// 무시하기
		/// </summary>
		None,

		/// <summary>
		/// 강제 새로고침
		/// </summary>
		ForceRefresh,
	}
	public static class CriticalActionTypeExtension
	{
		public static CriticalActionType Parse(string value, CriticalActionType defaultValue = CriticalActionType.Notify)
		{
			CriticalActionType ret;
			if (Enum.TryParse<CriticalActionType>(value, out ret))
				return ret;

			return defaultValue;
		}
	}
}
