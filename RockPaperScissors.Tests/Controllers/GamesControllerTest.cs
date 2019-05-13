using Microsoft.VisualStudio.TestTools.UnitTesting;
using RockPaperScissors.Controllers;
using RockPaperScissors.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RockPaperScissors.Tests.Controllers
{
    [TestClass]
    public class GamesControllerTest
    {
        private GamesController NewGamesController()
        {
            return new GamesController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        #region Post method

        [TestMethod]
        public void Post_ModelNull()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = null;

            // Act
            HttpResponseMessage result = controller.Post(postFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual("You must provide a player name.", description);
        }

        [TestMethod]
        public void Post_ModelNotNull_NameNull()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody(null);

            // Act
            HttpResponseMessage result = controller.Post(postFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual("You must provide a player name.", description);
        }

        [TestMethod]
        public void Post_ModelNotNull_NameEmpty()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody(string.Empty);

            // Act
            HttpResponseMessage result = controller.Post(postFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual("You must provide a player name.", description);
        }

        [TestMethod]
        public void Post_ModelNotNull_NameValid()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");

            // Act
            HttpResponseMessage result = controller.Post(postFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out PlayerEnteredGame player1EnteredGame));
            Assert.IsNotNull(player1EnteredGame.GameId);
            Assert.AreEqual($"Welcome {postFromBody.Name}." + Environment.NewLine +
                             "What will be your move: Rock, Paper or Scissors?", player1EnteredGame.Message);
        }

        #endregion

        #region Join method

        [TestMethod]
        public void Join_IdNull()
        {
            // Arrange
            GamesController controller = NewGamesController();
            string id = null;
            JoinFromBody joinFromBody = new JoinFromBody(null);

            // Act
            HttpResponseMessage result = controller.Join(id, joinFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual("You must provide a game id.", description);
        }

        [TestMethod]
        public void Join_IdEmpty()
        {
            // Arrange
            GamesController controller = NewGamesController();
            string id = string.Empty;
            JoinFromBody joinFromBody = new JoinFromBody(null);

            // Act
            HttpResponseMessage result = controller.Join(id, joinFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual("You must provide a game id.", description);
        }

        [TestMethod]
        public void Join_IdValid_GameDoesntExist()
        {
            // Arrange
            GamesController controller = NewGamesController();
            string id = "abcd";
            JoinFromBody joinFromBody = new JoinFromBody(null);

            // Act
            HttpResponseMessage result = controller.Join(id, joinFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual($"Game {id} doesn't exist.", description);
        }

        [TestMethod]
        public void Join_IdValid_GameExists_ModelNull()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            JoinFromBody joinFromBody = null;

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            HttpResponseMessage result = controller.Join(player1EnteredGame.GameId, joinFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual("You must provide a player name.", description);
        }

        [TestMethod]
        public void Join_IdValid_GameExists_ModelNotNull_NameNull()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            JoinFromBody joinFromBody = new JoinFromBody(null);

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            HttpResponseMessage result = controller.Join(player1EnteredGame.GameId, joinFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual("You must provide a valid name.", description);
        }

        [TestMethod]
        public void Join_IdValid_GameExists_ModelNotNull_NameEmpty()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            JoinFromBody joinFromBody = new JoinFromBody(string.Empty);

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            HttpResponseMessage result = controller.Join(player1EnteredGame.GameId, joinFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual("You must provide a valid name.", description);
        }

        [TestMethod]
        public void Join_IdValid_GameExists_ModelNotNull_NameValid_Player2DoesntExist()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            JoinFromBody joinFromBody = new JoinFromBody("Jack");

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            HttpResponseMessage result = controller.Join(player1EnteredGame.GameId, joinFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out PlayerEnteredGame player2EnteredGame));
            Assert.AreEqual(player1EnteredGame.GameId, player2EnteredGame.GameId);
            Assert.AreEqual($"Welcome {joinFromBody.Name}." + Environment.NewLine +
                             "What will be your move: Rock, Paper or Scissors?", player2EnteredGame.Message);
        }

        [TestMethod]
        public void Join_IdValid_GameExists_ModelNotNull_NameValid_Player2HasSameNameAsPlayer1()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            JoinFromBody joinFromBody = new JoinFromBody(postFromBody.Name);

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            HttpResponseMessage result = controller.Join(player1EnteredGame.GameId, joinFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual("A player with a similar name already entered the game.", description);
        }

        [TestMethod]
        public void Join_IdValid_GameExists_ModelNotNull_NameValid_Player2AlreadyExists()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            JoinFromBody joinFromBody1 = new JoinFromBody("Jack");
            JoinFromBody joinFromBody2 = new JoinFromBody("Jill");

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            controller.Join(player1EnteredGame.GameId, joinFromBody1);

            HttpResponseMessage result = controller.Join(player1EnteredGame.GameId, joinFromBody2);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual("Player 2 has already joined the game.", description);
        }

        #endregion

        #region Move method

        [TestMethod]
        public void Move_ModelNull()
        {
            // Arrange
            GamesController controller = NewGamesController();
            MoveFromBody moveFromBody = null;
            string id = null;

            // Act
            HttpResponseMessage result = controller.Move(id, moveFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual("You must provide a player name and a move.", description);
        }

        [TestMethod]
        public void Move_ModelNotNull_IdNull()
        {
            // Arrange
            GamesController controller = NewGamesController();
            MoveFromBody moveFromBody = new MoveFromBody(null, null);
            string id = null;

            // Act
            HttpResponseMessage result = controller.Move(id, moveFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual("You must provide a game id.", description);
        }

        [TestMethod]
        public void Move_ModelNotNull_IdEmpty()
        {
            // Arrange
            GamesController controller = NewGamesController();
            MoveFromBody moveFromBody = new MoveFromBody(null, null);
            string id = string.Empty;

            // Act
            HttpResponseMessage result = controller.Move(id, moveFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual("You must provide a game id.", description);
        }

        [TestMethod]
        public void Move_ModelNotNull_IdValid_GameDoesntExist()
        {
            // Arrange
            GamesController controller = NewGamesController();
            MoveFromBody moveFromBody = new MoveFromBody(null, null);
            string id = "abcd";

            // Act
            HttpResponseMessage result = controller.Move(id, moveFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual($"Game {id} doesn't exist.", description);
        }

        [TestMethod]
        public void Move_ModelNotNull_NameNull_IdValid_GameExists_Player1Exists_Player2DoesntExist()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            MoveFromBody moveFromBody = new MoveFromBody(null, null);

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            HttpResponseMessage result = controller.Move(player1EnteredGame.GameId, moveFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual("You must provide a player name.", description);
        }

        [TestMethod]
        public void Move_ModelNotNull_NameEmpty_IdValid_GameExists_Player1Exists_Player2DoesntExist()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            MoveFromBody moveFromBody = new MoveFromBody(string.Empty, null);

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            HttpResponseMessage result = controller.Move(player1EnteredGame.GameId, moveFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual("You must provide a player name.", description);
        }

        [TestMethod]
        public void Move_ModelNotNull_NameValid_MoveNull_IdValid_GameExists_Player1Exists_Player2DoesntExist()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            MoveFromBody moveFromBody = new MoveFromBody(postFromBody.Name, null);

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            HttpResponseMessage result = controller.Move(player1EnteredGame.GameId, moveFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual("You must provide a valid move.", description);
        }

        [TestMethod]
        public void Move_ModelNotNull_NameValid_MoveEmpty_IdValid_GameExists_Player1Exists_Player2DoesntExist()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            MoveFromBody moveFromBody = new MoveFromBody(postFromBody.Name, string.Empty);

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            HttpResponseMessage result = controller.Move(player1EnteredGame.GameId, moveFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual("You must provide a valid move.", description);
        }

        [TestMethod]
        public void Move_ModelNotNull_NameInvalid_MoveValid_IdValid_GameExists_Player1Exists_Player2DoesntExist()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            MoveFromBody moveFromBody = new MoveFromBody("Jack", "Rock");

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            HttpResponseMessage result = controller.Move(player1EnteredGame.GameId, moveFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual($"Player {moveFromBody.Name} is undefined for game {player1EnteredGame.GameId}.", description);
        }

        [TestMethod]
        public void Move_ModelNotNull_NameValid_MoveInvalid_IdValid_GameExists_Player1Exists_Player2DoesntExist()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            MoveFromBody moveFromBody = new MoveFromBody(postFromBody.Name, "Unknown");

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            HttpResponseMessage result = controller.Move(player1EnteredGame.GameId, moveFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual($"Move {moveFromBody.Move} isn't defined." +
                            Environment.NewLine +
                            "Authorized moves are: Rock, Paper or Scissors.", description);
        }

        [TestMethod]
        public void Move_ModelNotNull_NameValid_MoveValid_IdValid_GameExists_Player1Exists_Player2DoesntExist_Player1HasntPlayed()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            MoveFromBody moveFromBody = new MoveFromBody(postFromBody.Name, "Rock");

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            HttpResponseMessage getResultBefore = controller.Get(player1EnteredGame.GameId);
            getResultBefore.TryGetContentValue(out FilteredGame filteredGameBefore);

            HttpResponseMessage result = controller.Move(player1EnteredGame.GameId, moveFromBody);

            HttpResponseMessage getResultAfter = controller.Get(player1EnteredGame.GameId);
            getResultAfter.TryGetContentValue(out FilteredGame filteredGameAfter);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsNull(result.Content);

            Assert.AreEqual($"Waiting for player 1 {postFromBody.Name} to play." +
                            Environment.NewLine +
                            "Waiting for Player 2 to join the game.", filteredGameBefore.Information);
            Assert.AreEqual(postFromBody.Name, filteredGameBefore.Player1.Name);
            Assert.AreEqual(string.Empty, filteredGameBefore.Player1.Move);
            Assert.AreEqual(string.Empty, filteredGameBefore.Player2.Name);
            Assert.AreEqual(string.Empty, filteredGameBefore.Player2.Move);

            Assert.AreEqual("Waiting for Player 2 to join the game.", filteredGameAfter.Information);
            Assert.AreEqual(postFromBody.Name, filteredGameAfter.Player1.Name);
            Assert.AreEqual($"{moveFromBody.Name} has already played.", filteredGameAfter.Player1.Move);
            Assert.AreEqual(string.Empty, filteredGameAfter.Player2.Name);
            Assert.AreEqual(string.Empty, filteredGameAfter.Player2.Move);
        }

        [TestMethod]
        public void Move_ModelNotNull_NameValid_MoveValid_IdValid_GameExists_Player1Exists_Player2DoesntExist_Player1HasAlreadyPlayed()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            MoveFromBody moveFromBody = new MoveFromBody(postFromBody.Name, "Rock");

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            controller.Move(player1EnteredGame.GameId, moveFromBody);

            HttpResponseMessage getResultBefore = controller.Get(player1EnteredGame.GameId);
            getResultBefore.TryGetContentValue(out FilteredGame filteredGameBefore);

            HttpResponseMessage result = controller.Move(player1EnteredGame.GameId, moveFromBody);

            HttpResponseMessage getResultAfter = controller.Get(player1EnteredGame.GameId);
            getResultAfter.TryGetContentValue(out FilteredGame filteredGameAfter);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual($"Player 1 {postFromBody.Name} has already played.", description);

            Assert.AreEqual("Waiting for Player 2 to join the game.", filteredGameBefore.Information);
            Assert.AreEqual(postFromBody.Name, filteredGameBefore.Player1.Name);
            Assert.AreEqual($"{postFromBody.Name} has already played.", filteredGameBefore.Player1.Move);
            Assert.AreEqual(string.Empty, filteredGameBefore.Player2.Name);
            Assert.AreEqual(string.Empty, filteredGameBefore.Player2.Move);

            Assert.AreEqual("Waiting for Player 2 to join the game.", filteredGameAfter.Information);
            Assert.AreEqual(postFromBody.Name, filteredGameAfter.Player1.Name);
            Assert.AreEqual($"{postFromBody.Name} has already played.", filteredGameAfter.Player1.Move);
            Assert.AreEqual(string.Empty, filteredGameAfter.Player2.Name);
            Assert.AreEqual(string.Empty, filteredGameAfter.Player2.Move);
        }

        [TestMethod]
        public void Move_ModelNotNull_NameInvalid_MoveValid_IdValid_GameExists_Player1Exists_Player2Exists()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            JoinFromBody joinFromBody = new JoinFromBody("Jack");
            MoveFromBody moveFromBody = new MoveFromBody("Jill", "Rock");

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            HttpResponseMessage joinResponse = controller.Join(player1EnteredGame.GameId, joinFromBody);
            joinResponse.TryGetContentValue(out PlayerEnteredGame player2EnteredGame);

            HttpResponseMessage result = controller.Move(player1EnteredGame.GameId, moveFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual($"Player {moveFromBody.Name} is undefined for game {player1EnteredGame.GameId}.", description);
        }

        [TestMethod]
        public void Move_ModelNotNull_NameValid_MoveInvalid_IdValid_GameExists_Player1Exists_Player2Exists()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            JoinFromBody joinFromBody = new JoinFromBody("Jack");
            MoveFromBody moveFromBody = new MoveFromBody(joinFromBody.Name, "Move");

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            HttpResponseMessage joinResponse = controller.Join(player1EnteredGame.GameId, joinFromBody);
            joinResponse.TryGetContentValue(out PlayerEnteredGame player2EnteredGame);

            HttpResponseMessage result = controller.Move(player1EnteredGame.GameId, moveFromBody);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual($"Move {moveFromBody.Move} isn't defined." +
                            Environment.NewLine +
                            "Authorized moves are: Rock, Paper or Scissors.", description);
        }

        [TestMethod]
        public void Move_ModelNotNull_NameValid_MoveValid_IdValid_GameExists_Player1Exists_Player2Exists_Player1HasntPlayed_Player2HasntPlayed()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            JoinFromBody joinFromBody = new JoinFromBody("Jack");
            MoveFromBody moveFromBody = new MoveFromBody(joinFromBody.Name, "Rock");

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            HttpResponseMessage joinResponse = controller.Join(player1EnteredGame.GameId, joinFromBody);
            joinResponse.TryGetContentValue(out PlayerEnteredGame player2EnteredGame);

            HttpResponseMessage getResultBefore = controller.Get(player1EnteredGame.GameId);
            getResultBefore.TryGetContentValue(out FilteredGame filteredGameBefore);

            HttpResponseMessage result = controller.Move(player1EnteredGame.GameId, moveFromBody);

            HttpResponseMessage getResultAfter = controller.Get(player1EnteredGame.GameId);
            getResultAfter.TryGetContentValue(out FilteredGame filteredGameAfter);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsNull(result.Content);

            Assert.AreEqual($"Waiting for player 1 {postFromBody.Name} to play." +
                            Environment.NewLine +
                            $"Waiting for player 2 {joinFromBody.Name} to play.", filteredGameBefore.Information);
            Assert.AreEqual(postFromBody.Name, filteredGameBefore.Player1.Name);
            Assert.AreEqual(string.Empty, filteredGameBefore.Player1.Move);
            Assert.AreEqual(joinFromBody.Name, filteredGameBefore.Player2.Name);
            Assert.AreEqual(string.Empty, filteredGameBefore.Player2.Move);

            Assert.AreEqual($"Waiting for player 1 {postFromBody.Name} to play.", filteredGameAfter.Information);
            Assert.AreEqual(postFromBody.Name, filteredGameAfter.Player1.Name);
            Assert.AreEqual(string.Empty, filteredGameAfter.Player1.Move);
            Assert.AreEqual(joinFromBody.Name, filteredGameAfter.Player2.Name);
            Assert.AreEqual($"{moveFromBody.Name} has already played.", filteredGameAfter.Player2.Move);
        }

        [TestMethod]
        public void Move_ModelNotNull_NameValid_MoveValid_IdValid_GameExists_Player1Exists_Player2Exists_Player1HasntPlayed_Player2HasAlreadyPlayed()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            JoinFromBody joinFromBody = new JoinFromBody("Jack");
            MoveFromBody moveFromBody = new MoveFromBody(joinFromBody.Name, "Rock");

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            HttpResponseMessage joinResponse = controller.Join(player1EnteredGame.GameId, joinFromBody);
            joinResponse.TryGetContentValue(out PlayerEnteredGame player2EnteredGame);

            controller.Move(player1EnteredGame.GameId, moveFromBody);

            HttpResponseMessage getResultBefore = controller.Get(player1EnteredGame.GameId);
            getResultBefore.TryGetContentValue(out FilteredGame filteredGameBefore);

            HttpResponseMessage result = controller.Move(player1EnteredGame.GameId, moveFromBody);

            HttpResponseMessage getResultAfter = controller.Get(player1EnteredGame.GameId);
            getResultAfter.TryGetContentValue(out FilteredGame filteredGameAfter);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotAcceptable, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual($"Player 2 {joinFromBody.Name} has already played.", description);

            Assert.AreEqual($"Waiting for player 1 {postFromBody.Name} to play.", filteredGameBefore.Information);
            Assert.AreEqual(postFromBody.Name, filteredGameBefore.Player1.Name);
            Assert.AreEqual(string.Empty, filteredGameBefore.Player1.Move);
            Assert.AreEqual(joinFromBody.Name, filteredGameBefore.Player2.Name);
            Assert.AreEqual($"{joinFromBody.Name} has already played.", filteredGameBefore.Player2.Move);

            Assert.AreEqual($"Waiting for player 1 {postFromBody.Name} to play.", filteredGameAfter.Information);
            Assert.AreEqual(postFromBody.Name, filteredGameAfter.Player1.Name);
            Assert.AreEqual(string.Empty, filteredGameAfter.Player1.Move);
            Assert.AreEqual(joinFromBody.Name, filteredGameAfter.Player2.Name);
            Assert.AreEqual($"{joinFromBody.Name} has already played.", filteredGameAfter.Player2.Move);
        }

        [TestMethod]
        public void Move_ModelNotNull_NameValid_MoveValid_IdValid_GameExists_Player1Exists_Player2Exists_Player1HasPlayed_Player2HasntPlayed_Player1WinsRockScissors()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            JoinFromBody joinFromBody = new JoinFromBody("Jack");
            MoveFromBody moveFromBody1 = new MoveFromBody(postFromBody.Name, "Rock");
            MoveFromBody moveFromBody2 = new MoveFromBody(joinFromBody.Name, "Scissors");

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            controller.Move(player1EnteredGame.GameId, moveFromBody1);

            HttpResponseMessage joinResponse = controller.Join(player1EnteredGame.GameId, joinFromBody);
            joinResponse.TryGetContentValue(out PlayerEnteredGame player2EnteredGame);

            HttpResponseMessage getResultBefore = controller.Get(player1EnteredGame.GameId);
            getResultBefore.TryGetContentValue(out FilteredGame filteredGameBefore);

            HttpResponseMessage result = controller.Move(player1EnteredGame.GameId, moveFromBody2);

            HttpResponseMessage getResultAfter = controller.Get(player1EnteredGame.GameId);
            getResultAfter.TryGetContentValue(out FilteredGame filteredGameAfter);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsNull(result.Content);

            Assert.AreEqual($"Waiting for player 2 {moveFromBody2.Name} to play.", filteredGameBefore.Information);
            Assert.AreEqual(moveFromBody1.Name, filteredGameBefore.Player1.Name);
            Assert.AreEqual($"{moveFromBody1.Name} has already played.", filteredGameBefore.Player1.Move);
            Assert.AreEqual(moveFromBody2.Name, filteredGameBefore.Player2.Name);
            Assert.AreEqual(string.Empty, filteredGameBefore.Player2.Move);

            Assert.AreEqual($"{moveFromBody1.Name} wins the game.", filteredGameAfter.Information);
            Assert.AreEqual(moveFromBody1.Name, filteredGameAfter.Player1.Name);
            Assert.AreEqual(moveFromBody1.Move, filteredGameAfter.Player1.Move);
            Assert.AreEqual(moveFromBody2.Name, filteredGameAfter.Player2.Name);
            Assert.AreEqual(moveFromBody2.Move, filteredGameAfter.Player2.Move);
        }

        [TestMethod]
        public void Move_ModelNotNull_NameValid_MoveValid_IdValid_GameExists_Player1Exists_Player2Exists_Player1HasPlayed_Player2HasntPlayed_Player2WinsPaperRock()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            JoinFromBody joinFromBody = new JoinFromBody("Jack");
            MoveFromBody moveFromBody1 = new MoveFromBody(postFromBody.Name, "Rock");
            MoveFromBody moveFromBody2 = new MoveFromBody(joinFromBody.Name, "Paper");

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            controller.Move(player1EnteredGame.GameId, moveFromBody1);

            HttpResponseMessage joinResponse = controller.Join(player1EnteredGame.GameId, joinFromBody);
            joinResponse.TryGetContentValue(out PlayerEnteredGame player2EnteredGame);

            HttpResponseMessage getResultBefore = controller.Get(player1EnteredGame.GameId);
            getResultBefore.TryGetContentValue(out FilteredGame filteredGameBefore);

            HttpResponseMessage result = controller.Move(player1EnteredGame.GameId, moveFromBody2);

            HttpResponseMessage getResultAfter = controller.Get(player1EnteredGame.GameId);
            getResultAfter.TryGetContentValue(out FilteredGame filteredGameAfter);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsNull(result.Content);

            Assert.AreEqual($"Waiting for player 2 {moveFromBody2.Name} to play.", filteredGameBefore.Information);
            Assert.AreEqual(moveFromBody1.Name, filteredGameBefore.Player1.Name);
            Assert.AreEqual($"{moveFromBody1.Name} has already played.", filteredGameBefore.Player1.Move);
            Assert.AreEqual(moveFromBody2.Name, filteredGameBefore.Player2.Name);
            Assert.AreEqual(string.Empty, filteredGameBefore.Player2.Move);

            Assert.AreEqual($"{moveFromBody2.Name} wins the game.", filteredGameAfter.Information);
            Assert.AreEqual(moveFromBody1.Name, filteredGameAfter.Player1.Name);
            Assert.AreEqual(moveFromBody1.Move, filteredGameAfter.Player1.Move);
            Assert.AreEqual(moveFromBody2.Name, filteredGameAfter.Player2.Name);
            Assert.AreEqual(moveFromBody2.Move, filteredGameAfter.Player2.Move);
        }

        [TestMethod]
        public void Move_ModelNotNull_NameValid_MoveValid_IdValid_GameExists_Player1Exists_Player2Exists_Player1HasPlayed_Player2HasntPlayed_Player1WinsScissorsPaper()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            JoinFromBody joinFromBody = new JoinFromBody("Jack");
            MoveFromBody moveFromBody1 = new MoveFromBody(postFromBody.Name, "Scissors");
            MoveFromBody moveFromBody2 = new MoveFromBody(joinFromBody.Name, "Paper");

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            controller.Move(player1EnteredGame.GameId, moveFromBody1);

            HttpResponseMessage joinResponse = controller.Join(player1EnteredGame.GameId, joinFromBody);
            joinResponse.TryGetContentValue(out PlayerEnteredGame player2EnteredGame);

            HttpResponseMessage getResultBefore = controller.Get(player1EnteredGame.GameId);
            getResultBefore.TryGetContentValue(out FilteredGame filteredGameBefore);

            HttpResponseMessage result = controller.Move(player1EnteredGame.GameId, moveFromBody2);

            HttpResponseMessage getResultAfter = controller.Get(player1EnteredGame.GameId);
            getResultAfter.TryGetContentValue(out FilteredGame filteredGameAfter);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsNull(result.Content);

            Assert.AreEqual($"Waiting for player 2 {moveFromBody2.Name} to play.", filteredGameBefore.Information);
            Assert.AreEqual(moveFromBody1.Name, filteredGameBefore.Player1.Name);
            Assert.AreEqual($"{moveFromBody1.Name} has already played.", filteredGameBefore.Player1.Move);
            Assert.AreEqual(moveFromBody2.Name, filteredGameBefore.Player2.Name);
            Assert.AreEqual(string.Empty, filteredGameBefore.Player2.Move);

            Assert.AreEqual($"{moveFromBody1.Name} wins the game.", filteredGameAfter.Information);
            Assert.AreEqual(moveFromBody1.Name, filteredGameAfter.Player1.Name);
            Assert.AreEqual(moveFromBody1.Move, filteredGameAfter.Player1.Move);
            Assert.AreEqual(moveFromBody2.Name, filteredGameAfter.Player2.Name);
            Assert.AreEqual(moveFromBody2.Move, filteredGameAfter.Player2.Move);
        }

        [TestMethod]
        public void Move_ModelNotNull_NameValid_MoveValid_IdValid_GameExists_Player1Exists_Player2Exists_Player1HasPlayed_Player2HasntPlayed_DrawnGame()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            JoinFromBody joinFromBody = new JoinFromBody("Jack");
            MoveFromBody moveFromBody1 = new MoveFromBody(postFromBody.Name, "Scissors");
            MoveFromBody moveFromBody2 = new MoveFromBody(joinFromBody.Name, moveFromBody1.Move);

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            controller.Move(player1EnteredGame.GameId, moveFromBody1);

            HttpResponseMessage joinResponse = controller.Join(player1EnteredGame.GameId, joinFromBody);
            joinResponse.TryGetContentValue(out PlayerEnteredGame player2EnteredGame);

            HttpResponseMessage getResultBefore = controller.Get(player1EnteredGame.GameId);
            getResultBefore.TryGetContentValue(out FilteredGame filteredGameBefore);

            HttpResponseMessage result = controller.Move(player1EnteredGame.GameId, moveFromBody2);

            HttpResponseMessage getResultAfter = controller.Get(player1EnteredGame.GameId);
            getResultAfter.TryGetContentValue(out FilteredGame filteredGameAfter);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsNull(result.Content);

            Assert.AreEqual($"Waiting for player 2 {moveFromBody2.Name} to play.", filteredGameBefore.Information);
            Assert.AreEqual(moveFromBody1.Name, filteredGameBefore.Player1.Name);
            Assert.AreEqual($"{moveFromBody1.Name} has already played.", filteredGameBefore.Player1.Move);
            Assert.AreEqual(moveFromBody2.Name, filteredGameBefore.Player2.Name);
            Assert.AreEqual(string.Empty, filteredGameBefore.Player2.Move);

            Assert.AreEqual("The players played a drawn game.", filteredGameAfter.Information);
            Assert.AreEqual(moveFromBody1.Name, filteredGameAfter.Player1.Name);
            Assert.AreEqual(moveFromBody1.Move, filteredGameAfter.Player1.Move);
            Assert.AreEqual(moveFromBody2.Name, filteredGameAfter.Player2.Name);
            Assert.AreEqual(moveFromBody2.Move, filteredGameAfter.Player2.Move);
        }

        #endregion

        #region Get method

        [TestMethod]
        public void Get_IdNull()
        {
            // Arrange
            GamesController controller = NewGamesController();
            string id = null;

            // Act
            HttpResponseMessage result = controller.Get(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual("You must provide a game id.", description);
        }

        [TestMethod]
        public void Get_IdEmpty()
        {
            // Arrange
            GamesController controller = NewGamesController();
            string id = string.Empty;

            // Act
            HttpResponseMessage result = controller.Get(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual("You must provide a game id.", description);
        }

        [TestMethod]
        public void Get_IdValid_GameDoesntExist()
        {
            // Arrange
            GamesController controller = NewGamesController();
            string id = "abcd";

            // Act
            HttpResponseMessage result = controller.Get(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out string description));
            Assert.AreEqual($"Game {id} doesn't exist.", description);
        }

        [TestMethod]
        public void Get_IdValid_GameExists_Player1HasntPlayed_Player2DoesntExist()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            HttpResponseMessage result = controller.Get(player1EnteredGame.GameId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out FilteredGame content));
            Assert.AreEqual(content.ID, player1EnteredGame.GameId);
            Assert.AreEqual($"Waiting for player 1 {postFromBody.Name} to play." +
                            Environment.NewLine +
                            "Waiting for Player 2 to join the game.", content.Information);
            Assert.IsNotNull(content.Player1);
            Assert.AreEqual(postFromBody.Name, content.Player1.Name);
            Assert.AreEqual(string.Empty, content.Player1.Move);
            Assert.IsNotNull(content.Player2);
            Assert.AreEqual(string.Empty, content.Player2.Name);
            Assert.AreEqual(string.Empty, content.Player2.Move);
        }

        [TestMethod]
        public void Get_IdValid_GameExists_Player1HasntPlayed_Player2Exists_Player2HasntPlayed()
        {
            // Arrange
            GamesController controller = NewGamesController();
            PostFromBody postFromBody = new PostFromBody("John");
            JoinFromBody joinFromBody = new JoinFromBody("Jack");

            // Act
            HttpResponseMessage postResponse = controller.Post(postFromBody);
            postResponse.TryGetContentValue(out PlayerEnteredGame player1EnteredGame);

            controller.Join(player1EnteredGame.GameId, joinFromBody);

            HttpResponseMessage result = controller.Get(player1EnteredGame.GameId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue(out FilteredGame content));
            Assert.AreEqual(content.ID, player1EnteredGame.GameId);
            Assert.AreEqual($"Waiting for player 1 {postFromBody.Name} to play." +
                            Environment.NewLine +
                            $"Waiting for player 2 {joinFromBody.Name} to play.", content.Information);
            Assert.IsNotNull(content.Player1);
            Assert.AreEqual(postFromBody.Name, content.Player1.Name);
            Assert.AreEqual(string.Empty, content.Player1.Move);
            Assert.IsNotNull(content.Player2);
            Assert.AreEqual(joinFromBody.Name, content.Player2.Name);
            Assert.AreEqual(string.Empty, content.Player2.Move);
        }

        #endregion
    }
}
