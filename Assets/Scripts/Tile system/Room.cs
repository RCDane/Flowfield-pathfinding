using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Room : Tile {
    public List<RoomConnection> roomConnections = new List<RoomConnection>();
    [SerializeField]
    private Vector2 size = Vector2.one;
    
    public Vector2 Size { get { return size; } }
    public List<Room> Connections;
    

    
    public Vector2 GetOffset()
    {
        float x = (size.x + 1) / 2 - 1;
        float y = (size.y + 1) / 2 - 1 ;

        return new Vector2(x,y);
    }
    public void AddRoomConnection(RoomConnection con, Room other)
    {
        if (roomConnections == null)
            roomConnections = new List<RoomConnection>();
        if (!roomConnections.Contains(con))
        {
            roomConnections.Add(con);
            Connections.Add(other);
        }
    }
	public override void Place(Vector2 position)
    {

        base.Place(position);
    }

    public void Enter()
    {

    }
}


