using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedTest : MonoBehaviour
{
    public int amountOfParticles;
    public int quadtreequality;


    private List<Vector2> points;
    private QuadTree tree;


    QuadTree.Rectangle rect;
    // Use this for initialization
    void Start()
    {
        rect = new QuadTree.Rectangle(-4, 0, 0.5f, 0.5f);
        GeneratePoints();
        QuadTree.Rectangle r = new QuadTree.Rectangle(0, 0, 5, 5);
        tree = new QuadTree(r, quadtreequality);
        for (int i = 0; i < points.Count; i++)
        {
            Point p = new Point(i, points[i]);
            tree.insert(p);
        }
    }

    void GeneratePoints()
    {
        points = new List<Vector2>();
        for (int i = 0; i < amountOfParticles; i++)
        {
            points.Add(new Vector3(Random.Range(-5f, 5), Random.Range(-5f, 5), 0));
        }
    }
    Point[] vectors;
    bool quadtreeSearch = true;
    void Update()
    {
        if (rect.x > 4)
        {
            rect.x = -4;
            quadtreeSearch = !quadtreeSearch;
        }
        rect.x += Time.deltaTime * 0.4f;
        vectors = new Point[25];
        int index = 0;
        if (quadtreeSearch)
        {
            tree.query(rect, ref vectors, ref index);
        // }
        // else
        // {
        //     for (int i = 0; i < amountOfParticles; i++)
        //     {
        //         if (rect.contains(points[i]))
        //         {
        //             vectors.Add(vectors[i]);
        //         }
        //     }
        }

        // print(vectors.Count);
    }
    void OnDrawGizmos()
    {
        // List<Vector2> vectors = new List<Vector2>();
        // if(quadtreeSearch)
        // {
        // 	tree.query(rect, ref vectors);
        // }
        // else{
        // 	for (int i = 0; i < amountOfParticles; i++)
        // 	{
        // 		if(rect.contains(points[i]))
        // 		{
        // 			vectors.Add(points[i]);
        // 		}
        // 	}
        // }

        foreach (Point vec in vectors)
        {
            Gizmos.DrawCube(vec.position, Vector2.one * 0.05f);
        }
    }


}
