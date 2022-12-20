using Texas.API.Interfaces;
using Texas.API.Models;

namespace Texas.API.Logic
{
    public class Dealer : IDealer
    {
        private readonly ShowdownEvaluator _evaluator = new ShowdownEvaluator();

        private CardDeck _cardDeck;

        public IGame Game { get; }

        public IPlayerHole[] PlayerHoles { get; private set; }

        public Dealer(IGame game)
        {
            this.Game = game;
        }

        public void StartGame()
        {
            _cardDeck = new CardDeck();
            _cardDeck.ShuffleDeck();

            this.PlayerHoles = new IPlayerHole[4];
            _cardDeck.DealHoles(this.Game.Players, this.PlayerHoles);

            this.Game.Status = GameStatus.Preflop;
        }

        public void ProgressGame()
        {
            switch (this.Game.Status)
            {
                case GameStatus.Preflop:
                    _cardDeck.DealFlop(this.Game.CommunityCards);
                    this.Game.Status = GameStatus.Flop;

                    break;

                case GameStatus.Flop:
                    _cardDeck.DealTurn(this.Game.CommunityCards);
                    this.Game.Status = GameStatus.Turn;

                    break;

                case GameStatus.Turn:
                    _cardDeck.DealRiver(this.Game.CommunityCards);
                    this.Game.Status = GameStatus.River;

                    break;

                case GameStatus.River:
                    this.Game.Status = GameStatus.Final;

                    break;
            }
        }

        public bool Showdown()
        {
            if (this.Game.Status == GameStatus.Final)
            {
                var winners = _evaluator.EvaluateWinner(this.Game.CommunityCards, this.PlayerHoles);
                foreach (var winner in winners)
                {
                    this.Game.HasPlayer(winner.PlayerId, out var player);
                    player.PlayerStatus = PlayerStatus.Winner;
                }

                return true;
            }

            return false;
        }

        public void NextRound()
        {
            _cardDeck = null;
            this.PlayerHoles = new IPlayerHole[4];
            this.Game.NextRound();
        }
    }
}