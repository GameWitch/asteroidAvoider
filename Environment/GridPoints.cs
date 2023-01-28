using System.Collections.Generic;
using UnityEngine;


public static class GridPoints
{
    public static List<Vector3> GeneratePoints(int numPoints, float scale)
    {
        List<Vector3> points = new List<Vector3>();
        bool[,] grid;

        int gridScale = (int)scale / 10;
        grid = new bool[gridScale, gridScale];

        for (int i = 0; i < numPoints; i++)
        {
            int x = Random.Range(0, gridScale);
            int y = Random.Range(0, gridScale);
            while (grid[x, y])
            {
                x = Random.Range(0, gridScale);
                y = Random.Range(0, gridScale);
            }
            grid[x, y] = true;
            float floatX = Random.Range(x * gridScale, (x * gridScale) + gridScale);
            float floatY = Random.Range(y * gridScale, (y * gridScale) + gridScale);
            Vector3 v3 = new Vector3(floatX, floatY, 0f);
            points.Add(v3);
        }
        return points;
    }


}