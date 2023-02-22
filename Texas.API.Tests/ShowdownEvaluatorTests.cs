using Texas.API.Interfaces;
using Texas.API.Logic;
using Texas.API.Tests.Helper;

namespace Texas.API.Tests;

public class ShowdownEvaluatorTests
{
    private readonly ShowdownEvaluator _showdownEvaluator;

    public ShowdownEvaluatorTests()
    {
        _showdownEvaluator = new ShowdownEvaluator();
    }

    [Fact]
    public void ShowdownEvaluator_IncorrectArguments()
    {
        var exception = Record.Exception(() => _showdownEvaluator.EvaluateWinner(null, null));
        Assert.IsType<ArgumentException>(exception);

        exception = Record.Exception(() => _showdownEvaluator.EvaluateWinner(new ICard[4], new IPlayerHole[2]));
        Assert.IsType<ArgumentException>(exception);
    }

    [Theory]
    [InlineData("T8532", new[] { "96", "94" }, 0)]
    [InlineData("T9752", new[] { "K6", "J3" }, 0)]
    [InlineData("KJ853", new[] { "A2", "Q9" }, 0)]
    [InlineData("97542", new[] { "83", "83" }, 1)] // draw, special case
    public void ShowdownEvaluator_HighestCard(string community, string[] holes, int expectedWinnerIndex)
    {
        var communityCards = CardBuilder.ToCommunityCards(community).ToList();
        var playerHoles = CardBuilder.ToPlayerHoles(holes).ToList();

        var winners = _showdownEvaluator.EvaluateWinner(communityCards, playerHoles);
        Assert.Contains(playerHoles[expectedWinnerIndex], winners);
    }

    [Theory]
    [InlineData("T8532", new[] { "AT", "KT" }, 0)]
    [InlineData("TT752", new[] { "K6", "J3" }, 0)]
    [InlineData("J8532", new[] { "K8", "K2" }, 0)]
    [InlineData("QQ954", new[] { "T3", "K2" }, 1)]
    [InlineData("KT953", new[] { "T6", "A9" }, 0)]
    [InlineData("QJ932", new[] { "A2", "Q4" }, 1)]
    public void ShowdownEvaluator_OnePair(string community, string[] holes, int expectedWinnerIndex)
    {
        var communityCards = CardBuilder.ToCommunityCards(community).ToList();
        var playerHoles = CardBuilder.ToPlayerHoles(holes).ToList();

        var winners = _showdownEvaluator.EvaluateWinner(communityCards, playerHoles);
        Assert.Contains(playerHoles[expectedWinnerIndex], winners);
    }

    [Theory]
    [InlineData("T8532", new[] { "TA", "23" }, 1)]
    [InlineData("TT752", new[] { "QJ", "K2" }, 1)]
    [InlineData("J8532", new[] { "J8", "A2" }, 0)]
    [InlineData("QQ954", new[] { "TT", "JJ" }, 1)]
    [InlineData("KT953", new[] { "K5", "K3" }, 0)]
    [InlineData("QJ922", new[] { "QJ", "Q9" }, 0)]
    public void ShowdownEvaluator_TwoPair(string community, string[] holes, int expectedWinnerIndex)
    {
        var communityCards = CardBuilder.ToCommunityCards(community).ToList();
        var playerHoles = CardBuilder.ToPlayerHoles(holes).ToList();

        var winners = _showdownEvaluator.EvaluateWinner(communityCards, playerHoles);
        Assert.Contains(playerHoles[expectedWinnerIndex], winners);
    }

