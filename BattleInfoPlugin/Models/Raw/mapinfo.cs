﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper.Models.Raw;

namespace BattleInfoPlugin.Models.Raw
{
	// ReSharper disable InconsistentNaming
	public class kcsapi_mapinfo
	{
		public kcsapi_mapinfo_data[] api_map_info { get; set; }
	}

	public class kcsapi_mapinfo_data
	{
		public kcsapi_eventmap api_eventmap { get; set; }
	}
	// ReSharper restore InconsistentNaming
}
