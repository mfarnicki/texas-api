using Texas.API.Interfaces;

namespace Texas.API.Models
{
    public class Game : IGame
    {
        public string Id { get; }

        public IPlayer[] Players { get; }

        public string WaitingForId { get; set; }

        public string DealerId { get; set; }

        public GameStatus Status { get; set; }

        public ICard[] CommunityCards { get; private set; }

        public int CurrentPot { get; set; }

        public int HighestBet { get; set; }

        public Game(string gameId)
        {
            this.Id = gameId;
            this.Players = new IPlayer[4];
            this.Status = GameStatus.Idle;
            this.CommunityCards = new ICard[5];

#if DEBUG
            this.Players[1] = new Player("computer1", 1000) { Name = "Computer1" };
            this.Players[2] = new Player("computer2", 1000) { Name = "Computer2" };
            this.Players[3] = new Player("computer3", 1000) { Name = "Computer3" };
            this.DealerId = this.Players[1].Id;
#endif
        }

        public bool HasPlayer(string playerId, out IPlayer player)
        {
            player = this.Players.SingleOrDefault(p => p?.Id == playerId);
            return player != null;
        }

        public bool AddPlayer(IPlayer newPlayer, int position)
        {
            if (this.HasPlayer(newPlayer.Id, out _))
            {
                this.RemovePlayer(newPlayer.Id);
            }

            if (this.Players.All(p => p == null))
            {
                DealerId = newPlayer.Id;
            }

            return this.Players[position] == null && (this.Players[position] = newPlayer) != null;
        }

        public void RemovePlayer(string playerId)
        {
            for (int i = 0; i < 4; i++)
            {
                if (this.Players[i]?.Id == playerId)
                {
                    var nextPlayer = this.NextPlayer(playerId);
                    if (nextPlayer != null)
                    {
                        if (playerId == WaitingForId)
                        {
                            this.WaitingForId = nextPlayer.Id;
                        }

                        if (playerId == DealerId)
                        {
                            this.DealerId = nextPlayer.Id;
                        }
                    }

                    this.Players[i] = null;
                }
            }
        }

        public bool NextStage()
        {
            return this.Players.Aggregate(true, (next, player) => next && (player == null || player.Status == PlayerStatus.Fold || player.CurrentBet == this.HighestBet));
        }

        public void NextRound()
        {
            this.Status = GameStatus.Idle;

            for (int i = 0; i < 4; i++)
            {
                if (this.Players[i] != null)
                {
                    this.Players[i].Status = PlayerStatus.Idle;
                    this.Players[i].ClearBet();

                }
            }

            var prevPlayer = this.PrevPlayer(this.DealerId);
            DealerId = prevPlayer?.Id;

            this.CommunityCards = new ICard[5];
        }

        public IPlayer NextPlayer(string playerId)
        {
            for (int i = 0; i < 4; i++)
            {
                if (this.Players[i]?.Id == playerId)
                {
                    for (int j = i + 1; j < i + 5; j++)
                    {
                        if (this.Players[j % 4] != null)
                        {
                            return this.Players[j % 4];
                        }
                    }
                }
            }

            return null;
        }

        public IPlayer PrevPlayer(string playerId)
        {
            for (int i = 0; i < 4; i++)
            {
                if (this.Players[i]?.Id == playerId)
                {
                    for (int j = i - 1; j > i - 5; j--)
                    {
                        if (this.Players[(j + 4) % 4] != null)
                        {
                            return this.Players[(j + 4) % 4];
                        }
                    }
                }
            }

            return null;
        }
    }
}