using System.Collections.Concurrent;
using Texas.API.Interfaces;
using Texas.API.Models;

namespace Texas.API.Logic
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
                if (dealer.Game.Players.Count(p => p != null) < 1)
                {
                    // don't start a game without players
                    return null;
                }

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

        public IDealer PlayerMove(string gameId, IPlayerMove move)
        {
            if (managedGames.TryGetValue(gameId, out var dealer))
            {
                if (dealer.Game.WaitingForId != move.PlayerId)
                {
                    return null;
                }

                dealer.PlayerMove(move);
                return dealer;
            }

            return null;
        }

        public IDealer NextRound(string gameId)
        {
            if (managedGames.TryGetValue(gameId, out var dealer))
            {
                dealer.NextRound();
                return dealer;
            }

            return null;
        }

        public IDealer InitGame(string gameId)
        {
            var dealer = managedGames.GetOrAdd(gameId, id => new Dealer(new Game(id)));
            return dealer;
        }

        public bool TryAddPlayer(string gameId, int playerPosition, IPlayer player, out IGame game)
        {
            PlayerLeave(player.Id);

            if (managedGames.TryGetValue(gameId, out var dealer) && dealer.Game.AddPlayer(player, playerPosition))
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