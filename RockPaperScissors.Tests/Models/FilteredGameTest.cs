using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockPaperScissors.Models;

namespace RockPaperScissors.Tests.Models
{
    [TestClass]
    public class FilteredGameTest
    {
        [TestMethod]
        public void NewFilteredGame_GameIdNull_Player1Null_Player2Null()
        {
            // Arrange
            string gameId = null;
            Player player1 = null;
            Player player2 = null;

            // Act
            FilteredGame filteredGame = new FilteredGame(gameId, player1, player2);

            // Assert
            Assert.AreEqual(string.Empty, filteredGame.ID);
            Assert.IsNotNull(filteredGame.Player1);
            Assert.AreEqual(string.Empty, filteredGame.Player1.Name);
            Assert.AreEqual(string.Empty, filteredGame.Player1.Move);
            Assert.IsNotNull(filteredGame.Player2);
            Assert.AreEqual(string.Empty, filteredGame.Player2.Name);
            Assert.AreEqual(string.Empty, filteredGame.Player2.Move);
        }

        [TestMethod]
        public void NewFilteredGame_GameIdEmpty_Player1NotNull_Name1Null_Player2NotNull_Name2Null()
        {
            // Arrange
            string gameId = string.Empty;
            string player1Name = null;
            Player player1 = new Player(player1Name);
            string player2Name = null;
            Player player2 = new Player(player2Name);

            // Act
            FilteredGame filteredGame = new FilteredGame(gameId, player1, player2);

            // Assert
            Assert.AreEqual(string.Empty, filteredGame.ID);
            Assert.IsNotNull(filteredGame.Player1);
            Assert.AreEqual(string.Empty, filteredGame.Player1.Name);
            Assert.AreEqual(string.Empty, filteredGame.Player1.Move);
            Assert.IsNotNull(filteredGame.Player2);
            Assert.AreEqual(string.Empty, filteredGame.Player2.Name);
            Assert.AreEqual(string.Empty, filteredGame.Player2.Move);
        }

        [TestMethod]
        public void NewFilteredGame_GameIdNotEmpty_Player1NotNull_Name1Empty_Player2NotNull_Name2Empty()
        {
            // Arrange
            string gameId = "abcd";
            string player1Name = string.Empty;
            Player player1 = new Player(player1Name);
            string player2Name = string.Empty;
            Player player2 = new Player(player2Name);

            // Act
            FilteredGame filteredGame = new FilteredGame(gameId, player1, player2);

            // Assert
            Assert.AreEqual(gameId, filteredGame.ID);
            Assert.IsNotNull(filteredGame.Player1);
            Assert.AreEqual(string.Empty, filteredGame.Player1.Name);
            Assert.AreEqual(string.Empty, filteredGame.Player1.Move);
            Assert.IsNotNull(filteredGame.Player2);
            Assert.AreEqual(string.Empty, filteredGame.Player2.Name);
            Assert.AreEqual(string.Empty, filteredGame.Player2.Move);
        }

        [TestMethod]
        public void NewFilteredGame_GameIdNotEmpty_Player1NotNull_Name1NotEmpty_Player2NotNull_Name2NotEmpty()
        {
            // Arrange
            string gameId = "abcd";
            string player1Name = "John";
            Player player1 = new Player(player1Name);
            string player2Name = "Jack";
            Player player2 = new Player(player2Name);

            // Act
            FilteredGame filteredGame = new FilteredGame(gameId, player1, player2);

            // Assert
            Assert.AreEqual(gameId, filteredGame.ID);
            Assert.IsNotNull(filteredGame.Player1);
            Assert.AreEqual(player1Name, filteredGame.Player1.Name);
            Assert.AreEqual(string.Empty, filteredGame.Player1.Move);
            Assert.IsNotNull(filteredGame.Player2);
            Assert.AreEqual(player2Name, filteredGame.Player2.Name);
            Assert.AreEqual(string.Empty, filteredGame.Player2.Move);
        }
    }
}
