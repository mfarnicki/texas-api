using Newtonsoft.Json;

namespace Texas.API.Interfaces
{
    public interface ICard
    {
        [JsonProperty("suit")]
        Suit Suit { get; }

        [JsonProperty("value")]
        byte Value { get; }
    }

    public enum Suit
    {
        Spade = 0,
        Heart = 1,
        Diamond = 2,
        Club = 3
    }
}