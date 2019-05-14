using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockPaperScissors.Exceptions;
using RockPaperScissors.Models;
using System;

namespace RockPaperScissors.Tests.Models
{
    /// <summary>
    /// Summary description for GameTest
    /// </summary>
    [TestClass]
    public class GameTest
    {
        #region Constructor

        [TestMethod]
        public void NewGame_PlayerNameNull()
        {
            // Arrange
            string playerName = null;

            // Act
            Game game = new Game(playerName);

            // Assert
            Assert.IsNotNull(game.ID);
            Assert.AreNotEqual(string.Empty, game.ID);
            Assert.IsNotNull(game.Player1);
            Assert.AreEqual(string.Empty, game.Player1.Name);
            Assert.AreEqual(Move.Unknown, game.Player1.Move);
            Assert.IsNull(game.Player2);
        }

        [TestMethod]
        public void NewGame_PlayerNameEmpty()
        {
            // Arrange
            string playerName = string.Empty;

            // Act
            Game game = new Game(playerName);

            // Assert
            Assert.IsNotNull(game.ID);
            Assert.AreNotEqual(string.Empty, game.ID);
            Assert.IsNotNull(game.Player1);
            Assert.AreEqual(string.Empty, game.Player1.Name);
            Assert.AreEqual(Move.Unknown, game.Player1.Move);
            Assert.IsNull(game.Player2);
        }

        [TestMethod]
        public void NewGame_PlayerNameValid()
        {
            // Arrange
            string playerName = "John";

            // Act
            Game game = new Game(playerName);

            // Assert
            Assert.IsNotNull(game.ID);
            Assert.AreNotEqual(string.Empty, game.ID);
            Assert.IsNotNull(game.Player1);
            Assert.AreEqual(playerName, game.Player1.Name);
            Assert.AreEqual(Move.Unknown, game.Player1.Move);
            Assert.IsNull(game.Player2);
        }

        #endregion

        #region IsVoid

