using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleInfoPlugin.Models.Static;
using Grabacr07.KanColleWrapper.Models.Raw;
using Grabacr07.KanColleWrapper.Models;
using BattleInfoPlugin.Models.Raw;
using Livet;
using Nekoxy;

namespace BattleInfoPlugin.Models
{
	public class SortieInfo : NotificationObject
	{
		#region Definitions
		public enum NodeEventId
		{
			Obtain = 2,
			Loss = 3,
			NormalBattle = 4,
			BossBattle = 5,
			NoEvent = 6,
			AirEvent = 7,
			Escort = 8,
			TP = 9,
			LDAirBattle = 10,
		}
		public enum NodeEventKind
		{
			NoBattle = 0,
			Battle = 1,
			NightBattle = 2,
			NightDayBattle = 3,
			AirBattle = 4,
			ECBattle = 5,
			LDAirBattle = 6,
			ECNightDayBattle = 7,

			AirSearch = 0,
			Selectable = 2,
		}

		public class NodeEventInfo
		{
			public Session session { get; set; }

			public NodeEventKind Kind { get; set; }
			public NodeEventId Id { get; set; }

			public override string ToString()
			{
				if (session == null) return "???";

				SvData<map_start_next> svdata;
				if (!SvData.TryParse<map_start_next>(session, out svdata)) return "???";

				var api_itemget = svdata.Data.api_itemget;
				var api_happening = svdata.Data.api_happening;
				var api_itemget_eo = svdata.Data.api_itemget_eo_comment;

				switch (this.Id)
				{
					case NodeEventId.Obtain:
						if (api_itemget == null)
							return "자원획득";

						return string.Join(
							" ",
							api_itemget
								.Select(x => {
									var resname = NodeItemInfo.Exists(x.api_id - 1)
										? NodeItemInfo.Get(x.api_id - 1)
										: (x.api_name?.Length > 0 ? x.api_name : "???");

									return x.api_getcount > 1
										? string.Format("{0} +{1}", resname, x.api_getcount)
										: resname;
								})
								.ToArray()
						);

					case NodeEventId.Loss:
						if (api_happening == null || api_happening.api_count == 0)
							return "소용돌이";

						return string.Format(
								api_happening.api_count>1 ? "{0}-{1}" : "{0}",
								NodeItemInfo.Exists(api_happening.api_mst_id - 1)
									? NodeItemInfo.Get(api_happening.api_mst_id - 1)
									: "???",
								api_happening.api_count
							);

					case NodeEventId.NormalBattle:
						switch (this.Kind)
						{
							case NodeEventKind.Battle:
								return "적군조우";

							case NodeEventKind.NightBattle:
								return "야간전투";

							case NodeEventKind.NightDayBattle:
								return "야전>주간전";

							case NodeEventKind.AirBattle:
								return "항공전";

							case NodeEventKind.ECBattle:
								return "심해연합";

							case NodeEventKind.LDAirBattle:
								return "공습전";

							case NodeEventKind.ECNightDayBattle:
								return "야전>주간전 (연합)";

							default:
								return "전투(알 수 없음)";
						}

					case NodeEventId.BossBattle:
						switch (this.Kind)
						{
							case NodeEventKind.NightBattle:
								return "보스 (야간전투)";

							case NodeEventKind.NightDayBattle:
								return "보스 (야전>주간전)";

							case NodeEventKind.ECBattle:
								return "보스 (심해연합)";

							case NodeEventKind.ECNightDayBattle:
								return "보스 (야전>주간전,연합)";

							default:
								return "보스전";
						}

					case NodeEventId.NoEvent:
						if (this.Kind == NodeEventKind.Selectable)
							return "능동분기";
						return "기분탓";

					case NodeEventId.TP:
						return "수송지점";

					case NodeEventId.AirEvent:
						switch (this.Kind)
						{
							case NodeEventKind.AirSearch:
							{
								SvData<map_start_next2> svdata2;
								if (!SvData.TryParse<map_start_next2>(session, out svdata2))
									return "정찰실패";

								var data = svdata2.Data;
								if (data.api_itemget == null)
									return "정찰실패";

								return string.Format(
									data.api_itemget.api_getcount > 1 ? "{0}+{1}" : "{0}",
									NodeItemInfo.Exists(data.api_itemget.api_id - 1)
										? NodeItemInfo.Get(data.api_itemget.api_id - 1)
										: (data.api_itemget.api_name?.Length > 0 ? data.api_itemget.api_name : "???"),
									data.api_itemget.api_getcount
								);
							}

							case NodeEventKind.AirBattle:
							default:
								return "항공전";
						}

					case NodeEventId.Escort:
					{
						var x = api_itemget_eo;
						if (x == null)
							return "자원획득";

						return string.Format(
							x.api_getcount > 1 ? "{0}+{1}" : "{0}",
							NodeItemInfo.Exists(x.api_id - 1)
								? NodeItemInfo.Get(x.api_id - 1)
								: (x.api_name?.Length > 0 ? x.api_name : "???"),
							x.api_getcount
						);
					}

					case NodeEventId.LDAirBattle:
						return "공습전";

					default:
						return string.Format("??? ({0}.{1})", (int)this.Id, (int)this.Kind);
				}
			}
		}

