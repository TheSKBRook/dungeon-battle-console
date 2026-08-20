using ConsoleApp1.Models.Characters;

namespace ConsoleApp1.Models.Items
{
    internal interface IUsable
    {
        string Name { get; }
        int Use(Hero hero);
    }
}
