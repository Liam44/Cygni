using RockPaperScissors.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RockPaperScissors.Controllers
{
    [Route("api/[controller]")]
    public class GamesController : ApiController
    {
        static Dictionary<string, Game> Games { get; set; }

        // GET: api/Games/{id}
        [HttpGet]
        [Route("api/games/{id}/")]
        public HttpResponseMessage Get(string id)
        {
            GameId gameId = new GameId(id);

            // Checks if the game actually exists
            if (gameId.Game == null)
            {
                return Request.CreateResponse(gameId.StatusCode, gameId.Message);
            }

            //return Request.CreateResponse(HttpStatusCode.OK, gameId.Game);
            return Request.CreateResponse(gameId.Game.Filter());
        }

        // POST: api/Games
        [HttpGet, HttpPost]
        [Route("api/games/")]
        public HttpResponseMessage Post([FromBody]PostFromBody postFromBody)
        {
            if (postFromBody == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable,
                                              "You must provide a player name.");
            }

            if (string.IsNullOrEmpty(postFromBody.Name))
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable,
                                              "You must provide a player name.");
            }

            Game game = new Game(postFromBody.Name);

            if (Games == null)
            {
                Games = new Dictionary<string, Game>();
            }

            Games.Add(game.ID, game);

            // Checks that everything went fine during the creation of the new game session
            GameId gameId = new GameId(game.ID);
            if (gameId.Game.IsVoid)
            {
                return Request.CreateResponse(gameId.StatusCode, gameId.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK,
                                          new PlayerEnteredGame(game.ID, postFromBody.Name));
        }

        // POST api/games/join/{id}
        [HttpPost]
        [Route("api/games/{id}/join/")]
        public HttpResponseMessage Join(string id, [FromBody]JoinFromBody joinFromBody)
        {
            try
            {
                if (joinFromBody == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable,
                                                  "You must provide a player name.");
                }

                GameId gameId = new GameId(id);

                // Checks if the game actually exists
                if (gameId.Game == null)
                {
                    return Request.CreateResponse(gameId.StatusCode, gameId.Message);
                }

                if (string.IsNullOrEmpty(joinFromBody.Name))
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable,
                                                  "You must provide a valid name.");
                }

                if (string.Compare(gameId.Game.Player1.Name, joinFromBody.Name, true) == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable,
                                                  "A player with a similar name already entered the game.");
                }

                if (gameId.Game.Player2 != null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable,
                                                  "Player 2 has already joined the game.");
                }

                gameId.Game.Player2 = new Player(joinFromBody.Name);

                return Request.CreateResponse(HttpStatusCode.OK,
                                              new PlayerEnteredGame(id, joinFromBody.Name));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // POST api/games/move/{id}
        [HttpPost]
        [Route("api/games/{id}/move/")]
        public HttpResponseMessage Move(string id, [FromBody]MoveFromBody moveFromBody)
        {
            try
            {
                if (moveFromBody == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable,
                                                  "You must provide a player name and a move.");
                }

                GameId gameId = new GameId(id);

                // Checks if the game actually exists
                if (gameId.Game == null)
                {
                    return Request.CreateResponse(gameId.StatusCode, gameId.Message);
                }

                Game game = gameId.Game;

                if (string.IsNullOrEmpty(moveFromBody.Name))
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable,
                                                  "You must provide a player name.");
                }

                if (string.IsNullOrEmpty(moveFromBody.Move))
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable,
                                                  "You must provide a valid move.");
                }

                // Does the name match the player 1?
                if (string.Compare(game.Player1.Name, moveFromBody.Name, true) == 0)
                {
                    // Sets player 1's move
                    if (game.Player1.HasAlreadyPlayed)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable,
                                                      $"Player 1 {moveFromBody.Name} has already played.");
                    }
                    else
                    {
                        HttpResponseMessage responseMessage = SetPlayerMove(game.Player1, moveFromBody.Move);

                        if (responseMessage != null)
                        {
                            return responseMessage;
                        }
                    }
                }
                else if (game.Player2 == null || string.Compare(game.Player2.Name, moveFromBody.Name, true) != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                                                  $"Player {moveFromBody.Name} is undefined for game {id}.");
                }
                else
                {
                    // Sets player 2's move
                    if (game.Player2.HasAlreadyPlayed)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable,
                                                      $"Player 2 {moveFromBody.Name} has already played.");
                    }
                    else
                    {
                        HttpResponseMessage responseMessage = SetPlayerMove(game.Player2, moveFromBody.Move);

                        if (responseMessage != null)
                        {
                            return responseMessage;
                        }
                    }
                }

                return Request.CreateResponse();
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        private HttpResponseMessage SetPlayerMove(Player player, string move)
        {
            HttpResponseMessage result = null;

            try
            {
                Move moveEnum = (Move)Enum.Parse(typeof(Move), move, true);

                if (Enum.IsDefined(typeof(Move), moveEnum) && moveEnum != Models.Move.Unknown)
                {
                    player.Move = moveEnum;
                }
                else
                {
                    throw new Exception();
                };

            }
            catch (Exception)
            {
                result = Request.CreateResponse(HttpStatusCode.NotFound,
                                                $"Move {move} isn't defined." +
                                                Environment.NewLine +
                                                "Authorized moves are: Rock, Paper or Scissors.");
            }

            return result;
        }

        private class GameId
        {
            public HttpStatusCode StatusCode { get; set; }
            public string Message { get; set; }
            public Game Game { get; set; }

            public GameId(string id)
            {
                Game = null;
                StatusCode = HttpStatusCode.OK;

                if (string.IsNullOrEmpty(id))
                {
                    StatusCode = HttpStatusCode.NotFound;
                    Message = "You must provide a game id.";

                    return;
                }

                if (Games == null || !Games.ContainsKey(id))
                {
                    StatusCode = HttpStatusCode.NotFound;
                    Message = $"Game {id} doesn't exist.";

                    return;
                }

                Game = Games[id];

                if (Game.IsVoid)
                {
                    Game = null;
                    StatusCode = HttpStatusCode.NotAcceptable;
                    Message = $"Player 1 is undefined, making the game {id} void." +
                              Environment.NewLine +
                              "Please start a new game.";
                }
            }
        }
    }

    public class PlayerEnteredGame
    {
        public string GameId { get; private set; }
        public string Message { get; private set; }

        public PlayerEnteredGame(string gameId, string playerName)
        {
            GameId = gameId;
            Message = $"Welcome {playerName}." + Environment.NewLine +
                       "What will be your move: Rock, Paper or Scissors?";
        }
    }
}
