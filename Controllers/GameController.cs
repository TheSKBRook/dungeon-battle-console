using ConsoleApp1.Enums;
using ConsoleApp1.Models.Characters;
using ConsoleApp1.Models.Game;
using ConsoleApp1.Models.Items;
using ConsoleApp1.Views;
using System.Data;
using System.Text.Json;

namespace ConsoleApp1.Controllers
{
    internal class GameController
    {
        private readonly ConsoleGameView _view;
        private readonly Random _random = new Random();
        private readonly List<Enemy> _enemyTemplates;
        public GameController(ConsoleGameView view)
        {
            _view = view;
            _enemyTemplates = new List<Enemy>();

            _enemyTemplates.Add(new Goblin("Гоблін", 50));
            _enemyTemplates.Add(new Skeleton("Скелет", 30));
        }
        public void Run()
        {
            bool isRunning = true;

            while (isRunning)
            {
                _view.ShowGame();
                string? input = _view.ReadInput();
                if (!int.TryParse(input, out int inputNumber))
                {
                    _view.ShowGameMessage("\nНевірний ввід.");
                    continue;
                }

                GameMenuAction gameMenuAction = (GameMenuAction)inputNumber;

                switch (gameMenuAction)
                {
                    case GameMenuAction.NewGame: // нова гра
                        StartNewGame();
                        break;
                    case GameMenuAction.LoadGame: // завантажити гру
                        GameSession? loadedSession = LoadGameFromFile();
                        if (loadedSession == null)
                        {
                            _view.ShowGameMessage("Немає збереження");
                            break;
                        }
                        else
                        {
                            _view.ShowGameMessage("Гру завантажено");
                            RunBattle(loadedSession);
                            break;
                        }
                    case GameMenuAction.ExitGame: // вийти з гри
                        isRunning = false;
                        break;
                    default:
                        _view.ShowGameMessage("\nНемає такого пункту меню.");
                        continue;
                }
            }
        }
        private void RunBattle(GameSession gameSession)
        {

            _view.ClearBattleLog();

            int enemyDamage;
            int healAmount;
            int healedAmount;
            int heroBaseDamage;
            int heroDamage;

            string? action;

            bool isCritical;
            bool hasEscaped = false;
            bool hasSaved = false;

            Hero hero = gameSession.Hero;

            while (hero.IsAlive() && !hasEscaped && !hasSaved)
            {
                Enemy enemy = gameSession.CurrentEnemy;
                _view.ShowEnemyAppeared(enemy);

                while (hero.IsAlive() && enemy.IsAlive())
                {
                    heroBaseDamage = hero.GetBaseDamage(_random);
                    heroDamage = hero.GetFullDamage(heroBaseDamage);
                    enemyDamage = enemy.GetDamage(_random);
                    healAmount = GenerateHealAmount();
                    _view.ShowBattleScreen(gameSession.Hero, gameSession.CurrentEnemy, gameSession.Round);

                    /* МЕНЮ */
                    
                    action = _view.ReadInput();
                    if (!int.TryParse(action, out int actionNumber))
                    {
                        _view.ShowGameMessage("Введіть число для дії: ");
                        continue;
                    }

                    PlayerAction playerAction = (PlayerAction)actionNumber;

                    isCritical = heroBaseDamage >= 18;

                    switch (playerAction)
                    {
                        case PlayerAction.Attack: // Атака

                            if (isCritical)
                            {
                                heroDamage = heroDamage * 2;
                            }
                            hero.Attack(enemy, heroDamage);
                            _view.ShowHeroAttack(hero, enemy, heroDamage, isCritical);
                            break;
                        case PlayerAction.Heal: // Лікування
                            healedAmount = hero.Heal(healAmount);
                            _view.ShowHealedAmount(hero, healedAmount);
                            break;
                        case PlayerAction.ShowStatus: // Показати статус
                            _view.ShowHeroStatus(hero);
                            _view.ShowGameMessage("Натисніть Enter щоб продовжити");
                            _view.ReadInput();
                            continue;
                        case PlayerAction.Escape: // Втеча
                            hasEscaped = true;
                            break;
                        case PlayerAction.UseItem: // Використати предмет

                            Item? item = SelectInventoryItem(hero);

                            if (item == null)
                            {
                                continue;
                            }
                            if (item is IUsable)
                            {
                                int restored = hero.UseItem(item);
                                _view.ShowUsedItem(hero, item);
                                _view.ShowHealedAmount(hero, restored);
                                break;
                            }
                            _view.ShowGameMessage("\n" + item.Name + " не можна використати.");
                            continue;
                        case PlayerAction.EquipItem: // Екіпірувати предмет
                            Item? selectedItem = SelectInventoryItem(hero);
                            if (selectedItem == null)
                            {
                                continue;
                            }
                            if (selectedItem is IEquippable)
                            {
                                bool isEquipped = hero.EquipItem(selectedItem);
                                if (isEquipped)
                                {
                                    _view.ShowEquippedItem(hero, selectedItem);
                                    break;
                                }
                                else
                                {
                                    _view.ShowGameMessage("\n" + selectedItem.Name + " вже екіпірований або не можна екіпірувати.");
                                    continue;
                                }
                            }
                            else
                            {
                                _view.ShowGameMessage("\n" + selectedItem.Name + " не можна екіпірувати.");
                                continue;
                            }
                        case PlayerAction.SaveAndExit: // зберегти і вийти
                            hasSaved = true;
                            SaveGameToFile(gameSession);
                            break;
                        default:
                            _view.ShowGameMessage("\nНевірна дія. Спробуйте ще раз.");
                            continue;
                    }
                    if (hasEscaped || hasSaved)
                    {
                        break;
                    }

                    if (!enemy.IsAlive())
                    {
                        _view.ShowEnemyDefeated(enemy);
                        hero.AddGold(enemy.GoldReward);
                        Item? drop = enemy.GetDrop(_random);

                        if (drop != null)
                        {
                            hero.AddItem(drop);
                            _view.ShowEnemyDrop(enemy, drop);
                        }
                        _view.ShowInventory(hero);
                    }
                    else
                    {
                        enemy.Attack(hero, enemyDamage);
                        _view.ShowEnemyAttack(enemy, hero, enemyDamage);
                    }

                    _view.ShowHeroGold(hero);

                    gameSession.NextRound();

                    if (!enemy.IsAlive())
                    {
                        break;
                    }
                }
                if (hero.IsAlive() && !hasEscaped && !hasSaved)
                {
                    gameSession.SetCurrentEnemy(CreateRandomEnemy());

                }
            }

            if (hasSaved)
            {
                _view.ShowGameSaved();
            }
            else if (hasEscaped)
            {
                _view.ShowHeroRun(hero);
            }
            else
            {
                _view.ShowHeroDefeated(hero);
            }

            _view.ShowBattleResultScreen(hero);
            _view.ReadInput();
        }
        private int GenerateHealAmount()
        {
            return _random.Next(5, 12 + 1);
        }
        private Item? SelectInventoryItem(Hero hero)
        {
            if (hero.Inventory.Count == 0)
            {
                _view.ShowGameMessage("Інвентар порожній.");
                return null;
            }
            _view.ShowNumberedInventory(hero);
            _view.ShowGameMessage("Введіть номер предмету: ");

            string? inputNumber = _view.ReadInput();
            if (!int.TryParse(inputNumber, out int number))
            {
                _view.ShowGameMessage("Невірний ввід");
                return null;
            }
            int itemIndex = number - 1;
            if (itemIndex < 0 || itemIndex >= hero.Inventory.Count)
            {
                _view.ShowGameMessage("Немає предмету з таким номером.");
                return null;
            }

            return hero.Inventory[itemIndex];
        }
        private void GiveStartingItems(Hero hero)
        {
            hero.AddItem(new HealthPotion("Зілля здоров'я", 20));
            hero.AddItem(new HealthPotion("Мале зілля здоров'я", 10));
            hero.AddItem(new Sword("Короткий Бронзовий меч", 5));
        }
        private Enemy CreateRandomEnemy()
        {
            int templateIndex = _random.Next(0, _enemyTemplates.Count);
            Enemy template = _enemyTemplates[templateIndex];

            return template.CreateNew();
        }
        private void StartNewGame()
        {
            Hero hero = new Hero(_view.GetHeroName());
            Enemy enemy = CreateRandomEnemy();

            GameSession gameSession = new GameSession(hero, enemy, 1);

            _view.ShowGameStart(hero);

            GiveStartingItems(hero);
            hero.AddItem(new Sword("Довгий Бронзовий меч", 10));

            RunBattle(gameSession);
        }
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
        private void SaveGameToFile(GameSession gameSession)
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
        private Enemy? CreateEnemyFromSaveData(GameSaveData gameSaveData)
        {

            for (int i = 0; i < _enemyTemplates.Count; i++)
            {
                if (gameSaveData.EnemyType == _enemyTemplates[i].GetType().Name)
                {
                    Enemy enemy = _enemyTemplates[i].CreateNew();
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
        private GameSession? CreateGameSessionFromSaveData(GameSaveData gameSaveData)
        {
            Hero hero = CreateHeroFromSaveData(gameSaveData);
            Enemy? enemy = CreateEnemyFromSaveData(gameSaveData);

            if (enemy == null)
            {
                return null;
            }

            return new GameSession(hero, enemy, gameSaveData.Round);
        }
        private GameSession? LoadGameFromFile()
        {
            string filePath = Path.Combine(AppContext.BaseDirectory, "save.json");
            if (!File.Exists(filePath))
            {
                return null;
            }

            string json = File.ReadAllText(filePath);

            GameSaveData? gameSaveData = JsonSerializer.Deserialize<GameSaveData>(json);

            if (gameSaveData == null)
            {
                return null;
            }

            return CreateGameSessionFromSaveData(gameSaveData);
        }
    }
}