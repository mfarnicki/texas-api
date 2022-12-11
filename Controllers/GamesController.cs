using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly IGamesContainer gamesContainer;

    public GamesController(IGamesContainer container)
    {
        this.gamesContainer = container;
    }   

    [HttpGet]
    public async Task<IActionResult> GetAllGames()
    {
        var games = await this.gamesContainer.GetAllGames();
        return Ok(games);
    }

    [HttpPost]
    public async Task<IActionResult> AddNewGame()
    {
        var game = await this.gamesContainer.NewGame();
        return Ok(game);
    }
}
