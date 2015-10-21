﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using BattleInfoPlugin.Models;
using System.Windows;

namespace BattleInfoPlugin.ViewModels.Enemies
{
    public class EnemyFleetViewModel : ViewModel
    {
        public string Key { get; set; }

        public string Name
            => !string.IsNullOrWhiteSpace(this.Fleet.Name)
                ? this.Fleet.Name
                : "？？？";

        public FleetData Fleet { get; set; }

        #region EnemyShips

        private EnemyShipViewModel[] _EnemyShips;

        public EnemyShipViewModel[] EnemyShips
        {
            get { return this._EnemyShips; }
            set
            {
                this._EnemyShips = value;
                if (value == null) return;
                foreach (var val in value)
                {
                    val.ParentFleet = this;
                }
            }
        }

        #endregion

        public EnemyCellViewModel ParentCell { get; set; }

        public void DeleteEnemy()
        {
            System.Diagnostics.Debug.WriteLine($"DeleteEnemy:{this.Key}");
            if (MessageBoxResult.OK != MessageBox.Show(
                $"{this.Name}(key:{this.Key})의 데이터를 소거합니다",
                "확인",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question))
                return;

            this.ParentCell.ParentMap.WindowViewModel.RemoveEnemy(this.Key);
        }

        public void CopyIdToClipboard()
        {
            Clipboard.SetText(this.Key);
        }
    }
}
