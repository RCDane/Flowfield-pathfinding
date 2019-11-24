using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConnection  {
    public Room[] Rooms;
    public bool hasDoor;



    public static RoomConnection CreateConnection(Room room1, Room room2)
    {
        RoomConnection connection = new RoomConnection();
        connection.Rooms = new Room[2];
        connection.Rooms[0] = room1;
        connection.Rooms[1] = room2;

        room1.AddRoomConnection(connection, room2);
        room2.AddRoomConnection(connection, room1);

        return connection;
    }
}
