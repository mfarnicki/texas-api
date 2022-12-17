namespace Texas.API.Models
{
    public class CardDeck
    {
        private ICard[] _deck;
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

        internal ICard[] DealHoles(int playersCount)
        {
            var result = new ICard[playersCount];
            for (int i = 0; i < playersCount * 2; i += 2)
            {
                result[i] = _deck[i];
                result[i + 1] = _deck[playersCount + i];
            }

            _deck = _deck.Skip(playersCount * 2).ToArray();
            return result;
        }

        internal ICard[] DealFlop()
        {
            var result = new ICard[3];
            result[0] = _deck[1];
            result[1] = _deck[2];
            result[2] = _deck[3];

            _deck = _deck.Skip(4).ToArray();
            return result;
        }

        internal ICard DealTurn()
        {
            var result = _deck[1];

            _deck = _deck.Skip(2).ToArray();
            return result;
        }

        internal ICard DealRiver()
        {
            return this.DealTurn();
        }
    }
}