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
                await base.SendGameState(dealer.Game);
                foreach (var holes in dealer.PlayerHoles)
                {
                    await base.SendPlayerState(holes);
                }
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
                await base.SendGameState(dealer.Game);
                if (dealer.Showdown() != null)
                {
                    await base.SendPlayerStateToAll(dealer.Game, dealer.PlayerHoles);
                }
            }
            else
            {
                await base.SendError("Can't progress the game");
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await LeaveGame();
        }
    }
}