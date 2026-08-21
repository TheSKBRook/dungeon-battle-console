namespace DungeonBattleConsoleGame.Models.Game
{
    internal class GameSaveData
    {
        public string HeroName { get; set; } = "";
        public int HeroHealth { get; set; }
        public int HeroGold { get; set; }
        public int Round { get; set; }
        public string EnemyType { get; set; } = "";
        public string EnemyName { get; set; } = "";
        public int EnemyHealth { get; set; }
        public List<ItemSaveData> Inventory { get; set; } = new List<ItemSaveData>();
        public int EquippedItemIndex { get; set; } = -1;
        public Dictionary<string,int> DefeatedEnemies { get; set; } = new Dictionary<string,int>();
    }
}
