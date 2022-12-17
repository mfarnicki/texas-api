using Texas.API.Interfaces;

namespace Texas.API.Models
{
    public struct PlayerHole : IPlayerHole
    {
        public string PlayerId { get; }
        public ICard HoleCard1 { get; }

        public ICard HoleCard2 { get; }

        public PlayerHole(string playerId, ICard card1, ICard card2)
        {
            this.PlayerId = playerId;
            this.HoleCard1 = card1;
            this.HoleCard2 = card2;
        }
    }
}