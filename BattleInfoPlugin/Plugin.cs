using System;
using System.ComponentModel.Composition;
using BattleInfoPlugin.Models;
using BattleInfoPlugin.ViewModels;
using BattleInfoPlugin.Views;
using Grabacr07.KanColleViewer.Composition;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models.Raw;

namespace BattleInfoPlugin
{
	[Export(typeof(IPlugin))]
	[Export(typeof(ITool))]
	[Export(typeof(IRequestNotify))]
	[ExportMetadata("Guid", "3CFE46C3-E3AF-4737-BFB7-CAD1865C10CA")]
	[ExportMetadata("Title", "BattleInfo")]
	[ExportMetadata("Description", "전투정보를 표시합니다. 대파알림 및 랭크예측등을 제공합니다")]
	[ExportMetadata("Version", "1.7.1.1")]
	[ExportMetadata("Author", "@veigr")]
	public class Plugin : IPlugin, ITool, IRequestNotify
	{
		internal BrowserExtension browserEx { get; }
		private ToolViewModel vm { get; }

		public Plugin()
		{
			// 기존 설정 불러오기..?
			if (BattleInfoPlugin.Properties.Settings.Default.UpdateSettings)
			{
				BattleInfoPlugin.Properties.Settings.Default.Upgrade();
				BattleInfoPlugin.Properties.Settings.Default.UpdateSettings = false;
				BattleInfoPlugin.Properties.Settings.Default.Save();
			}

			this.vm = new ToolViewModel(this);
			browserEx = new Models.BrowserExtension();
		}

		public void Initialize()
		{
			// For Display Overlay Patch
			if (BattleInfoPlugin.Properties.Settings.Default.UseBrowserOverlay)
				KanColleClient.Current.Proxy.api_start2_getData.Subscribe(x => this.browserEx.Startup());

			/* For Enemy Info Data
			KanColleClient.Current.Proxy.api_start2.TryParse<kcsapi_start2>().Subscribe(x =>
			{
				RawStart2 = x.Data;
				Models.Repositories.Master.Current.Update(x.Data);
			});
			ResourceWriter = new KcsResourceWriter();
			SortieListener = new SortieDataListener();
			*/
		}

		public string Name => "BattleInfo";

		// タブ表示するたびに new されてしまうが、今のところ new しないとマルチウィンドウで正常に表示されない
		public object View => new ToolView {DataContext = this.vm};

		public event EventHandler<NotifyEventArgs> NotifyRequested;

		public void InvokeNotifyRequested(NotifyEventArgs e) => this.NotifyRequested?.Invoke(this, e);
	}
}
