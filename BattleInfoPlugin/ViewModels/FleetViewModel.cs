using System;
using System.Collections.Generic;
using System.Linq;
using BattleInfoPlugin.Models;
using Livet;
using Livet.EventListeners;

namespace BattleInfoPlugin.ViewModels
{
	public class FleetViewModel : ViewModel
	{
		private string defaultName { get; }

		#region Name変更通知プロパティ
		private string _Name;
		public string Name
		{
			get { return this._Name; }
			set
			{
				if (this._Name != value)
				{
					this._Name = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region Fleet変更通知プロパティ
		private FleetData _Fleet;
		public FleetData Fleet
		{
			get { return this._Fleet; }
			set
			{
				if (this._Fleet != value)
				{
					this._Fleet = value;
					this.RaisePropertyChanged();

					this.RaisePropertyChanged(nameof(this.IsVisible));
					//this.RaisePropertyChanged(nameof(this.FleetFormation));
					//this.RaisePropertyChanged(nameof(this.FleetGauge));

					this.Name = !string.IsNullOrWhiteSpace(value.Name)
						? value.Name
						: this.defaultName;
				}
			}
		}
		#endregion

		/*
		public string FleetGauge =>
			(this.Fleet?.AttackGauge ?? string.Empty) != string.Empty
				? this.Fleet.AttackGauge
				: string.Empty;

		public string FleetFormation =>
			(this.Fleet?.Formation ?? Formation.없음) != Formation.없음
				? this.Fleet.Formation.ToString()
				: "";
		*/

		#region AirCombatResults変更通知プロパティ
		private AirCombatResultViewModel[] _AirCombatResults;
		public AirCombatResultViewModel[] AirCombatResults
		{
			get { return this._AirCombatResults; }
			set
			{
				if (this._AirCombatResults != value)
				{
					this._AirCombatResults = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		public bool IsVisible => (this.Fleet?.Ships?.Count() ?? 0) > 0;


		public FleetViewModel() : this("")
		{
		}

		public FleetViewModel(string name)
		{
			this.defaultName = name;
			this._Name = name;
		}
	}
}
