using Texas.API.Data;
using Texas.API.Interfaces;

namespace Texas.API.Models
{
    public class GamesRepository : IGamesRepository
    {
        private readonly ICosmosService _cosmos;

        public GamesRepository(ICosmosService cosmos)
        {
            this._cosmos = cosmos;
        }

        public async Task<IEnumerable<IGame>> GetAllGames()
        {
            return await this._cosmos.ReadAll<Game>();
        }

        public async Task<IGame> GetSingleGame(Guid id)
        {
            return await this._cosmos.Read<Game>(id);
        }

        public async Task<IGame> NewGame()
        {
            var newGame = new Game(Guid.NewGuid().ToString());
            await this._cosmos.Upsert<Game>(newGame);

            return newGame;
        }

        public async Task<IGame> DeleteGame(Guid id)
        {
            var game = await this._cosmos.Delete<Game>(id);
            return game;
        }
    }
}