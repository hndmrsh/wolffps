using UnityEngine;
using System.Collections;

public class Level {

    private Material[] materials;
    private int[,] walls;

    public Level(Material[] materials, int[,] walls)
    {
        this.materials = materials;
        this.walls = walls;
    }

    public int GetWidth()
    {
        return walls.GetLength(0);
    }

    public int GetHeight()
    {
        return walls.GetLength(1);
    }

    public Material GetMaterial(int x, int y)
    {
        return materials[walls[x, y]];
    }

    public bool HasFloor(int x, int y)
    {
        return walls[x, y] >= 0;
    }
    public bool HasNorthWall(int x, int y)
    {
        return HasFloor(x, y) && (y == 0 || !HasFloor(x, y - 1));
    }

    public bool HasSouthWall(int x, int y)
    {
        return HasFloor(x, y) && (y == GetHeight() - 1 || !HasFloor(x, y + 1));
    }

    public bool HasWestWall(int x, int y)
    {
        return HasFloor(x, y) && (x == 0 || !HasFloor(x - 1, y));
    }

    public bool HasEastWall(int x, int y)
    {
        return HasFloor(x, y) && (x == GetWidth() - 1 || !HasFloor(x + 1, y));
    }

}
