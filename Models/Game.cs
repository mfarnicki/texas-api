public class Game : IGame
{
    public string Id { get; set; }

    public Guid GameId { get; set; }

    public Game()
    {
        this.GameId = Guid.NewGuid();
        this.Id = this.GameId.ToString();
    }
}