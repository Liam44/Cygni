namespace RockPaperScissors.Models
{
    public class JoinFromBody
    {
        public string Name { get; set; }

        public JoinFromBody(string name)
        {
            Name = name;
        }
    }
}