using Newtonsoft.Json;

namespace Texas.API.Interfaces
{
    public interface IPlayerHole
    {
        [JsonProperty("playerId")]
        string PlayerId { get; }
        [JsonProperty("holeCard1")]
        ICard HoleCard1 { get; }
        [JsonProperty("holeCard2")]
        ICard HoleCard2 { get; }
    }

}