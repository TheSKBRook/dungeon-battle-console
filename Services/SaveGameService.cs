using DungeonBattleConsoleGame.Models.Characters;
using DungeonBattleConsoleGame.Models.Game;
using DungeonBattleConsoleGame.Models.Items;
using System.Text.Json;

namespace DungeonBattleConsoleGame.Services
{
    internal class SaveGameService
    {
        private GameSaveData CreateSaveData(GameSession gameSession)
        {
            GameSaveData saveData = new GameSaveData();
            saveData.HeroName = gameSession.Hero.Name;
            saveData.HeroHealth = gameSession.Hero.Health;
            saveData.HeroGold = gameSession.Hero.Gold;
            saveData.Round = gameSession.Round;
            saveData.EnemyType = gameSession.CurrentEnemy.GetType().Name;
            saveData.EnemyHealth = gameSession.CurrentEnemy.Health;
            saveData.EnemyName = gameSession.CurrentEnemy.Name;

            for (int i = 0; gameSession.Hero.Inventory.Count > i; i++)
            {
                saveData.Inventory.Add(CreateItemSaveData(gameSession.Hero.Inventory[i]));
                if (gameSession.Hero.Inventory[i] == gameSession.Hero.EquippedItem)
                {
                    saveData.EquippedItemIndex = i;
                }
            }

            foreach (string enemyName in gameSession.GetDefeatedEnemyNames())
            {
                saveData.DefeatedEnemies[enemyName] = gameSession.GetDefeatedEnemyCount(enemyName);
            }

            return saveData;
        }
        private ItemSaveData CreateItemSaveData(Item item)
        {
            ItemSaveData saveData = new ItemSaveData();
            saveData.ItemType = item.GetType().Name;
            saveData.ItemName = item.Name;
            if (item is HealthPotion)
            {
                HealthPotion potion = (HealthPotion)item;
                saveData.Amount = potion.HealAmount;
            }
            else if (item is Sword)
            {
                Sword sword = (Sword)item;
                saveData.Amount = sword.DamageBonus;
            }

            return saveData;
        }
        public void SaveGameToFile(GameSession gameSession)
        {
            GameSaveData gameSave = CreateSaveData(gameSession);
            string json = JsonSerializer.Serialize(gameSave);

            string filePath = Path.Combine(AppContext.BaseDirectory, "save.json");
            File.WriteAllText(filePath, json);
        }
        private Hero CreateHeroFromSaveData(GameSaveData gameSaveData)
        {
            Hero hero = new Hero(gameSaveData.HeroName, gameSaveData.HeroHealth, gameSaveData.HeroGold);

            for (int i = 0; i < gameSaveData.Inventory.Count; i++)
            {
                Item? item = CreateItemFromSaveData(gameSaveData.Inventory[i]);
                if (item != null)
                {
                    hero.AddItem(item);
                }

            }
            if (gameSaveData.EquippedItemIndex >= 0 && gameSaveData.EquippedItemIndex < hero.Inventory.Count)
            {
                hero.EquipItem(hero.Inventory[gameSaveData.EquippedItemIndex]);
            }

            return hero;
        }
        private Enemy? CreateEnemyFromSaveData(GameSaveData gameSaveData, IReadOnlyList<Enemy> enemyTemplates)
        {

            for (int i = 0; i < enemyTemplates.Count; i++)
            {
                if (gameSaveData.EnemyType == enemyTemplates[i].GetType().Name)
                {
                    Enemy enemy = enemyTemplates[i].CreateNew();
                    enemy.TakeDamage(enemy.MaxHealth - gameSaveData.EnemyHealth);
                    return enemy;
                }
            }
            return null;
        }
        private Item? CreateItemFromSaveData(ItemSaveData itemSaveData)
        {
            if (itemSaveData.ItemType == nameof(HealthPotion))
            {
                return new HealthPotion(itemSaveData.ItemName, itemSaveData.Amount);
            }

            if (itemSaveData.ItemType == nameof(Sword))
            {
                return new Sword(itemSaveData.ItemName, itemSaveData.Amount);
            }

            return null;
        }
        private GameSession? CreateGameSessionFromSaveData(GameSaveData gameSaveData, IReadOnlyList<Enemy> enemyTemplates)
        {
            Hero hero = CreateHeroFromSaveData(gameSaveData);
            Enemy? enemy = CreateEnemyFromSaveData(gameSaveData, enemyTemplates);

            if (enemy == null)
            {
                return null;
            }

            GameSession gameSession = new GameSession(hero, enemy, gameSaveData.Round);

            foreach (string enemyName in gameSaveData.DefeatedEnemies.Keys)
            {
                int count = gameSaveData.DefeatedEnemies[enemyName];

                gameSession.RestoreEnemyDefeatCount(enemyName, count);
            }

            return gameSession;
        }
        public GameSession? LoadGameFromFile(IReadOnlyList<Enemy> enemyTemplates)
        {
            string filePath = Path.Combine(AppContext.BaseDirectory, "save.json");
            if (!File.Exists(filePath))
            {
                return null;
            }

            try
            {
                string json = File.ReadAllText(filePath);
                GameSaveData? gameSaveData = JsonSerializer.Deserialize<GameSaveData>(json);

                if (gameSaveData == null)
                {
                    return null;
                }
                return CreateGameSessionFromSaveData(gameSaveData, enemyTemplates);

            }
            catch (IOException)
            {
                return null;
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }
}