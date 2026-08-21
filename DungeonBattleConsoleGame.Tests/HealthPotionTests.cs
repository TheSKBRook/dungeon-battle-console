using DungeonBattleConsoleGame.Models.Characters;
using DungeonBattleConsoleGame.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonBattleConsoleGame.Tests
{
    [TestClass]
    public class HealthPotionTests
    {
        [TestMethod]
        public void PotionHeal_WhenHeroUsePotion_HealedHealth()
        {
            // Arrange
            Hero hero = new Hero("Тестовий герой");
            HealthPotion potion = new HealthPotion("Тестове зілля", 10);

            // Act
            hero.TakeDamage(30);
            int restoredHealth = potion.Use(hero);

            // Assert
            Assert.AreEqual(10, restoredHealth);
            Assert.AreEqual(80, hero.Health);
        }

        [TestMethod]
        public void PotionHeal_WhenHealingExceedsMaxHealth_RestoresOnlyMissingHealth()
        {
            // Arrange
            Hero hero = new Hero("Тестовий герой");
            HealthPotion potion = new HealthPotion("Тестове зілля", 10);

            // Act
            hero.TakeDamage(5);
            int restoredHealth = potion.Use(hero);

            // Assert
            Assert.AreEqual(5, restoredHealth);
            Assert.AreEqual(hero.MaxHealth, hero.Health);

        }
    }
}
