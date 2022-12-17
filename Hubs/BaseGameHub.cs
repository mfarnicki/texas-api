using Microsoft.AspNetCore.SignalR;
using Texas.API.Interfaces;

namespace Texas.API.Hubs
{
    public abstract class BaseGameHub : Hub
    {
        private const string AllGames = "AllGames";
        private const string GameState = "GameState";
        private const string PlayerState = "PlayerState";
        private const string ErrorState = "ErrorState";

        protected async Task ListGames(IGame[] games)
        {
            await this.Clients.Caller.SendAsync(AllGames, games);
        }

        protected async Task ListGamesToAll(IGame[] games)
        {

            await this.Clients.All.SendAsync(AllGames, games);
        }

        protected async Task SendGameState(IGame game)
        {
            await this.Clients.Group(game.Id).SendAsync(GameState, game);
        }

        protected async Task SendPlayerState(IPlayerHole playerHoles)
        {
            await this.Clients.Client(playerHoles.PlayerId).SendAsync(PlayerState, playerHoles);
        }

        protected async Task SendPlayerStateToAll(IGame game, IList<IPlayerHole> playerHoles)
        {
            await this.Clients.Group(game.Id).SendAsync(PlayerState, playerHoles);
        }

        protected async Task SendError(string errorMessage)
        {
            await this.Clients.Caller.SendAsync(ErrorState, errorMessage);
        }

        protected async Task Join(IGame game)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, game.Id);
            await this.Clients.Caller.SendAsync(GameState, game);
        }

        protected async Task Leave(IGame game)
        {
            await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, game.Id);
            await this.SendGameState(game);
        }
    }
}