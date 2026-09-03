using DungeonBattleConsoleGame.Models.Characters;
using DungeonBattleConsoleGame.Models.Game;
using DungeonBattleConsoleGame.Services;

namespace DungeonBattleConsoleGame.Tests
{
    [TestClass]
    public class SaveGameServiceTests
    {
        [TestMethod]
        public async Task SaveGameToFileAsync_WhenPathProvided_CreatesFile()
        {
            //Arrange
            string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
            Hero hero = new Hero("TestHero");
            Goblin goblin = new Goblin("Goblin", 10);
            GameSession gameSession = new GameSession(hero, goblin, 1);
            SaveGameService saveGameService = new SaveGameService(filePath);
            try
            {
                //Act
                await saveGameService.SaveGameToFileAsync(gameSession);

                //Assert
                Assert.IsTrue(File.Exists(filePath));
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }
        [TestMethod]
        public async Task SaveAndLoadGame_WhenDataIsValid_RestoresGameSession()
        {
            //Arrange
            string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
            Hero hero = new Hero("TestHero");
            hero.TakeDamage(27);

            Goblin goblin = new Goblin("Goblin", 10);
            goblin.TakeDamage(3);

            GameSession gameSession = new GameSession(hero, goblin, 6);
            SaveGameService saveGameService = new SaveGameService(filePath);
            List<Enemy> enemyTemplates = new List<Enemy>();
            enemyTemplates.Add(new Goblin("Goblin", 10));

            try
            {

                //Act
                await saveGameService.SaveGameToFileAsync(gameSession);
                GameSession? loadedSession = await saveGameService.LoadGameFromFileAsync(enemyTemplates);

                //Assert
                Assert.IsNotNull(loadedSession);
                Assert.AreEqual(hero.Name, loadedSession.Hero.Name);
                Assert.AreEqual(hero.Health, loadedSession.Hero.Health);
                Assert.AreEqual(gameSession.Round, loadedSession.Round);
                Assert.AreEqual(gameSession.CurrentEnemy.Name, loadedSession.CurrentEnemy.Name);
                Assert.AreEqual(gameSession.CurrentEnemy.Health, loadedSession.CurrentEnemy.Health);
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }
    }
}
