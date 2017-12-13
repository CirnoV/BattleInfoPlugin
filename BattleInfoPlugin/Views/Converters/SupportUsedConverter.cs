using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using BattleInfoPlugin.Models;

namespace BattleInfoPlugin.Views.Converters
{
	public class SupportUsedConverter : IValueConverter
	{
		public static SupportUsedConverter Instance { get; } = new SupportUsedConverter();
		public static object Convert(object value)
			=> SupportUsedConverter.Instance.Convert(value, null, null, null);

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var _value = BattleFlags.SupportType.Unset;

			if (value is BattleFlags.SupportType)
				_value = (BattleFlags.SupportType)value;

			else if (value is string)
				Enum.TryParse<BattleFlags.SupportType>(value as string, out _value);

			switch (_value)
			{
				case BattleFlags.SupportType.Unset:
					return "";

				case BattleFlags.SupportType.Unused:
					return "지원 없음";

				case BattleFlags.SupportType.GunFire:
					return "포격 지원";

				case BattleFlags.SupportType.Torpedo:
					return "뇌격 지원";

				case BattleFlags.SupportType.Airstrike:
					return "항공 지원";

				default:
					return "???";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

	}
}
