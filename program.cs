using System;
using System.Collections.Generic;
using System.Threading;


public delegate void BattleEventHandler(object sender, BattleEventArgs args);


public class BattleEventArgs : EventArgs
{
    public string Message { get; }

    public BattleEventArgs(string message)
    {
        Message = message;
    }
}


public interface ISpell
{
    string Name { get; }
    int ManaCost { get; }
    void Cast(Mage caster, Mage target);
}


public abstract class Mage
{
    public string Name { get; }
    public int Health { get; protected set; }
    public int Mana { get; protected set; }

    public event BattleEventHandler Attacked;
    public event BattleEventHandler Defended;
    public event BattleEventHandler SpellCasted;

    protected Mage(string name, int health, int mana)
    {
        Name = name;
        Health = health;
        Mana = mana;
    }

    public abstract void Attack(Mage target);
    public abstract void Defend(ISpell spell);
    public abstract void CastSpell(ISpell spell, Mage target);

    public void DisplayStatus()
    {
        Console.WriteLine($"{Name} - Health: {Health}, Mana: {Mana}");
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health < 0) Health = 0;
    }

    protected void OnAttacked(string message)
    {
        Attacked?.Invoke(this, new BattleEventArgs(message));
    }

    protected void OnDefended(string message)
    {
        Defended?.Invoke(this, new BattleEventArgs(message));
    }

    protected void OnSpellCasted(string message)
    {
        SpellCasted?.Invoke(this, new BattleEventArgs(message));
    }

    public bool IsAlive => Health > 0;

    public abstract string GetArt();
}


public class FireSpell : ISpell
{
    public string Name => "Fireball";
    public int ManaCost => 20;

    public void Cast(Mage caster, Mage target)
    {
        int damage = 30;
        target.TakeDamage(damage);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{caster.Name} cast {Name} on {target.Name}, dealing {damage} damage!");
        Console.ResetColor();
    }
}

public class WaterSpell : ISpell
{
    public string Name => "Water Blast";
    public int ManaCost => 15;

    public void Cast(Mage caster, Mage target)
    {
        int damage = 25;
        target.TakeDamage(damage);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"{caster.Name} cast {Name} on {target.Name}, dealing {damage} damage!");
        Console.ResetColor();
    }
}


public class FireMage : Mage
{
    public FireMage(string name) : base(name, 100, 50)
    {
    }

    public override void Attack(Mage target)
    {
        int damage = 20;
        target.TakeDamage(damage);
        OnAttacked($"{Name} attacked {target.Name} with a fireball, dealing {damage} damage.");
    }

    public override void Defend(ISpell spell)
    {
        OnDefended($"{Name} defended against {spell.Name}.");
    }

    public override void CastSpell(ISpell spell, Mage target)
    {
        if (Mana >= spell.ManaCost)
        {
            spell.Cast(this, target);
            Mana -= spell.ManaCost;
            OnSpellCasted($"{Name} cast {spell.Name} on {target.Name}.");
        }
        else
        {
            Console.WriteLine($"{Name} does not have enough mana to cast {spell.Name}.");
        }
    }

    public override string GetArt()
    {
        return @"
        (    )
         (  )    (  )      
          )       )      )
       (               (    )
      ) ( )       )    ( ) )
  ( ( ( ) ) )   (  ) ( (  ) ( )";
    }
}

public class WaterMage : Mage
{
    public WaterMage(string name) : base(name, 120, 80)
    {
    }

    public override void Attack(Mage target)
    {
        int damage = 15;
        target.TakeDamage(damage);
        OnAttacked($"{Name} attacked {target.Name} with a water blast, dealing {damage} damage.");
    }

    public override void Defend(ISpell spell)
    {
        OnDefended($"{Name} defended against {spell.Name}.");
    }

    public override void CastSpell(ISpell spell, Mage target)
    {
        if (Mana >= spell.ManaCost)
        {
            spell.Cast(this, target);
            Mana -= spell.ManaCost;
            OnSpellCasted($"{Name} cast {spell.Name} on {target.Name}.");
        }
        else
        {
            Console.WriteLine($"{Name} does not have enough mana to cast {spell.Name}.");
        }
    }

    public override string GetArt()
    {
        return @"
          /\
         ( /   @ @    ()
          \  __| |__  /
           -/   '   \-
          /-|       |-\
         / /-\     /-\ \
         / /-~     ~-\ \
";
    }
}


public class Knight : Mage
{
    public Knight(string name) : base(name, 150, 30)
    {
    }

