using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonBattleConsoleGame.Models.Game
{
    internal class ItemSaveData
    {
        public string ItemType { get; set; } = "";
        public string ItemName { get; set; } = "";
        public int Amount { get; set; } // кількість лікування для зілля або бонус атаки для меча.
    }
}
