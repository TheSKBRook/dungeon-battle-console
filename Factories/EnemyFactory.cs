using DungeonBattleConsoleGame.Models.Characters;

namespace DungeonBattleConsoleGame.Factories
{
    internal class EnemyFactory
    {
        private readonly Random _random;
        private readonly List<Enemy> _enemyTemplates;
        public IReadOnlyList<Enemy> EnemyTemplates
        {
            get { return _enemyTemplates; }
        }

        public EnemyFactory(Random random)
        {
            _random = random;
            _enemyTemplates = new List<Enemy>();
            _enemyTemplates.Add(new Goblin("Гоблін", 50));
            _enemyTemplates.Add(new Skeleton("Скелет", 30));
        }
        public Enemy CreateRandomEnemy()
        {
            int templateIndex = _random.Next(0, _enemyTemplates.Count);
            Enemy template = _enemyTemplates[templateIndex];

            return template.CreateNew();
        }

    }
}