    public override void Attack(Mage target)
    {
        int damage = 25;
        target.TakeDamage(damage);
        OnAttacked($"{Name} attacked {target.Name} with a sword slash, dealing {damage} damage.");
    }

    public override void Defend(ISpell spell)
    {
        OnDefended($"{Name} defended against {spell.Name}.");
    }

    public override void CastSpell(ISpell spell, Mage target)
    {
        Console.WriteLine($"{Name} cannot cast spells. Attacking instead.");
        Attack(target);
    }

    public override string GetArt()
    {
        return @"
             ,     ,
         (\____/)
          (_oo_)
            (O)
          __||__      \)
       []/______\ [] /
       / \______/ \/
      /    /__\
  (\   /   //
 (^) /___//
";
    }
}

public class Wizard : Mage
{
    public Wizard(string name) : base(name, 80, 100)
    {
    }

    public override void Attack(Mage target)
    {
        int damage = 15;
        target.TakeDamage(damage);
        OnAttacked($"{Name} attacked {target.Name} with a magical bolt, dealing {damage} damage.");
    }

    public override void Defend(ISpell spell)
    {
        int manaGain = 20;
        Mana += manaGain;
        OnDefended($"{Name} absorbed {spell.Name}, gaining {manaGain} mana.");
    }

    public override void CastSpell(ISpell spell, Mage target)
    {
        if (Mana >= spell.ManaCost)
        {
            spell.Cast(this, target);
            Mana -= spell.ManaCost;
            OnSpellCasted($"{Name} cast {spell.Name} on {target.Name}.");
        }
        else
        {
            Console.WriteLine($"{Name} does not have enough mana to cast {spell.Name}.");
        }
    }

    public override string GetArt()
    {
        return @"
      ___
  (  ' v '  )
 -(_)-------(_)- 
  |||   W   |||
   ||| | | |||
    |||||||||
     ||   ||
     ||   ||
";
    }
}


public class Archer : Mage
{
    public Archer(string name) : base(name, 100, 60)
    {
    }

    public override void Attack(Mage target)
    {
        int damage = 18;
        target.TakeDamage(damage);
        OnAttacked($"{Name} attacked {target.Name} with a precise arrow shot, dealing {damage} damage.");
    }

    public override void Defend(ISpell spell)
    {
        OnDefended($"{Name} quickly dodged {spell.Name}.");
    }

    public override void CastSpell(ISpell spell, Mage target)
    {
        Console.WriteLine($"{Name} is not skilled in magic. Attacking instead.");
        Attack(target);
    }

    public override string GetArt()
    {
        return @"
      /\
    ( 0 0 )
   /   V   \
  /  _   _  \
 /  /o   o\  \
 |    (__)    |
  \   ______  /
    |   _  _|
    |__||__|
";
    }
}

public class Necromancer : Mage
{
    public Necromancer(string name) : base(name, 90, 120)
    {
    }

    public override void Attack(Mage target)
    {
        int damage = 25;
        target.TakeDamage(damage);
        OnAttacked($"{Name} attacked {target.Name} with dark energy, dealing {damage} damage.");
    }

    public override void Defend(ISpell spell)
    {
        int healthGain = 30;
        Health += healthGain;
        OnDefended($"{Name} used {spell.Name} to absorb life force, gaining {healthGain} health.");
    }

    public override void CastSpell(ISpell spell, Mage target)
    {
        spell.Cast(this, target);
        OnSpellCasted($"{Name} cast {spell.Name} on {target.Name}.");
    }

    public override string GetArt()
    {
        return @"
       __
     _|  |_
  __|      |__
|_          _|
  |___||___|
    ";
    }
}


public class Battle
{
    private readonly Mage _player;
    private readonly Mage _bot;
    private readonly Random _random = new Random();

    public event BattleEventHandler BattleEvent;

    public Battle(Mage player, Mage bot)
    {
        _player = player;
        _bot = bot;

        _player.Attacked += HandleBattleEvent;
        _player.Defended += HandleBattleEvent;
        _player.SpellCasted += HandleBattleEvent;

        _bot.Attacked += HandleBattleEvent;
        _bot.Defended += HandleBattleEvent;
        _bot.SpellCasted += HandleBattleEvent;
    }

