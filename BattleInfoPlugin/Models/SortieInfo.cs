using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleInfoPlugin.Models.Static;
using Grabacr07.KanColleWrapper.Models.Raw;
using Grabacr07.KanColleWrapper.Models;
using BattleInfoPlugin.Models.Raw;
using BattleInfoPlugin.Views.Converters;
using Livet;
using Nekoxy;

namespace BattleInfoPlugin.Models
{
	public class SortieInfo : NotificationObject
	{
		/// <summary>
		/// Initialize, Practice, Update, UpdateRank 가 처리된 후에 호출
		/// </summary>
		public event EventHandler Updated;

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
		/// 방문한 노드 기록
		/// </summary>
		public BaseNodeData[] NodeHistory => this._NodeHistory.ToArray();
		private List<BaseNodeData> _NodeHistory { get; set; } = new List<BaseNodeData>();

		/// <summary>
		/// 현재 방문중인 노드 정보
		/// </summary>
		public BaseNodeData CurrentNode => this._NodeHistory.LastOrDefault();

		/// <summary>
		/// "World-Map Difficulty" 형식의 맵 정보
		/// </summary>
		public string MapDisplay => string.Format(
			"{0}-{1} {2}",
			this.World,
			this.Map,
			this.Difficulty.ToDisplayString()
		).Trim();


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

			this.Updated?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// 연습전인 경우에 초기화되는 정보
		/// </summary>
		public void Practice(BattleData battleData)
		{
			this.World = World;
			this.Map = Map;
			this.Node = 0;
			this.NodeEvent = new NodeEventInfo();
			this.Difficulty = Difficulty;

			this._NodeHistory.Clear();
			this._NodeHistory.Add(
				new PracticeNodeData()
				{
					AliasFirst = battleData.AliasFirst?.Clone(),
					AliasSecond = battleData.AliasSecond?.Clone(),
					EnemyFirst = battleData.EnemyFirst?.Clone(),
					EnemySecond = battleData.EnemySecond?.Clone(),

					BattleFlags = battleData.CurrentBattleFlag?.Clone()
		}
			);

			this.Updated?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// 노드 정보 갱신, 새로운 노드에 진입한 경우 호출
		/// </summary>
		/// <param name="Node">노드</param>
		/// <param name="EventId">노드 Id</param>
		/// <param name="EventKind">노드 종류</param>
		public void Update(int Node, int EventId, int EventKind, Session session, BattleData battleData)
		{
			this.Node = Node;
			this.NodeEvent = new NodeEventInfo
			{
				Id = (NodeEventId)EventId,
				Kind = (NodeEventKind)EventKind,
				session = session
			};

			this._NodeHistory.ForEach(x => x.Outdate());
			this._NodeHistory.Add(
				new SortieNodeData(this.World, this.Map, Node, this.NodeEvent)
				{
					AliasFirst = battleData.AliasFirst?.Clone(),
					AliasSecond = battleData.AliasSecond?.Clone(),
					EnemyFirst = battleData.EnemyFirst?.Clone(),
					EnemySecond = battleData.EnemySecond?.Clone(),

					BattleFlags = battleData.CurrentBattleFlag?.Clone()
				}
			);

			this.Updated?.Invoke(this, EventArgs.Empty);
		}

		internal void UpdateResult(BattleData BattleData)
		{
			if (typeof(SortieNodeData) != CurrentNode.GetType()) return;

			(this.CurrentNode as SortieNodeData)
				.UpdateResult(BattleData);

			this.Updated?.Invoke(this, EventArgs.Empty);
		}
	}

	#region Definitions
	public enum NodeEventId
	{
		None = 0,
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

		public NodeEventId Id { get; set; }
		public NodeEventKind Kind { get; set; }

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
							api_happening.api_count > 1 ? "{0}-{1}" : "{0}",
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
	public class BaseNodeData
	{
		/// <summary>
		/// 노드 알파벳
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// 노드 설명 (적군조우 등)
		/// </summary>
		public virtual string Detail { get; set; }

		/// <summary>
		/// 알파벳과 설명을 합친 풀네임
		/// </summary>
		public string FullName => string.Format("{0} {1}", this.Name, this.Detail).Trim();

