using DungeonBattleConsoleGame.Controllers;
using DungeonBattleConsoleGame.Services;
using DungeonBattleConsoleGame.Views;

Console.OutputEncoding = System.Text.Encoding.UTF8;

ConsoleGameView view = new ConsoleGameView();
SaveGameService saveGameService = new SaveGameService();
GameController gameController = new GameController(view, saveGameService);

gameController.Run();