using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[RequireComponent(typeof(Rigidbody2D))]
public class FlowField : MonoBehaviour
{


    //private Vector2 distanceBetweenPoints;
    [SerializeField]
    private Vector2 flowfieldSize;

    Vector3[,] vectors; // punkt vektorer
    Vector3[,] directions; // retnings vektorer
    int[,] Weights; // sværhedsgrad for punkter 

    int wallWeight;

    [SerializeField]
    private int xSize, ySize;
    [SerializeField]
    private Vector2 offSet;

    [SerializeField]
    private LayerMask wallLayer;

    bool created;


    private int[,] cachedWeights;

    string DebugMessage = "FlowField";
    string colorDebugMessage = "FlowColor";
    string FlowPositions = "FlowPositions";

    public int lowestWeight { get; private set; }
    public int HighestWeight { get; private set; }
    public QuadTree GetQuadtree()
    {
        return quadTree;
    }

    private float distanceBetweenPointsX;
    private float distanceBetweenPointsY;
    QuadTree quadTree;

    void prepareCache()
    {
        cachedWeights = new int[xSize, ySize];
        for (int i = 0; i < cachedWeights.GetLength(0); i++)
        {
            for (int j = 0; j < cachedWeights.GetLength(1); j++)
            {
                cachedWeights[i, j] = -1;
            }
        }
    }

