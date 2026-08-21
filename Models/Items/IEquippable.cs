using DungeonBattleConsoleGame.Models.Characters;

namespace DungeonBattleConsoleGame.Models.Items
{
    internal interface IEquippable
    {
        void Equip(Hero hero);
    }
}
