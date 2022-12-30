using Texas.API.Interfaces;

namespace Texas.API.Models
{
    public struct Card : ICard
    {
        public Suit Suit { get; }

        public byte Value { get; }

        public Card(Suit suit, byte value)
        {
            if (value < 2 || value > 14)
            {
                throw new ArgumentException("Card value must be in [2,14] range.", nameof(value));
            }

            this.Suit = suit;
            this.Value = value;
        }
    }
}