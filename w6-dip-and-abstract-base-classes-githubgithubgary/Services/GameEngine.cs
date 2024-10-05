using static Crayon.Output;
using W6_assignment_template.Data;
using System.Runtime.InteropServices;
using W6_assignment_template.Models.Characters;

namespace W6_assignment_template.Services
{
    public class GameEngine
    {
        private readonly IContext _context;
        private readonly Player _player;
        private readonly Goblin _goblin;
        private readonly Ghost _ghost;

        public GameEngine(IContext context)
        {
            _context = context;
            _player = context.Characters.OfType<Player>().FirstOrDefault();
            _goblin = _context.Characters.OfType<Goblin>().FirstOrDefault();
            _ghost = _context.Characters.OfType<Ghost>().FirstOrDefault();
        }

        public void Run()
        {
            if (_player == null || _goblin == null)
            {
                Console.WriteLine("Failed to initialize game characters.");
                return;
            }

            bool EndProg = false;
            Console.WriteLine(Bright.Blue().Text("Welcome to Character Management\r\n"));

            while ((EndProg == false) && (_context.Characters != null))
            {
                Console.WriteLine("     " + Bright.Blue().Underline().Text("Main Menu:\r\n"));
                Console.WriteLine($"1. Display Characters");
                Console.WriteLine($"2. Add Character");
                Console.WriteLine($"3. Level Up Character");
                Console.WriteLine($"4. Find Character(s)");
                Console.WriteLine($"5. Character Actions");
                Console.WriteLine($"0. Exit");
                Console.Write($"\r\nEnter your choice: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListCampaign();
                        break;
                    case "2":
                        AddCharacters();
                        break;
                    case "3":
                        LevelUpCharacters();
                        break;
                    case "4":
                        FindCharacters();
                        break;
                    case "5":
                        CharacterActions();
                        break;
                    case "0":
                        EndProg = ExitProgram();
                        break;
                    default:
                        Console.WriteLine(Bright.Red("Invalid choice. Please try again."));
                        break;
                }
                Console.Clear();
            }
        }

        public bool ConfirmYN()
        {
            bool ans = false;

            while (ans == false)
            {
                Console.Write($"{Green("Yes (Y)")} or {Red("No (N)")}? ");
                var response = char.ToUpper(Convert.ToChar(Console.Read()));
                switch (response)
                {
                    case 'Y':
                        ans = true;
                        break;

                    case 'N':
                        ans = true;
                        break;

                    default:
                        Console.WriteLine($"You can only answer with {Green("Yes (Y)")} or {Red("No (N)")}.");
                        Console.Clear();
                        continue;
                }
            }
            return ans;
        }
        public void ListCampaign()
        {
            string? output = null;
            Console.Clear();
            Console.WriteLine($"The campaign includes the following player character(s):\r\n");

            if (_context.Characters.Count() == 0)
            {
                Console.WriteLine($"There are {Bright.Red().Text("no")} player character(s) in the current campaign.");
            }
            else
            {
                //ListCharacters(_context.Characters);
                List<string> list = _context.ListCharacters();
                Console.WriteLine(list);
            }
            Console.Write($"\r\n\r\nPress {Yellow("<Enter>")} key when you are ready to continue...");
            var input = Console.ReadLine();
        }

