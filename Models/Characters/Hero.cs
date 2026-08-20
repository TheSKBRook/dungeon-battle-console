using ConsoleApp1.Models.Items;

namespace ConsoleApp1.Models.Characters
{
    internal class Hero : Character
    {
        public int Gold { get; private set; }
        public Item? EquippedItem { get; private set; }
        public int AttackBonus { get; private set; }
        private List<Item> inventory = new List<Item>();
        public IReadOnlyList<Item> Inventory
        {
            get { return inventory; }
        }
        public Hero(string name) : this(name, 100, 10)
        {
        }
        public Hero(string name, int currentHealth, int currentGold) : base(name, 100)
        {
            Health = currentHealth;
            Gold = currentGold;
        }
        public void AddGold(int amount)
        {
            Gold += amount;
        }
        public int Heal(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }
            int healthBefore = Health;
            Health += amount;
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }

            return Health - healthBefore;
        }
        public void AddItem(Item item)
        {
            inventory.Add(item);
        }
        public int UseItem(Item item)
        {
            if (!inventory.Contains(item))
            {
                return 0;
            }
            if (item is IUsable usable)
            {
                int result = usable.Use(this);
                inventory.Remove(item);
                return result;
            }

            return 0;
        }
        public void EquipWeapon(int bonus)
        {
            AttackBonus = bonus;
        }
        public bool EquipItem(Item item)
        {
            if (!inventory.Contains(item))
            {
                return false;
            }
            if (item is IEquippable equippable)
            {
                if (item == EquippedItem)
                {
                    return false;
                }
                equippable.Equip(this);
                EquippedItem = item;
                return true;
            }
            return false;
        }
        public int GetBaseDamage(Random random)
        {
            return random.Next(5, 21);
        }
        public int GetFullDamage(int baseDamage)
        {
            return baseDamage + AttackBonus;
        }
        public IEnumerable<Item> GetUsableItems()
        {
            return inventory.Where(item => item is IUsable);
        }
        public Item? FindItemByNmae(string name)
        {
            return inventory.FirstOrDefault(item => item.Name == name);
        }
        public IEnumerable<Item> GetInventorySortedByName()
        {
            return inventory.OrderBy(item => item.Name);
        }
        public IEnumerable<string> GetInventoryItemNames()
        {
            return inventory.Select(item => item.Name);
        }
    }
}