using Texas.API.Interfaces;
using Texas.API.Models;

namespace Texas.API.Tests.Helper
{
    public static class CardBuilder
    {
        public static IEnumerable<ICard> ToCommunityCards(string community, string colors = null)
        {
            if (community.Length != 5)
            {
                throw new ArgumentException(nameof(community));
            }

            for (int i = 0; i < community.Length; i++)
            {
                var suit = colors != null ? ToSuit(colors[i]) : (Suit)(i % 4);
                yield return ToCard(suit, community[i]);
            }
        }

        public static IEnumerable<IPlayerHole> ToPlayerHoles(string[] holes)
        {
            for (int i = 0; i < holes.Length; i++)
            {
                var suit = (Suit)i;
                yield return new PlayerHole($"Player_{i}", ToCard(suit, holes[i][0]), ToCard(suit, holes[i][1]));
            }
        }

        public static IEnumerable<IPlayerHole> ToPlayerHolesWithColors(string[] holes)
        {
            for (int i = 0; i < holes.Length; i++)
            {
                yield return new PlayerHole($"Player_{i}", ToCard(ToSuit(holes[i][1]), holes[i][0]), ToCard(ToSuit(holes[i][3]), holes[i][2]));
            }
        }

        private static ICard ToCard(Suit suit, char value)
        {
            switch (value)
            {
                case 'T':
                    return new Card(suit, 10);

                case 'J':
                    return new Card(suit, 11);

                case 'Q':
                    return new Card(suit, 12);

                case 'K':
                    return new Card(suit, 13);

                case 'A':
                    return new Card(suit, 14);

                default:
                    if (byte.TryParse(value.ToString(), out var byteValue))
                    {
                        return new Card(suit, byteValue);
                    }

                    throw new ArgumentException(nameof(value), "Invalid value");
            }
        }

        private static Suit ToSuit(char color)
        {
            switch (color)
            {
                case 'S':
                    return Suit.Spade;

                case 'D':
                    return Suit.Diamond;

                case 'H':
                    return Suit.Heart;

                case 'C':
                    return Suit.Club;

                default:
                    throw new ArgumentException(nameof(color), " Invalid color");
            }
        }
    }
}