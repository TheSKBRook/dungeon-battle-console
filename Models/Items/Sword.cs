using ConsoleApp1.Models.Characters;

namespace ConsoleApp1.Models.Items
{
    internal class Sword : Item, IEquippable
    {
        public int DamageBonus { get; }
        public Sword(string name, int damage) : base(name)
        {
            DamageBonus = damage;
        }

        public void Equip(Hero hero)
        {
            hero.EquipWeapon(DamageBonus);
        }
    }
}
