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

    private Fighter[] _fighters =
    {
        new Barbarian(),
        new Knight(),
        new Glutton(),
        new Warrior(),
        new Assassin()
    };

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

        foreach(var fighter in _fighters)
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
            
            if(userInput < 0 || userInput > _fighters.Length - 1)
            {
                Console.WriteLine("Бойца под таким номером нет. Повторите ввод...");
            }
            else
            {
                isWork= false;
            }
        }

        switch (_fighters[userInput])
        {
            case Barbarian _:
                return new Barbarian();
            case Knight _:
                return new Knight();
            case Glutton _:
                return new Glutton();
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

        while(_fighterLeft.IsAlive && _fighterRight.IsAlive)
        {
            Console.WriteLine($"{_fighterLeft.Name} ходит...");

            _fighterLeft.Attack(_fighterRight);

            if (_fighterRight.IsAlive == false)
                continue;

            _fighterLeft.TryToActivateAbility(_fighterLeft);

            Console.WriteLine($"{_fighterRight.Name} ходит...");

            _fighterRight.Attack(_fighterLeft);
            _fighterRight.TryToActivateAbility(_fighterRight);
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
    protected Ability Ability;

    public string Name { get; protected set; }

    public int Armor { get; protected set; }
    public int BaseArmor { get; protected set; }

    public int Damage { get; protected set; }
    public int BaseDamage { get; protected set; }

    public int MaxHealth { get; protected set; }
    public int Health { get; protected set; }
    public bool IsAlive { get; protected set; }

    public void Attack(Fighter fighter)
    {
        Console.WriteLine($"{Name}: Атакую!");

        fighter.TakeDamage(Damage);
    }

    public void TakeDamage(int damage)
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

    public void TryToActivateAbility(Fighter fighter)
    {
        Ability.TryActivate(fighter);
    }

    public void SetMaxHealth(int maxHealth)
    {
        MaxHealth = maxHealth;
    }

    public void AddHealth(int health)
    {
        Health += health;

        if (Health > 0)
            Health = MaxHealth;
    }

    public void SetDamage(int damage)
    {
        Damage = damage;
    }

    public void SetArmor(int armor)
    {
        Armor = armor;
    }

    public void ShowInfo()
    {
        Console.WriteLine($"||Класс бойца: {Name} | Запас здоровья: {MaxHealth} | Броня: {Armor} | Урон: {Damage} | Способность: {Ability.Name}||");
        Ability.ShowInfo();
        Console.WriteLine("_______________________________________");
    }
}

class Barbarian : Fighter
{

    public Barbarian()
    {
        Ability = new StoneSkin();
        Name = "Варвар";
        MaxHealth = 150;
        Health = MaxHealth;
        BaseArmor = 3;
        Armor = BaseArmor;
        BaseDamage = 33;
        Damage = BaseDamage;
        IsAlive = true;
    }
}

class Knight : Fighter
{
    public Knight()
    {
        Ability = new Healing();
        Name = "Рыцарь";
        MaxHealth = 250;
        Health = MaxHealth;
        BaseArmor = 10;
        Armor = BaseArmor;
        BaseDamage = 10;
        Damage = BaseDamage;
        IsAlive = true;
    }
}

class Glutton : Fighter
{

    public Glutton()
    {
        Ability = new FatMan();
        Name = "Обжора";
        MaxHealth = 150;
        Health = MaxHealth;
        BaseArmor = 4;
        Armor = BaseArmor;
        BaseDamage = 30;
        Damage = BaseDamage;
        IsAlive = true;
    }
}

class Warrior : Fighter
{
    public Warrior()
    {
        Ability = new Berserk();
        Name = "Воин";
        MaxHealth = 200;
        Health = MaxHealth;
        BaseArmor = 8;
        Armor = BaseArmor;
        BaseDamage = 25;
        Damage = BaseDamage;
        IsAlive = true;
    }
}

class Assassin : Fighter
{
    public Assassin()
    {
        Ability = new DoubleDamage();
        Name = "Убийца";
        MaxHealth = 130;
        Health = MaxHealth;
        BaseArmor = 7;
        Armor = BaseArmor;
        BaseDamage = 35;
        Damage = BaseDamage;
        IsAlive = true;
    }
}

abstract class Ability
{
    protected int ChanceOfTriggering;
    protected Random Random = new Random();

    public string Name { get; protected set; }

    public virtual void TryActivate(Fighter fighter)
    {
        if (ShouldTrigger(fighter))
            Enable(fighter);
    }

    public abstract void ShowInfo();

    protected int GetRandomNumber()
    {
        int minimumRandomNumber = 0;
        int maximumRandomNumber = 100;

        return Random.Next(minimumRandomNumber, maximumRandomNumber);
    }

    protected virtual bool ShouldTrigger(Fighter fighter)
    {
        int resultNumber = GetRandomNumber();

        return resultNumber <= ChanceOfTriggering;
    }

    protected abstract void Enable(Fighter fighter);
}

class Healing : Ability
{
    private int _minimumHealingPoint = 15;
    private int _maximumHealingPoint = 26;

    public Healing()
    {
        Name = "Исцеление";
        ChanceOfTriggering = 10;
    }

    public override void ShowInfo()
    {
        Console.WriteLine($"{Name}: c вероятностью в {ChanceOfTriggering}% восстановит от {_minimumHealingPoint} до {_maximumHealingPoint} единиц здоровья.");
    }

    protected override void Enable(Fighter fighter)
    {
        int totalHealingPoint = Random.Next(_minimumHealingPoint, _maximumHealingPoint);

        fighter.AddHealth(totalHealingPoint);

        Console.WriteLine($"{fighter.Name} применяет {Name} и восстанавливает себе {totalHealingPoint} здоровья.");
    }
}

class StoneSkin : Ability
{
    private int _armorBoost = 10;
    private int _amountOfMoves = 3;
    private int _moveCounter = 0;
    private bool _isActive = false;

    public StoneSkin()
    {
        Name = "Каменная кожа";
        ChanceOfTriggering = 15;
    }

    public override void TryActivate(Fighter fighter)
    {
        if (_moveCounter == _amountOfMoves)
            Disabled(fighter);

        if (_isActive)
        {
            _moveCounter++;
        }
        else
        {
            base.TryActivate(fighter);
        }
    }

    public override void ShowInfo()
    {
        Console.WriteLine($"{Name}: с вероятностью {ChanceOfTriggering}% увеличит броню на {_armorBoost} единиц, на {_amountOfMoves} хода.");
    }

    protected override void Enable(Fighter fighter)
    {
        int totalArmor = fighter.Armor + _armorBoost;

        fighter.SetArmor(totalArmor);

        Console.WriteLine($"{fighter.Name} применяет {Name}, броня увеличена на {_armorBoost} единиц.");
    }

    private void Disabled(Fighter fighter)
    {
        _isActive = false;
        _moveCounter = 0;

        fighter.SetArmor(fighter.BaseArmor);

        Console.WriteLine($"{Name} иссякла. Уровень брони вернулся в норму.");
    }
}

class FatMan : Ability
{
    private int _thresholdHealthForTriggering = 30;
    private int _boostMaxHealth = 300;
    private int _healingHealth;
    private int _reducedDamage = 25;
    private int _reducedArmor = 3;
    private bool _isActive = false;

    public FatMan()
    {
        Name = "Толстяк";
        _healingHealth = _boostMaxHealth;
    }

    public override void ShowInfo()
    {
        Console.WriteLine($"{Name}: когда уровень жизней упадет до {_thresholdHealthForTriggering} очков," +
            $" максимальное количество ХП возрастет до {_boostMaxHealth}\n" +
            $" излечит {_healingHealth} очков жизней, но взамен снизит показатель урона на {_reducedDamage} единиц" +
            $" и показатель брони на {_reducedArmor} единиц.\n" +
            $"Срабатывает один раз за бой.");
    }

    protected override bool ShouldTrigger(Fighter fighter)
    {
        return fighter.Health <= _thresholdHealthForTriggering && _isActive == false;
    }

    protected override void Enable(Fighter fighter)
    {
        int totalDamage = fighter.Damage - _reducedDamage;
        int totalArmor = fighter.Armor - _reducedArmor;

        fighter.SetMaxHealth(_boostMaxHealth);
        fighter.AddHealth(_healingHealth);
        fighter.SetDamage(totalDamage);
        fighter.SetArmor(totalArmor);

        _isActive = true;

        Console.WriteLine($"{fighter.Name} применяет способность {Name}");
    }
}

class Berserk : Ability
{
    private int _armorBoost = 10;
    private int _damageBoost = 10;
    private int _thresholdHealthForTriggering = 50;

    public Berserk()
    {
        Name = "Берсерк";
    }

    public override void ShowInfo()
    {
        Console.WriteLine($"{Name}: когда уровень ХП упадет до {_thresholdHealthForTriggering} единиц," +
            $" увеличит показатели брони на {_armorBoost} и урон {_damageBoost} единиц.");
    }

    protected override bool ShouldTrigger(Fighter fighter)
    {
        return fighter.Health <= _thresholdHealthForTriggering;
    }

    protected override void Enable(Fighter fighter)
    {
        int totalArmor = _armorBoost + fighter.Armor;
        int totalDamage = _damageBoost + fighter.Damage;

        fighter.SetArmor(totalArmor);
        fighter.SetDamage(totalDamage);

        Console.WriteLine($"{fighter.Name} применяет способность {Name}. Урон +{_damageBoost}. Броня +{_armorBoost}");
    }
}

class DoubleDamage : Ability
{
    private int _damageMultiplier = 2;
    private bool _isActive = false;

    public DoubleDamage()
    {
        Name = "Сильнейший удар";
        ChanceOfTriggering = 25;
    }

    public override void TryActivate(Fighter fighter)
    {
        if (_isActive)
        {
            Disabled(fighter);
        }
        else
        {
            base.TryActivate(fighter);
        }
    }

    public override void ShowInfo()
    {
        Console.WriteLine($"{Name}: c вероятностью {ChanceOfTriggering}% следующий удар нанесет критический урон (х{_damageMultiplier})");
    }

    protected override void Enable(Fighter fighter)
    {
        int totalDamage = fighter.Damage * _damageMultiplier;

        fighter.SetDamage(totalDamage);

        Console.WriteLine("Следующий удар нанесет двойной урон.");

        _isActive = true;
    }

    private void Disabled(Fighter fighter)
    {
        _isActive = false;

        fighter.SetDamage(fighter.BaseDamage);
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