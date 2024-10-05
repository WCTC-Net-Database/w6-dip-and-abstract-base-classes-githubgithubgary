namespace W6_assignment_template.Interfaces
{
    public interface ICharacter
    {
        string Name { get; set; }
        void Attack(ICharacter target);
        void Move();
        string Print();
    }

}
