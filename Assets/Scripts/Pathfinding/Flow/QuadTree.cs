using System.Collections.Generic;
using UnityEngine;

public struct Point
{
    public int index;
    public Vector2 position;

    public Point(int index, Vector2 pos)
    {
        position = pos;
        this.index = index;
    }
}

public class QuadTree
{
    public int capacity = 5;
    public List<Point> points;

    public bool divided;


    public Rectangle boundary;
    QuadTree northeast;
    QuadTree northwest;
    QuadTree southeast;
    QuadTree southwest;


    public struct Rectangle
    {

        public float x, y;
        public float xSize, ySize;

        public Rectangle(Vector2 pos, Vector2 Size)
        {
            this.x = pos.x;
            this.y = pos.y;
            this.xSize = Size.x;
            this.ySize = Size.y;
        }
        public Rectangle(float x, float y, float xSize, float ySize)
        {
            this.x = x;
            this.y = y;
            this.xSize = xSize;
            this.ySize = ySize;
        }

        public bool contains(Vector2 point)
        {
            return (point.x > x - xSize &&
                    point.x < x + xSize &&
                    point.y < y + ySize &&
                    point.y > y - ySize);
        }

        public bool intersects(Vector2 pos, Vector2 Size)
        {
            return !(pos.x - Size.x > this.x + this.xSize ||
              pos.x + Size.x < this.x - this.xSize ||
              pos.y - Size.y > this.y + this.ySize ||
              pos.y + Size.y < this.y - this.ySize);
        }

        public bool intersects(Rectangle rectangle)
        {
            return !(rectangle.x - rectangle.xSize > this.x + this.xSize ||
              rectangle.x + rectangle.xSize < this.x - this.xSize ||
              rectangle.y - rectangle.ySize > this.y + this.ySize ||
              rectangle.y + rectangle.ySize < this.y - this.ySize);
        }

        public static void Draw(Rectangle rect)
        {
            Vector2 topleft = new Vector2(rect.x - rect.xSize, rect.y + rect.ySize);
            Vector2 topright = new Vector2(rect.x + rect.xSize, rect.y + rect.ySize);
            Vector2 bottomright = new Vector2(rect.x + rect.xSize, rect.y - rect.ySize);
            Vector2 bottomleft = new Vector2(rect.x - rect.xSize, rect.y - rect.ySize);

            Gizmos.DrawLine(topleft, topright);
            Gizmos.DrawLine(topright, bottomright);
            Gizmos.DrawLine(bottomright, bottomleft);
            Gizmos.DrawLine(bottomleft, topleft);


        }

    }

    public static void Draw(QuadTree qtree)
    {
        if (qtree.divided)
        {
            QuadTree.Draw(qtree.northwest);
            QuadTree.Draw(qtree.northeast);
            QuadTree.Draw(qtree.southeast);
            QuadTree.Draw(qtree.southwest);
            return;
        }
        Rectangle.Draw(qtree.boundary);

    }
    public Point[] query(Rectangle range, ref Point[] found, ref int index)
    {
        if (found == null)
        {
            found = new Point[50];
        }

        if (!range.intersects(this.boundary))
        {
            return found;
        }
        if (this.divided)
        {
            this.northwest.query(range, ref found, ref index);
            this.northeast.query(range, ref found, ref index);
            this.southwest.query(range, ref found, ref index);
            this.southeast.query(range, ref found, ref index);
            return found;
        }
        for (int i = 0; i < points.Count; i++)
        {
            if (range.contains(points[i].position))
            {
                index++;
                found[index] = (points[i]);
            }
        }


        return found;
    }

    public Point[] query(Rectangle range, ref int amount)
    {

        Point[] found = new Point[25];


        if (!range.intersects(this.boundary))
        {
            return found;
        }
        if (this.divided)
        {
            this.northwest.query(range, ref found, ref amount);
            this.northeast.query(range, ref found, ref amount);
            this.southwest.query(range, ref found, ref amount);
            this.southeast.query(range, ref found, ref amount);
            return found;
        }
        for (int i = 0; i < points.Count; i++)
        {
            if (range.contains(points[i].position))
            {
                amount++;
                found[amount] = (points[i]);
            }
        }


        return found;
    }

    public QuadTree(Rectangle boundary, int capacity)
    {
        this.boundary = boundary;
        this.capacity = capacity;
        this.points = new List<Point>();
        this.divided = false;
    }
    public bool insert(Point point)
    {
        if (!this.boundary.contains(point.position))
        {
            return false;
        }

        if (this.points.Count < this.capacity)
        {
            this.points.Add(point);
            return true;
        }

        if (!this.divided)
        {
            this.Subdivide();
        }

        if (this.northeast.insert(point) || this.northwest.insert(point) ||
          this.southeast.insert(point) || this.southwest.insert(point))
        {
            return true;
        }

        return false;
    }

    public void Subdivide()
    {
        float x = this.boundary.x;
        float y = this.boundary.y;
        float w = this.boundary.xSize / 2;
        float h = this.boundary.ySize / 2;

        Rectangle ne = new Rectangle(x + w, y - h, w, h);
        this.northeast = new QuadTree(ne, this.capacity);
        Rectangle nw = new Rectangle(x - w, y - h, w, h);
        this.northwest = new QuadTree(nw, this.capacity);
        Rectangle se = new Rectangle(x + w, y + h, w, h);
        this.southeast = new QuadTree(se, this.capacity);
        Rectangle sw = new Rectangle(x - w, y + h, w, h);
        this.southwest = new QuadTree(sw, this.capacity);
        for (int i = 0; i < points.Count; i++)
        {
            if (this.northeast.insert(points[i]) || this.northwest.insert(points[i]) ||
                this.southeast.insert(points[i]) || this.southwest.insert(points[i]))
            {
                continue;
            }
        }
        points.Clear();
        this.divided = true;
    }
}
