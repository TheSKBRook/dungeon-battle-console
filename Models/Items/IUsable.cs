using DungeonBattleConsoleGame.Models.Characters;

namespace DungeonBattleConsoleGame.Models.Items
{
    internal interface IUsable
    {
        string Name { get; }
        int Use(Hero hero);
    }
}
