using Texas.API.Models;

public class Game : IGame
{
    public string Id { get; }

    public IPlayer[] Players { get; }

    public GameStatus Status { get; set; }

    public ICard[] CommunityCards { get; }

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

        return this.Players[position] == null && (this.Players[position] = newPlayer) != null;
    }

    public void RemovePlayer(string playerId)
    {
        for (int i = 0; i < 4; i++)
        {
            if (this.Players[i]?.PlayerId == playerId)
            {
                this.Players[i] = null;
            }
        }
    }
}