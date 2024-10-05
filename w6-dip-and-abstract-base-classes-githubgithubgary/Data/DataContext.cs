using System.Text.Json;
using static Crayon.Output;
using System;
using System.Runtime.InteropServices;
using W6_assignment_template.Models.Characters;

namespace W6_assignment_template.Data
{
    public class DataContext : IContext
    {
        public List<CharacterBase> Characters { get; set; }  // Generalized to store all character types

        private readonly JsonSerializerOptions options;

        public DataContext()
        {
            options = new JsonSerializerOptions
            {
                Converters = { new CharacterBaseConverter() },
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            LoadData();
        }

        private void LoadData()
        {
            var jsonData = File.ReadAllText("Files/input.json");
            Characters = JsonSerializer.Deserialize<List<CharacterBase>>(jsonData, options); // Load all character types
        }

        public void AddCharacter(CharacterBase character)
        {
            Characters.Add(character);
            SaveData();
        }

        public void UpdateCharacter(CharacterBase character)
        {
            var existingCharacter = Characters.FirstOrDefault(c => c.Name == character.Name);
            if (existingCharacter != null)
            {
                existingCharacter.Level = character.Level;
                existingCharacter.HP = character.HP;

                if (existingCharacter is Player player && character is Player updatedPlayer)
                {
                    player.Gold = updatedPlayer.Gold;  // Specific to Player
                }
                if (existingCharacter is Goblin goblin && character is Goblin updatedGoblin)
                {
                    goblin.Treasure = updatedGoblin.Treasure;  // Specific to Goblin
                }

                SaveData();
            }
        }

        public void DeleteCharacter(string characterName)
        {
            var character = Characters.FirstOrDefault(c => c.Name == characterName);
            if (character != null)
            {
                Characters.Remove(character);
                SaveData();
            }
        }

        public List<CharacterBase> FindCharacter(string searchFor)
        {
            List<CharacterBase> foundCharacters = null;

            foundCharacters = Characters.Where(c => c.Name.Contains(searchFor, StringComparison.InvariantCultureIgnoreCase)).ToList();

            List<CharacterBase> pcList = new List<CharacterBase>();  // Use this to build a list of match

            foreach (CharacterBase p in foundCharacters)
            {
                pcList.Add(p);  // Print out each of the characters
            }
            return pcList;
        }
        public List<string> ListCharacters([Optional] List<CharacterBase> characters)
        {
            int? i = 0;
            List<string> list = new List<string>();

            if (characters == null)
                Characters = Characters.ToList();

            foreach (CharacterBase pc in characters)
            {
                if (pc == null) continue;
                i = i + 1;
                list.Add(($"Player {Red("#")}{Bold().Red().Text(i.ToString())}: {pc.Print()}."));
            }
            return list;
        }
        private void SaveData()
        {
            var jsonData = JsonSerializer.Serialize(Characters, options);
            File.WriteAllText("Files/input.json", jsonData);
        }
    }
}
