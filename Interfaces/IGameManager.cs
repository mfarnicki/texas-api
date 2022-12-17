namespace Texas.API.Interfaces
{
    public interface IGameManager
    {
        IGame[] GetAllGames();
        IDealer StartGame(string gameId);
        IGame ProgressGame(string gameId);
        IGame InitGame(string gameId);
        bool TryAddPlayer(string gameId, int playerPosition, string playerName, string connectionId, out IGame game);
        IGame PlayerLeave(string playerId);
        bool DeleteGame(string gameId);
    }
}