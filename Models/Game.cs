using Texas.API.Interfaces;
using Texas.API.Models;

public class Game : IGame
{
    public string Id { get; }

    public IPlayer[] Players { get; }

    public string WaitingForId { get; set; }

    public string DealerId { get; set; }

    public GameStatus Status { get; set; }

    public ICard[] CommunityCards { get; private set; }

    public Game(string gameId)
    {
        this.Id = gameId;
        this.Players = new IPlayer[4];
        this.Status = GameStatus.Idle;
        this.CommunityCards = new ICard[5];
    }

    public bool HasPlayer(string playerId, out IPlayer player)
    {
        player = this.Players.SingleOrDefault(p => p?.PlayerId == playerId);
        return player != null;
    }

    public bool AddPlayer(IPlayer newPlayer, int position)
    {
        if (this.HasPlayer(newPlayer.PlayerId, out _))
        {
            this.RemovePlayer(newPlayer.PlayerId);
        }

        if (this.Players.All(p => p == null))
        {
            DealerId = newPlayer.PlayerId;
        }

        return this.Players[position] == null && (this.Players[position] = newPlayer) != null;
    }

    public void RemovePlayer(string playerId)
    {
        for (int i = 0; i < 4; i++)
        {
            if (this.Players[i]?.PlayerId == playerId)
            {
                var nextPlayer = this.NextPlayer(playerId);
                if (nextPlayer != null)
                {
                    if (playerId == WaitingForId)
                    {
                        this.WaitingForId = nextPlayer.PlayerId;
                    }

                    if (playerId == DealerId)
                    {
                        this.DealerId = nextPlayer.PlayerId;
                    }
                }

                this.Players[i] = null;
            }
        }
    }

    public void NextRound()
    {
        this.Status = GameStatus.Idle;

        for (int i = 0; i < 4; i++)
        {
            if (this.Players[i] != null)
            {
                this.Players[i].PlayerStatus = PlayerStatus.Idle;
            }
        }

        var prevPlayer = this.PrevPlayer(this.DealerId);
        DealerId = prevPlayer.PlayerId;

        this.CommunityCards = new ICard[5];
    }

    private IPlayer NextPlayer(string playerId)
    {
        for (int i = 0; i < 4; i++)
        {
            if (this.Players[i]?.PlayerId == playerId)
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

    private IPlayer PrevPlayer(string playerId)
    {
        for (int i = 0; i < 4; i++)
        {
            if (this.Players[i]?.PlayerId == playerId)
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