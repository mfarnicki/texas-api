namespace Texas.API.Data
{
    public interface ICosmosService
    {
        Task Upsert<T>(T item) where T : IGame;
        Task<T> Read<T>(Guid id) where T : IGame;
        Task<T> Delete<T>(Guid id) where T : IGame;
        Task<IEnumerable<T>> ReadAll<T>() where T : IGame;
    }
}