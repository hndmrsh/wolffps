using System;
using System.Collections.Generic;
using System.IO;
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
            ResetMinsMaxs();
        }

        public TileType GetTileTypeForTag(TileTag tag)
        {
            TileType type = TileType.None;
            if (map != null && map.ContainsKey(tag))
            {
                map.TryGetValue(tag, out type);
            }

            return type;
        }

        public bool SetTileTypeForTag(TileTag tag, TileType type)
        {
            if (map == null)
            {
                map = new Dictionary<TileTag, TileType>();
            }

            bool changed = false;
            if (!map.ContainsKey(tag))
            {
                map.Add(tag, type);
                changed = true;
            }

            // re-calculate min/max
            MinX = Math.Min(MinX, tag.X);
            MinY = Math.Min(MinY, tag.Y);
            MaxX = Math.Max(MaxX, tag.X);
            MaxY = Math.Max(MaxY, tag.Y);

            return changed;
        }

        public bool DeleteTileAtTag(TileTag tag)
        {
            if (map != null && map.ContainsKey(tag))
            {
                map.Remove(tag);

                // check if the tile MAY have been on the outskirts of the level, which would require
                // us to re-determine the min/max x and y.
                if (tag.X == MinX || tag.X == MaxX || tag.Y == MinY || tag.Y == MaxY)
                {
                    RecalculateLevelBoundaries();
                }

                return true;
            }

            return false;
        }

        private void RecalculateLevelBoundaries()
        {
            ResetMinsMaxs();

            foreach (TileTag tag in map.Keys)
            {
                MinX = Math.Min(MinX, tag.X);
                MinY = Math.Min(MinY, tag.Y);
                MaxX = Math.Max(MaxX, tag.X);
                MaxY = Math.Max(MaxY, tag.Y);
            }
        }

        private void ResetMinsMaxs()
        {
            MinX = int.MaxValue;
            MinY = int.MaxValue;
            MaxX = int.MinValue;
            MaxY = int.MinValue;
        }

    }
}
