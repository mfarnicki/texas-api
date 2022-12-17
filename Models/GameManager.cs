using System.Collections.Concurrent;
using Texas.API.Interfaces;

namespace Texas.API.Models
{
    public class GameManager : IGameManager
    {
        private ConcurrentDictionary<string, IDealer> managedGames = new ConcurrentDictionary<string, IDealer>();

        public IGame[] GetAllGames()
        {
            return this.managedGames.Select(g => g.Value.Game).ToArray();
        }

        public IDealer StartGame(string gameId)
        {
            if (managedGames.TryGetValue(gameId, out var dealer))
            {
                dealer.StartGame();
                return dealer;
            }

            return null;
        }

        public IDealer ProgressGame(string gameId)
        {
            if (managedGames.TryGetValue(gameId, out var dealer))
            {
                dealer.ProgressGame();
                return dealer;
            }

            return null;
        }

        public IDealer ResetGame(string gameId)
        {
            if (managedGames.TryGetValue(gameId, out var dealer))
            {
                dealer.ResetGame();
                return dealer;
            }

            return null;
        }

        public IGame InitGame(string gameId)
        {
            var dealer = managedGames.GetOrAdd(gameId, id => new Dealer(new Game(id)));
            return dealer.Game;
        }

        public bool TryAddPlayer(string gameId, int playerPosition, string playerName, string connectionId, out IGame game)
        {
            PlayerLeave(connectionId);

            var newPlayer = new Player { PlayerId = connectionId, PlayerName = playerName };
            if (managedGames.TryGetValue(gameId, out var dealer) && dealer.Game.AddPlayer(newPlayer, playerPosition))
            {
                game = dealer.Game;
                return true;
            }

            game = null;
            return false;
        }

        public IGame PlayerLeave(string playerId)
        {
            var dealer = managedGames.Values.SingleOrDefault(dealer => dealer.Game.HasPlayer(playerId, out var player));
            if (dealer != null)
            {
                dealer.Game.RemovePlayer(playerId);
                return dealer.Game;
            }

            return null;
        }

        public bool DeleteGame(string gameId)
        {
            return managedGames.TryRemove(gameId, out _);
        }
    }
}