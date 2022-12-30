using Texas.API.Interfaces;
using Texas.API.Models;

namespace Texas.API.Logic
{
    public class CardDeck
    {
        private Stack<ICard> _deck;
        private readonly Random _random;

        public CardDeck()
        {
            _deck = new Stack<ICard>(52);
            _random = new Random();
            for (byte suit = 0; suit < 4; suit++)
            {
                var cardSuit = (Suit)suit;
                for (byte value = 2; value <= 14; value++)
                {
                    _deck.Push(new Card(cardSuit, value));
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
            var localDeck = _deck.ToArray();
            for (int i = 0; i < 52; i++)
            {
                var rand = _random.Next(52);
                var card = localDeck[rand];
                localDeck[rand] = localDeck[i];
                localDeck[i] = card;
            }

            _deck = new Stack<ICard>(localDeck);
        }

        private void CutDeck()
        {
            var cut = _random.Next(52);
            var localDeck = _deck.ToArray();
            for (int i = 0; i < 52; i++)
            {
                var normCut = (i + cut) % 52;
                var card = localDeck[normCut];
                localDeck[normCut] = localDeck[i];
                localDeck[i] = card;
            }

            _deck = new Stack<ICard>(localDeck);
        }

        internal void DealHoles(IPlayer[] players, IPlayerHole[] holes)
        {
            for (int i = 0; i < 4; i++)
            {
                if (players[i] == null)
                {
                    continue;
                }

                var holeCard1 = _deck.Pop();
                var holeCard2 = _deck.Pop();

                holes[i] = new PlayerHole(players[i].Id, holeCard1, holeCard2);

                players[i].Status = PlayerStatus.Waiting;
            }
        }

        internal void DealFlop(ICard[] communityCards)
        {
            _deck.Pop();
            communityCards[0] = _deck.Pop();
            communityCards[1] = _deck.Pop();
            communityCards[2] = _deck.Pop();
        }

        internal void DealTurn(ICard[] communityCards)
        {
            _deck.Pop();
            communityCards[3] = _deck.Pop();
        }

        internal void DealRiver(ICard[] communityCards)
        {
            _deck.Pop();
            communityCards[4] = _deck.Pop();
        }
    }
}