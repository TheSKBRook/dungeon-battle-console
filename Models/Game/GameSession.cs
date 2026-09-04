using DungeonBattleConsoleGame.Models.Characters;

namespace DungeonBattleConsoleGame.Models.Game
{
    internal class GameSession
    {
        public Hero Hero { get; }
        public Enemy CurrentEnemy { get; private set; }
        public int Round { get; private set; }
        private readonly Dictionary<string, int> _defeatedEnemies = new Dictionary<string, int>();
        private readonly HashSet<string> _encounteredEnemies = new HashSet<string>();
        public event Action<string>? NewEnemyEncountered;
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
        public int GetDefeatedEnemyCount(string enemyName) =>
            _defeatedEnemies.TryGetValue(enemyName, out int count)
            ? count
            : 0;
        public IEnumerable<string> GetDefeatedEnemyNames() => 
            _defeatedEnemies.Keys;
        public void RestoreEnemyDefeatCount(string enemyName, int count)
        {
            _defeatedEnemies[enemyName] = count;
        }
        public bool RegisterEncounteredEnemy(string enemyName)
        {
            if (_encounteredEnemies.Add(enemyName))
            {
                NewEnemyEncountered?.Invoke(enemyName);
                return true;
            }
            return false;
        }
        public IEnumerable<string> GetEncounteredEnemyNames() => 
            _encounteredEnemies;
        public IEnumerable<BestiaryEntry> GetBestiaryEntries() => 
            _encounteredEnemies.Select(enemyName => new BestiaryEntry(enemyName, GetDefeatedEnemyCount(enemyName)));
    }
}