using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MSTree
{

    public static Tree MakeTreeWithDegrees(int numPoints)
    {
        float[] angles = new float[5] { -25, -15, 0, 15, 25 };

        Tree msTree = new Tree();
        msTree.points = new List<Point>();

        Point firstPoint = new Point();
        firstPoint.thisPoint = Vector3.zero;
        msTree.points.Add(firstPoint);

        for (int i = 1; i < numPoints; i++)
        {            
            float dist = Random.Range(150f, 350f);

            int randI = Random.Range(0, angles.Length);
            Vector3 pos = Quaternion.AngleAxis(angles[randI], Vector3.forward) * Vector3.right * dist;

            pos += msTree.points[i - 1].thisPoint;
            Point point = new Point();
            point.thisPoint = pos;
            msTree.points.Add(point);

            dist = Vector3.Distance(msTree.points[i - 1].thisPoint, pos);
            Edge edge = new Edge(pos, dist, i);
            Point tempPoint = msTree.points[i - 1];
            tempPoint.minimumEdge = edge;
            msTree.points[i - 1] = tempPoint;
        }
        return msTree;
    }

    public static Tree MakeRandoTree(int numPoints, Vector3 startPos)
    {
        Tree msTree = new Tree();
        msTree.points = new List<Point>();

        Point firstPoint = new Point();
        firstPoint.thisPoint = startPos;
        msTree.points.Add(firstPoint);

        for (int i = 1; i < numPoints; i++)
        {
            Vector3 directionDistance = Vector3.zero;
            float dist = Random.Range(150f, 350f);

            while (directionDistance == Vector3.zero)
            {
                float X = Random.Range(0f, 10f);
                float Y = Random.Range(-10f, 10f);
                
                directionDistance = new Vector3(X, Y, 0).normalized * dist;
            }

            directionDistance += msTree.points[i - 1].thisPoint;
            Point point = new Point();
            point.thisPoint = directionDistance;
            msTree.points.Add(point);

            dist = Vector3.Distance(msTree.points[i - 1].thisPoint, directionDistance);
            Edge edge = new Edge(directionDistance, dist, i);
            Point tempPoint = msTree.points[i-1];
            tempPoint.minimumEdge = edge;
            msTree.points[i-1] = tempPoint;

        }
        return msTree;
    }

    public static Tree MakeRandomPath(int numPoints, float scale)
    {
        Tree msTree = new Tree();
        msTree.points = PopulatePointsList(numPoints, scale);

        for (int i = 0; i < msTree.points.Count; i++)
        {
            int op = i + 1;
            if (op == msTree.points.Count)
            {
                break;
            }
            float dist = Vector3.Distance(msTree.points[op].thisPoint, msTree.points[i].thisPoint);
            Edge edge = new Edge(msTree.points[op].thisPoint, dist, op);
            Point tempPoint = msTree.points[i];
            tempPoint.minimumEdge = edge;
            msTree.points[i] = tempPoint;
        }
        return msTree;
    }

    public static Tree MakeMSTree(int numPoints, float scale)
    {
        List<Point> points = PopulatePointsList(numPoints, scale);

        // here we find each edge for every point and find the minimum edge
        // then we add all the points to the tree
        Tree msTree = FindMinimumEdge(points);

        // here we look for two points that are each others minimum edge so we can change that
        // and we do it twice because i am dumb lol
        CheckForRedundantBranches(msTree);
        CheckForRedundantBranches(msTree);
        return msTree;
    }

    private static List<Point> PopulatePointsList(int numPoints, float scale)
    {
        List<Point> points = new List<Point>();

        List<Vector3> p = GridPoints.GeneratePoints(numPoints, scale);
        // here we make each point and give it a position
        for (int i = 0; i < numPoints; i++)
        {
            Point point = new Point();
            point.edges = new List<Edge>();
            point.minimumEdge = new Edge();
            point.nextMinEdge = new Edge();
            point.thisPoint = p[i];
            points.Add(point);
        }

        return points;
    }

    private static Tree FindMinimumEdge(List<Point> points)
    {
        Tree msTree = new Tree();
        msTree.points = new List<Point>();
        for (int tp = 0; tp < points.Count; tp++)
        {
            float minimumDist = float.MaxValue;
            for (int op = 0; op < points.Count; op++)
            {
                if (op == tp) continue;

                float dist = Vector3.Distance(points[op].thisPoint, points[tp].thisPoint);
                Edge edge = new Edge(points[op].thisPoint, dist, op);
                points[tp].edges.Add(edge);

                if (dist < minimumDist)
                {
                    minimumDist = dist;
                    Point tempPoint = points[tp];
                    if (tempPoint.minimumEdge.otherPoint == Vector3.zero)
                    {
                        tempPoint.minimumEdge = edge;

                    }
                    tempPoint.nextMinEdge = tempPoint.minimumEdge;
                    tempPoint.minimumEdge = edge;
                    points[tp] = tempPoint;
                }
            }
            msTree.points.Add(points[tp]);
        }
        return msTree;
    }

    private static void CheckForRedundantBranches(Tree msTree)
    {
        for (int tp = 0; tp < msTree.points.Count; tp++)
        {
            // for this point we look at every other point
            for (int op = 0; op < msTree.points.Count; op++)
            {
                //we ignore if they're the same point
                if (tp == op) continue;
                if (msTree.points[tp].minimumEdge.otherPoint == msTree.points[op].thisPoint)
                {
                    if (msTree.points[op].minimumEdge.otherPoint == msTree.points[tp].thisPoint)
                    {
                        // if both points are each other's closest point, we choose next minimum edge
                        int index = Random.Range(0, msTree.points[op].edges.Count);

                        Point tempPoint = msTree.points[op];
                        tempPoint.minimumEdge = msTree.points[op].nextMinEdge;
                        tempPoint.nextMinEdge = msTree.points[op].edges[index];
                        msTree.points[op] = tempPoint;

                    }

                }
            }
        }
    }

}
public struct Tree
{
    public List<Point> points;
}


public struct Point
{
    public Vector3 thisPoint;
    public Edge minimumEdge;
    public Edge nextMinEdge;
    public List<Edge> edges;
}


public struct Edge
{
    public Vector3 otherPoint;
    public float distance;
    public int indexInList;

    public Edge(Vector3 otherPoint, float distance, int indexInList)
    {

        this.otherPoint = otherPoint;
        this.distance = distance;
        this.indexInList = indexInList;
    }
}
