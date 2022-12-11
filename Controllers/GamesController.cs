using Microsoft.AspNetCore.Mvc;
using Texas.API.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly IGamesRepository _gamesRepository;

    public GamesController(IGamesRepository gamesRepository)
    {
        this._gamesRepository = gamesRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGames()
    {
        var games = await this._gamesRepository.GetAllGames();
        return Ok(games);
    }

    [HttpPost]
    public async Task<IActionResult> AddGame()
    {
        var game = await this._gamesRepository.NewGame();
        return Ok(game);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteGame(Guid id)
    {
        var game = await this._gamesRepository.DeleteGame(id);
        return Ok(game);
    }
}
