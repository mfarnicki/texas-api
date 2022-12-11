public class Game : IGame
{
    public Guid Id {get;}

    public Game()
    {
        this.Id = Guid.NewGuid();
    }
}