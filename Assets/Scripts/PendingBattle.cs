using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    [Serializable]
    public class PendingBattle
    {
        public BattleTemplate Template { get; set; }

        public string UpgradeText { get; set; }
    }
}
