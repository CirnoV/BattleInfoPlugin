using System;
using System.Windows;
using MetroRadiance.UI.Controls;
using BattleInfoPlugin.ViewModels;

namespace BattleInfoPlugin.Views
{
	/// <summary>
	/// HistoryWindow.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class HistoryWindow : MetroWindow
	{
		public HistoryWindow()
		{
			InitializeComponent();
			WeakEventManager<Window, EventArgs>.AddHandler(
				Application.Current.MainWindow,
				"Closed",
				(_, __) => this.Close()
			);
		}
	}
}
