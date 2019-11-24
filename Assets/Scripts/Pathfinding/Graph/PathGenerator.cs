using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class PathGenerator : MonoBehaviour
{
    public void GenerateWorld(Tile[,] tiles, Room[] startandEnd)
    {
        int rndX;
        int rndY;
        
        Room start;
        Room end;
        int check = 0;
        do // choose start point and end
        {
            rndX = Random.Range(0, tiles.GetLength(0));
            rndY = Random.Range(0, tiles.GetLength(1));
            start = tiles[rndX, rndY] as Room;
            startandEnd[0] = start;
            rndX = Random.Range(0, tiles.GetLength(0));
            rndY = Random.Range(0, tiles.GetLength(1));
            end = tiles[rndX, rndY] as Room;
            startandEnd[1] = end;
        } while (start == end && check++ < 10);
        print("amount of checks " + check);
        int sizeX = tiles.GetLength(0);
        int sizeY = tiles.GetLength(1);
        int direction = 0;
        
        
        check = 0;
        //Room RoomToGiveDoor;
        do
        {
            rndX = Random.Range(0, sizeX -1 );
            rndY = Random.Range(0, sizeY - 1);
            
            direction =Random.Range(0, 3);

            switch (direction)
            {
                case 0: // UP
                    if(rndY + 1 <= sizeY)
                    {
                        CreateConnection(tiles[rndX, rndY] as Room, tiles[rndX, rndY + 1] as Room);
                    }
                    break;
                case 1: // Right
                    if (rndX + 1 <= sizeX)
                    {
                        CreateConnection(tiles[rndX, rndY] as Room, tiles[rndX + 1, rndY] as Room);
                    }
                    break;
                case 2: // Down
                    if (rndY - 1 >= 0)
                    {
                        CreateConnection(tiles[rndX, rndY] as Room, tiles[rndX, rndY - 1] as Room);
                    }
                    break;
                case 3: // Left
                    if (rndX - 1 >= 0)
                    {
                        CreateConnection((Room)tiles[rndX, rndY], (Room)tiles[rndX -1, rndY]);
                    }
                    break;
            }
        } while (!CheckIfPathIsCreated(start,end));
        print("amount of checks " + check);
        print("Connected");



        GenerateConnectionIfNone(tiles, start);
        // TODO build paths to Rooms that do not have connections

        // TODO: fill other Rooms with interesting stuff (Enemies and treasure)

        // TODO: prepare for player
    }

    void GenerateConnectionIfNone(Tile[,] tiles, Room start)
    {
        // go through all Rooms that are already connected to start
        List<Room> RoomsToGoThrough = new List<Room>();
        List<Room> toRemove = new List<Room>();
        List<Room> hasGoneThrough = new List<Room>();

        RoomsToGoThrough.Add(start);
        do
        {
            for (int i = 0; i < RoomsToGoThrough.Count; i++)
            {
                for (int j = 0; j < RoomsToGoThrough[i].Connections.Count; j++)
                {
                    if (!hasGoneThrough.Contains(RoomsToGoThrough[i].Connections[j]))
                    {
                        RoomsToGoThrough.Add(RoomsToGoThrough[i].Connections[j]);
                    }
                }
                hasGoneThrough.Add(RoomsToGoThrough[i]);
            }
            RoomsToGoThrough.Clear();
            for (int i = 0; i < toRemove.Count; i++)
            {
                RoomsToGoThrough.Remove(toRemove[i]);
            }
        } while (RoomsToGoThrough.Count > 0);
        print("Amount of rooms connected" + hasGoneThrough.Count);
        // go over all Rooms if not connected connect until connected





        // for (int i = 0; i < tiles.GetLength(0); i++){
        //     for (int j = 0; j < tiles.GetLength(1); j++){
        //         if(tiles[i,j].connections.Count == 0){
                    
        //         }
        //     }
        // }
    }

    public static void CreateConnection(Room Room1, Room Room2)
    {
        if (Room1.Connections.Contains(Room2))
            return;
        RoomConnection connection = new RoomConnection();
        connection.Rooms = new Room[2];
        connection.Rooms[0] = Room1;
        connection.Rooms[1] = Room2;

        Room1.AddRoomConnection(connection, Room2);
        Room2.AddRoomConnection(connection, Room1);


    }

    private static bool CheckIfPathIsCreated(Room from, Room to)
    {
        List<Room> RoomsToGoThrough = new List<Room>();
        List<Room> toRemove = new List<Room>();
        List<Room> hasGoneThrough = new List<Room>();

        RoomsToGoThrough.Add(from);
        do
        {
            for (int i = 0; i < RoomsToGoThrough.Count; i++)
            {
                if (RoomsToGoThrough[i].Connections.Contains(to))
                    return true;
                for (int j = 0; j < RoomsToGoThrough[i].Connections.Count; j++)
                {
                    if (!hasGoneThrough.Contains(RoomsToGoThrough[i].Connections[j]))
                    {
                        RoomsToGoThrough.Add(RoomsToGoThrough[i].Connections[j]);
                    }
                }
                hasGoneThrough.Add(RoomsToGoThrough[i]);
            }
            RoomsToGoThrough.Clear();
            for (int i = 0; i < toRemove.Count; i++)
            {
                RoomsToGoThrough.Remove(toRemove[i]);
            }
        } while (RoomsToGoThrough.Count > 0);
        return false;
    }
    private void Update()
    {
    }
}