using ConsoleApp1.Models.Characters;

namespace ConsoleApp1.Models.Game
{
    internal class GameSession
    {
        public Hero Hero { get; }
        public Enemy CurrentEnemy { get; private set; }
        public int Round { get; private set; }

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
    }
}