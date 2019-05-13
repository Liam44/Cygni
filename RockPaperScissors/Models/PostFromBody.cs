namespace RockPaperScissors.Models
{
    public class PostFromBody
    {
        public string Name { get; set; }

        public PostFromBody(string name)
        {
            Name = name;
        }
    }
}