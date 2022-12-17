using Newtonsoft.Json;
using Texas.API.Models;

public interface IGame
{
    [JsonProperty("id")]
    string Id { get; }

    [JsonProperty("players")]
    IPlayer[] Players { get; }

    [JsonProperty("status")]
    GameStatus Status { get; set; }

    [JsonProperty("communityCards")]
    ICard[] CommunityCards { get; }

    bool HasPlayer(string playerId, out IPlayer player);
    bool AddPlayer(IPlayer newPlayer, int position);
    void RemovePlayer(string playerId);
}