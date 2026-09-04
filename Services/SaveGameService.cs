using DungeonBattleConsoleGame.Models.Characters;
using DungeonBattleConsoleGame.Models.Game;
using DungeonBattleConsoleGame.Models.Items;
using System.Text.Json;

namespace DungeonBattleConsoleGame.Services
{
    internal class SaveGameService
    {
        private readonly string _filePath;
        private const string SaveFileName = "save.json";
        public SaveGameService(string? filePath = null)
        {
            _filePath = filePath ?? Path.Combine(AppContext.BaseDirectory, SaveFileName);
        }
        private GameSaveData CreateSaveData(GameSession gameSession)
        {
            GameSaveData saveData = new GameSaveData
            {
                HeroName = gameSession.Hero.Name,
                HeroHealth = gameSession.Hero.Health,
                HeroGold = gameSession.Hero.Gold,
                Round = gameSession.Round,
                EnemyType = gameSession.CurrentEnemy.GetType().Name,
                EnemyHealth = gameSession.CurrentEnemy.Health,
                EnemyName = gameSession.CurrentEnemy.Name
            };

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

            foreach (string enemyName in gameSession.GetEncounteredEnemyNames())
            {
                saveData.EncounteredEnemies.Add(enemyName);
            }

            return saveData;
        }
        private ItemSaveData CreateItemSaveData(Item item)
        {
            ItemSaveData saveData = new ItemSaveData
            {
                ItemType = item.GetType().Name,
                ItemName = item.Name,
                Amount = item switch
                {
                    HealthPotion potion => potion.HealAmount,
                    Sword sword => sword.DamageBonus,
                    _ => 0
                }
            };

            return saveData;
        }
        public async Task SaveGameToFileAsync(GameSession gameSession)
        {
            GameSaveData gameSave = CreateSaveData(gameSession);
            string json = JsonSerializer.Serialize(gameSave);

            await File.WriteAllTextAsync(_filePath, json);
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
            return itemSaveData.ItemType switch
            {
                nameof(HealthPotion) => new HealthPotion(itemSaveData.ItemName, itemSaveData.Amount),
                nameof(Sword) => new Sword(itemSaveData.ItemName, itemSaveData.Amount),
                _ => null
            };
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

            foreach (string enemyName in gameSaveData.EncounteredEnemies)
            {
                gameSession.RegisterEncounteredEnemy(enemyName);
            }

            return gameSession;
        }
        public async Task<GameSession?> LoadGameFromFileAsync(IReadOnlyList<Enemy> enemyTemplates)
        {
            if (!File.Exists(_filePath))
            {
                return null;
            }

            try
            {
                string json = await File.ReadAllTextAsync(_filePath);
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