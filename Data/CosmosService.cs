using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace Texas.API.Data
{
    public class CosmosService : ICosmosService
    {
        private const string DatabaseId = "texas";
        private const string ContainerId = "games";
        private readonly Container _container;

        public CosmosService(string connectionString)
        {
            var client = new CosmosClient(connectionString);
            this._container = client.GetContainer(DatabaseId, ContainerId);
        }

        public async Task Upsert<T>(T item) where T : IGame
        {
            await this._container.UpsertItemAsync<T>(item, new PartitionKey(item.Id.ToString()));
        }

        public async Task<T> Read<T>(Guid id) where T : IGame
        {
            var idString = id.ToString();
            var item = await this._container.ReadItemAsync<T>(idString, new PartitionKey(idString));

            return item.Resource;
        }

        public async Task<T> Delete<T>(Guid id) where T : IGame
        {
            var idString = id.ToString();
            return await this._container.DeleteItemAsync<T>(idString, new PartitionKey(idString));
        }

        public async Task<IEnumerable<T>> ReadAll<T>() where T : IGame
        {
            var allItems = new List<T>();

            var queryable = this._container.GetItemLinqQueryable<T>();
            var iterator = queryable.ToFeedIterator();
            foreach (var item in await iterator.ReadNextAsync())
            {
                allItems.Add(item);
            }

            return allItems;
        }
    }
}