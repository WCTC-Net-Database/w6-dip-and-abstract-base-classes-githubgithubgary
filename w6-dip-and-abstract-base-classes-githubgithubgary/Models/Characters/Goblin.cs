using W6_assignment_template.Interfaces;
using static Crayon.Output;

namespace W6_assignment_template.Models.Characters
{
    public class Goblin : CharacterBase, ILootable
    {
        //        public string Treasure { get; set; }
        public string Treasure { get; set; }
        public Goblin(string name, string type, int level, int hp, string treasure)
            : base(name, type, level, hp)
        {
            if (treasure != null)
            {
                Treasure = treasure;
            }
        }

        public override void UniqueBehavior()
        {
            throw new NotImplementedException();
        }
        public string PrintTreasure()
        {
            return $"{Treasure}";
        }
        public string PrintTreasure(string? delimiter)
        {
            string? treasure = null;
            if (Treasure != null)
            {
                treasure = Treasure.Replace("|", delimiter);
            }
            else
            {
                treasure = string.Empty;
            }
            return $"{treasure}";
        }
        public override string Print()
        {
            string text = "Treasure";
            string text2 = PrintTreasure(",");
            string msg = $"{Bold().Magenta().Text(Name)} the {Bold().Cyan().Text(Type)} is at level {Bold().Rgb(255, 165, 0).Text(Level.ToString())} with {HP} hitpoints and the the following {text}: {Bold().Green().Text(text2)}";
            return msg;
        }
    }
}
