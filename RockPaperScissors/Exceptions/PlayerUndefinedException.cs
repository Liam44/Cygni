using System;

namespace RockPaperScissors.Exceptions
{
    public class PlayerUndefinedException : Exception
    {
        public PlayerUndefinedException() : base("Player 1 is undefined.")
        {
        }
    }
}
