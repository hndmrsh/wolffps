using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfFPS_Level_Editor
{
    class InputOutputUtil
    {

        public static void Save(Level level)
        {
            int levelWidth = level.MaxX - level.MinX + 1;
            int levelHeight = level.MaxY - level.MinY + 1;

            string[] file = new string[3 + (level.MaxY - level.MinY + 1)];
            file[0] = (levelWidth) + "," + (levelHeight);
            file[1] = "1"; // TODO support more texture types
            file[2] = "stone"; // TODO support more texture types


            for (int y = level.MinY; y <= level.MaxY; y++)
            {
                string row = "";

                for (int x = level.MinX; x <= level.MaxX; x++)
                {
                    TileType type = level.GetTileTypeForTag(new TileTag()
                    {
                        X = x,
                        Y = y
                    });
                    row += type.GetCharRepresentation();
                }

                file[(y - level.MinY) + 3] = row;
            }

            System.IO.File.WriteAllLines(@"E:\Projects\Unity\WolfFPS\Assets\Levels\generated_test.lvl", file);
        }

    }
}
