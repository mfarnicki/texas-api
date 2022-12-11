using Newtonsoft.Json;

public interface IGame
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("gameId")]
    public Guid GameId { get; set; }
}