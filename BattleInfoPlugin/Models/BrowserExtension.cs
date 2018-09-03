using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Linq;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using BattleInfoPlugin.Models.Raw;
using Grabacr07.KanColleViewer.Views.Controls;
using Grabacr07.KanColleViewer;
using Grabacr07.KanColleWrapper;
using System.Net;
using Dispatcher = System.Windows.Threading.Dispatcher;
using WebBrowser = CefSharp.Wpf.ChromiumWebBrowser;

namespace BattleInfoPlugin.Models
{
	public class overlayMarkers
	{
		public string img { get; set; }
		public int[] pos { get; set; }
		public int[] size { get; set; }
	}
	public class overlayNode
	{
		public Dictionary<string, int[]> letters { get; set; }
		public overlayMarkers[] markers { get; set; }
	}
	public class overlayData : Dictionary<string, overlayNode>
	{
	}

	internal class BrowserExtension
	{
		private overlayData overlayTableData { get; set; }
		private WebBrowser browserView { get; set; }

		private Dispatcher dispatcher { get; set; }

		public void Startup()
		{
			#region Read data JSON
			try
			{
				HttpWebRequest rq = WebRequest.Create("https://raw.githubusercontent.com/KC3Kai/KC3Kai/master/src/data/nodes.json") as HttpWebRequest;
				rq.Timeout = 5000;
				HttpWebResponse response = rq.GetResponse() as HttpWebResponse;

				using (var reader = new StreamReader(response.GetResponseStream()))
				{
					var bytes = Encoding.UTF8.GetBytes(reader.ReadToEnd());
					var serializer = new DataContractJsonSerializer(typeof(overlayData), new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true });
					using (var stream = new MemoryStream(bytes))
					{
						var rawResult = serializer.ReadObject(stream) as overlayData;
						this.overlayTableData = rawResult;
					}
				}
			}
			catch
			{
				return;
			}
			#endregion

			#region Find webbrowser and add overlay element
			Grabacr07.KanColleViewer.Application app = Grabacr07.KanColleViewer.Application.Instance as Grabacr07.KanColleViewer.Application;
			KanColleHost host = null;

			this.dispatcher = app.Dispatcher;
			this.dispatcher.Invoke(() =>
			{
				host = FindElement<KanColleHost>(app.MainWindow.Content as UIElement);
				if (host == null) return;

				try
				{
					var browser = browserView = host.WebBrowser;

					var css = "#battleinfo_display_overlay { position:fixed; left:0; top:0; width:800px; height:480px; z-index:9145; pointer-events:none; font:bold 22px sans-serif; }"
						+ " @keyframes fadeOut { from { opacity: 1 } to { opacity: 0 } }"
						+ " #battleinfo_display_overlay > .overlay_node, #battleinfo_display_overlay > .overlay_marker { animation-duration: 4s; animation-fill-mode: both; animation-delay: 10s; }"
						+ " #battleinfo_display_overlay > .overlay_node { position:absolute; display:inline-block; text-shadow: #F5F5F5 1px 1px 2px, #F5F5F5 0 0 2px; color:#333; font:inherit; animation-name: fadeOut }"
						+ " #battleinfo_display_overlay > .overlay_marker { position:absolute; display:block; border:2px solid #37373f; border-radius:9999px; background-color:#F5F5F5; animation-name: fadeOut }";

					var js = "var css = document.createElement('style'); css.type='text/css'; css.innerHTML='" + css + "'; document.body.appendChild(css);"
					+ "var layer = document.createElement('div'); layer.id='battleinfo_display_overlay'; document.body.appendChild(layer);";

					browserView.GetBrowser().MainFrame.ExecuteJavaScriptAsync(js);
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex);
					return;
				}
			});
			#endregion

			#region HTTP captures
			var proxy = KanColleClient.Current.Proxy;

