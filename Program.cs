using ConsoleApp1;
using ConsoleApp1.Controllers;
using ConsoleApp1.Views;

Console.OutputEncoding = System.Text.Encoding.UTF8;

ConsoleGameView view = new ConsoleGameView();
GameController gameController = new GameController(view);

gameController.Run();