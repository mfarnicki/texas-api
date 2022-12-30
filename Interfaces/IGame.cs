using Newtonsoft.Json;

namespace Texas.API.Interfaces
{
    public interface IGame
    {
        [JsonProperty("id")]
        string Id { get; }

        [JsonProperty("players")]
        IPlayer[] Players { get; }

        [JsonProperty("waitingFor")]
        string WaitingForId { get; set; }

        [JsonProperty("dealerId")]
        string DealerId { get; set; }

        [JsonProperty("status")]
        GameStatus Status { get; set; }

        [JsonProperty("communityCards")]
        ICard[] CommunityCards { get; }

        [JsonProperty("currentPot")]
        int CurrentPot { get; set; }

        [JsonProperty("highestBet")]
        int HighestBet { get; set; }

        bool HasPlayer(string playerId, out IPlayer player);
        bool AddPlayer(IPlayer newPlayer, int position);
        void RemovePlayer(string playerId);
        bool NextStage();
        void NextRound();

        IPlayer NextPlayer(string playerId);
        IPlayer PrevPlayer(string playerId);
    }

    public enum GameStatus
    {
        Idle = 0,
        Preflop = 1,
        Flop = 2,
        Turn = 3,
        River = 4,
        Final = 5
    }
}
