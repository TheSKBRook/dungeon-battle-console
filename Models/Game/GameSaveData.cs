namespace DungeonBattleConsoleGame.Models.Game
{
    internal class GameSaveData
    {
        public string HeroName { get; init; } = "";
        public int HeroHealth { get; init; }
        public int HeroGold { get; init; }
        public int Round { get; init; }
        public string EnemyType { get; init; } = "";
        public string EnemyName { get; init; } = "";
        public int EnemyHealth { get; init; }
        public List<ItemSaveData> Inventory { get; init; } = new List<ItemSaveData>();
        public int EquippedItemIndex { get; set; } = -1;
        public Dictionary<string,int> DefeatedEnemies { get; init; } = new Dictionary<string, int>();
        public HashSet<string> EncounteredEnemies { get; init; } = new HashSet<string>();
    }
}
