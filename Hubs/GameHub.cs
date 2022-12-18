using Texas.API.Interfaces;

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
            var game = _gameManager.InitGame(Guid.NewGuid().ToString());
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

        public async Task AddPlayer(string gameId, int playerPosition, string playerName)
        {
            if (_gameManager.TryAddPlayer(gameId, playerPosition, playerName, this.Context.ConnectionId, out var game))
            {
                await base.SendGameState(game);
            }
            else
            {
                await base.SendError("Can't join here");
            }
        }

        public async Task JoinGame(string gameId)
        {
            var game = _gameManager.InitGame(gameId);
            await base.Join(game);
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
                    await base.SendPlayerState(hole);
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

        public async Task RestartGame(string gameId)
        {
            var dealer = _gameManager.ResetGame(gameId);
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