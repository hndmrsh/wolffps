using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfFPS_Level_Editor
{
    enum TileType
    {
        None, Door, Floor0
    }

    static class TileTypeMethods
    {
        #region TileType enum extensions
        public static string GetImageResourceName(this TileType type)
        {
            switch (type)
            {
                case TileType.Door:
                    return "door";
                case TileType.Floor0:
                    return "stone";
                default:
                    return null;
            }
        }

        public static char GetCharRepresentation(this TileType type)
        {
            switch (type)
            {
                case TileType.Door:
                    return 'D';
                case TileType.Floor0:
                    return '0';
                default:
                    return ' ';
            }
        }
        #endregion

        public static TileType GetTileTypeForChar(char c)
        {
            switch (c)
            {
                case 'D':
                    return TileType.Door;
                case '0':
                    return TileType.Floor0;
                default:
                    return TileType.None;
            }
        }

    }
}
