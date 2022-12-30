using Texas.API.Interfaces;

namespace Texas.API.Models
{
    public class Player : IPlayer
    {
        public Player(string id, int chips)
        {
            this.Id = id;
            this.Chips = chips;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public PlayerStatus Status { get; set; }

        public int Chips { get; private set; }

        public int CurrentBet { get; private set; }

        public void ClearBet()
        {
            this.CurrentBet = 0;
        }

        public int PlaceBet(int amount)
        {
            this.Chips -= amount;
            this.CurrentBet += amount;

            return amount;
        }

        public void Payout(int amount)
        {
            this.Chips += amount;
        }
    }
}