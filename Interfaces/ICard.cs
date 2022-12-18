using Newtonsoft.Json;
using Texas.API.Models;

namespace Texas.API.Interfaces
{
    public interface ICard
    {
        [JsonProperty("suit")]
        Suit Suit { get; }

        [JsonProperty("value")]
        byte Value { get; }
    }
}