using Texas.API.Data;
using Texas.API.Interfaces;
using Texas.API.Models;

namespace Texas.API
{
    static class CompositionRoot
    {
        public static void AddCompositionRoot(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<ICosmosService>(options =>
            {
                var cosmosDbConnectionString = builder.Configuration.GetConnectionString("CosmosDb");
                return new CosmosService(cosmosDbConnectionString);
            });

            builder.Services.AddScoped<IGamesRepository, GamesRepository>();
            builder.Services.AddSingleton<IGameManager, GameManager>();
        }
    }
}