using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoissonDiscSampling
{
    public static AsteroidField GeneratePoints(float radius, Vector2 sampleRegionSize, int numSamplesBeforeRejection = 30)
    {
        float cellSize = radius / Mathf.Sqrt(2);

        int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize)];

        List<Vector2> asteroidPoints = new List<Vector2>();
        List<Vector2> openPoints = new List<Vector2>();
        List<Vector2> spawnPoints = new List<Vector2>();

        spawnPoints.Add(sampleRegionSize / 2);
        while (spawnPoints.Count > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2 spawnCenter = spawnPoints[spawnIndex];
            bool candidateAccepted = false;

            Vector2 candidate;

            for (int i = 0; i < numSamplesBeforeRejection; i++)
            {
                candidate = MakePoint(spawnCenter, radius);

                if (IsValid(candidate, sampleRegionSize, cellSize, radius, asteroidPoints, grid))
                {
                    asteroidPoints.Add(candidate);
                    spawnPoints.Add(candidate);
                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = asteroidPoints.Count;
                    candidateAccepted = true;
                    break;
                }
            }
            if (!candidateAccepted)
            {
                //candidate = MakePoint(spawnCenter, radius);
                //openPoints.Add(candidate);
                spawnPoints.RemoveAt(spawnIndex);
            }
        }
        AsteroidField asteroidField = new AsteroidField();
        asteroidField.asteroidPoints = asteroidPoints;
        asteroidField.openPoints = openPoints;
        return asteroidField;
    }



    static bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, float radius, List<Vector2> points, int[,] grid)
    {
        if (candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y)
        {
            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);
            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

            for (int x = searchStartX; x <= searchEndX; x++)
            {
                for (int y = searchStartY; y <= searchEndY; y++)
                {
                    int pointIndex = grid[x, y] - 1;
                    if (pointIndex != -1)
                    {
                        float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
                        if (sqrDst < radius * radius)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        return false;
    }

    private static Vector2 MakePoint(Vector2 spawnCenter, float radius)
    {
        float angle = Random.value * Mathf.PI * 2;
        Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
        return spawnCenter + dir * Random.Range(radius, 2 * radius);
    }
}

public struct Cell
{
    public bool starPresent;
    public GameObject gameObject;
    public Vector2 point;
}

public struct AsteroidField
{
    public List<Vector2> asteroidPoints;
    public List<Vector2> openPoints;
}