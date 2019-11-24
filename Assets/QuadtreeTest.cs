using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadtreeTest : MonoBehaviour
{
    [SerializeField]
    List<Point> points = new List<Point>();
    QuadTree quadTree;

    void Start()
    {
        InPlaymode = true;
        Camera cam = Camera.main;
        Vector2 camPos = cam.transform.position;
        float height = cam.orthographicSize;
        float width = cam.aspect * height;
        Vector2 camSize = new Vector2(width, height);



        QuadTree.Rectangle rect = new QuadTree.Rectangle(Vector2.zero, camSize);
        quadTree = new QuadTree(rect, 1);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SpawnPoint();
            print("running");
        }
    }

    void SpawnPoint()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2 rnd = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            rnd += (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            quadTree.insert(new Point(i, rnd));
        }
    }
    bool InPlaymode;
    void OnDrawGizmos()
    {
        if(!InPlaymode)
            return;
        Point[] points = new Point[2000];



        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        QuadTree.Rectangle rect = new QuadTree.Rectangle(mousePosition, Vector2.one);
        QuadTree.Rectangle.Draw(rect);
        int amount = 0;
        quadTree.query(rect, ref points, ref amount);

        QuadTree.Draw(quadTree);
        Gizmos.color = Color.red;
        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.DrawCube(points[i].position, Vector2.one * 0.1f);
        }
        print("quadtree points:   " + points.Length);
        points = new Point[2000];
        amount =0;
        quadTree.query(quadTree.boundary, ref points, ref amount);

        print("mouse rect points" + points.Length);
    }
}
