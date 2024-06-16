using Views;

namespace Services.Map
{
    public class LevelSettings
    {
        public LevelSettings(MapView floor)
        {
            Floor = floor;
        }

        public MapView Floor { get; }
    }
}