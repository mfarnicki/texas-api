using Microsoft.AspNetCore.SignalR;
using Texas.API.Models;

namespace Texas.API.Hubs
{
    public class GameHub : Hub
    {
        private static readonly GameManager _gameManager = new GameManager();

        public async Task JoinGame(string gameId, int playerPosition, string playerName)
        {
            if (_gameManager.TryAddPlayer(gameId, playerPosition, playerName, this.Context.ConnectionId, out var game))
            {
                await this.Clients.Group(gameId).SendAsync("GameState", game);
            }
            else
            {
                await this.Clients.Caller.SendAsync("Error", "Can't join this place");
            }
        }

        public async Task LeaveGame()
        {
            var playerId = this.Context.ConnectionId;
            var game = _gameManager.PlayerLeave(playerId);
            if (game != null)
            {
                var gameId = game.Id;
                await this.Groups.RemoveFromGroupAsync(playerId, gameId);
                await this.Clients.Group(gameId).SendAsync("GameState", game);
            }
        }

        public async Task InitGame(string gameId)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, gameId);
            var game = _gameManager.InitGame(gameId);
            await this.Clients.Caller.SendAsync("GameState", game);
        }

        public async Task StartGame(string gameId)
        {
            var game = _gameManager.StartGame(gameId);
            if (game != null)
            {
                await this.Clients.Group(gameId).SendAsync("GameState", game);
            }
            else
            {
                await this.Clients.Group(gameId).SendAsync("Error", "Can't start the game");
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await LeaveGame();
        }
    }
}