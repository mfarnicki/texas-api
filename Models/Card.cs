namespace Texas.API.Models
{
    public struct Card : ICard
    {
        public Suit Suit { get; }

        public byte Value { get; }

        public Card(Suit suit, byte value)
        {
            if (value < 1 || value > 13)
            {
                throw new ArgumentException("Card value must be in [1,13] range.", nameof(value));
            }

            this.Suit = suit;
            this.Value = value;
        }
    }

    public enum Suit
    {
        Spade = 0,
        Heart = 1,
        Diamond = 2,
        Club = 3
    }

}