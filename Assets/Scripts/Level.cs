using UnityEngine;
using System.Collections;

public class Level {

    public enum SpaceType
    {
        Undefined, Floor, Door
    }

    public enum Direction
    {
        North, South, East, West
    }

    private Material[] materials;
    private char[,] level;

    public Level(Material[] materials, char[,] level)
    {
        this.materials = materials;
        this.level = level;
    }

    public int GetWidth()
    {
        return level.GetLength(0);
    }

    public int GetHeight()
    {
        return level.GetLength(1);
    }

    public Material GetMaterial(int x, int y)
    {
        Debug.Log("mat for " + x + "," + y);

        if (char.IsNumber(level[x, y]))
        {
            return materials[int.Parse(level[x, y].ToString())];
        }

        return null;
    }


    public SpaceType GetSpaceType(int x, int y)
    {
        if (char.IsDigit(level[x, y]))
        {
            return SpaceType.Floor;
        }
        else if (level[x, y] == 'D')
        {
            return SpaceType.Door;
        }

        return SpaceType.Undefined;
    }

    /// <summary>
    /// The space is walkable. This is different to a floor, as floors are surrounded by walls, where arbitrary walkable
    /// spaces do not need to be (e.g. door spaces have their walls built in to the structure).
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool HasWalkableSpace(int x, int y)
    {
        return level[x, y] != '\0';
    }

    public bool HasWall(int x, int y, Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                return y == 0 || !HasWalkableSpace(x, y - 1);
            case Direction.South:
                return y == GetHeight() - 1 || !HasWalkableSpace(x, y + 1);
            case Direction.West:
                return x == 0 || !HasWalkableSpace(x - 1, y);
            case Direction.East:
                return x == GetWidth() - 1 || !HasWalkableSpace(x + 1, y);
        }

        return false;
    }

}
