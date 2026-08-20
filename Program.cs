using DungeonBattleConsoleGame;
using DungeonBattleConsoleGame.Controllers;
using DungeonBattleConsoleGame.Views;

Console.OutputEncoding = System.Text.Encoding.UTF8;

ConsoleGameView view = new ConsoleGameView();
GameController gameController = new GameController(view);

gameController.Run();