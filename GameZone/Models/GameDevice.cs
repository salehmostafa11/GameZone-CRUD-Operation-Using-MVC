namespace GameZone.Models
{
    public class GameDevice
    {
        // composite primary key will created by fluant api
        public int GameId { get; set; }
        public Game Game { get; set; } = default!;

        public int DeviceId { get; set; }
        public Device Device { get; set; } = default!;
    }
}
