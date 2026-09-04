namespace DungeonBattleConsoleGame.Models.Game
{
    internal class ItemSaveData
    {
        public string ItemType { get; init; } = "";
        public string ItemName { get; init; } = "";
        public int Amount { get; init; } // кількість лікування для зілля або бонус атаки для меча.
    }
}