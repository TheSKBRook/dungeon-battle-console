using DungeonBattleConsoleGame.Models.Characters;
using DungeonBattleConsoleGame.Models.Game;

namespace DungeonBattleConsoleGame.Tests
{
    [TestClass]
    public class GameSessionTests
    {
        [TestMethod]
        public void RegisterEnemyDefeat_WhenSameEnemyDefeatedTwice_IncreasesCount()
        {
            // Arrange
            Hero hero = new Hero("Тестовий герой");
            Goblin goblin = new Goblin("Гоблін", 10);
            GameSession gameSession = new GameSession(hero, goblin, 1);

            // Act
            gameSession.RegisterEnemyDefeat("Гоблін");
            gameSession.RegisterEnemyDefeat("Гоблін");
            int enemyCount = gameSession.GetDefeatedEnemyCount("Гоблін");

            // Assert
            Assert.AreEqual(2, enemyCount);
        }
        [TestMethod]
        public void GetEnemyDefeatCount_WhenEnemyHasNotBeenDefeated_ReturnsZero()
        {
            // Arrange
            Hero hero = new Hero("Тестовий герой");
            Goblin goblin = new Goblin("Гоблін", 10);
            GameSession gameSession = new GameSession(hero, goblin, 1);

            // Act
            int enemyCount = gameSession.GetDefeatedEnemyCount("Скелет");

            // Assert
            Assert.AreEqual(0, enemyCount);
        }
        [TestMethod]
        public void RestoreEnemyDefeatCount_WhenSavedCountProvided_RestoresCount()
        {
            // Arrange
            Hero hero = new Hero("Тестовий герой");
            Goblin goblin = new Goblin("Гоблін", 10);
            GameSession gameSession = new GameSession(hero, goblin, 1);

            // Act
            gameSession.RestoreEnemyDefeatCount("Гоблін", 3);
            int enemyCount = gameSession.GetDefeatedEnemyCount("Гоблін");

            // Assert
            Assert.AreEqual(3, enemyCount);
        }
        [TestMethod]
        public void RegisterEncounteredEnemy_WhenSameEnemyRegisterTwice_ReturnTrueThenFalse()
        {
            // Arrange
            Hero hero = new Hero("Test Hero");
            Goblin goblin = new Goblin("Гоблін", 10);
            GameSession gameSession = new GameSession(hero, goblin, 1);

            // Act
            bool test1 = gameSession.RegisterEncounteredEnemy("Гоблін");
            bool test2 = gameSession.RegisterEncounteredEnemy("Гоблін");

            // Assert
            Assert.IsTrue(test1);
            Assert.IsFalse(test2);
        }
    }
}
