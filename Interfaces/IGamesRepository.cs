namespace Texas.API.Interfaces
{
    public interface IGamesRepository
    {
        Task<IGame> NewGame();

        Task<IEnumerable<IGame>> GetAllGames();

        Task<IGame> GetSingleGame(Guid id);

        Task<IGame> DeleteGame(Guid id);
    }
}
