namespace RockPaperScissors.Models
{
    public class FilteredGame
    {
        public string ID { get; set; }

        public string Information { get; set; }

        public FilteredPlayer Player1 { get; internal set; }
        public FilteredPlayer Player2 { get; internal set; }

        public FilteredGame(string gameId, Player player1, Player player2)
        {
            ID = gameId ?? string.Empty;
            Player1 = new FilteredPlayer(player1?.Name, string.Empty);
            Player2 = new FilteredPlayer(player2?.Name, string.Empty);
        }
    }

    public class FilteredPlayer
    {
        public string Name { get; internal set; }
        public string Move { get; internal set; }

        public FilteredPlayer(string name, string move)
        {
            Name = name ?? string.Empty;
            Move = move ?? string.Empty;
        }
    }
}
