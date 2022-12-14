using System.Collections.Concurrent;

namespace Texas.API.Models
{
    public class GameManager
    {
        private ConcurrentDictionary<string, IGame> managedGames = new ConcurrentDictionary<string, IGame>();

        internal IGame StartGame(string gameId)
        {
            if (managedGames.TryGetValue(gameId, out var game) && game.Player1 != null && game.Player2 != null)
            {
                game.State = GameState.Active;
                return game;
            }

            return null;
        }

        internal IGame EndGame(string gameId)
        {
            if (managedGames.TryRemove(gameId, out var game))
            {
                return game;
            }

            return null;
        }

        internal bool TryAddPlayer(string gameId, byte playerNo, string connectionId, out IGame game)
        {
            PlayerLeave(connectionId);

            var newPlayer = new Player { PlayerId = connectionId };
            game = managedGames.GetOrAdd(gameId, new Game());
            switch (playerNo)
            {
                case 1:
                    return game.Player1 == null && (game.Player1 = newPlayer) != null;
                case 2:
                    return game.Player2 == null && (game.Player2 = newPlayer) != null;
            }

            return false;
        }

        internal IGame PlayerLeave(string playerId)
        {
            var game = managedGames.Values.SingleOrDefault(game => game.HasPlayer(playerId, out var player));
            if (game != null)
            {
                if (game.Player1?.PlayerId == playerId)
                {
                    game.Player1 = null;
                }

                if (game.Player2?.PlayerId == playerId)
                {
                    game.Player2 = null;
                }

                return game;
            }

            return null;
        }
    }
}