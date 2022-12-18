using Newtonsoft.Json;
using Texas.API.Models;

public interface IPlayer
{
    [JsonProperty("playerId")]
    string PlayerId { get; }

    [JsonProperty("playerName")]
    string PlayerName { get; }

    [JsonProperty("playerStatus")]
    PlayerStatus PlayerStatus { get; set; }
}