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
	public class NodeEventIdColorConverter: IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var _value = NodeEventId.None;

			if (value is NodeEventId)
				_value = (NodeEventId)value;

			else if (value is string)
				Enum.TryParse<NodeEventId>(value as string, out _value);

			switch (_value)
			{
				case NodeEventId.Escort:
				case NodeEventId.Obtain:
					return "#FF3FBD2B";

				case NodeEventId.Loss:
					return "#FFA33CEA";

				case NodeEventId.NormalBattle:
					return "#FFF01717";

				case NodeEventId.BossBattle:
					return "#FFD40C0C";

				case NodeEventId.NoEvent:
					return "#FF0A8AB9";

				case NodeEventId.AirEvent:
					return "#FF929A11";

				case NodeEventId.LDAirBattle:
					return "#FF51A6C5";

				case NodeEventId.TP:
				default:
					return "#48FFFFFF";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
