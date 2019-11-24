using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;


public class RoomManager : MonoBehaviour {
    [SerializeField]
    private Vector2Int size;
    [ SerializeField]
    private GameObject Room;
    [ SerializeField]
    private GameObject wall;
    [ SerializeField]
    private GameObject door;
    [SerializeField]
    private Room[] TypesOfRooms;
    [SerializeField]
    private int amountOfSpecialRooms;
    [SerializeField]
    PathGenerator pathGenerator;
    [SerializeField]
    private Tile[,] rooms;
    [SerializeField]
    Transform[] horizontalWalls, verticalWalls;



    Room startRoom, endRoom;

    string debugMessage = "RoomPaths";
	// Use this for initialization
	void Start () {
        DebugManager.AddDebugName(debugMessage);
        // Profiler.BeginSample("My Profiler Sameple");
        GenerateMap();
        // Profiler.EndSample();
	}
	
	// Update is called once per frame
	void Update () {
		// if(Input.GetKeyDown(KeyCode.Space)){
        //     GenerateMap();
        // }
	}
    GameObject roomParent;
    public void GenerateMap()
    {
        rooms = new Tile[size.x, size.y];
        if (roomParent != null)
            Destroy(roomParent);
        roomParent = new GameObject();
        Instantiate(roomParent, Vector2.zero, Quaternion.identity);
        for (int i = 0; i < amountOfSpecialRooms; i++)
        {
            PlaceSpecialRooms(roomParent.transform);
        }
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {   if(rooms[i,j] == null)
                    rooms[i,j] = PlaceRoom(Room,new Vector2(-size.x/2 + i, -size.y / 2 + j), roomParent.transform);
            }
        }
        Room[] startAndEnd = new Room[2];
        pathGenerator.GenerateWorld(rooms, startAndEnd);

        startRoom = startAndEnd[0];
        endRoom = startAndEnd[1];
        GameObject wallParent = new GameObject();
        wallParent.name = "Wall Parent";
        // Instantiate(wallParent);
        NewPlaceWalls(wallParent.transform);

    }

    void PlaceSpecialRooms(Transform parent)
    {
        int roomIndex;
        bool checking = false;
        int security = 0;
        Vector2Int position;
        do
        {
            roomIndex = Random.Range(0, TypesOfRooms.Length);
            //print("roomIndex: " +roomIndex);
            position = new Vector2Int(Random.Range(0, rooms.GetLength(0) - (int)TypesOfRooms[roomIndex].Size.x), Random.Range(0, rooms.GetLength(1) - (int)TypesOfRooms[roomIndex].Size.y));
            //print("position: " + position);
            checking = false;

            for (int i = 0; i < TypesOfRooms[roomIndex].Size.x; i++)
            {
                for (int j = 0; j < TypesOfRooms[roomIndex].Size.y; j++)
                {
                    if (rooms[i + position.x, j + position.y] != null)
                    {
                        checking = true;
                    }
                }
            }
        } while (checking && ++security < 10);

        if (!checking)
        {
            Vector2 pos = TypesOfRooms[roomIndex].GetOffset() + (Vector2)position;
            pos.x += -size.x / 2;
            pos.y += -size.y / 2;

            Room room = PlaceRoom(TypesOfRooms[roomIndex].gameObject, pos, parent).GetComponent<Room>();
            FillTiles(room, position);
            //Instantiate(TypesOfRooms[roomIndex].gameObject, , Quaternion.identity, parent);
        }
    }
    void FillTiles(Room room, Vector2Int pos)
    {
        for (int i = 0; i < room.Size.x; i++)
        {
            for (int j = 0; j < room.Size.y; j++)
            {
                
                rooms[pos.x + i, pos.y + j] = room;
            }
        }
    }
    Tile PlaceRoom(GameObject obj ,Vector2 position, Transform parent)
    {
        return Instantiate(obj, position, Quaternion.identity, parent).GetComponent<Tile>();
    }


    void NewPlaceWalls(Transform parent)
    {
        
        for (int i = 0; i < rooms.GetLength(0); i++)
        {
            float xVal = i - size.x / 2;
            for (int j = 0; j < rooms.GetLength(1) + 1; j++)
            {
                if(j == 0 ||j == rooms.GetLength(1))
                {
                    Instantiate(wall, new Vector3(xVal, -size.y / 2 - 0.5f + j), Quaternion.Euler(0, 0, 90),parent);
                }
                else
                {
                    if (rooms[i,j] == rooms[i,j - 1])
                    {

                    }
                    else if (!(rooms[i, j] as Room).Connections.Contains((rooms[i,j -1 ] as Room)))
                        Instantiate(wall, new Vector3(xVal, -size.y / 2 - 0.5f + j), Quaternion.Euler(0, 0, 90),parent);
                    else
                    {
                        Instantiate(door, new Vector3(xVal, -size.y / 2 - 0.5f + j), Quaternion.Euler(0, 0, 90),parent);
                    }
                }

            }
        }

        for (int i = 0; i < rooms.GetLength(1); i++)
        {
            float yVal = i - size.y / 2;
            for (int j = 0; j < rooms.GetLength(0) + 1; j++)
            {
                //Instantiate(wall, new Vector3(-size.y / 2 - 0.5f + j, yVal), Quaternion.identity);
                if (j == 0 || j == rooms.GetLength(0))
                {
                    Instantiate(wall, new Vector3(-size.x / 2 - 0.5f + j, yVal), Quaternion.identity,parent);
                }
                else
                {
                    if(rooms[j, i] == rooms[j - 1, i])
                    {

                    }
                    else if (!(rooms[j, i] as Room).Connections.Contains((rooms[j -1, i] as Room)))
                        Instantiate(wall, new Vector3(-size.x / 2 - 0.5f + j, yVal), Quaternion.identity,parent);
                    else
                    {
                        Instantiate(door, new Vector3(-size.x / 2 - 0.5f + j, yVal), Quaternion.identity,parent);
                    }
                }
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        DebugConnections();
    }
    
    void DebugConnections()
    {
        if(!DebugManager.Check(debugMessage))
            return;

        Gizmos.color = Color.magenta;
    if(rooms.Length > 0) {
        for (int i = 0; i < rooms.GetLength(0); i++)
        {
                for (int j = 0; j < rooms.GetLength(1); j++)
                {
                    Vector2 pos = rooms[i, j].transform.position;
                    foreach (Room item in (rooms[i, j] as Room).Connections)
                    {
                        Gizmos.DrawLine(pos, item.transform.position);
                    }
                }
        }
        // Gizmos.color = Color.blue;
        Gizmos.DrawSphere(startRoom.transform.position, 0.3f);
        Gizmos.DrawSphere(endRoom.transform.position, 0.3f);

    }
        
    }

    
}
