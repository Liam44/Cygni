using RockPaperScissors.Exceptions;
using System;

namespace RockPaperScissors.Models
{
    public class Game
    {
        public string ID { get; set; }

        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public Game(string player1Name)
        {
            ID = Guid.NewGuid().ToString("B").ToUpper();
            Player1 = new Player(player1Name);
        }

        /// <summary>
        /// Filters the information to be shown, according to the game state
        /// </summary>
        /// <returns>
        /// An instance of the FilteredGame class, which Information field indicates:
        /// - if both players have joined the game or not;
        /// - if any of the players has already played or not;
        /// - which player won the game, if both players have made their moves.
        /// The respective Move field from each player are set only when the game is over.
        /// </returns>
        public FilteredGame Filter()
        {
            FilteredGame result = new FilteredGame(ID, Player1, Player2);

            // This case should never happen, but one never knows...
            if (Player1 == null) { throw new PlayerUndefinedException(); }

            if (Player1.Move == Move.Unknown)
            {
                result.Information = $"Waiting for player 1 {Player1.Name} to play.";
            }
            else
            {
                result.Player1.Move = $"{Player1.Name} has already played.";
            }

            if (Player2 == null)
            {
                if (!string.IsNullOrEmpty(result.Information))
                {
                    result.Information += Environment.NewLine;
                }

                result.Information += "Waiting for Player 2 to join the game.";
            }
            else if (Player2.Move == Move.Unknown)
            {
                if (!string.IsNullOrEmpty(result.Information))
                {
                    result.Information += Environment.NewLine;
                }

                result.Information += $"Waiting for player 2 {Player2.Name} to play.";
            }
            else
            {
                result.Player2.Move = $"{Player2.Name} has already played.";
            }

            if (!string.IsNullOrEmpty(result.Information))
            {
                return result;
            }

            result.Player1.Move = Player1.Move.ToString();
            result.Player2.Move = Player2.Move.ToString();

            if (Player1.Move == Player2.Move)
            {
                result.Information = "The players played a drawn game.";

                return result;
            }

            Player winner = null;
            switch (Player1.Move)
            {
                case Move.Paper:
                    if (Player2.Move == Move.Scissors)
                    {
                        winner = Player2;
                    }
                    else
                    {
                        winner = Player1;
                    }
                    break;
                case Move.Rock:
                    if (Player2.Move == Move.Paper)
                    {
                        winner = Player2;
                    }
                    else
                    {
                        winner = Player1;
                    }
                    break;
                default:
                    if (Player2.Move == Move.Rock)
                    {
                        winner = Player2;
                    }
                    else
                    {
                        winner = Player1;
                    }
                    break;
            }

            result.Information = $"{winner.Name} wins the game.";

            return result;
        }
    }
}
