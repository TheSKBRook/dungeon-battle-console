using DungeonBattleConsoleGame.Controllers;
using DungeonBattleConsoleGame.Factories;
using DungeonBattleConsoleGame.Services;
using DungeonBattleConsoleGame.Views;

Console.OutputEncoding = System.Text.Encoding.UTF8;

ConsoleGameView view = new ConsoleGameView();
SaveGameService saveGameService = new SaveGameService();
EnemyFactory enemyFactory = new EnemyFactory(new Random());
GameController gameController = new GameController(view, saveGameService, enemyFactory);

gameController.Run();