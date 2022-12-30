using Texas.API.Interfaces;

namespace Texas.API.Logic
{
    public class PotManager
    {
        private const int SmallBlind = 10;
        private readonly IGame _game;

        public PotManager(IGame game)
        {
            this._game = game;
            this._game.CurrentPot = 0;
            this._game.HighestBet = 0;
        }

        public void Blinds()
        {
            var smallBlindPlayer = this._game.NextPlayer(this._game.DealerId);
            this._game.CurrentPot += smallBlindPlayer.PlaceBet(SmallBlind);
            this._game.HighestBet = SmallBlind;

            this.WaitForNext(smallBlindPlayer.Id);
        }

        public void Bet(IPlayer player, int amount)
        {
            var bet = player.PlaceBet(amount);
            this._game.HighestBet = bet;
            this._game.CurrentPot += bet;

            this.WaitForNext(player.Id);
        }

        public void Call(IPlayer player)
        {
            var toCall = this._game.HighestBet - player.CurrentBet;
            this._game.CurrentPot += player.PlaceBet(toCall);

            this.WaitForNext(player.Id);
        }

        public void Fold(IPlayer player)
        {
            player.ClearBet();
            player.Status = PlayerStatus.Fold;

            this.WaitForNext(player.Id);
        }

        public void Payout(IPlayer player, int numberOfWinners)
        {
            var amount = this._game.CurrentPot / numberOfWinners;
            player.Payout(amount);
        }

        private void WaitForNext(string playerId)
        {
            this._game.WaitingForId = this._game.NextPlayer(playerId)?.Id;
        }
    }
}