    [Theory]
    [InlineData("TT532", new[] { "TA", "TK" }, 0)]
    [InlineData("A9722", new[] { "23", "A3" }, 0)]
    [InlineData("JT532", new[] { "TT", "JJ" }, 1)]
    [InlineData("QQ954", new[] { "Q8", "Q7" }, 0)]
    [InlineData("KT953", new[] { "KA", "33" }, 1)]
    [InlineData("QJ922", new[] { "2T", "QQ" }, 1)]
    public void ShowdownEvaluator_ThreeOfKind(string community, string[] holes, int expectedWinnerIndex)
    {
        var communityCards = CardBuilder.ToCommunityCards(community).ToList();
        var playerHoles = CardBuilder.ToPlayerHoles(holes).ToList();

        var winners = _showdownEvaluator.EvaluateWinner(communityCards, playerHoles);
        Assert.Contains(playerHoles[expectedWinnerIndex], winners);
    }

    [Theory]
    [InlineData("AKK22", new[] { "22", "AK" }, 0)]
    [InlineData("A9722", new[] { "22", "AA" }, 0)]
    [InlineData("JJTT5", new[] { "TT", "JJ" }, 1)]
    [InlineData("QQ944", new[] { "44", "QA" }, 0)]
    [InlineData("KT933", new[] { "KT", "33" }, 1)]
    [InlineData("QJ922", new[] { "2Q", "22" }, 1)]
    [InlineData("QQQJJ", new[] { "QA", "QJ" }, 0)]
    public void ShowdownEvaluator_FourOfKind(string community, string[] holes, int expectedWinnerIndex)
    {
        var communityCards = CardBuilder.ToCommunityCards(community).ToList();
        var playerHoles = CardBuilder.ToPlayerHoles(holes).ToList();

        var winners = _showdownEvaluator.EvaluateWinner(communityCards, playerHoles);
        Assert.Contains(playerHoles[expectedWinnerIndex], winners);
    }

    [Theory]
    [InlineData("AK322", new[] { "32", "33" }, 1)]
    [InlineData("A9322", new[] { "33", "A2" }, 0)]
    [InlineData("AKK22", new[] { "AA", "22" }, 1)]
    [InlineData("QQ943", new[] { "Q4", "Q3" }, 0)]
    [InlineData("KTT33", new[] { "KT", "T3" }, 0)]
    [InlineData("QQQJJ", new[] { "KT", "AK" }, 0)] // special case, draw
    public void ShowdownEvaluator_FullHouse(string community, string[] holes, int expectedWinnerIndex)
    {
        var communityCards = CardBuilder.ToCommunityCards(community).ToList();
        var playerHoles = CardBuilder.ToPlayerHoles(holes).ToList();

        var winners = _showdownEvaluator.EvaluateWinner(communityCards, playerHoles);
        Assert.Contains(playerHoles[expectedWinnerIndex], winners);
    }

    [Theory]
    [InlineData("A5322", new[] { "46", "KQ" }, 0)]
    [InlineData("A4322", new[] { "53", "K2" }, 0)]
    [InlineData("AK743", new[] { "52", "AA" }, 0)]
    [InlineData("A7543", new[] { "K2", "62" }, 1)]
    [InlineData("T8652", new[] { "97", "J9" }, 0)]
    [InlineData("KQQJJ", new[] { "T9", "AK" }, 0)]
    [InlineData("AKQ22", new[] { "JT", "Q2" }, 1)]
    [InlineData("AAQ54", new[] { "AK", "32" }, 1)]
    public void ShowdownEvaluator_Straight(string community, string[] holes, int expectedWinnerIndex)
    {
        var communityCards = CardBuilder.ToCommunityCards(community).ToList();
        var playerHoles = CardBuilder.ToPlayerHoles(holes).ToList();

        var winners = _showdownEvaluator.EvaluateWinner(communityCards, playerHoles);
        Assert.Contains(playerHoles[expectedWinnerIndex], winners);
    }

