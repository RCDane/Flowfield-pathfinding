using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
public class AITestSolution : MonoBehaviour {
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private int amountOfUnits;
    [SerializeField]
    private float seperation;


    private Vector2[] units;

    private QuadTree tree;

    [SerializeField]
    private FlowField flow;

    private float distanceToOther;
    private float acceleration;
	// Use this for initialization
	void Start () {
        flow.UpdateFlowField(0, 0);
	}
	

    void CreateUnits()
    {
        units = new Vector2[amountOfUnits];
        for (int i = 0; i < amountOfUnits; i++)
        {
            units[i] = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        }
    }
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
            flow.UpdateFlowField((int)mousePos.x, (int)mousePos.y);
        }

        Steer();
	}
    // boids stay near the center of the swarm;
    Vector2 Rule1()
    {
        Vector2 middle = Vector2.zero;
        for (int i = 0; i < amountOfUnits; i++)
        {
            middle += units[i];
        }
        
        return middle / (amountOfUnits - 1);
    }
    // boids keep a distance to eachother
    void Rule2()
    {
        int amount = 0;
        float x = 0;
        float y = 0;
        float seperationRect = seperation * 2;
        for (int i = 0; i < amountOfUnits; i++)
        {
            amount = 0;
            x = units[i].x;
            y = units[i].y;
            QuadTree.Rectangle rect = new QuadTree.Rectangle(x, y, seperationRect, seperationRect);
            Point[] nearby = tree.query(rect, ref amount);
            for (int j = 0; j < amount; j++)
            {
                if(i != nearby[j].index)
                {
                    units[i].x -= Push(units[i].x, nearby[j].position.x, seperation);
                    units[i].y -= Push(units[i].y, nearby[j].position.y, seperation);
                }
            }
        }
    }
    
    float Push(float one, float two, float dist)
    {
        if (Mathf.Abs(one - two) < dist)
            return (one - two);
        return 0;
    }

    void Steer()
    {

    }
}
