using ConsoleApp1.Models.Items;

namespace ConsoleApp1.Models.Characters
{
    internal class Goblin : Enemy
    {
        public Goblin(string name, int health) : base(name, health, 10)
        {
        }

        public override int GetDamage(Random random)
        {
            return random.Next(3, 13);
        }

        public override Item? GetDrop(Random random)
        {
            if (random.Next(0, 2) == 0)
            {
                return new HealthPotion("Мале зілля здоров'я", 10);
            }
            return null;
        }
        public override Enemy CreateNew()
        {
            return new Goblin(Name, MaxHealth);
        }
    }
}