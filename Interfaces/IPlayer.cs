using Newtonsoft.Json;

public interface IPlayer
{
    [JsonProperty("playerId")]
    string PlayerId { get; }

    [JsonProperty("playerName")]
    string PlayerName { get; }
}