# Cygni
Rock, Paper and Scissors game handled by a REST web API developed in C#

LAUNCHING SERVER:
- Download/Clone the project,
- Open the solution with Visual Studio 2017 or later version,
- Restore NuGet packages if needed,
- Build the solution and execute the project.

IIS Express is by default linked to Microsoft Edge but can easily be modified according to your preferences.
______________
AVAILABLE COMMANDS:
- POST api/games
  - INPUT:
    Expecting a Json string "{"name": "<some_name>"}" in the request-body, starts a new game.
  - OUTPUT:
    A Json string formated as below:
      "{"GameId": "<some_game_id>",
        "Message": "Welcome <some_name>.
                    What will be your move: Rock, Paper or Scissors?"}".
    If any error occur, a message describing the error is sent to the client.
  - EFFECT:
    The first player is then created with the given name and is invited to make their move (see below).
  - ERRORS:
    An error occurs if the given name is missing or blank.
  __________________
- GET api/games/{id}
  - INPUT:
    None.
  - OUTPUT:
    A Json string formatted as below:
    "{"ID": "<some_game_id>",
      "Information": "<some_information_message>",
      "Player1": {"Name": "<some_name_1>",
                  "Move": "<some_move_1>"},
      "Player2": {"Name": "<some_name_2>",
                  "Move": "<some_move_2>"}
     }"

  At any time of the game, gives the state of the game, which can be:
    => Waiting for player 1 "..." to play;
    => Waiting for player 2 to join the game;
    => Waiting for player 2 "..." to play;
    => The players have played a drawn game;
    => "..." wins the game.

  Note that:
    => the information relative to the second player are left blank until the second player has actually joined the game;
    => the respective moves of both players are only shown when the game is over.
  ERRORS:
    None.
  ________________________
- POST api/games/{id}/join
  - INPUT:
    Expecting a Json string "{"name": "<some_name>"}" in the request-body, allows a second player to join the game.
      WARNING: the second player's name must be different from the first player's, and the entry is case unsensitive.
  - OUTPUT:
    A Json string formated as below:
      "{"GameId": "<some_game_id>",
        "Message": "Welcome <some_name>.
                    What will be your move: Rock, Paper or Scissors?"}".
  - EFFECT:
    The second player is then created with the given name and is invited to make their move (see below).
  - ERRORS:
    An error occurs:
      - if the given name is missing or blank;
      - if a player with the same name has already joined the game;
      - if more than two players are created.
  ________________________
- POST api/games/{id}/move
  - INPUT:
    Expecting a Json string "{"name": "<players_name>", "move": "<some_move>"}" in the request-body, allows one of the players to make a move.
    The only possible moves are:
      Rock;
      Paper;
      Scissors.
    Note that the entries (both name and move) are case unsensitive.
  - OUTPUT:
    Error message if any.
  - EFFECT:
    Registers the player's move.
  - ERRORS.
    An error occurs:
      - if the given player's name is missing, blank or isn't defined in the game session;
      - if the given move is missing, blank or is not recognized;
      - if a player tries to play more than once during the same game session.
______________
ARCHITECTURE:
  The solution includes two projects:
    - The code source project which contains:
      Controllers:
        HomeController  => allows the default blank page to be displayed
        GamesController => manages all the commands as described above
      Exceptions:
        PlayerUndefinedException  => User defined exception thrown in case the player 1 is undefined, which should never happen
      Models:
        ErrorViewModel  => viewmodel created by default
        FilteredGame    => allows filtered information about a game session to be sent to the client by the "GET" command
                           also contains a class named FilteredPlayer which allows to send filtered information about a player to the client
        Game            => allows all information relative to a game session to be stored
        JoinFromBody    => allows the GamesController to read information sent to the server via the "JOIN" command
        Move            => enumarate the possible moves
        MoveFromBody    => allows the GamesController to read information sent to the server via the "MOVE" command
        Player          => manages all information relative to the players
        PostFromBody    => allows the GamesController to read information sent to the server via the "POST" command
    - The tests project which contains:
      Controllers (38 tests):
        GamesControllerTest => tests relative to all public functions declared in the controller
      Models (25 tests):
        FilteredGameTest    => tests relative to the FilterGame class
        FilteredPlayerTest  => tests relative to the FilterPlayer class
        GameTest            => tests relative to the Game class
        PlayerTest          => tests relative to the Player class
