using System;
using System.Runtime.InteropServices;
using W6_assignment_template.Models.Characters;

namespace W6_assignment_template.Data
{
    public interface IContext
    {
        List<CharacterBase> Characters { get; set; }

        void AddCharacter(CharacterBase character);

        void UpdateCharacter(CharacterBase character);

        void DeleteCharacter(string characterName);

        List<CharacterBase> FindCharacter(string searchFor);

        List<string> ListCharacters([Optional] List<CharacterBase> characters);
    }
}
