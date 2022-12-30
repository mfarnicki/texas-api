namespace Texas.API.Interfaces
{
    public interface IGameManager
    {
        IGame[] GetAllGames();
        IDealer StartGame(string gameId);
        IDealer ProgressGame(string gameId);
        IDealer PlayerMove(string gameId, IPlayerMove move);
        IDealer NextRound(string gameId);
        IDealer InitGame(string gameId);
        bool TryAddPlayer(string gameId, int playerPosition, IPlayer player, out IGame game);
        IGame PlayerLeave(string playerId);
        bool DeleteGame(string gameId);
    }
}