        public void ListCharacters(List<CharacterBase> characters)
        {
            int? i = 0;
            string? n = null;
            int? l = null;
            string? t = null;
            string? e = null;
            string? text = null;

            foreach (CharacterBase pc in characters)
            {
                if (pc == null) continue;
                i = i + 1;
                Console.WriteLine($"Player {Red("#")}{Bold().Red().Text(i.ToString())}: {pc.Print()}.");
            }
        }
        public void FindCharacters()
        {
            //###################################################################
            // stackoverflow.com/questions/45239889/linq-case-insensitive
            // var foundCharacter = _campaignList.Where(c => c.Name == "Bob").ToList();
            //###################################################################

            while (true)
            {
                Console.Clear();
                Console.Write($"Which infamous hero or heroine are you in search of my leige (Enter {Yellow("<Done>")} to exit)? ");
                string input = Console.ReadLine();
                if (input.Length == 0)
                {
                    Console.WriteLine("I don't believe I heard you correctly...");
                    Thread.Sleep(2000);
                }
                else
                {
                    if (input.ToLower() == "done")
                    {
                        Console.Clear();
                        Console.WriteLine("Safe journey my leige, may the Great One watch over you.");
                        break;
                    }
                    else
                    {
                        List<CharacterBase> pc = _context.FindCharacter(input);
                        if (pc.Count() > 0)
                        {
                            Console.Clear();
                            Console.WriteLine($"The gods have shown you favor and granted your request! The ancient scrolls have revealed the following name(s)...\r\n");
                            ListCharacters(pc);
                            Console.Write($"\r\n\r\nPress {Yellow("<Enter>")} key when you are ready to continue...");
                            input = Console.ReadLine();
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine($"I do regret my leige that I did not find a reference in the archives that match your inquiry. Perhaps \r\nif you might grace the coffers of the temple with a small donation ({Green("Cha Ching")}), the Great One might \r\n show favor for your next request!");
                            Console.Write($"\r\nShall I search again for you, my liege (Enter {Yellow("<Done>")} to exit)? ");
                            input = Console.ReadLine();
                            if (input.ToLower() == "done")
                            {
                                Console.Clear();
                                Console.Write("Safe journey my leige. Please do remember to stop by our gift shop for bit of ale and something to remember your visit!");
                                Thread.Sleep(5000);
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
            }
        }
        public void LevelUpCharacter(CharacterBase tmpPlayerCharacter)
        {
            int nextLevel = 0;
            int levels = 0;
            bool success = false;
            string? n = tmpPlayerCharacter.Name;
            int l = tmpPlayerCharacter.Level;
            string? cn = tmpPlayerCharacter.Type;
            string? text = null;
            string? e = null;
            switch (tmpPlayerCharacter.GetType().ToString())
            {
                case "W6_assignment_template.Models.Player":
                    Player player = (Player)tmpPlayerCharacter;
                    e = player.PrintEquipment(",");
                    text = "equipment";
                    break;
                case "W6_assignment_template.Models.Ghost":
                    Ghost ghost = (Ghost)tmpPlayerCharacter;
                    e = ghost.PrintTreasure(",");
                    text = "treasure";
                    break;
                case "W6_assignment_template.Models.Goblin":
                    Goblin goblin = (Goblin)tmpPlayerCharacter;
                    e = goblin.PrintTreasure(",");
                    text = "treasure";
                    break;
            }

            Console.Clear();
            int index = 1;
            while (true)
            {
                Console.WriteLine($"Player #{index}: {Bold().Magenta().Text(n)} the {Bold().Cyan().Text(cn)} is at level {Bold().Rgb(255, 165, 0).Text(l.ToString())} with the following {text}: {Bold().Green().Text(e)}.");

                nextLevel = 0;
                Console.Write($"\r\nHow many levels has {Bold().Magenta().Text(n)} earned? ");
                var input = Console.ReadLine();
                if (input == null)
                {
                    Console.WriteLine($"You must enter how many levels has {Bold().Magenta().Text(n)} earned? ");
                    continue;
                }
                else
                {
                    success = int.TryParse(input, out levels);
                    if (success == false)
                    {
                        Console.WriteLine(Bright.Red("Invalid choice. Please try again."));
                    }
                }

                if ((success) && (levels >= 1))
                {
                    nextLevel = levels + l;

                    Console.WriteLine($"\r\nIs {Bold().Magenta().Text(n)} the {Bold().Cyan().Text(cn)} ready to level up from level {Bold().Rgb(255, 165, 0).Text(l.ToString())} to level {Bold().Rgb(255, 165, 0).Text(nextLevel.ToString())}?");
                    bool UpdCharacter = ConfirmYN();
                    Console.Clear();
                    if (UpdCharacter)
                    {
                        Console.WriteLine($"The Dungeon Master has confirmed that {Bold().Magenta().Text(n)} the {Bold().Cyan().Text(cn)} is ready for the responsibility that comes with level {nextLevel} power.");
                        tmpPlayerCharacter.Level = nextLevel;
                    }
                    else
                    {
                        Console.WriteLine($"The Dungeon Master has confirmed that {Bold().Magenta().Text(n)} the {Bold().Cyan().Text(cn)} is not ready for such great power.");
                    }
                    break;
                }
                else
                {
                    Console.WriteLine($"We were unable to move {Bold().Magenta().Text(n)} the {Bold().Cyan().Text(cn)} to the next level, please try again.");
                }
            }
        }
        public void LevelUpCharacters()
        {
            string? input = null;
            bool endInput = false;

            while (endInput != true)
            {
                Console.Clear();
                ListCampaign();
                if (_context.Characters.Count() > 0)
                {
                    Console.WriteLine("\r\n\r\nEnter a player number to level up.");
                    input = Console.ReadLine();
                    if (input == null)
                    {
                        Console.WriteLine($"You must select a player number or enter {Yellow("0 (zero)")} to exit.");
                        endInput = false;
                    }
                    else
                    {
                        int choice = -1;
                        bool success = Int32.TryParse(input, out choice);
                        if (success == true)
                        {
                            if (string.Compare((choice - 1).ToString(), (_context.Characters.Count() - 1).ToString()) > 0)
                            {
                                Console.WriteLine($"Player number {Bright.Red().Text(input)} is not valid, please select another player number.");
                                endInput = false;
                            }
                            else
                            {
                                switch (input)
                                {
                                    case "0":
                                        endInput = true;
                                        break;
                                    default:
                                        int idx = -1;
                                        if (int.TryParse(input, out idx))
                                        {
                                            LevelUpCharacter(_context.Characters[idx - 1]);
                                            endInput = true;
                                            break;
                                        }
                                        else
                                        {
                                            continue; // This has to be here otherwise the complier complains with an error.
                                        }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"You must select a player number or enter {Yellow("0 (zero)")} to exit.");
                            endInput = false;
                        }
                    }
                }
                else
                {
                    // Message is displayed by the ListCharacters method so no need for one here
                    endInput = true;
                    break;
                }
            }
        }
        private bool GetEquipment(Player character)
        {
            bool endInput = false;
            string? equipname = null;
            List<string> equipmentList = new List<string>();
            string? equipment = null;

            while (endInput == false)
            {
                Console.Write($"\r\nEnter your character's equipment (Type {Yellow("<Done>")} when finished or {Yellow("<End>")} to exit): ");
                equipname = Console.ReadLine();
                if (equipname == null)
                {
                    Console.Write($"Type {Yellow("<Done>")} when finished entering equipment or {Yellow("<End>")} to exit: ");
                    endInput = false;
                }
                else
                {
                    if (equipname.ToLower() == "done")
                    {
                        var x = equipmentList.ToString();
                        equipment = string.Join("|", equipmentList);
                        character.Equipment = equipment;
                        endInput = true;
                    }
                    else
                    {
                        if (equipname.ToLower() == "end")
                        {
                            equipmentList.Clear();
                            endInput = true;
                        }
                        else
                        {
                            equipmentList.Add(equipname);
                        }
                    }
                }
            }
            if (equipmentList.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void AddCharacters()
        {
            bool endInput = false;
            bool continueInput = false;
            string? answer = null;

            CharacterBase character = null;
            List<string> questions = new List<string>() { "name", "class", "level", "HP" }; // Default for most of the character types
            List<string> answers = null;
            int count = 0;
            int max_count = questions.Count();
            bool first_time = true;
            string? characterType = null;

            while (endInput == false)
            {
                Console.Clear();
                Console.Write($"\r\nThe 'Great One' has provided you with these mighty character types to choose from:\r\n\r\n");

                Console.WriteLine("     " + Bright.Blue().Underline().Text("Character Type Menu:\r\n"));
                Console.WriteLine($"1. Player (Humaniod)");
                Console.WriteLine($"2. Ghost");
                Console.WriteLine($"3. Goblin");
                Console.WriteLine($"0. Exit");
                Console.Write($"\r\nEnter your choice: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        characterType = "Player";
                        questions = new List<string>() { "name", "class", "level", "HP" , "Gold"};  // Provide a new list of questions for this type
                        endInput = true;
                        break;
                    case "2":
                        characterType = "Ghost";
                        questions = new List<string>() { "name", "class", "level", "HP" , "Treasure"};
                        endInput = true;
                        break;
                    case "3":
                        characterType = "Goblin";
                        questions = new List<string>() { "name", "class", "level", "HP" , "Treasure"};
                        endInput = true;
                        break;
                    case "0":
                        characterType = null;
                        endInput = true;
                        break;
                    default:
                        Console.WriteLine(Bright.Red("Invalid choice. Please try again."));
                        endInput = false;
                        break;
                }
            }

            //This means we have questions and someone chose a type of character to add
            if ((max_count > 0) && (characterType != null))
            {
                endInput = false;
                continueInput = true;
                Console.Clear();
                while (endInput == false)
                {
                    Console.Write($"\r\nYou must enter a character's {questions[count]} (Type {Yellow("<End>")} to exit): ");
                    answer = Console.ReadLine();
                    if (answer.Length == 0)
                    {
                        Console.Write($"\r\nYou must enter a character's {questions[count]} (Type {Yellow("<End>")} to exit): ");
                        endInput = false;
                    }
                    else
                    {
                        if (answer.ToLower() == "end")
                        {
                            endInput = false;
                            continueInput = false;
                        }
                        else
                        {
                            if (first_time == true)
                            {
                                answers = new List<string>();
                                first_time = false;
                            }
                            answers.Add(answer);
                            count += 1;
                            if (count >= max_count)
                            {
                                endInput = true;
                                continueInput = true;
                            }
                        }
                    }
                }

                if (continueInput == true)
                {
                    string? stuff = null;
                    endInput = false;
                    // Ask for more equipment until the user indicates they are done
                    switch (characterType)
                    {
                        case "Player":
                            //TODO - Get Gold here
                            int gold = 50;
                            Player pc  = new Player(answers[0], answers[1], Int32.Parse(answers[2]), Int32.Parse(answers[3]), gold);
                            endInput = GetEquipment(pc);
                            stuff = pc.PrintEquipment(",");
                            stuff = stuff + "," + gold.ToString() + " gold";
                            character = pc;
                            break;
                        case "Goblin":
                            Goblin gbc = new Goblin(answers[0], answers[1], Int32.Parse(answers[2]), Int32.Parse(answers[3]), answers[4]);
                            stuff = answers[4];
                            character = gbc;
                            break;
                        case "Ghost":
                            Ghost ghc = new Ghost(answers[0], answers[1], Int32.Parse(answers[2]), Int32.Parse(answers[3]), answers[4]);
                            stuff = answers[4];
                            character = ghc;
                            break;
                        default:
                            character = null;
                            break;
                    }

                    if (character != null)
                    {
                        string n = character.Name;
                        int l = character.Level;
                        string t = character.Type;
                        string? str = null;

                        Console.Clear();
                        Console.WriteLine($"Add, {Bold().Magenta().Text(n)} the {Bold().Cyan().Text(t)} at level {Bold().Rgb(255, 165, 0).Text(l.ToString())} with the following stuff: {Bold().Green().Text(stuff)}.");
                        bool AddCharacter = ConfirmYN();
                        if (AddCharacter)
                        {
                            Console.Clear();
                            str = string.Format($"Welcome, {Bold().Magenta().Text(n)} the {Bold().Cyan().Text(t)}! Congradulations on achieving level {Bold().Rgb(255, 165, 0).Text(l.ToString())}. Your stuff includes: {Bold().Green().Text(stuff)}.");
                            _context.AddCharacter(character);
                        }
                        else
                        {
                            str = string.Format($"Sorry {Bold().Magenta().Text(n)} the {Bold().Cyan().Text(t)} will not be joining of the campaign!");
                        }
                        Console.WriteLine(str);
                    }
                }
            }
        }
        public void CharacterActions()
        {
            Console.Clear();
            _goblin.Move();
            _goblin.Attack(_player);

            _player.Move();
            _player.Attack(_goblin);

            _ghost.Move();
            _ghost.Attack(_player);

            Console.Write($"\r\n\r\nPress {Yellow("<Enter>")} key when you are ready to continue...");
            string input = Console.ReadLine();
        }
        public bool ExitProgram()
        {
            //TODO - Check if changes, ask, exit or not
            Console.Clear();
            Console.WriteLine($"Exiting...");
            Thread.Sleep(1000);

            Console.Write($"\r\n\r\n{Yellow("Goodbye!")}");
            Thread.Sleep(1000);
            return true;
        }
    }
}
