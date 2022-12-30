using Newtonsoft.Json;

namespace Texas.API.Interfaces
{
    public interface IPlayerMove
    {
        [JsonProperty("playerId")]
        string PlayerId { get; }

        [JsonProperty("move")]
        MoveType Move { get; }

        [JsonProperty("amount")]
        int Amount { get; }
    }

    public enum MoveType
    {
        Blind = 0,
        Bet = 1,
        Raise = 2,
        Call = 3,
        Fold = 4,
    }
}