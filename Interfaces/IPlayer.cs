using Newtonsoft.Json;

namespace Texas.API.Interfaces
{
    public interface IPlayer
    {
        [JsonProperty("id")]
        string Id { get; }

        [JsonProperty("name")]
        string Name { get; }

        [JsonProperty("status")]
        PlayerStatus Status { get; set; }

        [JsonProperty("chips")]
        int Chips { get; }

        [JsonProperty("currentBet")]
        int CurrentBet { get; }

        void ClearBet();
        int PlaceBet(int amount);
        void Payout(int amount);
    }

    public enum PlayerStatus
    {
        Idle = 0,
        Waiting = 1,
        AllIn = 2,
        Fold = 3,
        Won = 4,
        Lost = 5
    }
}