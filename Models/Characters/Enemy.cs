using DungeonBattleConsoleGame.Models.Items;

namespace DungeonBattleConsoleGame.Models.Characters
{
    abstract class Enemy : Character
    {
        public int GoldReward { get; }
        protected Enemy(string name, int health, int goldReward) : base(name, health)
        {
            GoldReward = goldReward;
        }
        public abstract int GetDamage(Random random);

        public virtual Item? GetDrop(Random random)
        {
            return null;
        }
        public abstract Enemy CreateNew();
    }
}