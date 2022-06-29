

using System.Reflection;
using System.Text;



Game newGame = new Game();

newGame.Start();


class Hero
{
    public string Name { get; set; }

    public int BaseStrength { get; set; }

    public int BaseDefense { get; set; }
    public int OriginalHealth { get; set; }
    public int CurrentHealth { get; set; }

    public Weapon EquippedWeapon { get; set; }

    public Armour EquippedArmour { get; set; } 

    public void ShowStats()
    {
        Console.WriteLine($"Name: {Name} \nBaseStrength: {BaseStrength} \nBaseDefense: {BaseDefense} \nOriginalHealth: {OriginalHealth} \nCurrentHealth: {CurrentHealth}");

    }

    public void ShowInventory()
    {
        Console.WriteLine($"Weapon: {EquippedWeapon} \n Armour: {EquippedArmour}");
    }

    public void EquipWeapon(Weapon weapon)
    {
        EquippedWeapon = weapon;
    }
    public void EquipArmour(Armour armour)
    {
        EquippedArmour = armour;
    }
    public Hero(string name, int baseStrength, int baseDefense, int originalHealth)
    {
        Name = name;    
        BaseStrength = baseStrength;
        BaseDefense = baseDefense;
        OriginalHealth = originalHealth;
        CurrentHealth = originalHealth;
    }
}

class Monster
{
    public string Name { get; set; }
    public int Defense { get; set; }
    public int Strength { get; set; }
    public int OriginalHealth { get; set; }
    public int CurrentHealth { get; set; } 

    public Monster(string name, int defense, int originalHealth, int strength)
    {
        Name = name;
        Defense = defense; 
        OriginalHealth=originalHealth;
        CurrentHealth = originalHealth;
        Strength = strength;
    }   

}

class Weapon
{
    public string Name { get; set; }
    public int Power { get; set; }
    public Weapon(string name, int power)
    {
        Name = name;
        Power = power;
    }
}

class Armour
{
    public string Name { get; set; }
    public int Power { get; set; }

    public Armour(string name, int power)
    {
        Name = name;
        Power = power;
    }
}

static class WeaponList
{
    public static Weapon spear = new Weapon("spear", 2);
    public static Weapon sword = new Weapon("sword", 2);
    public static Weapon hammer = new Weapon("thor hammer", 2);
    public static Weapon LightSaber = new Weapon("lightsaber", 2);
    public static Weapon Axe = new Weapon("axe", 2);

    public static List<Weapon> getAll()// This method is used to get all the weapons in the weapon list and returns a list of weapons to be used in the Game class
    {
        List<Weapon> list = new List<Weapon>();
        foreach (FieldInfo field in typeof(WeaponList).GetFields())
        {
            list.Add((Weapon)field.GetValue(null));
        }
        return list;
    }
}

static class ArmourList
{
    public static Armour bulletVest = new Armour("bullet vest", 2);
    public static Armour infinityStone = new Armour("infinity stone", 2);
    public static Armour magneticGlove = new Armour("magnetic glove", 2);
    public static List<Armour> getAll()// This method is used to get all the armour in the armour list and returns a list of armour to be used in the Game class
    { 
        List<Armour> list = new List<Armour>();
        foreach(FieldInfo field in typeof(ArmourList).GetFields())
        {
            list.Add((Armour)field.GetValue(null));
        }
        return list;
    }

}



class Fight
{
    public Hero Hero { get; set; }
    public Monster Monster { get; set; }

    public Fight(Hero hero, Monster monster)
    {
        Hero = hero;
        Monster = monster;
    }

    public void Begin()
    {
        Console.WriteLine($"Beginning fight between {Hero.Name} and {Monster.Name}...");

        while (!Win() && !Lose())
        {
            int DamageDoneByHero = HeroTurn();
            Monster.CurrentHealth -= DamageDoneByHero;
            Console.WriteLine($"Hero hit: monster health - {Monster.CurrentHealth}");

            int DamageDoneByMonster = MonsterTurn();
            Hero.CurrentHealth -= DamageDoneByMonster;
            Console.WriteLine($"Monster hit: hero health - {Hero.CurrentHealth}");
        }
    }

    public int HeroTurn()
    {
        return Hero.BaseStrength + Hero.EquippedWeapon.Power;
    }

    public int MonsterTurn()
    {
        return Monster.Strength - (Hero.BaseDefense + Hero.EquippedArmour.Power);
    }

    public bool Win()// This method is used to determine when the monster health is finally at zero to credit the hero with a win
    {
        return Monster.CurrentHealth <= 0;
    }

    public bool Lose()// This method is used to determine when the Hero's health is at zero to credit the hero with a loss.
    {
        return Hero.CurrentHealth <= 0;
    }
    
}

class Game
{
    public int GamesPlayed { get; set; } = 0;
    public int FightsWon { get; set; } = 0;

    public int FightsLost { get; set; } = 0;

    public Hero HeroInstance { get; set; } 
    public List<Monster> Monsters { get; set; } = new List<Monster>();

