using UnityEngine;
using System.Collections;
using System.IO;

public class LevelGenerator : MonoBehaviour {

    public string levelName;
    public GameObject floor, ceiling, wall, door;
    public Material[] materials;

    private const float TILE_SIZE = 0.25f;
    private const float TILE_SIZE_SCALED = TILE_SIZE * 10f;

	// Use this for initialization
	void Start () {
        Level level = LevelReader.LoadLevel(Application.dataPath + "/Levels/" + levelName + ".lvl", materials);

        float centerX = -((level.GetWidth() * TILE_SIZE_SCALED) / 2f);
        float centerY = (level.GetHeight() * TILE_SIZE_SCALED) / 2f;

        Vector3 levelScale = new Vector3(level.GetWidth() * TILE_SIZE, TILE_SIZE, level.GetHeight() * TILE_SIZE);
        ceiling.transform.localScale = levelScale;
        floor.transform.localScale = levelScale;

        Instantiate(ceiling, new Vector3(centerX, ceiling.transform.position.y, centerY), ceiling.transform.rotation);
        Instantiate(floor, new Vector3(centerX, floor.transform.position.y, centerY), floor.transform.rotation);

        for (int x = 0; x < level.GetWidth(); x++)
        {
            for (int y = 0; y < level.GetHeight(); y++)
            {
                switch (level.GetSpaceType(x, y))
                {
                    case Level.SpaceType.Floor:
                        GenerateWalls(level, x, y);
                        break;
                    case Level.SpaceType.Door:
                        GenerateDoor(level, x, y);
                        break;
                }
            }
        }

	}

    private void GenerateDoor(Level level, int x, int y)
    {
        Vector3 pos = new Vector3(0, door.transform.position.y, 0);
        Vector3 rot = new Vector3(door.transform.rotation.eulerAngles.x, 0, door.transform.rotation.eulerAngles.z);
        string name = "Door: " + x + "," + y;

        if (level.HasWall(x, y, Level.Direction.North) && level.HasWall(x, y, Level.Direction.South))
        {
            // E-W door
            pos.x = -(x * TILE_SIZE_SCALED + (TILE_SIZE_SCALED / 2f));
            pos.z = y * TILE_SIZE_SCALED + (TILE_SIZE_SCALED / 2f);
            rot.y = 90;
            GameObject doorInstance = (GameObject)Instantiate(door, pos, Quaternion.Euler(rot));
        }
        else if (level.HasWall(x, y, Level.Direction.East) && level.HasWall(x, y, Level.Direction.West))
        {
            // N-S door
            pos.x = -(x * TILE_SIZE_SCALED + (TILE_SIZE_SCALED / 2f));
            pos.z = y * TILE_SIZE_SCALED + (TILE_SIZE_SCALED / 2f);
            rot.y = 0;
            GameObject doorInstance = (GameObject)Instantiate(door, pos, Quaternion.Euler(rot));
        }
    }

    private void GenerateWalls(Level level, int x, int y)
    {
        Vector3 pos = new Vector3(0, wall.transform.position.y, 0);
        Vector3 rot = new Vector3(wall.transform.rotation.eulerAngles.x, 0, wall.transform.rotation.eulerAngles.z);
        Material mat = level.GetMaterial(x, y);
        string name = "Wall: " + x + "," + y;

        if (level.HasWall(x, y, Level.Direction.North))
        {
            pos.x = -(x * TILE_SIZE_SCALED + (TILE_SIZE_SCALED / 2f));
            pos.z = y * TILE_SIZE_SCALED;
            rot.y = 0;
            GameObject wallInstance = (GameObject) Instantiate(wall, pos, Quaternion.Euler(rot));
            wallInstance.GetComponent<Renderer>().material = mat;
            wallInstance.name = name + "N";
        }

        if (level.HasWall(x, y, Level.Direction.South))
        {
            pos.x = -(x * TILE_SIZE_SCALED + (TILE_SIZE_SCALED / 2f));
            pos.z = y * TILE_SIZE_SCALED + TILE_SIZE_SCALED;
            rot.y = 180;
            GameObject wallInstance = (GameObject)Instantiate(wall, pos, Quaternion.Euler(rot));
            wallInstance.GetComponent<Renderer>().material = mat;
            wallInstance.name = name + "S";
        }

        if (level.HasWall(x, y, Level.Direction.East))
        {
            pos.x = -(x * TILE_SIZE_SCALED + TILE_SIZE_SCALED);
            pos.z = y * TILE_SIZE_SCALED + (TILE_SIZE_SCALED / 2f);
            rot.y = 90;
            GameObject wallInstance = (GameObject)Instantiate(wall, pos, Quaternion.Euler(rot));
            wallInstance.GetComponent<Renderer>().material = mat;
            wallInstance.name = name + "E";
        }

        if (level.HasWall(x, y, Level.Direction.West))
        {
            pos.x = -(x * TILE_SIZE_SCALED);
            pos.z = y * TILE_SIZE_SCALED + (TILE_SIZE_SCALED / 2f);
            rot.y = 270;
            GameObject wallInstance = (GameObject)Instantiate(wall, pos, Quaternion.Euler(rot));
            wallInstance.GetComponent<Renderer>().material = mat;
            wallInstance.name = name + "W";
        }
    }

}
