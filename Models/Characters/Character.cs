using System.Linq.Expressions;

namespace DungeonBattleConsoleGame.Models.Characters
{
    internal class Character
    {
        public string Name { get; }
        public int Health { get; protected set; }
        public int MaxHealth { get; protected set; }
        public Character(string name, int health)
        {
            Name = name;
            MaxHealth = health;
            Health = MaxHealth;
        }
        public void TakeDamage(int damage)
        {
            if (damage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(damage));
            }

            Health -= damage;
            if (Health < 0)
            {
                Health = 0;
            }
        }
        public void Attack(Character target, int damage)
        {
            target.TakeDamage(damage);
        }
        public bool IsAlive()
        {
            return Health > 0;
        }
    }
}