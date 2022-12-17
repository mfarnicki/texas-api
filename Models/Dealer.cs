using Texas.API.Interfaces;

namespace Texas.API.Models
{
    public class Dealer : IDealer
    {
        private CardDeck _cardDeck;

        public IGame Game { get; }

        public IList<IPlayerHole> PlayerHoles { get; private set; }

        public Dealer(IGame game)
        {
            this.Game = game;
        }

        public void StartGame()
        {
            _cardDeck = new CardDeck();
            _cardDeck.ShuffleDeck();

            this.PlayerHoles = new List<IPlayerHole>();
            var holes = _cardDeck.DealHoles(this.Game.Players.Count(p => p != null));

            var i = 0;
            foreach (var player in this.Game.Players)
            {
                if (player == null)
                {
                    continue;
                }

                this.PlayerHoles.Add(new PlayerHole(player.PlayerId, holes[i], holes[i + 1]));
                i += 2;
            }

            this.Game.Status = GameStatus.Preflop;
        }

        public void ProgressGame()
        {
            switch (this.Game.Status)
            {
                case GameStatus.Preflop:
                    var flop = _cardDeck.DealFlop();
                    this.Game.CommunityCards[0] = flop[0];
                    this.Game.CommunityCards[1] = flop[1];
                    this.Game.CommunityCards[2] = flop[2];
                    this.Game.Status = GameStatus.Flop;

                    break;

                case GameStatus.Flop:
                    var turn = _cardDeck.DealTurn();
                    this.Game.CommunityCards[3] = turn;
                    this.Game.Status = GameStatus.Turn;

                    break;

                case GameStatus.Turn:
                    var river = _cardDeck.DealRiver();
                    this.Game.CommunityCards[4] = river;
                    this.Game.Status = GameStatus.River;

                    break;

                case GameStatus.River:
                    // ToDo: winner
                    this.Game.Status = GameStatus.Final;

                    break;
            }
        }

        public string Showdown()
        {
            if (this.Game.Status == GameStatus.Final)
            {
                //todo
                //calculate winner
                return "winnerId";
            }

            return null;
        }

        public void ResetGame()
        {
            _cardDeck = null;
            this.PlayerHoles = null;
            this.Game.Reset();
        }
    }
}