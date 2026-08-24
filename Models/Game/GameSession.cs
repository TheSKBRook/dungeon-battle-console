using DungeonBattleConsoleGame.Models.Characters;

namespace DungeonBattleConsoleGame.Models.Game
{
    internal class GameSession
    {
        public Hero Hero { get; }
        public Enemy CurrentEnemy { get; private set; }
        public int Round { get; private set; }
        private Dictionary<string, int> _defeatedEnemies = new Dictionary<string, int>();
        private readonly HashSet<string> _encounteredEnemies = new HashSet<string>();

        public GameSession(Hero hero, Enemy enemy, int round)
        {
            Hero = hero;
            CurrentEnemy = enemy;
            Round = round;
        }
        public void NextRound()
        {
            Round++;
        }
        public void SetCurrentEnemy(Enemy enemy)
        {
            CurrentEnemy = enemy;
        }
        public void RegisterEnemyDefeat(string enemyName)
        {
            if (!_defeatedEnemies.ContainsKey(enemyName))
            {
                _defeatedEnemies.Add(enemyName, 0);
            }
            _defeatedEnemies[enemyName]++;
        }
        public int GetDefeatedEnemyCount(string enemyName)
        {
            if (_defeatedEnemies.ContainsKey(enemyName))
            {
                return _defeatedEnemies[enemyName];
            }
            return 0;
        }
        public IEnumerable<string> GetDefeatedEnemyNames()
        {
                return _defeatedEnemies.Keys;
        }
        public void RestoreEnemyDefeatCount(string enemyName, int count)
        {
            _defeatedEnemies[enemyName] = count;
        }
        public bool RegisterEncounteredEnemy(string enemyName)
        {
            if (_encounteredEnemies.Add(enemyName))
            {
                return true;
            }
            return false;
        }
    } 
}