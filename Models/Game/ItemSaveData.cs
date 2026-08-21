namespace DungeonBattleConsoleGame.Models.Game
{
    internal class ItemSaveData
    {
        public string ItemType { get; set; } = "";
        public string ItemName { get; set; } = "";
        public int Amount { get; set; } // кількість лікування для зілля або бонус атаки для меча.
    }
}