using Newtonsoft.Json;
using Texas.API.Models;

public interface IGame
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("player1")]
    public IPlayer Player1 { get; set; }

    [JsonProperty("player2")]
    public IPlayer Player2 { get; set; }

    [JsonProperty("state")]
    GameState State { get; set; }

    bool HasPlayer(string playerId, out IPlayer player);
}