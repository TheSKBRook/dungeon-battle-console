using DungeonBattleConsoleGame.Enums;
using DungeonBattleConsoleGame.Factories;
using DungeonBattleConsoleGame.Models.Characters;
using DungeonBattleConsoleGame.Models.Game;
using DungeonBattleConsoleGame.Models.Items;
using DungeonBattleConsoleGame.Services;
using DungeonBattleConsoleGame.Views;

namespace DungeonBattleConsoleGame.Controllers
{
    internal class GameController
    {
        private readonly ConsoleGameView _view;
        private readonly SaveGameService _saveGameService;
        private readonly EnemyFactory _enemyFactory;
        private readonly BattleController _battleController;
        public GameController(ConsoleGameView view, SaveGameService saveGameService, EnemyFactory enemyFactory, BattleController battleController)
        {
            _view = view;
            _saveGameService = saveGameService;
            _enemyFactory = enemyFactory;
            _battleController = battleController;
        }
        public async Task RunAsync()
        {
            bool isRunning = true;

            while (isRunning)
            {
                _view.ShowGame();
                string? input = _view.ReadInput();
                if (!int.TryParse(input, out int inputNumber))
                {
                    _view.ShowGameMessage("\nНевірний ввід.\nНатисніть Enter, щоб продовжити");
                    _view.ReadInput();
                    continue;
                }

                GameMenuAction gameMenuAction = (GameMenuAction)inputNumber;

                switch (gameMenuAction)
                {
                    case GameMenuAction.NewGame: // нова гра
                        await StartNewGameAsync();
                        break;
                    case GameMenuAction.LoadGame: // завантажити гру
                        GameSession? loadedSession = await _saveGameService.LoadGameFromFileAsync(_enemyFactory.EnemyTemplates);
                        if (loadedSession == null)
                        {
                            _view.ShowGameMessage("Немає збереження.\nНатисніть Enter, щоб продовжити");
                            _view.ReadInput();
                            break;
                        }
                        else
                        {
                            _view.ShowGameMessage("Гру завантажено");
                            await _battleController.RunBattleAsync(loadedSession);
                            break;
                        }
                    case GameMenuAction.ExitGame: // вийти з гри
                        isRunning = false;
                        break;
                    default:
                        _view.ShowGameMessage("\nНемає такого пункту меню.\nНатисніть Enter, щоб продовжити");
                        _view.ReadInput();
                        continue;
                }
            }
        }
        private void GiveStartingItems(Hero hero)
        {
            hero.AddItem(new HealthPotion("Зілля здоров'я", 20));
            hero.AddItem(new HealthPotion("Мале зілля здоров'я", 10));
            hero.AddItem(new Sword("Короткий Бронзовий меч", 5));
        }
        private async Task StartNewGameAsync()
        {
            Hero hero = new Hero(_view.GetHeroName());
            Enemy enemy = _enemyFactory.CreateRandomEnemy();

            GameSession gameSession = new GameSession(hero, enemy, 1);

            _view.ShowGameStart(hero);

            GiveStartingItems(hero);
            hero.AddItem(new Sword("Довгий Бронзовий меч", 10));

            await _battleController.RunBattleAsync(gameSession);
        }
    }
}