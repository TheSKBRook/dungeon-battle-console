using DungeonBattleConsoleGame.Enums;
using DungeonBattleConsoleGame.Factories;
using DungeonBattleConsoleGame.Models.Characters;
using DungeonBattleConsoleGame.Models.Game;
using DungeonBattleConsoleGame.Models.Items;
using DungeonBattleConsoleGame.Services;
using DungeonBattleConsoleGame.Views;

namespace DungeonBattleConsoleGame.Controllers
{
    internal class BattleController
    {
        private readonly ConsoleGameView _view;
        private readonly Random _random;
        private readonly SaveGameService _saveGameService;
        private readonly EnemyFactory _enemyFactory;

        public BattleController(ConsoleGameView view, SaveGameService saveGameService, EnemyFactory enemyFactory, Random random)
        {
            _view = view;
            _saveGameService = saveGameService;
            _enemyFactory = enemyFactory;
            _random = random;
        }
        public void RunBattle(GameSession gameSession)
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
                        _view.AddBattleLog("Введіть число для дії: ");
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
                            _view.AddBattleLog("\n" + item.Name + " не можна використати.");
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
                                    _view.AddBattleLog("\n" + selectedItem.Name + " вже екіпірований або не можна екіпірувати.");
                                    continue;
                                }
                            }
                            else
                            {
                                _view.AddBattleLog("\n" + selectedItem.Name + " не можна екіпірувати.");
                                continue;
                            }
                        case PlayerAction.SaveAndExit: // зберегти і вийти
                            hasSaved = true;
                            _saveGameService.SaveGameToFile(gameSession);
                            break;
                        default:
                            _view.AddBattleLog("\nНевірна дія. Спробуйте ще раз.");
                            continue;
                    }
                    if (hasEscaped || hasSaved)
                    {
                        break;
                    }

                    if (!enemy.IsAlive())
                    {
                        gameSession.RegisterEnemyDefeat(enemy.Name);
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
                    gameSession.SetCurrentEnemy(_enemyFactory.CreateRandomEnemy());

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

            _view.ShowEnemyDefeatStatistics(gameSession);
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
                _view.AddBattleLog("Інвентар порожній.");
                return null;
            }
            _view.ShowNumberedInventory(hero);
            _view.ShowGameMessage("Введіть номер предмету: ");

            string? inputNumber = _view.ReadInput();
            if (!int.TryParse(inputNumber, out int number))
            {
                _view.ShowGameMessage("Невірний ввід\nНатисніть Enter, щоб продовжити");
                _view.ReadInput();
                return null;
            }
            int itemIndex = number - 1;
            if (itemIndex < 0 || itemIndex >= hero.Inventory.Count)
            {
                _view.AddBattleLog("Немає предмету з таким номером.");
                return null;
            }

            return hero.Inventory[itemIndex];
        }
    }
}