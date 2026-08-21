using DungeonBattleConsoleGame.Models.Characters;
using DungeonBattleConsoleGame.Models.Items;

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

        [TestMethod]
        public void TakeDamage_WhenDamageIsNegative_ThrowsException()
        {
            // Arrange
            Hero hero = new Hero("Тестовий герой");

            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => hero.TakeDamage(-1));
        }

        [TestMethod]
        public void Heal_WhenAmountIsNegative_ThrowsException()
        {
            // Arange
            Hero hero = new Hero("Тестовий герой");

            // Asset
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => hero.Heal(-1));
        }

        [TestMethod]
        public void EquipItem_WhenItemsIsNotEquippable_ReturnFalse()
        {
            // Arrange
            Hero hero = new Hero("Тестовий герой");
            HealthPotion potion = new HealthPotion("Тестове зілля", 5);

            // Act
            hero.AddItem(potion);
            bool isEquiped = hero.EquipItem(potion);

            //Assert
            Assert.IsFalse(isEquiped);
            Assert.IsNull(hero.EquippedItem);

        }

        [DataTestMethod]
        [DataRow(1, 99)]
        [DataRow(20, 80)]
        [DataRow(150, 0)]
        public void TakeDamage_WhenDamageIsApplied_ReducesHealthWithoutGoingBelowZero(int damage, int expectedHealth)
        {
            // Arrange
            Hero hero = new Hero("Тестовий герой");

            // Act
            hero.TakeDamage(damage);

            // Assert
            Assert.AreEqual(expectedHealth, hero.Health);
        }

        [DataTestMethod]
        [DataRow(5, 10)]
        [DataRow(10, 15)]
        [DataRow(20, 25)]
        public void GetFullDamage_WhenWeaponIsEquipped_AddsAttackBonus(int baseDamage, int expectedDamage)
        {
            // Arrange
            Hero hero = new Hero("Тестовий герой");
            Sword sword = new Sword("Тестовий меч", 5);
            hero.AddItem(sword);
            hero.EquipItem(sword);

            // Act
            int fullDamage = hero.GetFullDamage(baseDamage);

            // Assert
            Assert.AreEqual(expectedDamage, fullDamage);

        }

        [TestMethod]
        public void EquipItem_WhenNewSwordIsEquipped_ReplacesPreviousAttackBonus()
        {
            // Arrange
            Hero hero = new Hero("Тестовий герой");
            Sword sword1 = new Sword("Тестовий меч 1", 5);
            Sword sword2 = new Sword("Тестовий меч 2", 10);

            hero.AddItem(sword1);
            hero.AddItem(sword2);

            // Act
            hero.EquipItem(sword1);
            hero.EquipItem(sword2);

            // Assert
            Assert.AreSame(sword2, hero.EquippedItem);
            Assert.AreEqual(sword2.DamageBonus, hero.AttackBonus);
        }
    }
}