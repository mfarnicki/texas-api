namespace Texas.API.Models
{
    public class CardDeck
    {
        private readonly ICard[] _deck;
        private readonly Random _random;

        public CardDeck()
        {
            _deck = new ICard[52];
            _random = new Random();
            for (byte suit = 0; suit < 4; suit++)
            {
                var cardSuit = (Suit)suit;
                for (byte value = 1; value <= 13; value++)
                {
                    _deck[suit * 13 + value - 1] = new Card(cardSuit, value);
                }
            }
        }

        public ICard[] Deck
        {
            get => this._deck;
        }

        public void ShuffleDeck()
        {
            this.SingleShuffle();
            this.SingleShuffle();
            this.CutDeck();
            this.SingleShuffle();
        }

        private void SingleShuffle()
        {
            for (int i = 0; i < 52; i++)
            {
                var rand = _random.Next(52);
                var card = _deck[rand];
                _deck[rand] = _deck[i];
                _deck[i] = card;
            }
        }

        private void CutDeck()
        {
            var cut = _random.Next(52);
            for (int i = 0; i < 52; i++)
            {
                var normCut = (i + cut) % 52;
                var card = _deck[normCut];
                _deck[normCut] = _deck[i];
                _deck[i] = card;
            }
        }
    }
}