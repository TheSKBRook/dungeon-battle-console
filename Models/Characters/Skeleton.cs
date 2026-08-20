using DungeonBattleConsoleGame.Models.Items;

namespace DungeonBattleConsoleGame.Models.Characters
{
    internal class Skeleton : Enemy
    {
        public Skeleton(string name, int health) : base(name, health, 15)
        {
        }

        public override int GetDamage(Random random)
        {
            return random.Next(5, 16);
        }

        public override Item? GetDrop(Random random)
        {
            if (random.Next(0, 3) == 0)
            {
                return new HealthPotion("Велике зілля здоров'я", 25);
            }
            return null;
        }
        public override Enemy CreateNew()
        {
            return new Skeleton(Name, MaxHealth);
        }
    }
}