using Newtonsoft.Json;
using Texas.API.Models;

public interface IGame
{
    [JsonProperty("id")]
    string Id { get; set; }

    [JsonProperty("players")]
    IPlayer[] Players { get; }

    [JsonIgnore]
    ICard[] Deck { get; }

    [JsonProperty("status")]
    GameStatus Status { get; }

    void Start();

    bool HasPlayer(string playerId, out IPlayer player);
    void RemovePlayer(string playerId);
    bool AssignPlayer(IPlayer newPlayer, int playerPosition);
}