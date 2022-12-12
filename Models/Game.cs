public class Game : IGame
{
    public Guid Id { get; set; }

    public Game()
    {
        this.Id = Guid.NewGuid();
    }
}