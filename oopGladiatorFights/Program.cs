using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oopGladiatorFights
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Arena arena = new Arena();

            arena.Start();
        }
    }
}

class Arena
{
    private Fight _fight;
    private Fighter[] _fighters = new Fighter[5];

    public Arena()
    {
        _fighters[0] = new Barbarian();
        _fighters[1] = new Knight();
        _fighters[2] = new Thief();
        _fighters[3] = new Warrior();
        _fighters[4] = new Assassin();
    }

    public void Start()
    {
        Console.WriteLine("Добро пожаловать на арену!");

        ChooseFighters();
        _fight.Start();

        Console.ReadLine();
    }

    private void ChooseFighters()
    {
        Fighter fighterRight;
        Fighter fighterLeft;

        int numberFighter = 0;

        foreach (var fighter in _fighters)
        {
            Console.WriteLine($"Боец под номером ({numberFighter})");
            fighter.ShowInfo();
            numberFighter++;
        }

        Console.Write("Выберите номер бойца слева: ");
        fighterLeft = GetFighter();
        Console.WriteLine();

        Console.Write("Выберите номер бойца справа: ");
        fighterRight = GetFighter();
        Console.WriteLine();

        _fight = new Fight(fighterLeft, fighterRight);
    }

    private Fighter GetFighter()
    {
        bool isWork = true;
        int userInput = 0;

        while (isWork)
        {
            userInput = UserUtils.GetNumber();

            if (userInput < 0 || userInput > _fighters.Length - 1)
            {
                Console.WriteLine("Бойца под таким номером нет. Повторите ввод...");
            }
            else
            {
                isWork = false;
            }
        }

        switch (_fighters[userInput])
        {
            case Barbarian _:
                return new Barbarian();
            case Knight _:
                return new Knight();
            case Thief _:
                return new Thief();
            case Warrior _:
                return new Warrior();
            case Assassin _:
                return new Assassin();
            default:
                Console.WriteLine("Некорректный ввод данных");
                return null;
        }
    }
}

class Fight
{
    private Fighter _fighterLeft;
    private Fighter _fighterRight;

    public Fight(Fighter fighterLeft, Fighter fighterRight)
    {
        _fighterLeft = fighterLeft;
        _fighterRight = fighterRight;
    }

    public void Start()
    {
        AnnounceFighters();

        while (_fighterLeft.IsAlive && _fighterRight.IsAlive)
        {
            Console.WriteLine($"{_fighterLeft.Name} ходит...");

            _fighterLeft.Attack(_fighterRight);

            if (_fighterRight.IsAlive == false)
                continue;

            Console.WriteLine($"{_fighterRight.Name} ходит...");

            _fighterRight.Attack(_fighterLeft);
        }

        ShowWinner();
    }

    private void ShowWinner()
    {
        if (_fighterLeft.IsAlive)
        {
            Console.WriteLine($"Победил боец слева: {_fighterLeft.Name}.");
        }
        else
        {
            Console.WriteLine($"Победил боец справа: {_fighterRight.Name}.");
        }
    }

    private void AnnounceFighters()
    {
        Console.WriteLine($"Боец слева:");
        _fighterLeft.ShowInfo();

        Console.WriteLine($"Боец справа:");
        _fighterRight.ShowInfo();
    }
}

abstract class Fighter
{
    protected Random PercentChance = new Random();
    protected int MinimumChance = 0;
    protected int MaximumChance = 100;

    protected string Ability;
    protected int ChanceOfTrigger;
    protected bool AbilityIsActive;

    public string Name { get; protected set; }

    public int Armor { get; protected set; }
    public int BaseArmor { get; protected set; }

    public int Damage { get; protected set; }
    public int BaseDamage { get; protected set; }

    public int MaxHealth { get; protected set; }
    public int Health { get; protected set; }
    public bool IsAlive { get; protected set; }

    public virtual void Attack(Fighter fighter)
    {
        Console.WriteLine($"{Name}: Атакую!");

        fighter.TakeDamage(Damage);
    }

