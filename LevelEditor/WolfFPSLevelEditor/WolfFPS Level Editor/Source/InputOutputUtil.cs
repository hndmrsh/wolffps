using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfFPS_Level_Editor
{
    class InputOutputUtil
    {

        public static void Save(Level level, string path)
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

            System.IO.File.WriteAllLines(path, file);
        }

        public static Level Load(string path)
        {
            StreamReader file = new StreamReader(path);
            Level level = new Level();

            // LINE 1: dimensions
            string[] levelDimens = file.ReadLine().Split(',');
            int w = int.Parse(levelDimens[0]);
            int h = int.Parse(levelDimens[1]);
            
            // LINE 2: number of materials
            int numMaterials = int.Parse(file.ReadLine());
            string[] materials = new string[numMaterials];

            // LINE 3--numMaterials: materials
            for (int m = 0; m < numMaterials; m++)
            {
                materials[m] = file.ReadLine();
            }

            // REMAINING LINES: level dimensions
            for (int y = 0; y < h; y++)
            {
                char[] line = file.ReadLine().ToCharArray();
                for (int x = 0; x < w; x++)
                {
                    if (line[x] != ' ')
                    {
                        TileTag tag = new TileTag();
                        tag.X = x;
                        tag.Y = y;

                        level.SetTileTypeForTag(tag, TileTypeMethods.GetTileTypeForChar(line[x]));
                    }
                }
            }

            file.Close();

            return level;
        }

    }
}
