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
	public class AntiAirFireConverter : IValueConverter
	{
		public static AntiAirFireConverter Instance { get; } = new AntiAirFireConverter();
		public static object Convert(object value)
			=> AntiAirFireConverter.Instance.Convert(value, null, null, null);

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var _value = BattleFlags.AntiAirFireFlag.Unset;

			if (value is BattleFlags.AntiAirFireFlag)
				_value = (BattleFlags.AntiAirFireFlag)value;

			else if (value is string)
				Enum.TryParse<BattleFlags.AntiAirFireFlag>(value as string, out _value);

			switch (_value)
			{
				case BattleFlags.AntiAirFireFlag.Unset:
					return "";

				case BattleFlags.AntiAirFireFlag.Unused:
					return "발동 안됨";

				case BattleFlags.AntiAirFireFlag.Used:
					return "발동됨";

				case BattleFlags.AntiAirFireFlag.First:
					return "1차 발동";

				case BattleFlags.AntiAirFireFlag.Second:
					return "2차 발동";

				case BattleFlags.AntiAirFireFlag.Both:
					return "1,2차 발동";

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
