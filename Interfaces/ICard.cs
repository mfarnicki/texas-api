using Newtonsoft.Json;
using Texas.API.Models;

public interface ICard
{
    [JsonProperty("suit")]
    Suit Suit { get; }

    [JsonProperty("value")]
    byte Value { get; }
}