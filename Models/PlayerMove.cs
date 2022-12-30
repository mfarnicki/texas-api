using Texas.API.Interfaces;

namespace Texas.API.Models
{
    public class PlayerMove : IPlayerMove
    {
        public string PlayerId { get; set; }

        public MoveType Move { get; set; }

        public int Amount { get; set; }
    }
}