        [TestMethod]
        public void GameIsNotVoid()
        {
            // Arrange
            string playerName = "John";
            Game game = new Game(playerName);

            // Act
            bool result = game.IsVoid;

            // Arrange
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GameIsVoid()
        {
            // Arrange
            string playerName = "John";
            Game game = new Game(playerName) { Player1 = null };

            // Act
            bool result = game.IsVoid;

            // Arrange
            Assert.IsTrue(result);
        }

        #endregion

        #region Filter

        [TestMethod]
        public void Filter_Player1Null()
        {
            // Arrange
            string player1Name = null;
            Game game = new Game(player1Name) { Player1 = null };

            // Act

            // Assert
            Exception ex = Assert.ThrowsException<PlayerUndefinedException>(() => game.Filter());
            Assert.AreEqual("Player 1 is undefined.", ex.Message);
        }

        [TestMethod]
        public void Filter_Player1NotNull_Player1HasntPlayed_Player2Null()
        {
            // Arrange
            string player1Name = "John";
            Player player2 = null;
            Game game = new Game(player1Name) { Player2 = player2 };

            // Act
            FilteredGame filteredGame = game.Filter();

            // Assert
            Assert.AreEqual(game.ID, filteredGame.ID);
            Assert.AreEqual($"Waiting for player 1 {player1Name} to play." +
                            Environment.NewLine +
                            "Waiting for Player 2 to join the game.", filteredGame.Information);
            Assert.AreEqual(player1Name, filteredGame.Player1.Name);
            Assert.AreEqual(string.Empty, filteredGame.Player1.Move);
            Assert.AreEqual(string.Empty, filteredGame.Player2.Name);
            Assert.AreEqual(string.Empty, filteredGame.Player2.Move);
        }

        [TestMethod]
        public void Filter_Player1NotNull_Player1HasAlreadyPlayed_Player2Null()
        {
            // Arrange
            string player1Name = "John";
            Move player1Move = Move.Rock;
            Player player2 = null;
            Game game = new Game(player1Name) { Player2 = player2 };
            game.Player1.Move = player1Move;

            // Act
            FilteredGame filteredGame = game.Filter();

            // Assert
            Assert.AreEqual(game.ID, filteredGame.ID);
            Assert.AreEqual($"Waiting for Player 2 to join the game.", filteredGame.Information);
            Assert.AreEqual(player1Name, filteredGame.Player1.Name);
            Assert.AreEqual($"{player1Name} has already played.", filteredGame.Player1.Move);
            Assert.AreEqual(string.Empty, filteredGame.Player2.Name);
            Assert.AreEqual(string.Empty, filteredGame.Player2.Move);
        }

        [TestMethod]
        public void Filter_Player1NotNull_Player1HasntPlayed_Player2NotNull_Player2HasntPlayed()
        {
            // Arrange
            string player1Name = "John";
            string player2Name = "Jack";
            Player player2 = new Player(player2Name);
            Game game = new Game(player1Name) { Player2 = player2 };

            // Act
            FilteredGame filteredGame = game.Filter();

            // Assert
            Assert.AreEqual(game.ID, filteredGame.ID);
            Assert.AreEqual($"Waiting for player 1 {player1Name} to play." +
                            Environment.NewLine +
                            $"Waiting for player 2 {player2Name} to play.", filteredGame.Information);
            Assert.AreEqual(player1Name, filteredGame.Player1.Name);
            Assert.AreEqual(string.Empty, filteredGame.Player1.Move);
            Assert.AreEqual(player2Name, filteredGame.Player2.Name);
            Assert.AreEqual(string.Empty, filteredGame.Player2.Move);
        }

        [TestMethod]
        public void Filter_Player1NotNull_Player1HasAlreadyPlayed_Player2NotNull_Player2HasntPlayed()
        {
            // Arrange
            string player1Name = "John";
            Move player1Move = Move.Rock;
            string player2Name = "Jack";
            Player player2 = new Player(player2Name);
            Game game = new Game(player1Name) { Player2 = player2 };
            game.Player1.Move = player1Move;

            // Act
            FilteredGame filteredGame = game.Filter();

            // Assert
            Assert.AreEqual(game.ID, filteredGame.ID);
            Assert.AreEqual($"Waiting for player 2 {player2Name} to play.", filteredGame.Information);
            Assert.AreEqual(player1Name, filteredGame.Player1.Name);
            Assert.AreEqual($"{player1Name} has already played.", filteredGame.Player1.Move);
            Assert.AreEqual(player2Name, filteredGame.Player2.Name);
            Assert.AreEqual(string.Empty, filteredGame.Player2.Move);
        }

        [TestMethod]
        public void Filter_Player1NotNull_Player1HasntPlayed_Player2NotNull_Player2HasAlreadyPlayed()
        {
            // Arrange
            string player1Name = "John";
            string player2Name = "Jack";
            Move player2Move = Move.Scissors;
            Player player2 = new Player(player2Name) { Move = player2Move };
            Game game = new Game(player1Name) { Player2 = player2 };

            // Act
            FilteredGame filteredGame = game.Filter();

            // Assert
            Assert.AreEqual(game.ID, filteredGame.ID);
            Assert.AreEqual($"Waiting for player 1 {player1Name} to play.", filteredGame.Information);
            Assert.AreEqual(player1Name, filteredGame.Player1.Name);
            Assert.AreEqual(string.Empty, filteredGame.Player1.Move);
            Assert.AreEqual(player2Name, filteredGame.Player2.Name);
            Assert.AreEqual($"{player2Name} has already played.", filteredGame.Player2.Move);
        }

        [TestMethod]
        public void Filter_Player1NotNull_Player1HasAlreadyPlayed_Player2NotNull_Player2HasAlreadyPlayed_DrawnGame()
        {
            // Arrange
            string player1Name = "John";
            Move player1Move = Move.Rock;
            string player2Name = "Jack";
            Move player2Move = player1Move;
            Player player2 = new Player(player2Name) { Move = player2Move };
            Game game = new Game(player1Name) { Player2 = player2 };
            game.Player1.Move = player1Move;

            // Act
            FilteredGame filteredGame = game.Filter();

            // Assert
            Assert.AreEqual(game.ID, filteredGame.ID);
            Assert.AreEqual("The players played a drawn game.", filteredGame.Information);
            Assert.AreEqual(player1Name, filteredGame.Player1.Name);
            Assert.AreEqual(player1Move.ToString(), filteredGame.Player1.Move);
            Assert.AreEqual(player2Name, filteredGame.Player2.Name);
            Assert.AreEqual(player2Move.ToString(), filteredGame.Player2.Move);
        }

        [TestMethod]
        public void Filter_Player1NotNull_Player1HasAlreadyPlayed_Player2NotNull_Player2HasAlreadyPlayed_Player1Wins_RockScissors()
        {
            // Arrange
            string player1Name = "John";
            Move player1Move = Move.Rock;
            string player2Name = "Jack";
            Move player2Move = Move.Scissors;
            Player player2 = new Player(player2Name) { Move = player2Move };
            Game game = new Game(player1Name) { Player2 = player2 };
            game.Player1.Move = player1Move;

            // Act
            FilteredGame filteredGame = game.Filter();

            // Assert
            Assert.AreEqual(game.ID, filteredGame.ID);
            Assert.AreEqual($"{player1Name} wins the game.", filteredGame.Information);
            Assert.AreEqual(player1Name, filteredGame.Player1.Name);
            Assert.AreEqual(player1Move.ToString(), filteredGame.Player1.Move);
            Assert.AreEqual(player2Name, filteredGame.Player2.Name);
            Assert.AreEqual(player2Move.ToString(), filteredGame.Player2.Move);
        }

        [TestMethod]
        public void Filter_Player1NotNull_Player1HasAlreadyPlayed_Player2NotNull_Player2HasAlreadyPlayed_Player2Wins_RockPaper()
        {
            // Arrange
            string player1Name = "John";
            Move player1Move = Move.Rock;
            string player2Name = "Jack";
            Move player2Move = Move.Paper;
            Player player2 = new Player(player2Name) { Move = player2Move };
            Game game = new Game(player1Name) { Player2 = player2 };
            game.Player1.Move = player1Move;

            // Act
            FilteredGame filteredGame = game.Filter();

            // Assert
            Assert.AreEqual(game.ID, filteredGame.ID);
            Assert.AreEqual($"{player2Name} wins the game.", filteredGame.Information);
            Assert.AreEqual(player1Name, filteredGame.Player1.Name);
            Assert.AreEqual(player1Move.ToString(), filteredGame.Player1.Move);
            Assert.AreEqual(player2Name, filteredGame.Player2.Name);
            Assert.AreEqual(player2Move.ToString(), filteredGame.Player2.Move);
        }

        [TestMethod]
        public void Filter_Player1NotNull_Player1HasAlreadyPlayed_Player2NotNull_Player2HasAlreadyPlayed_Player1Wins_ScissorsPaper()
        {
            // Arrange
            string player1Name = "John";
            Move player1Move = Move.Scissors;
            string player2Name = "Jack";
            Move player2Move = Move.Paper;
            Player player2 = new Player(player2Name) { Move = player2Move };
            Game game = new Game(player1Name) { Player2 = player2 };
            game.Player1.Move = player1Move;

            // Act
            FilteredGame filteredGame = game.Filter();

            // Assert
            Assert.AreEqual(game.ID, filteredGame.ID);
            Assert.AreEqual($"{player1Name} wins the game.", filteredGame.Information);
            Assert.AreEqual(player1Name, filteredGame.Player1.Name);
            Assert.AreEqual(player1Move.ToString(), filteredGame.Player1.Move);
            Assert.AreEqual(player2Name, filteredGame.Player2.Name);
            Assert.AreEqual(player2Move.ToString(), filteredGame.Player2.Move);
        }

        #endregion
    }
}
