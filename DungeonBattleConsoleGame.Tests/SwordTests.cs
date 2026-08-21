using DungeonBattleConsoleGame.Models.Characters;
using DungeonBattleConsoleGame.Models.Items;

namespace DungeonBattleConsoleGame.Tests
{
    [TestClass]
    public class SwordTests
    {
        [TestMethod]
        public void AttackBonus_WhenHeroEquipSword_AddingBonus()
        {
            // Arrange
            Sword sword = new Sword("Тестовий меч", 5);
            Hero hero = new Hero("Тестовий герой");

            // Act
            hero.AddItem(sword);
            bool isEquiped = hero.EquipItem(sword);

            // Assert
            Assert.IsTrue(isEquiped);
            Assert.AreSame(sword, hero.EquippedItem);
            Assert.AreEqual(5, hero.AttackBonus);
        }
    }
}