			proxy.api_port.Subscribe(x => this.resetLayer());

			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_battle_midnight/battle").Subscribe(x => this.resetLayer());
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_battle_midnight/sp_midnight").Subscribe(x => this.resetLayer());
			proxy.api_req_combined_battle_airbattle.Subscribe(x => this.resetLayer());
			proxy.api_req_combined_battle_battle.Subscribe(x => this.resetLayer());
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/battle_water").Subscribe(x => this.resetLayer());
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/midnight_battle").Subscribe(x => this.resetLayer());
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/sp_midnight").Subscribe(x => this.resetLayer());
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_practice/battle").Subscribe(x => this.resetLayer());
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_practice/midnight_battle").Subscribe(x => this.resetLayer());
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_practice/battle_result").Subscribe(x => this.resetLayer());
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_sortie/airbattle").Subscribe(x => this.resetLayer());
			proxy.api_req_sortie_battle.Subscribe(x => this.resetLayer());
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_sortie/ld_airbattle").Subscribe(x => this.resetLayer());
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/ld_airbattle").Subscribe(x => this.resetLayer());
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/ec_battle").Subscribe(x => this.resetLayer());
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/each_battle").Subscribe(x => this.resetLayer());
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/each_battle_water").Subscribe(x => this.resetLayer());
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/ec_midnight_battle").Subscribe(x => this.resetLayer());
			proxy.api_req_sortie_battleresult.Subscribe(x => this.resetLayer());
			proxy.api_req_combined_battle_battleresult.Subscribe(x => this.resetLayer());

			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_map/start").TryParse<map_start_next>().Subscribe(x => this.updateLayer(x.Data));
			proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_map/next").TryParse<map_start_next>().Subscribe(x => this.updateLayer(x.Data));
			#endregion
		}

		private void resetLayer()
		{
			this.dispatcher.Invoke(() =>
			{
				var js = "document.getElementById('battleinfo_display_overlay').innerHTML = '';";
				browserView.GetBrowser().MainFrame.ExecuteJavaScriptAsync(js);
			});
		}
		private void updateLayer(map_start_next data)
		{
			this.resetLayer();

			string world = string.Format("World {0}-{1}", data.api_maparea_id, data.api_mapinfo_no);
			if (!this.overlayTableData.ContainsKey(world)) return;

			var html = "";

			var node = this.overlayTableData[world];
			if (node.letters != null)
			{
				foreach (var letter in node.letters)
				{
					html += string.Format(
						"<div class=\"overlay_node\" style=\"left:{1}px; top:{2}px\">{0}</div>",
						letter.Key,
						letter.Value[0] + 32,
						letter.Value[1] + 20
					);
				}
			}
			if (node.markers != null)
			{
				foreach (var marker in node.markers)
				{
					html += string.Format(
						"<img class=\"overlay_marker\" style=\"left:{1}px; top:{2}px; width:{3}px; height:{4}px\" src=\"https://raw.githubusercontent.com/KC3Kai/KC3Kai/master/src/assets/img/{0}\">",
						marker.img,
						marker.pos[0] + 20,
						marker.pos[1] + 20,
						marker.size[0],
						marker.size[1]
					);
				}
			}

			this.dispatcher.Invoke(() =>
			{
				var js = "document.getElementById('battleinfo_display_overlay').innerHTML = '" + html.Replace("'", "\\'") + "';";
				browserView.GetBrowser().MainFrame.ExecuteJavaScriptAsync(js);
			});
		}

		private static bool IsInstanceOrSubclass<T>(Type type)
		{
			return typeof(T) == type || type.IsSubclassOf(typeof(T));
		}
		private static T FindElement<T>(UIElement root) where T : UIElement
		{
			if (root == null) return null;

			T output = null;
			Type rootType = root.GetType();

			if (rootType == typeof(T)) return (T)root;

			if (typeof(IAddChild).IsAssignableFrom(rootType))
			{
				if (IsInstanceOrSubclass<Panel>(rootType))
				{
					foreach (UIElement ui in (root as Panel).Children)
					{
						output = FindElement<T>(ui);
						if (output != null) return output;
					}
				}
				else if (IsInstanceOrSubclass<ItemsControl>(rootType))
				{
					var generator = (root as ItemsControl).ItemContainerGenerator;
					for (int i = 0; i < generator.Items.Count; i++)
					{
						var template = generator.ContainerFromIndex(i) as FrameworkElement;
						output = FindElement<T>(template);
						if (output != null) return output;
					}
				}
				else if (IsInstanceOrSubclass<ContentControl>(rootType))
				{
					var template = (root as ContentControl).Content as FrameworkElement;
					return FindElement<T>(template);
				}
			}

			return null;
		}
	}
}
