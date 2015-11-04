using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfFPS_Level_Editor
{
    class Level
    {
        public int MinX { get; set; }
        public int MinY { get; set; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }

        private Dictionary<TileTag, TileType> map;

        public Level()
        {
            MinX = int.MaxValue;
            MinY = int.MaxValue;
            MaxX = int.MinValue;
            MaxY = int.MinValue;
        }

        public TileType GetTileTypeForTag(TileTag tag)
        {
            TileType type = TileType.None;
            if (map.ContainsKey(tag))
            {
                map.TryGetValue(tag, out type);
            }

            return type;
        }

        public void SetTileTypeForTag(TileTag tag, TileType type)
        {
            if (map == null)
            {
                map = new Dictionary<TileTag, TileType>();
            }

            map.Add(tag, type);

            // re-calculate min/max
            MinX = Math.Min(MinX, tag.X);
            MinY = Math.Min(MinY, tag.Y);
            MaxX = Math.Max(MaxX, tag.X);
            MaxY = Math.Max(MaxY, tag.Y);
        }

    }
}
