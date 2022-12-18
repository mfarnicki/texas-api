using Texas.API.Interfaces;

namespace Texas.API.Logic
{
    public class ShowdownEvaluator
    {
        private readonly int[] cardWeight = new int[15];

        private readonly int[] smallStraight = new[] { 9, 1, 1, 1 };

        private readonly int[] bigStraight = new[] { 1, 1, 1, 1 };

        private readonly int[] flushMul = new[] { 1, 32, 243 };

        public ShowdownEvaluator()
        {
            cardWeight[0] = 0;
            for (int i = 1; i < 15; i++)
            {
                cardWeight[i] = cardWeight[i - 1] * 2 + 1;
            }
        }

        public IList<IPlayerHole> EvaluateWinner(IList<ICard> communityCards, IList<IPlayerHole> playerHoles)
        {
            var scoresMap = new List<KeyValuePair<int, IPlayerHole>>();

            if (playerHoles == null || !playerHoles.Any())
            {
                throw new ArgumentException(nameof(playerHoles));
            }

            if (communityCards == null || communityCards.Count != 5)
            {
                throw new ArgumentException(nameof(communityCards));
            }

            foreach (var hole in playerHoles)
            {
                var score = this.EvaluateScore(communityCards, hole.HoleCard1, hole.HoleCard2);
                scoresMap.Add(new KeyValuePair<int, IPlayerHole>(score, hole));
            }

            return scoresMap.Where(m => m.Key == scoresMap.Max(m => m.Key)).Select(m => m.Value).ToList();
        }

        private int EvaluateScore(IList<ICard> communityCards, ICard holeCard1, ICard holeCard2)
        {
            var setScores = new List<int>();
            setScores.Add(EvaluateSetScore(communityCards));

            for (int i = 0; i < 5; i++)
            {
                var community = new List<ICard>(communityCards);
                community[i] = holeCard1;
                setScores.Add(EvaluateSetScore(community));
            }

            for (int i = 0; i < 5; i++)
            {
                var community = new List<ICard>(communityCards);
                community[i] = holeCard2;
                setScores.Add(EvaluateSetScore(community));
            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (i >= j) continue;
                    var community = new List<ICard>(communityCards);
                    community[i] = holeCard1;
                    community[j] = holeCard2;
                    setScores.Add(EvaluateSetScore(community));
                }
            }

            return setScores.Max();
        }

        private int EvaluateSetScore(IList<ICard> cardSet)
        {
            var cardValues = cardSet.Sum(c => cardWeight[c.Value]);
            var isFlush = cardSet.Sum(c => (int)c.Suit) == 0 || flushMul.Contains(cardSet.Aggregate(1, (acc, card) => acc * (int)card.Suit));

            var orderedSet = cardSet.OrderByDescending(c => c.Value).ToList();

            var differences = new int[4];
            for (int i = 1; i < 5; i++)
            {
                differences[i - 1] = orderedSet[i - 1].Value - orderedSet[i].Value;
            }

            if (differences.SequenceEqual(smallStraight))
            {
                // A, 2, 3, 4, 5 straight
                // value of ace equals 1 in such case
                cardValues -= cardWeight[14] + cardWeight[1];
                return (isFlush ? 20_000_000 : 5_000_000) + cardValues;
            }

            if (differences.SequenceEqual(bigStraight))
            {
                // straight with/without flush
                return (isFlush ? 20_000_000 : 5_000_000) + cardValues;
            }

            if (isFlush)
            {
                // flush
                return 6_000_000 + cardValues;
            }

            var cardGroups = orderedSet.GroupBy(card => card.Value, card => card, (key, cards) => cards).OrderByDescending(g => g.Count());
            var groupsCount = cardGroups.Count();
            var firstGroupCount = cardGroups.First().Count();
            var firstGroupValue = cardGroups.First().First().Value;
            var bonus = firstGroupValue * 50_000;

            if (groupsCount == 2)
            {
                if (firstGroupCount == 4)
                {
                    // four of a kind
                    return 15_000_000 + bonus + cardValues;
                }
                else
                {
                    // flush!
                    return 7_000_000 + bonus + cardValues;
                }
            }

            if (groupsCount == 3)
            {
                if (firstGroupCount == 3)
                {
                    // three of a kind
                    return 3_000_000 + bonus + cardValues;
                }
                else
                {
                    // two pair
                    return 2_000_000 + bonus + cardValues;
                }
            }

            if (groupsCount == 4)
            {
                // one pair
                return 1_000_000 + bonus + cardValues;
            }

            return cardValues;
        }
    }
}