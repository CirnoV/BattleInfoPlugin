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
	public class UsedFlagConverter : IValueConverter
	{
		public static UsedFlagConverter Instance { get; } = new UsedFlagConverter();
		public static object Convert(object value)
			=> UsedFlagConverter.Instance.Convert(value, null, null, null);

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var _value = BattleFlags.UsedFlag.Unset;

			if (value is BattleFlags.UsedFlag)
				_value = (BattleFlags.UsedFlag)value;

			else if (value is string)
				Enum.TryParse<BattleFlags.UsedFlag>(value as string, out _value);

			switch (_value)
			{
				case BattleFlags.UsedFlag.Unset:
					return "";

				case BattleFlags.UsedFlag.Unused:
					return "발동 안됨";

				case BattleFlags.UsedFlag.Alias:
					return "아군 발동";

				case BattleFlags.UsedFlag.Enemy:
					return "적군 발동";

				case BattleFlags.UsedFlag.Both:
					return "쌍방 발동";

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
