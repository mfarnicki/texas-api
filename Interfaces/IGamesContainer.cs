public interface IGamesContainer
{
    Task<IGame> NewGame();

    Task<string> GetAllGames();

    Task ClearGames();
}