    [Theory]
    [InlineData("AKT72", "HHSSS", new[] { "4C6D", "QS5S" }, 1)]
    [InlineData("AKT72", "HHSSS", new[] { "TC6D", "QS5S" }, 1)]
    [InlineData("AKT72", "HHSSS", new[] { "TCKD", "QS5S" }, 1)]
    [InlineData("AKT72", "HHSSS", new[] { "ACAD", "QS5S" }, 1)]
    [InlineData("AKT72", "HHSSS", new[] { "QCJD", "QS5S" }, 1)]
    [InlineData("AKT72", "HHSSS", new[] { "JS6S", "QS5S" }, 1)]
    [InlineData("AKT22", "HHSSS", new[] { "TC2D", "QS5S" }, 0)]
    [InlineData("AKT22", "HHSSS", new[] { "2C2D", "QS5S" }, 0)]
    [InlineData("AKT76", "HHSSS", new[] { "9S8S", "QS5S" }, 0)]
    public void ShowdownEvaluator_Flush(string community, string communityColors, string[] holes, int expectedWinnerIndex)
    {
        var communityCards = CardBuilder.ToCommunityCards(community, communityColors).ToList();
        var playerHoles = CardBuilder.ToPlayerHolesWithColors(holes).ToList();

        var winners = _showdownEvaluator.EvaluateWinner(communityCards, playerHoles);
        Assert.Contains(playerHoles[expectedWinnerIndex], winners);
    }

    [Theory]
    [InlineData("A5332", "CCCHD", new[] { "4C2C", "KSQD" }, 0)]
    [InlineData("A5332", "CCCHD", new[] { "4C2C", "5SQD" }, 0)]
    [InlineData("A5332", "CCCHD", new[] { "4C2C", "AS5D" }, 0)]
    [InlineData("A5332", "CCCHD", new[] { "4C2C", "7S2H" }, 0)]
    [InlineData("A5332", "CCCHD", new[] { "4C2C", "4SKD" }, 0)]
    [InlineData("A5332", "CCCHD", new[] { "4C2C", "KCQC" }, 0)]
    [InlineData("A5332", "CCCHD", new[] { "4C2C", "AS2H" }, 0)]
    [InlineData("AA533", "CHCCD", new[] { "4C2C", "ASAD" }, 0)]
    [InlineData("AQJT4", "DCCCH", new[] { "9C8C", "ACKC" }, 1)]
    [InlineData("AKQJT", "SCCCC", new[] { "9C8C", "AC2D" }, 1)]
    public void ShowdownEvaluator_Poker(string community, string communityColors, string[] holes, int expectedWinnerIndex)
    {
        var communityCards = CardBuilder.ToCommunityCards(community, communityColors).ToList();
        var playerHoles = CardBuilder.ToPlayerHolesWithColors(holes).ToList();

        var winners = _showdownEvaluator.EvaluateWinner(communityCards, playerHoles);
        Assert.Contains(playerHoles[expectedWinnerIndex], winners);
    }

    [Theory]
    [InlineData("AQT52", "CCCHD", new[] { "KSJD", "ADQH", "THTD", "3S2H" }, 0)]
    [InlineData("AQT52", "CCCHD", new[] { "KSJD", "7C9C", "THTD", "3SAH" }, 1)]
    [InlineData("AQT52", "CCCHD", new[] { "KSTD", "7C9D", "THTS", "3SAH" }, 2)]
    [InlineData("AQQ52", "CCSHD", new[] { "KSJD", "7C9C", "THTD", "QD2H" }, 3)]
    [InlineData("KQJ33", "CCCHD", new[] { "ACTC", "3C3S", "KSKH", "7C6C" }, 0)]
    [InlineData("AKK53", "CHDCC", new[] { "KCKS", "ASAH", "4H2S", "4C2C" }, 3)]
    public void ShowdownEvaluator_FourPlayers(string community, string communityColors, string[] holes, int expectedWinnerIndex)
    {
        var communityCards = CardBuilder.ToCommunityCards(community, communityColors).ToList();
        var playerHoles = CardBuilder.ToPlayerHolesWithColors(holes).ToList();

        var winners = _showdownEvaluator.EvaluateWinner(communityCards, playerHoles);
        Assert.Contains(playerHoles[expectedWinnerIndex], winners);
    }
}