    public virtual void TakeDamage(int damage)
    {
        int currentDamage;

        currentDamage = damage * (100 - Armor) / 100;
        Health -= currentDamage;

        IsAlive = Health > 0;

        if (IsAlive)
        {
            Console.WriteLine($"{Name} получает урон в размере {currentDamage} единиц.");
        }
        else
        {
            Health = 0;

            Console.WriteLine($"{Name} убит!");

            IsAlive = false;
        }
    }

    public void ShowInfo()
    {
        Console.WriteLine($"||Класс бойца: {Name} | Запас здоровья: {MaxHealth} | Броня: {Armor} | Урон: {Damage} | Способность: {Ability}||");
        Console.WriteLine("_______________________________________");
    }

    protected abstract void ShowAbilityDescription();

    protected abstract void TryEnableAbility();

    protected abstract void DisableAbility();

    protected void IncreaseHealth(int health)
    {
        Health += health;

        if (Health > MaxHealth)
            Health = MaxHealth;
    }

    protected void IncreaseDamage(int damage)
    {
        Damage += damage;
    }

    protected void IncreaseArmor(int armor)
    {
        Armor += armor;
    }

    protected bool WillThereBeChance()
    {
        int randomChance = PercentChance.Next(MinimumChance, MaximumChance);

        return randomChance >= ChanceOfTrigger;
    }
}

class Barbarian : Fighter
{
    private int _armorBoost = 10;
    private int _numberOfMoves = 5;
    private int _movesCount = 0;

    public Barbarian()
    {
        Ability = "Каменная кожа";
        Name = "Варвар";
        ChanceOfTrigger = 15;

        MaxHealth = 150;
        Health = MaxHealth;

        BaseArmor = 3;
        Armor = BaseArmor;

        BaseDamage = 33;
        Damage = BaseDamage;

        IsAlive = true;
        AbilityIsActive = false;
    }

    public override void Attack(Fighter fighter)
    {
        base.Attack(fighter);

        if (_movesCount == _numberOfMoves)
            DisableAbility();

        if (AbilityIsActive)
            _movesCount++;

        if (AbilityIsActive == false)
            TryEnableAbility();
    }

    protected override void TryEnableAbility()
    {
        if(WillThereBeChance())
        {
            AbilityIsActive = true;

            IncreaseArmor(_armorBoost);

            Console.WriteLine($"{Name}: Способность '{Ability}' активирована!" +
                $" Броня увеличена на {_armorBoost} на {_numberOfMoves} ходов.");
        }
    }

    protected override void DisableAbility()
    {
        AbilityIsActive = false;
        _movesCount = 0;
        Armor = BaseArmor;

        Console.WriteLine($"{Name}: Действие способности закончилось.");
    }

    protected override void ShowAbilityDescription()
    {
        Console.WriteLine($"{Ability}: С вероятностью {ChanceOfTrigger}%, увеличит броню на {_armorBoost}ед., на {_numberOfMoves} ходов");
    }
}

class Knight : Fighter
{
    private int _healingPoint = 20;

    public Knight()
    {
        Ability = "Исцеление";
        Name = "Рыцарь";
        ChanceOfTrigger = 10;

        MaxHealth = 250;
        Health = MaxHealth;

        BaseArmor = 10;
        Armor = BaseArmor;

        BaseDamage = 10;
        Damage = BaseDamage;

        IsAlive = true;
        AbilityIsActive = false;
    }

    public override void Attack(Fighter fighter)
    {
        base.Attack(fighter);

        TryEnableAbility();
    }

    protected override void DisableAbility()
    {
        AbilityIsActive = false;
    }

    protected override void TryEnableAbility()
    {
        if (WillThereBeChance())
        {
            AbilityIsActive = true;

            IncreaseHealth(_healingPoint);

            DisableAbility();

            Console.WriteLine($"{Name}: Способность {Ability} активирована! +{_healingPoint}хр.");
        }
    }

    protected override void ShowAbilityDescription()
    {
        Console.WriteLine($"{Ability}: С вероятностью {ChanceOfTrigger}% востановит {_healingPoint}ед. здоровья.");
    }
}

class Thief : Fighter
{

    public Thief()
    {
        Ability = "Уклонение";
        Name = "Вор";
        ChanceOfTrigger = 30;

        MaxHealth = 150;
        Health = MaxHealth;

        BaseArmor = 2;
        Armor = BaseArmor;

        BaseDamage = 30;
        Damage = BaseDamage;

        IsAlive = true;
        AbilityIsActive = false;
    }