    public void Start()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("****************************");
        Console.WriteLine("       Mage Battle!");
        Console.WriteLine("****************************\n");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"{_player.Name} VS {_bot.Name}\n");
        Console.ResetColor();

        while (_player.IsAlive && _bot.IsAlive)
        {
            Console.Clear();
            Console.WriteLine(_player.GetArt());
            Console.WriteLine(_bot.GetArt());
            Console.WriteLine();

            PlayerTurn();
            if (!_bot.IsAlive) break;
            BotTurn();

            Console.WriteLine();
            _player.DisplayStatus();
            _bot.DisplayStatus();
            Console.WriteLine();

            Thread.Sleep(1000); 
        }

        var winner = _player.IsAlive ? _player.Name : _bot.Name;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Winner: {winner}!");
        Console.ResetColor();
    }

    private void PlayerTurn()
    {
        Console.WriteLine($"{_player.Name}'s turn. Choose an action:");
        Console.WriteLine("1. Attack");
        Console.WriteLine("2. Defend");
        Console.WriteLine("3. Cast Spell");
        Console.Write("Enter choice: ");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                _player.Attack(_bot);
                break;
            case "2":
                _player.Defend(new FireSpell()); 
                break;
            case "3":
                Console.WriteLine("Choose a spell:");
                Console.WriteLine("a. Fireball");
                Console.WriteLine("b. Water Blast");
                Console.Write("Enter spell choice: ");
                var spellChoice = Console.ReadLine();

                ISpell spell;
                switch (spellChoice)
                {
                    case "a":
                        spell = new FireSpell();
                        break;
                    case "b":
                        spell = new WaterSpell();
                        break;
                    default:
                        Console.WriteLine("Invalid spell choice. Skipping turn.");
                        return;
                }

                _player.CastSpell(spell, _bot);
                break;
            default:
                Console.WriteLine("Invalid choice. Skipping turn.");
                break;
        }
    }

    private void BotTurn()
    {
        int choice = _random.Next(1, 4);
        switch (choice)
        {
            case 1:
                _bot.Attack(_player);
                break;
            case 2:
                _bot.Defend(new WaterSpell()); 
                break;
            case 3:
                ISpell botSpell;
                if (_bot is Wizard)
                {
                    botSpell = new FireSpell(); 
                }
                else if (_bot is Necromancer)
                {
                    botSpell = new WaterSpell(); 
                }
                else
                {
                    botSpell = new FireSpell(); 
                }

                _bot.CastSpell(botSpell, _player);
                break;
            default:
                Console.WriteLine("Invalid choice for bot. Skipping turn.");
                break;
        }
    }

    private void HandleBattleEvent(object sender, BattleEventArgs args)
    {
        BattleEvent?.Invoke(sender, args);
    }
}

public class Program
{
    public static void Main()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("****************************");
        Console.WriteLine("     Welcome to the game!");
        Console.WriteLine("****************************\n");
        Console.ResetColor();

        Console.WriteLine("Choose your character:");
        Console.WriteLine("a. Fire Mage");
        Console.WriteLine("b. Water Mage");
        Console.WriteLine("c. Knight");
        Console.WriteLine("d. Wizard");
        Console.WriteLine("e. Archer");
        Console.WriteLine("f. Necromancer");
        Console.Write("Enter character choice: ");
        var choice = Console.ReadLine();

        Mage playerMage;
        switch (choice)
        {
            case "a":
                playerMage = new FireMage("Player");
                break;
            case "b":
                playerMage = new WaterMage("Player");
                break;
            case "c":
                playerMage = new Knight("Player");
                break;
            case "d":
                playerMage = new Wizard("Player");
                break;
            case "e":
                playerMage = new Archer("Player");
                break;
            case "f":
                playerMage = new Necromancer("Player");
                break;
            default:
                Console.WriteLine("Invalid choice. Defaulting to Fire Mage.");
                playerMage = new FireMage("Player");
                break;
        }

        Mage botMage;
        int botChoice = new Random().Next(1, 4);
        switch (botChoice)
        {
            case 1:
                botMage = new FireMage("Bot");
                break;
            case 2:
                botMage = new WaterMage("Bot");
                break;
            case 3:
                botMage = new Wizard("Bot");
                break;
            default:
                botMage = new FireMage("Bot");
                break;
        }

        Battle battle = new Battle(playerMage, botMage);
        battle.BattleEvent += (sender, args) => Console.WriteLine(args.Message);
        battle.Start();
    }
}

