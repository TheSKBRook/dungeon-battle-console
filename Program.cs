using DungeonBattleConsoleGame.Controllers;
using DungeonBattleConsoleGame.Factories;
using DungeonBattleConsoleGame.Services;
using DungeonBattleConsoleGame.Views;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Random random = new Random();
ConsoleGameView view = new ConsoleGameView();
SaveGameService saveGameService = new SaveGameService();
EnemyFactory enemyFactory = new EnemyFactory(random);
BattleController battleController = new BattleController(view, saveGameService, enemyFactory, random);
GameController gameController = new GameController(view, saveGameService, enemyFactory, battleController);

await gameController.RunAsync();