    public Game()
    {
        HeroInstance = new Hero("Yakari",2 , 5, 100);

        Monsters.Add(new Monster("Thanos", 2, 100, 10));
        Monsters.Add(new Monster("Scarlet Witch", 1, 100, 5 ));
        Monsters.Add(new Monster("Papyrus", 2, 100, 10));
        Monsters.Add(new Monster("Zoom", 2, 100, 5));
        Monsters.Add(new Monster("Darken Rahl",1, 100, 10));
    }

    public void MainMenu() //To display the menu options in the game.
    {
        Console.WriteLine($"a. Display Statistics\nb. Display Inventory\nc. Fight");
        switch (Console.ReadLine())
        {
            case "a":
                DisplayStatistics();
                break;
            case "b":
                DisplayInventory();
                break;
            case "c":
                Fight();
                break;
            default:
                Console.WriteLine("You entered an invalid option.");
                MainMenu();
                break;
        }
    }

    public void ChangeEquippedWeapon()// This method creates a list of weapons using a stringBuilder and a weapon can be selected or changed based on the number input
    {
        List<Weapon> weaponList = WeaponList.getAll();
        StringBuilder instruction = new StringBuilder();
        for (int i = 0; i < weaponList.Count; i++)
        {
            instruction.Append($"{(i + 1).ToString()}. {weaponList[i].Name.ToUpper()} \n");
        }
        Console.WriteLine("Choose your new weapon: ");
        Console.WriteLine(instruction.ToString());
        String choice = Console.ReadLine();
        try
        {
            int choiceInt = Int32.Parse(choice);
            if (choiceInt > weaponList.Count)
            {
                Console.WriteLine("You entered an invalid option");
                ChangeEquippedWeapon();
            }
            else
            {
                Weapon chosenWeapon = (weaponList[choiceInt - 1]);
                HeroInstance.EquipWeapon(chosenWeapon);
                Console.WriteLine($"Your new equipped weapon is: {chosenWeapon.Name}.");
                DisplayInventory();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("You entered an invalid option");
            ChangeEquippedWeapon();
        }
    }

    public void ChangeEquippedArmour() // This method creates a list of armours using a stringBuilder and an armour can be selected or changed based on the number input.
    {
        List<Armour> armourList = ArmourList.getAll();
        StringBuilder instruction = new StringBuilder();
        for (int i = 0; i < armourList.Count; i++)
        {
            instruction.Append($"{(i + 1).ToString()}. {armourList[i].Name.ToUpper()} \n");
        }
        Console.WriteLine("Choose your new armour: ");
        Console.WriteLine(instruction.ToString());
        String choice = Console.ReadLine();
        try
        {
            int choiceInt = Int32.Parse(choice);
            if (choiceInt > armourList.Count)
            {
                Console.WriteLine("You entered an invalid option");
                ChangeEquippedArmour();
            }
            else
            {
                Armour chosenArmour = (armourList[choiceInt - 1]);
                HeroInstance.EquipArmour(chosenArmour);
                Console.WriteLine($"Your new equipped armour is: {chosenArmour.Name}.");
                DisplayInventory();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("You entered an invalid option");
            ChangeEquippedArmour();
        }
    }


    public void DisplayInventory()
    {
        Console.WriteLine($"a. Change EquipedWeapon \nb. Change EquippedArmour\nc. Main Menu\n");
        switch (Console.ReadLine())
        {
            case "a":
                ChangeEquippedWeapon();
                break;
            case "b":
                ChangeEquippedArmour();
                break;
            case "c":
                MainMenu();
                break;
            default :
                Console.WriteLine("You entered an invalid option");
                DisplayInventory();
                break;
        }
    }

    public void DisplayStatistics()
    {
        Console.WriteLine($"Games Played: {GamesPlayed} \nFights Won: {FightsWon} \nFights Lost: {FightsLost}\n");
        MainMenu();
    }

    public void Fight()
    {
        if (HeroInstance.EquippedWeapon == null)
        {
            Console.WriteLine("Please equip your hero with a weapon.");
            ChangeEquippedWeapon();
            return;
        }

        if (HeroInstance.EquippedArmour == null)
        {
            Console.WriteLine("Please equip your hero with an armour.");
            ChangeEquippedArmour();
            return;
        }

        if (Monsters.Count == 0)
        {
            Console.WriteLine("There are no more monsters left to fight.");
            MainMenu();
            return;
        }

        Random random = new Random();
        int randomIndex = random.Next(Monsters.Count);
        Monster currentMonster = Monsters[randomIndex];
        Fight fight = new Fight(HeroInstance, currentMonster);
        fight.Begin();
        GamesPlayed++;
        if (fight.Win())
        {
            FightsWon++;
            Monsters.Remove(currentMonster);
            Console.WriteLine($"You won the fight against {currentMonster.Name}");
        }
        else if (fight.Lose())
        {
            FightsLost++;
            Console.WriteLine($"You lost the fight against {currentMonster.Name}");
        }
        MainMenu();
    }


    public void Start()
    {
        Console.WriteLine("Player name");
        string PlayerName = Console.ReadLine();
        MainMenu();
    }
}