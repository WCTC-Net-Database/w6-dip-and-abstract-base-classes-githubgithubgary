using Microsoft.VisualBasic;
using static Crayon.Output;


namespace W6_assignment_template.Models.Characters
{
    public class Player : CharacterBase
    {
        public int Gold { get; set; }
        public string? Equipment { get; set; }
        public Player(string name, string type, int level, int hp, int gold)
            : base(name, type, level, hp)
        {
            Gold = gold;
            Equipment = "None";
        }

        public override void UniqueBehavior()
        {
            throw new NotImplementedException();
        }
        public string PrintEquipment()
        {
            return $"{Equipment}";
        }
        public string PrintEquipment(string? delimiter)
        {
            string? equipment = null;
            if (Equipment != null)
            {
                equipment = Equipment.Replace("|", delimiter);
            }
            else
            {
                equipment = string.Empty;
            }
            return $"{equipment}";
        }
        public override string Print()
        {
            string text = "Equipment";
            string text2 = Gold.ToString() + ", " + PrintEquipment(",");
            string msg = $"{Bold().Magenta().Text(Name)} the {Bold().Cyan().Text(Type)} is at level {Bold().Rgb(255, 165, 0).Text(Level.ToString())} with {HP} hitpoints and the the following gold and {text} : {Bold().Green().Text(text2)}";
            return msg;
        }
    }
}
