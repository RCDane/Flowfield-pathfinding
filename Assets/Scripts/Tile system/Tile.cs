using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    
	public virtual void Place(Vector2 position)
    {
        transform.position = position;
    }
    
}
