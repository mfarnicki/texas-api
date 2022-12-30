namespace Texas.API.Interfaces
{
    public interface IDealer
    {
        IGame Game { get; }

        IPlayerHole[] PlayerHoles { get; }

        void StartGame();

        void ProgressGame();

        void PlayerMove(IPlayerMove playerMove);

        bool Showdown();

        void NextRound();
    }
}