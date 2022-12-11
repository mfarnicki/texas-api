namespace Texas.API.Interfaces
{
    public interface IGamesRepository
    {
        Task<IGame> NewGame();

        Task<IEnumerable<IGame>> GetAllGames();

        Task<IGame> DeleteGame(Guid id);
    }
}
