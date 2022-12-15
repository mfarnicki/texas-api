using System.Collections.Concurrent;

namespace Texas.API.Models
{
    public class GameManager
    {
        private ConcurrentDictionary<string, IGame> managedGames = new ConcurrentDictionary<string, IGame>();

        internal IGame StartGame(string gameId)
        {
            if (managedGames.TryGetValue(gameId, out var game))
            {
                game.Start();
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

        internal IGame InitGame(string gameId)
        {
            return managedGames.GetOrAdd(gameId, new Game { Id = gameId });
        }

        internal bool TryAddPlayer(string gameId, int playerPosition, string playerName, string connectionId, out IGame game)
        {
            PlayerLeave(connectionId);

            var newPlayer = new Player { PlayerId = connectionId, PlayerName = playerName };
            return managedGames.TryGetValue(gameId, out game) && game.AssignPlayer(newPlayer, playerPosition);
        }

        internal IGame PlayerLeave(string playerId)
        {
            var game = managedGames.Values.SingleOrDefault(game => game.HasPlayer(playerId, out var player));
            if (game != null)
            {
                game.RemovePlayer(playerId);
                return game;
            }

            return null;
        }
    }
}