		/// <summary>
		/// 노드의 완성된 데이터 집합
		/// </summary>
		public class FullNodeData
		{
			private MapNodeInfo.NodeInfo NodeInfo { get; set; }
			public int World => NodeInfo.World;
			public int Map => NodeInfo.Map;
			public int Node => NodeInfo.Node;
			public string Name => NodeInfo.Display;

			private NodeEventInfo EventInfo { get; set; }
			public string Detail => EventInfo.ToString();

			public bool Outdated { get; private set; }

			public FullNodeData(int World, int Map, int Node, NodeEventInfo EventInfo)
			{
				this.NodeInfo = MapNodeInfo.GetNodeInfo(World, Map, Node);
				this.EventInfo = EventInfo;
			}
			public void Outdate()
			{ 
				this.Outdated = true;
			}
		}
		#endregion

		/// <summary>
		/// 현재 출격중인 해역의 난이도
		/// </summary>
		public MapDifficulty Difficulty {
			get { return this._Difficulty; }
			set
			{
				if (this._Difficulty != value)
				{
					this._Difficulty = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private MapDifficulty _Difficulty { get; set; }

		/// <summary>
		/// 현재 출격중인 지역 (A-B 중 A)
		/// </summary>
		public int World {
			get { return this._World; }
			private set
			{
				if (this._World != value)
				{
					this._World = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private int _World { get; set; }

		/// <summary>
		/// 현재 출격중인 해역 (A-B 중 B)
		/// </summary>
		public int Map
		{
			get { return this._Map; }
			private set
			{
				if(this._Map != value)
				{
					this._Map = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private int _Map { get; set; }

		/// <summary>
		/// 현재 방문중인 노드 번호
		/// </summary>
		public int Node
		{
			get { return this._Node; }
			private set
			{
				if (this._Node != value)
				{
					this._Node = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private int _Node { get; set; }

		/// <summary>
		/// 현재 방문중인 노드의 종류
		/// </summary>
		public NodeEventInfo NodeEvent
		{
			get { return this._NodeEvent; }
			private set
			{
				if (this._NodeEvent != value)
				{
					this._NodeEvent = value;
					this.RaisePropertyChanged();
				}
			}
		}
		private NodeEventInfo _NodeEvent { get; set; }

		/// <summary>
		/// 방문한 노드 기록 및 현재 방문중인 노드 정보
		/// </summary>
		public FullNodeData[] NodeHistory => this._NodeHistory.ToArray();
		public FullNodeData CurrentNode => this._NodeHistory.LastOrDefault();
		private List<FullNodeData> _NodeHistory { get; set; }


		/// <summary>
		/// 정보 초기화
		/// </summary>
		/// <param name="World">지역</param>
		/// <param name="Map">해역</param>
		/// <param name="Difficulty">해역 난이도</param>
		public void Initialize(int World, int Map, MapDifficulty Difficulty = MapDifficulty.None)
		{
			this.World = World;
			this.Map = Map;
			this.Node = 0;
			this.NodeEvent = new NodeEventInfo();
			this.Difficulty = Difficulty;

			this._NodeHistory.Clear();
		}

		/// <summary>
		/// 노드 정보 갱신, 새로운 노드에 진입한 경우 호출
		/// </summary>
		/// <param name="Node">노드</param>
		/// <param name="EventKind">노드 종류</param>
		/// <param name="EventId">노드 Id</param>
		public void Update(int Node, int EventKind, int EventId, Session session)
		{
			this.Node = Node;
			this.NodeEvent = new NodeEventInfo
			{
				Kind = (NodeEventKind)EventKind,
				Id = (NodeEventId)EventId,
				session = session
			};

			this._NodeHistory.Add(
				new FullNodeData(this.World, this.Map, Node, this.NodeEvent)
			);
		}
	}
}
