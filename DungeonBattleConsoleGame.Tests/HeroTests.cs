using DungeonBattleConsoleGame.Models.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DungeonBattleConsoleGame.Tests
{
    [TestClass]
    public class HeroTests
    {
        [TestMethod]
        public void Heal_WhenHeroIsDamaged_RestoresHealth()
        {
            // Arrange
            Hero hero = new Hero("Тестовий герой");
            hero.TakeDamage(30);

            // Act
            int restoredHealth = hero.Heal(10);

            // Assert
            Assert.AreEqual(10, restoredHealth);
            Assert.AreEqual(80, hero.Health);
        }

        [TestMethod]
        public void Heal_WhenHealingExceedsMaxHealth_RestoresOnlyMissingHealth()
        {
            // Arrange
            Hero hero = new Hero("Тестовий герой");
            hero.TakeDamage(5);

            // Act
            int restoredHealth = hero.Heal(20);

            // Assert
            Assert.AreEqual(5, restoredHealth);
            Assert.AreEqual(hero.MaxHealth, hero.Health);
        }
    }
}