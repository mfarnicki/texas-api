using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;

public class GamesContainer : IGamesContainer
{
    private readonly IDistributedCache cache;

public GamesContainer(IDistributedCache cache)
{
    this.cache = cache;
}

    public async Task<string> GetAllGames()
    {
        var value = await this.cache.GetStringAsync("ostatnia");
        return value;
    }

    public async Task<IGame> NewGame()
    {
        var newGame = new Game();
        await this.cache.SetStringAsync("ostatnia", newGame.Id.ToString());
        await this.cache.SetStringAsync(newGame.Id.ToString(),newGame.Id.ToString());

        return newGame;
    }

    public async Task ClearGames()
    {
        await this.cache.RemoveAsync("ostatnia");
    }
}