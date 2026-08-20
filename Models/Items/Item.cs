namespace DungeonBattleConsoleGame.Models.Items
{
    abstract class Item
    {
        public string Name { get; }
        protected Item(string name)
        {
            Name = name;
        }
    }
}
