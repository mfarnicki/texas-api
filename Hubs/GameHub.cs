using Texas.API.Interfaces;
using Texas.API.Models;

namespace Texas.API.Hubs
{
    public class GameHub : BaseGameHub
    {
        private readonly IGameManager _gameManager;

        public GameHub(IGameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public async Task NewGame()
        {
            _gameManager.InitGame(Guid.NewGuid().ToString());
            await base.ListGamesToAll(_gameManager.GetAllGames());
        }

        public async Task ListGames()
        {
            await base.ListGames(_gameManager.GetAllGames());
        }

        public async Task DeleteGame(string gameId)
        {
            if (_gameManager.DeleteGame(gameId))
            {
                await base.ListGamesToAll(_gameManager.GetAllGames());
            }
        }

        public async Task<string> AddPlayer(string gameId, int playerPosition, Player player)
        {
            player.Id = this.Context.ConnectionId;
            if (_gameManager.TryAddPlayer(gameId, playerPosition, player, out var game))
            {
                await base.SendGameState(game);
                return await Task.FromResult(player.Id);
            }
            else
            {
                await base.SendError("Can't join here");
            }

            return await Task.FromResult<string>(null);
        }

        public async Task JoinGame(string gameId)
        {
            var dealer = _gameManager.InitGame(gameId);
            await base.Join(dealer);
        }

        public async Task LeaveGame()
        {
            var playerId = this.Context.ConnectionId;
            var game = _gameManager.PlayerLeave(playerId);
            if (game != null)
            {
                await base.Leave(game);
            }
        }

        public async Task StartGame(string gameId)
        {
            var dealer = _gameManager.StartGame(gameId);
            if (dealer != null)
            {
                foreach (var hole in dealer.PlayerHoles)
                {
                    if (hole != null)
                    {
                        await base.SendPlayerState(hole);
                    }
                }

                await base.SendGameState(dealer.Game);
            }
            else
            {
                await base.SendError("Can't start the game");
            }
        }

        public async Task ProgressGame(string gameId)
        {
            var dealer = _gameManager.ProgressGame(gameId);
            if (dealer != null)
            {
                if (dealer.Showdown())
                {
                    await base.SendAllPlayersState(dealer.Game, dealer.PlayerHoles);
                }

                await base.SendGameState(dealer.Game);
            }
            else
            {
                await base.SendError("Can't progress the game");
            }
        }

        public async Task PlayerMove(string gameId, PlayerMove move)
        {
            var dealer = _gameManager.PlayerMove(gameId, move);
            if (dealer != null)
            {
                if (dealer.Showdown())
                {
                    await base.SendAllPlayersState(dealer.Game, dealer.PlayerHoles);
                }

                await base.SendGameState(dealer.Game);
            }
            else
            {
                await base.SendError("It's not your turn");
            }
        }

        public async Task NextRound(string gameId)
        {
            var dealer = _gameManager.NextRound(gameId);
            if (dealer != null)
            {
                await base.SendGameState(dealer.Game);
                await base.SendAllPlayersState(dealer.Game, dealer.PlayerHoles);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await LeaveGame();
        }
    }
}