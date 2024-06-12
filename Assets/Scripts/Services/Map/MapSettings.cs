using UnityEngine;
using Views;

namespace Services.Map
{
    public class MapSettings
    {
        public MapSettings(MapView floor)
        {
            Floor = floor;
        }

        public MapView Floor { get; }
    }
}