    void Start()
    {

        DebugManager.AddDebugName(DebugMessage);
        DebugManager.AddDebugName(colorDebugMessage);
        DebugManager.AddDebugName(FlowPositions);
        prepareCache();
        CreateField();

        distanceBetweenPointsX = (vectors[0, 0] - vectors[1, 0]).magnitude;
        distanceBetweenPointsY = (vectors[0, 0] - vectors[0, 1]).magnitude;

        Invoke("CollisionCheck", 0.1f);
        // CollisionCheck();
    }
    void CreateField()
    {
        float xOffset = flowfieldSize.x / 2;
        float yOffset = flowfieldSize.y / 2;
        vectors = new Vector3[xSize, ySize];

        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                vectors[i, j] = new Vector3(offSet.x + i * ((float)flowfieldSize.x / xSize) - xOffset, 
                                            offSet.y + j * ((float)flowfieldSize.y / ySize) - yOffset);
            }
        }
        PackQuadTree();
    }
    Collider2D[] coll;
    void PackQuadTree()
    {
        QuadTree.Rectangle rect = new QuadTree.Rectangle(Vector2.zero + offSet, new Vector2(flowfieldSize.x / 2, flowfieldSize.x / 2));
        quadTree = new QuadTree(rect, 5);
        int rows = vectors.GetLength(1);
        collArr = new Collider2D[5];
        int amount = 0;
        for (int i = 0; i < vectors.GetLength(0); i++)
        {
            for (int j = 0; j < vectors.GetLength(1); j++)
            {
                // Collider2D col = Physics2D.OverlapPoint(vectors[i, j], wallLayer);
                // if (col != null)
                // {
                Point point = new Point(i + ySize * j, vectors[i, j]);
                quadTree.insert(point);
                // }
            }
        }
    }
    int amount = 0;
    public Vector2 GetVector(Vector2 point)
    {
        QuadTree.Rectangle rect = new QuadTree.Rectangle(point, new Vector2(distanceBetweenPointsX + 0.01f, distanceBetweenPointsY+ 0.01f));
        amount = 0;
        Point[] nearest = quadTree.query(rect, ref amount);
        int x = -1;
        float lowest = 1000000;
        for (int i = 0; i < nearest.Length; i++)
        {
            float dist = Vector2.SqrMagnitude(nearest[i].position - point);
            if (dist < lowest)
            {
                lowest = dist;
                x = i;
            }
        }
        Vector2Int vec = new Vector2Int(nearest[x].index % ySize, nearest[x].index / ySize);
        return GenerateVector(nearest[x].index % ySize, nearest[x].index / ySize);
    }

    Vector2 GenerateVector(int x, int y)
    {
        Vector2 direction;
        if (x + 1 < xSize && x - 1 >= 0)
        {
            direction.x = Weights[x - 1, y] - Weights[x + 1, y];
        }
        else if (x + 1 < xSize)
        {
            direction.x = 1;
        }
        else
        {
            direction.x = -1;
        }
        if (y + 1 < ySize && y - 1 >= 0)
        {
            direction.y = Weights[x, y - 1] - Weights[x, y + 1];
        }
        else if (y + 1 < ySize)
        {
            direction.y = 1;
        }
        else
        {
            direction.y = -1;
        }
        direction.Normalize();
        // if (directions[x, y] == null)
        //     directions[x, y] = direction;
        return direction;
    }
    void CollisionCheck()
    {
        Weights = new int[xSize, ySize];
        Collider2D[] coll = new Collider2D[0];
        int amount = 0;
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                amount = Physics2D.OverlapPointNonAlloc(vectors[i, j], coll, wallLayer);
                if (amount > 0)
                {
                    Weights[i, j] = 10;
                    continue;
                }
                Weights[i, j] = 1;
            }
        }
    }
    Camera cam;


    /// <summary>
    /// OnGUI is called for rendering and handling GUI events.
    /// This function can be called multiple times per frame (one call per event).
    /// </summary>
    void OnGUI()
    {

        if (DebugManager.Check(DebugMessage))
        {
            List<Vector2Int> pointsToRender = new List<Vector2Int>();
            cam = Camera.main;
            Vector2 camPos = cam.transform.position;
            float height = cam.orthographicSize;
            float width = cam.aspect * height;
            Vector2 camSize = new Vector2(width, height);
            for (int i = 0; i < vectors.GetLength(0); i++)
            {
                for (int j = 0; j < vectors.GetLength(1); j++)
                    if (CheckBoundary((Vector2)vectors[i, j], camPos, camSize))
                    {
                        pointsToRender.Add(new Vector2Int(i, j));
                    }
            }
            print("points  " + pointsToRender.Count);
            foreach (var point in pointsToRender)
            {
                // print(point);
                Vector2 pos = cam.WorldToScreenPoint(vectors[point.x, point.y]);
                pos.y = Screen.height - pos.y;
                pos.x += -12.5f;
                pos.y += -12.5f;
                Rect rect = new Rect(pos, Vector2.one * 25);
                var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
                GUI.Label(rect, Weights[point.x, point.y].ToString(), style);
            }
            pointsToRender.Clear();
        }


        if (DebugManager.Check(FlowPositions))
        {
            List<Vector2Int> pointsToRender = new List<Vector2Int>();
            cam = Camera.main;
            Vector2 camPos = cam.transform.position;
            float height = cam.orthographicSize;
            float width = cam.aspect * height;
            Vector2 camSize = new Vector2(width, height);
            for (int i = 0; i < vectors.GetLength(0); i++)
            {
                for (int j = 0; j < vectors.GetLength(1); j++)
                    if (CheckBoundary((Vector2)vectors[i, j], camPos, camSize))
                    {
                        pointsToRender.Add(new Vector2Int(i, j));
                    }
            }
            // print("points  " + pointsToRender.Count);
            foreach (var point in pointsToRender)
            {
                // print(point);
                Vector2 pos = cam.WorldToScreenPoint(vectors[point.x, point.y]);
                pos.y = Screen.height - pos.y;
                pos.x += -12.5f;
                pos.y += -12.5f;
                Rect rect = new Rect(pos, Vector2.one * 40);
                var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
                GUI.Label(rect, point.x.ToString() + " " + point.y.ToString(), style);
            }
            pointsToRender.Clear();
        }
    }
    
    void DrawSize()
    {
        float offsetX = flowfieldSize.x / 2;
        float offsetY = flowfieldSize.y / 2;

        Vector3 topLeft = (Vector2)transform.position + new Vector2(-offsetX, offsetY);
        Vector3 topRight = (Vector2)transform.position + new Vector2(-offsetX, offsetY);
        Vector3 bottomRight = (Vector2)transform.position + new Vector2(-offsetX, offsetY);
        Vector3 bottomLeft = (Vector2)transform.position + new Vector2(-offsetX, offsetY);
        topLeft.z = -1;
        topRight.z = -1;
        bottomRight.z = -1;
        bottomLeft.z = -1;
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }

    Vector2Int GetClosest(Vector2 pos)
    {
        int x = -1;
        int y = -1;
        float lowest = 1000000;
        for (int i = 0; i < vectors.GetLength(0); i++)
        {
            for (int j = 0; j < vectors.GetLength(1); j++)
            {
                float dist = Vector2.SqrMagnitude((Vector2)vectors[i, j] - pos);
                if (dist < lowest)
                {
                    lowest = dist;
                    x = i;
                    y = j;
                }
            }
        }
        return new Vector2Int(x, y);
    }

    void Update()
    {
        if (Application.isPlaying && Input.GetMouseButtonDown(1))
        {
            if (cam == null)
                cam = Camera.main;

            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            print(mousePos);
            Vector2Int closest = GetClosest(mousePos);

            UpdateFlowField(closest.x, closest.y);
        }
    }

    int checks = 0;

    void CheckPosition(int x, int y)
    {
        if (xSize > x + 1)
        {
            int extraWeight = GetWeightOfPoint(x + 1, y);

            if ((Weights[x + 1, y] > Weights[x, y] + extraWeight + 1))
            {
                Weights[x + 1, y] = Weights[x, y] + 1 + extraWeight;
                NextToGoThrough.Add(new Vector2Int(x + 1, y));
            }
        }
        if (0 <= x - 1)
        {
            int extraWeight = GetWeightOfPoint(x - 1, y);

            if ((Weights[x - 1, y] > Weights[x, y] + extraWeight + 1))
            {
                Weights[x - 1, y] = Weights[x, y] + 1 + extraWeight;
                NextToGoThrough.Add(new Vector2Int(x - 1, y));
            }
        }
        if (ySize > y + 1)
        {
            int extraWeight = GetWeightOfPoint(x, y + 1);

            if ((Weights[x, y + 1] > Weights[x, y] + extraWeight + 1))
            {
                Weights[x, y + 1] = Weights[x, y] + 1 + extraWeight;
                NextToGoThrough.Add(new Vector2Int(x, y + 1));
            }
        }
        if (0 <= y - 1)
        {
            int extraWeight = GetWeightOfPoint(x, y - 1);

            if ((Weights[x, y - 1] > Weights[x, y] + extraWeight + 1))
            {
                Weights[x, y - 1] = Weights[x, y] + 1 + extraWeight;
                NextToGoThrough.Add(new Vector2Int(x, y - 1));
            }
        }
    }
    List<Vector2Int> toGoThrough = new List<Vector2Int>();
    List<Vector2Int> NextToGoThrough = new List<Vector2Int>();
    public void UpdateFlowField(int x, int y)
    {
        // første stadie
        toGoThrough = new List<Vector2Int>();
        toGoThrough.Add(new Vector2Int(x, y));
        NextToGoThrough = new List<Vector2Int>();
        PrepareFlowField();
        Weights[x, y] = 0; //sætter start punktets værdi til 0
        int sec = 0;

        // Andet Stadie
        while (toGoThrough.Count > 0 && sec++ < 1000)
        {
            for (int i = 0; i < toGoThrough.Count; i++)
            {
                CheckPosition(toGoThrough[i].x, toGoThrough[i].y);
            }
            // tredje stadie
            toGoThrough.Clear();
            toGoThrough.AddRange(NextToGoThrough);
            NextToGoThrough.Clear();
        }
        // fjerde stadie
        lowestWeight = Weights[x, y];
        HighestWeight = Findbiggest();
    }

    int Findbiggest()
    {
        int biggest = -1;
        // Vector2Int h = new Vector2Int(0, 0);
        for (int i = 0; i < Weights.GetLength(0); i++)
        {
            for (int j = 0; j < Weights.GetLength(1); j++)
            {
                if (Weights[i, j] > biggest)
                {
                    biggest = Weights[i, j];
                    // h = new Vector2Int(i, j);
                }
            }
        }
        return biggest;
    }

    void PrepareFlowField()
    {
        directions = new Vector3[xSize, ySize];

        Weights = new int[xSize, ySize];

        for (int i = 0; i < Weights.GetLength(0); i++)
        {
            for (int j = 0; j < Weights.GetLength(1); j++)
            {
                Weights[i, j] = 1000000000;
            }
        }
    }

    private Collider2D[] collArr = new Collider2D[5];
    int GetWeightOfPoint(int x, int y)
    {
        if (cachedWeights[x, y] != -1)
        {
            return cachedWeights[x, y];
        }
        collArr = new Collider2D[5];

        int i = Physics2D.OverlapPointNonAlloc(vectors[x, y], collArr, wallLayer);
        int val = 0;
        if (i > 0)
        {
            val = GetExtraWeight(collArr, i);
        }
        cachedWeights[x, y] = val;
        return val;
    }

    int GetExtraWeight(Collider2D[] coll, int amount)
    {
        int val = 0;
        for (int i = 0; i < amount; i++)
        {
            if (coll[i].GetComponent<ValueHolder>() != null)
            {
                val += coll[i].GetComponent<ValueHolder>().Value;
            }
        }
        return val;
    }
    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
        DrawSize();
        // QuadTree.Draw(quadTree);
        if (!DebugManager.Check(colorDebugMessage))
            return;
        List<Vector2Int> pointsToRender = new List<Vector2Int>();
        cam = Camera.main;
        Vector2 camPos = cam.transform.position;
        float height = cam.orthographicSize;
        float width = cam.aspect * height;
        Vector2 camSize = new Vector2(width, height);

        if (vectors.Length != 0)
            for (int i = 0; i < vectors.GetLength(0); i++)
            {
                for (int j = 0; j < vectors.GetLength(1); j++)
                    if (CheckBoundary((Vector2)vectors[i, j], camPos, camSize))
                    {
                        pointsToRender.Add(new Vector2Int(i, j));

                    }
            }
        print(HighestWeight);
        foreach (var point in pointsToRender)
        {
            float colorval = 1 - (float)Weights[point.x, point.y] / (float)HighestWeight;
            // colorval = Mathf.Sin(colorval * 100);
            Gizmos.color = new Color(colorval, colorval, colorval);

            Gizmos.DrawCube(vectors[point.x, point.y], Vector3.one * 0.03f);
        }
    }

    bool CheckBoundary(Vector2 point, Vector2 pos, Vector2 size)
    {
        return (point.x > pos.x - size.x &&
                point.x < pos.x + size.x &&
                point.y < pos.y + size.y &&
                point.y > pos.y - size.y);
    }
}
