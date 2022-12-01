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
            numberFighter++;
            Console.WriteLine($"Боец под номером ({numberFighter})");
            fighter.ShowInfo();
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
        Fighter fighter = null;

        bool isWork = true;

        while (isWork)
        {
            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    fighter = new Barbarian();
                    isWork = false;
                    break;
                case "2":
                    fighter = new Knight();
                    isWork = false;
                    break;
                case "3":
                    fighter = new Glutton();
                    isWork = false;
                    break;
                case "4":
                    fighter = new Warrior();
                    isWork = false;
                    break;
                case "5":
                    fighter = new Assassin();
                    isWork = false;
                    break;
                default:
                    Console.WriteLine("Некорректный ввод данных");
                    break;
            }
        }

        return fighter;
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
        Console.WriteLine($"Боец слева:");
        _fighterLeft.ShowInfo();

        Console.WriteLine($"Боец справа:");
        _fighterRight.ShowInfo();

        while(_fighterLeft.IsAlive && _fighterRight.IsAlive)
        {
            Console.WriteLine($"{_fighterLeft.Name} ходит...");

            _fighterLeft.Attack(_fighterRight);

            if (_fighterRight.IsAlive == false)
                continue;

            _fighterLeft.TryToActivateAbility(_fighterLeft);

            Console.WriteLine($"{_fighterRight.Name} ходит...");

            _fighterRight.Attack(_fighterLeft);
            _fighterRight.TryToActivateAbility( _fighterRight);
        }

        if (_fighterLeft.IsAlive)
        {
            Console.WriteLine($"Победил боец слева: {_fighterLeft.Name}.");
        }
        else
        {
            Console.WriteLine($"Победил боец справа: {_fighterRight.Name}.");
        }
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
        Ability.OnTurn(fighter);
    }

    public void ChangeMaxHealth(int maxHealth)
    {
        MaxHealth = maxHealth;
    }

    public void ChangeHealth(int health)
    {
        Health += health;

        if (Health > 0)
            Health = MaxHealth;
    }

    public void ChangeDamage(int damage)
    {
        Damage = damage;
    }

    public void ChangeArmor(int armor)
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

    public virtual void OnTurn(Fighter fighter)
    {
        if (ShouldTrigger(fighter))
            Trigger(fighter);
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

    protected abstract void Trigger(Fighter fighter);
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

    protected override void Trigger(Fighter fighter)
    {
        int totalHealingPoint = Random.Next(_minimumHealingPoint, _maximumHealingPoint);

        fighter.ChangeHealth(totalHealingPoint);

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

    public override void OnTurn(Fighter fighter)
    {
        if (_moveCounter == _amountOfMoves)
            OffTrigger(fighter);

        if (_isActive)
        {
            _moveCounter++;
        }
        else
        {
            base.OnTurn(fighter);
        }
    }

    public override void ShowInfo()
    {
        Console.WriteLine($"{Name}: с вероятностью {ChanceOfTriggering}% увеличит броню на {_armorBoost} единиц, на {_amountOfMoves} хода.");
    }

    protected override void Trigger(Fighter fighter)
    {
        int totalArmor = fighter.Armor + _armorBoost;

        fighter.ChangeArmor(totalArmor);

        Console.WriteLine($"{fighter.Name} применяет {Name}, броня увеличена на {_armorBoost} единиц.");
    }

    private void OffTrigger(Fighter fighter)
    {
        _isActive = false;
        _moveCounter = 0;

        fighter.ChangeArmor(fighter.BaseArmor);

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

    protected override void Trigger(Fighter fighter)
    {
        int totalDamage = fighter.Damage - _reducedDamage;
        int totalArmor = fighter.Armor - _reducedArmor;

        fighter.ChangeMaxHealth(_boostMaxHealth);
        fighter.ChangeHealth(_healingHealth);
        fighter.ChangeDamage(totalDamage);
        fighter.ChangeArmor(totalArmor);

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

    protected override void Trigger(Fighter fighter)
    {
        int totalArmor = _armorBoost + fighter.Armor;
        int totalDamage = _damageBoost + fighter.Damage;

        fighter.ChangeArmor(totalArmor);
        fighter.ChangeDamage(totalDamage);

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

    public override void OnTurn(Fighter fighter)
    {
        if (_isActive)
        {
            OffTrigger(fighter);
        }
        else
        {
            base.OnTurn(fighter);
        }
    }

    public override void ShowInfo()
    {
        Console.WriteLine($"{Name}: c вероятностью {ChanceOfTriggering}% следующий удар нанесет критический урон (х{_damageMultiplier})");
    }

    protected override void Trigger(Fighter fighter)
    {
        int totalDamage = fighter.Damage * _damageMultiplier;

        fighter.ChangeDamage(totalDamage);

        Console.WriteLine("Следующий удар нанесет двойной урон.");

        _isActive = true;
    }

    private void OffTrigger(Fighter fighter)
    {
        _isActive = false;

        fighter.ChangeDamage(fighter.BaseDamage);
    }
}