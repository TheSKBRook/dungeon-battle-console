using ConsoleApp1.Models.Characters;

namespace ConsoleApp1.Models.Items
{
    internal class HealthPotion : Item, IUsable
    {
        public int HealAmount { get; }
        public HealthPotion(string name, int healAmount) : base(name)
        {
            HealAmount = healAmount;
        }
        public int Use(Hero hero)
        {
            return hero.Heal(HealAmount);
        }
    }
}