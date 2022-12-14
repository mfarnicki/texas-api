using Texas.API.Models;

public class Game : IGame
{
    public string Id { get; set; }

    public IPlayer Player1 { get; set; }
    public IPlayer Player2 { get; set; }

    public GameStatus Status { get; set; }

    public Game()
    {
        this.Id = Guid.NewGuid().ToString();
        Status = GameStatus.Idle;
    }

    public bool HasPlayer(string playerId, out IPlayer player)
    {
        if (this.Player1?.PlayerId == playerId)
        {
            player = this.Player1;
            return true;
        }

        if (this.Player2?.PlayerId == playerId)
        {
            player = this.Player2;
            return true;
        }

        player = null;
        return false;
    }
}