		/// <summary>
		/// 아주 자세한 설명.
		/// 추가적인 설명이 필요할 경우에 사용.
		/// </summary>
		public virtual string Description { get; set; }

		/// <summary>
		/// 노드가 생성된 시간 (기본값으로 Now)
		/// </summary>
		public virtual DateTimeOffset NodeTime { get; set; } = DateTimeOffset.Now;

		/// <summary>
		/// 전투 정보
		/// </summary>
		public virtual BattleHistory BattleData { get; private set; }

		/// <summary>
		/// 현재 노드가 아닌지 여부
		/// </summary>
		public bool Outdated { get; private set; }

		/// <summary>
		/// 과거의 노드로 처리
		/// </summary>
		public void Outdate() => this.Outdated = true;

		#region FleetData s
		public FleetData AliasFirst { get; set; }
		public FleetData AliasSecond { get; set; }
		public FleetData EnemyFirst { get; set; }
		public FleetData EnemySecond { get; set; }

		public BattleFlags BattleFlags { get; set; }
		#endregion

		/// <summary>
		/// 전투 결과가 확정된 이후 그 결과를 기록
		/// </summary>
		/// <param name="rank">전투의 랭크</param>
		/// <param name="Calculator">전투 계산기</param>
		public void UpdateResult(BattleData BattleData)
		{
			this.BattleData = new BattleHistory(BattleData);

			this.AliasFirst = BattleData.AliasFirst?.Clone();
			this.AliasSecond = BattleData.AliasSecond?.Clone();
			this.EnemyFirst = BattleData.EnemyFirst?.Clone();
			this.EnemySecond = BattleData.EnemySecond?.Clone();

			this.BattleFlags = BattleData.CurrentBattleFlag?.Clone();
		}
	}

	/// <summary>
	/// 출격 노드 정보
	/// </summary>
	public class SortieNodeData : BaseNodeData
	{
		public MapNodeInfo.NodeInfo NodeInfo { get; private set; }
		public int World => NodeInfo.World;
		public int Map => NodeInfo.Map;
		public int Node => NodeInfo.Node;
		public override string Name => NodeInfo.Display;

		public NodeEventInfo EventInfo { get; private set; }
		public override string Detail => EventInfo.ToString();

		public override string Description => this.GetDescription();

		public SortieNodeData(int World, int Map, int Node, NodeEventInfo EventInfo)
		{
			this.NodeInfo = MapNodeInfo.GetNodeInfo(World, Map, Node);
			this.EventInfo = EventInfo;
		}

		/// <summary>
		/// 상세 정보를 생성
		/// </summary>
		/// <returns></returns>
		private string GetDescription()
		{
			var sb = new StringBuilder();
			sb.AppendFormat("{0} {1}", this.Name, this.Detail);
			sb.AppendLine();

			if (this.BattleData != null)
			{
				var calc = this.BattleData.BattleCalculator;
				var flags = this.BattleData.BattleFlags;

				sb.AppendLine();
				sb.AppendFormat("전투 결과: {0}", this.BattleData.RankResult.ToString());
				sb.AppendLine();
				sb.AppendFormat("드롭: {0}", string.IsNullOrEmpty(this.BattleData.DropShipName) ? "없음" : this.BattleData.DropShipName);
				sb.AppendLine();
			}

			sb.AppendLine();
			sb.Append("(클릭하여 자세히 보기)");

			return sb.ToString().Trim();
		}
	}

	/// <summary>
	/// 연습전 정보
	/// </summary>
	public class PracticeNodeData : BaseNodeData
	{
		public override string Name => "";
		public override string Detail => "연습전";

		public PracticeNodeData()
		{
		}
	}
	#endregion

	internal static class SortieInfoExtension
	{
		public static string ToDisplayString(this MapDifficulty difficulty)
		{
			switch (difficulty)
			{
				case MapDifficulty.Easy: return "丙";
				case MapDifficulty.Normal: return "乙";
				case MapDifficulty.Hard: return "甲";
				default:
					return "";
			}
		}
	}
}