    public override void TakeDamage(int damage)
    {
        if (AbilityIsActive)
        {
            Console.WriteLine($"{Name}: Уклонение.");

            DisableAbility();
        }
        else
        {
            base.TakeDamage(damage);
        }
    }

    protected override void DisableAbility()
    {
        AbilityIsActive = false;
    }

    protected override void TryEnableAbility()
    {
        if (WillThereBeChance())
            AbilityIsActive = true;
    }

    protected override void ShowAbilityDescription()
    {
        Console.WriteLine($"{Ability}: С вероятностью {ChanceOfTrigger}% позволяет уклониться от атаки.");
    }
}

class Warrior : Fighter
{
    private int _damageBoost = 20;
    private int _armorBoost = 15;
    private int _numberOfMoves = 10;
    private int _movesCount = 0;

    public Warrior()
    {
        Ability = "Берсеркер";
        Name = "Воин";
        ChanceOfTrigger = 5;

        MaxHealth = 200;
        Health = MaxHealth;

        BaseArmor = 8;
        Armor = BaseArmor;

        BaseDamage = 25;
        Damage = BaseDamage;

        IsAlive = true;
        AbilityIsActive = false;
    }

    public override void Attack(Fighter fighter)
    {
        base.Attack(fighter);

        if (_movesCount == _numberOfMoves)
            DisableAbility();

        if (AbilityIsActive)
            _movesCount++;

        if (AbilityIsActive == false)
            TryEnableAbility();
    }

    protected override void DisableAbility()
    {
        AbilityIsActive = false;
        Armor = BaseArmor;
        Damage = BaseDamage;
        _movesCount = 0;

        Console.WriteLine($"{Name}: Действие способности закончилось.");
    }

    protected override void TryEnableAbility()
    {
        if (WillThereBeChance())
        {
            AbilityIsActive = true;

            IncreaseArmor(_armorBoost);
            IncreaseDamage(_damageBoost);

            Console.WriteLine($"{Name}: {Ability} активирован!" +
                $"Броня увеличена на {_armorBoost}, урон увеличен на {_damageBoost}, на {_numberOfMoves} ходов.");
        }
    }

    protected override void ShowAbilityDescription()
    {
        Console.WriteLine($"{Ability}: C Вероятностью {ChanceOfTrigger}%," +
            $" увеличит урон на {_damageBoost} и броню на {_armorBoost}, на {_numberOfMoves} ходов.");
    }
}

class Assassin : Fighter
{
    public Assassin()
    {
        Ability = "Двойной удар";
        Name = "Убийца";
        ChanceOfTrigger = 25;

        MaxHealth = 130;
        Health = MaxHealth;
        BaseArmor = 2;
        Armor = BaseArmor;
        BaseDamage = 25;
        Damage = BaseDamage;
        IsAlive = true;
    }

    public override void Attack(Fighter fighter)
    {
        TryEnableAbility();

        if (AbilityIsActive)
        {
            base.Attack(fighter);
            base.Attack(fighter);
        }
        else
        {
            base.Attack(fighter);
        }
    }

    protected override void DisableAbility()
    {
        AbilityIsActive = false;
    }

    protected override void TryEnableAbility()
    {
        if (WillThereBeChance())
            AbilityIsActive = true;
        Console.WriteLine($"{Name}: Способность активирована!");
    }

    protected override void ShowAbilityDescription()
    {
        Console.WriteLine($"{Ability}: С вероятностью {ChanceOfTrigger}% Нанесет два удара подряд.");
    }
}

static class UserUtils
{
    public static int GetNumber()
    {
        bool isNumberWork = true;
        int userNumber = 0;

        while (isNumberWork)
        {
            bool isNumber = true;
            string userInput = Console.ReadLine();

            if (isNumber = int.TryParse(userInput, out int number))
            {
                userNumber = number;
                isNumberWork = false;
            }
            else
            {
                Console.WriteLine($"Не правильный ввод данных!!!  Повторите попытку");
            }
        }

        return userNumber;
    }
}