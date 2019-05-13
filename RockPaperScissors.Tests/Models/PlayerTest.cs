using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockPaperScissors.Models;

namespace RockPaperScissors.Tests.Models
{
    [TestClass]
    public class PlayerTest
    {
        [TestMethod]
        public void NewPlayer_NameNull() {
            // Arrange
            string name = null;

            // Act
            Player player = new Player(name);

            // Assert
            Assert.AreEqual(string.Empty, player.Name);
            Assert.AreEqual(Move.Unknown, player.Move);
        }

        [TestMethod]
        public void NewPlayer_NameEmpty()
        {
            // Arrange
            string name = string.Empty;

            // Act
            Player player = new Player(name);

            // Assert
            Assert.AreEqual(string.Empty, player.Name);
            Assert.AreEqual(Move.Unknown, player.Move);
        }

        [TestMethod]
        public void NewPlayer_NameValid()
        {
            // Arrange
            string name = "John";

            // Act
            Player player = new Player(name);

            // Assert
            Assert.AreEqual(name, player.Name);
            Assert.AreEqual(Move.Unknown, player.Move);
        }

        [TestMethod]
        public void PlayerHasntPlayed()
        {
            // Arrange
            Player player = new Player("John");

            // Act

            // Assert
            Assert.AreEqual(Move.Unknown, player.Move);
            Assert.IsFalse(player.HasAlreadyPlayed);
        }

        [TestMethod]
        public void PlayerHasAlreadyPlayed()
        {
            // Arrange
            Player player = new Player("John");
            Move move = Move.Rock;

            // Act
            Move originalState = player.Move;
            player.Move = move;

            // Assert
            Assert.AreEqual(Move.Unknown, originalState);
            Assert.AreEqual(move, player.Move);
            Assert.IsTrue(player.HasAlreadyPlayed);
        }
    }
}
