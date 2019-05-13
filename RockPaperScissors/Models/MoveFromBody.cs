namespace RockPaperScissors.Models
{
    public class MoveFromBody
    {
        public string Name { get; set; }
        public string Move { get; set; }

        public MoveFromBody(string name, string move)
        {
            Name = name;
            Move = move;
        }
    }
}