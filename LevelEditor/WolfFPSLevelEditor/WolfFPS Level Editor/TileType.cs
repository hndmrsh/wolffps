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

    }
}
