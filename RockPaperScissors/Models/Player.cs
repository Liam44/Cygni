namespace RockPaperScissors.Models
{
    public class Player
    {
        public string Name { get; set; }
        public Move Move { get; set; }

        public Player(string name)
        {
            Name = name ?? string.Empty;
            Move = Move.Unknown;
        }

        public bool HasAlreadyPlayed
        {
            get
            {
                return Move != Move.Unknown;
            }
            private set { }
        }
    }
}
