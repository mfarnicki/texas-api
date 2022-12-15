using Texas.API.Models;

public class Game : IGame
{
    private readonly CardDeck _cardDeck;
    public string Id { get; set; }

    public IPlayer[] Players { get; private set; }

    public GameStatus Status { get; private set; }

    public Game()
    {
        this.Id = Guid.NewGuid().ToString();
        this.Players = new IPlayer[4];
        this.Status = GameStatus.Idle;
        _cardDeck = new CardDeck();
    }

    public void Start()
    {
        this.Status = GameStatus.Active;
        _cardDeck.ShuffleDeck();
    }

    public bool HasPlayer(string playerId, out IPlayer player)
    {
        foreach (var currentPlayer in this.Players)
        {
            if (currentPlayer?.PlayerId == playerId)
            {
                player = currentPlayer;
                return true;
            }
        }

        player = null;
        return false;
    }

    public void RemovePlayer(string playerId)
    {
        for (int i = 0; i < 4; i++)
        {
            if (this.Players[i]?.PlayerId == playerId)
            {
                this.Players[i] = null;
            }
        }
    }

    public bool AssignPlayer(IPlayer newPlayer, int playerPosition)
    {
        return this.Players[playerPosition] == null && (this.Players[playerPosition] = newPlayer) != null;
    }

    public ICard[] Deck
    {
        get => _cardDeck.Deck;
    }
}