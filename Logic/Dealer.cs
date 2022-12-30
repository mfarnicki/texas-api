using Texas.API.Interfaces;

namespace Texas.API.Logic
{
    public class Dealer : IDealer
    {
        private readonly ShowdownEvaluator _evaluator = new ShowdownEvaluator();

        private PotManager _potManager;

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

            _potManager = new PotManager(this.Game);
            _potManager.Blinds();

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

        public void PlayerMove(IPlayerMove playerMove)
        {
            if (this.Game.HasPlayer(playerMove.PlayerId, out var player))
            {
                switch (playerMove.Move)
                {
                    case MoveType.Bet:
                        _potManager.Bet(player, playerMove.Amount);
                        break;

                    case MoveType.Call:
                        _potManager.Call(player);
                        break;

                    case MoveType.Fold:
                        _potManager.Fold(player);
                        break;
                }
            }

            if (this.Game.NextStage())
            {
                foreach (var p in this.Game.Players)
                {
                    if (p != null)
                    {
                        p.ClearBet();
                    }
                }

                this.ProgressGame();
            }
        }

        public bool Showdown()
        {
            if (this.Game.Players.Count(p => p?.Status == PlayerStatus.Fold) == (this.Game.Players.Count(p => p != null) - 1))
            {
                var winner = this.Game.Players.Single(p => p != null && p.Status != PlayerStatus.Fold);
                winner.Status = PlayerStatus.Won;
                _potManager.Payout(winner, 1);

                return true;
            }

            if (this.Game.Status == GameStatus.Final)
            {
                var winners = _evaluator.EvaluateWinner(this.Game.CommunityCards, this.PlayerHoles);
                foreach (var winner in winners)
                {
                    this.Game.HasPlayer(winner.PlayerId, out var player);
                    player.Status = PlayerStatus.Won;
                    _potManager.Payout(player, winners.Count);
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