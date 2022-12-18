namespace Texas.API.Interfaces
{
    public interface IDealer
    {
        IGame Game { get; }

        IList<IPlayerHole> PlayerHoles { get; }

        void StartGame();

        void ProgressGame();

        bool Showdown();

        void ResetGame();
    }
}