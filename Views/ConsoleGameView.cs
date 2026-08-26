using DungeonBattleConsoleGame.Models.Characters;
using DungeonBattleConsoleGame.Models.Game;
using DungeonBattleConsoleGame.Models.Items;

namespace DungeonBattleConsoleGame.Views
{

    internal class ConsoleGameView
    {
        private readonly Queue<string> _battleLog = new Queue<string>();
        const int MaxBattleLogEntries = 5;
        private void ShowCenteredText(string text)
        {
            int width = (Console.WindowWidth - text.Length) / 2;

            if (width < 0)
            {
                width = 0;
            }
            Console.SetCursorPosition(width, Console.CursorTop);

            Console.WriteLine(text);
        }
        private void ShowBattleLog()
        {
            foreach (string message in _battleLog)
            {
                ShowGameMessage(message);
            }
        }
        public void ShowGameTitle()
        {
            ShowCenteredText("=== Битва в підземеллі ===");
        }
        public void ShowGameStart(Hero hero)
        {
            ShowGameMessage("\n" + hero.Name + " герой що відправився в підземелля!");
        }
        public void ShowHeroStatus(Hero hero)
        {
            ShowGameMessage("\nСтатус героя:");
            ShowGameMessage("\nІм'я: " + hero.Name);
            ShowGameMessage("Здоров'я: " + hero.Health + " / " + hero.MaxHealth);
            ShowGameMessage("Бонус до атаки: " + hero.AttackBonus);
            ShowGameMessage("Золото: " + hero.Gold);
            ShowGameMessage("Інвентар: ");
            foreach (Item item in hero.GetInventorySortedByName())
            {
                ShowGameMessage("- " + item.Name);
            }
            List<Item> usableItems = hero.GetUsableItems().ToList();
            if (usableItems.Any())
            {
                ShowGameMessage("Предмети, що можна використати");
                foreach (Item item in usableItems)
                {
                    ShowGameMessage("- " + item.Name);
                }
            }
            else
            {
                ShowGameMessage("Предметів для використання немає");
            }
            if (hero.EquippedItem != null)
            {
                ShowGameMessage("Екіпіруваний предмет: " + hero.EquippedItem.Name);
            }
            else
            {
                ShowGameMessage("Екіпіруваних предметів: Немає");
            }
        }
        public void ShowInventory(Hero hero)
        {
            ShowGameMessage("\nІнвентар героя:");

            foreach (Item item in hero.Inventory)
            {
                ShowGameMessage("- " + item.Name);
            }
        }
        public void ShowNumberedInventory(IReadOnlyList<Item> items)
        {
            int itemNumber = 1;
            foreach (Item item in items)
            {
                ShowGameMessage(itemNumber + ". " + item.Name);
                itemNumber++;
            }
        }
        public void ShowActionMenu()
        {
            ShowGameMessage("\nДоступні дії:\n1 - атакувати\n2 - лікуватися\n3 - показати статус" +
                                      "\n4 - втекти\n5 - використати предмет\n6 - екіпірувати предмет\n" +
                                      "7 - зберегти та повернутися в меню\n8 - бестіарій");
        }
        public string GetHeroName()
        {
            string? name;
            ShowGameMessage("\nВведіть ім'я героя: ");
            name = ReadInput();
            while (string.IsNullOrWhiteSpace(name))
            {
                ShowGameMessage("\nВведіть ім'я героя: ");
                name = ReadInput();
            }

            return name;
        }
        public string? ReadInput()
        {
            return Console.ReadLine();
        }
        public void ShowRoundNumber(int round)
        {
            ShowGameMessage("\nРаунд: " + round);
        }
        public void ShowHeroAttack(Hero hero, Enemy enemy, int heroDamage, bool isCritical)
        {
            if (isCritical)
            {
                AddBattleLog("\nКритичний удар!");
            }
            AddBattleLog("\n" + hero.Name + " завдає " + enemy.Name + " " + heroDamage + " шкоди.\nУ " + enemy.Name + " залишилось " + enemy.Health + " очок здоров'я.");
        }
        public void ShowEnemyAttack(Enemy enemy, Hero hero, int enemyDamage)
        {
            AddBattleLog("\n" + enemy.Name + " завдає " + enemyDamage + " шкоди герою.\nУ " + hero.Name + " залишилось " + hero.Health + " очок здоров'я.");
        }
        public void ShowHealedAmount(Hero hero, int healedAmount)
        {
            AddBattleLog("\n" + hero.Name + " відновив " + healedAmount + " очок здоров'я.\nУ " + hero.Name + " залишилось " + hero.Health + " очок здоров'я.");
        }
        public void ShowUsedItem(Hero hero, Item item)
        {
            AddBattleLog("\n" + hero.Name + " використав " + item.Name + ".");
        }
        public void ShowEquippedItem(Hero hero, Item selectedItem)
        {
            AddBattleLog("\n" + hero.Name + " екіпірував " + selectedItem.Name + ".\nНова сила атаки: " + hero.AttackBonus);
        }
        public void ShowEnemyDefeated(Enemy enemy)
        {
            AddBattleLog("\n" + enemy.Name + " переможений!");
        }
        public void ShowEnemyDrop(Enemy enemy, Item droppedItem)
        {
            AddBattleLog("\n" + enemy.Name + " залишив після себе " + droppedItem.Name + ".");
        }
        public void ShowHeroRun(Hero hero)
        {
            AddBattleLog("\n" + hero.Name + " втік з бою!");
        }
        public void ShowHeroWin(Hero hero)
        {
            ShowGameMessage("\n" + hero.Name + " переміг усіх ворогів!");
        }
        public void ShowHeroDefeated(Hero hero)
        {
            AddBattleLog("\n" + hero.Name + " був переможений!");
        }
        public void ShowHeroGold(Hero hero)
        {
            AddBattleLog("\n" + hero.Name + " має " + hero.Gold + " золота.");
        }
        public void ShowGameMessage(string message)
        {
            Console.WriteLine(message);
        }
        public void ShowEnemyAppeared(Enemy enemy)
        {
            string text = ("\nВорог " + enemy.Name + " з'явився та має " + enemy.Health + " HP");
            AddBattleLog(text);
        }
        public void ShowMainMenu()
        {
            ShowGameMessage("\n1. Нова гра \n2. Завантажити гру \n3. Вийти з гри");
        }
        public void ClearBattleLog()
        {
            _battleLog.Clear();
        }
        public void AddBattleLog(string message)
        {
            if (_battleLog.Count >= MaxBattleLogEntries)
            {
                _battleLog.Dequeue();
            }
            _battleLog.Enqueue(message);
        }
        public void ShowBattleScreen(Hero hero, Enemy enemy, int round)
        {
            Console.Clear();
            ShowGameTitle();
            ShowGameMessage("У " + hero.Name + " зараз " + hero.Health + "HP");
            ShowGameMessage("У " + enemy.Name + " зараз " + enemy.Health + "HP");
            ShowBattleLog();
            ShowRoundNumber(round);
            ShowActionMenu();
        }
        public void ShowGame()
        {
            Console.Clear();
            ShowGameTitle();
            ShowMainMenu();
        }
        public void ShowBattleResultScreen(Hero hero)
        {
            Console.Clear();
            ShowGameTitle();
            ShowBattleLog();
            ShowHeroStatus(hero);
            ShowGameMessage("Натисніть Enter, щоб повернутися в головне меню");
        }
        public void ShowGameSaved()
        {
            AddBattleLog("Гру збережено. Повертаємось в головне меню.");
        }
        public void ShowEnemyDefeatStatistics(GameSession gameSession)
        {
            AddBattleLog("Переможені вороги: ");
            foreach (string enemyName in gameSession.GetDefeatedEnemyNames())
            {
                int enemyCount = gameSession.GetDefeatedEnemyCount(enemyName);
                AddBattleLog("- " + enemyName + ": " + enemyCount);
            }
        }
        public void ShowNewBestiaryEntry(string enemyName)
        {
            AddBattleLog($"Новий запис у бестіарії: {enemyName}");
        }
        public void ShowBestiaryScreen(GameSession gameSession)
        {
            Console.Clear();
            ShowGameTitle();
            ShowGameMessage("Бестіарій:");
            if (!gameSession.GetBestiaryEntries().Any())
            {
                ShowGameMessage("Немає записів.");
                return;
            }
            foreach (BestiaryEntry entry in gameSession.GetBestiaryEntries())
            {
                ShowGameMessage($"- {entry.EnemyName}. Переможено: {entry.DefeatCount}");
            }
        }
    }
}