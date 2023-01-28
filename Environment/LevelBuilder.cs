using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    
    [SerializeField] GameObject[] pickups;

    [SerializeField] GameObject asteroidLauncher;
    [SerializeField] GameObject landingPad;

    [SerializeField] GameObject asteroid;
    [SerializeField] Transform obstacleParent;

    [SerializeField] BoundaryCleanup boundary;


    [Header("Poisson Disc")]
    [SerializeField] int samplesBeforeRejection;

    ScoreKeeper scoreKeeper;

    float endOfField = 50f;
    float heightOfField = 50f;
    int level;
    AsteroidField asteroidFieldPoints;

    void Awake()
    {
        scoreKeeper = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreKeeper>();
        level = scoreKeeper.GetLevel();


        endOfField += level * 50;

        landingPad.transform.position = new Vector3(endOfField + 10f, heightOfField / 2, 0f);
        landingPad.transform.Rotate(new Vector3(0f, 0f, 90f));

        boundary.ShapeBoundary(endOfField, heightOfField);

        GeneratePoints();

        PlacePickups();

        CreateAsteroidField();

        CreateAsteroidLaunchers();

    }

    private void PlacePickups()
    {
        for (int i = 2; i < level; i++)
        {
            Vector3 openPoint = ChooseOpenPoint();

            int index = Random.Range(0, pickups.Length);
            Instantiate(pickups[index], openPoint, Quaternion.identity);
           
        }
    }

    private Vector3 ChooseOpenPoint()
    {
        int index = Random.Range(0, asteroidFieldPoints.asteroidPoints.Count);
        Vector3 openPoint = asteroidFieldPoints.asteroidPoints[index];
        asteroidFieldPoints.asteroidPoints.RemoveAt(index);
        asteroidFieldPoints.openPoints.Add(openPoint);
        return openPoint;
    }

    private void CreateAsteroidLaunchers()
    {
        if (level > 1)
        {
            float canonX = 25f;
            float canonY = 0;
            float canonZ = 0;

            for (int i = 1; i < level; i++)
            {
                if (i == 7)
                {
                    canonZ = 25f;
                    canonY = 0;
                }
                Vector3 pos = new Vector3(canonX, canonY, canonZ);
                GameObject clone = Instantiate(asteroidLauncher, pos, Quaternion.identity);
                clone.GetComponent<AsteroidLauncher>().GetAsteroidGameObject(asteroid);
                clone.transform.parent = obstacleParent;
                canonY += 10f;
            }
        }
    }

    void GeneratePoints()
    {
        float radius = 12 - level * .5f;
        if (radius <= 5)
        {
            radius = 5;
        }

        Vector2 sampleRegionSize;
        sampleRegionSize.y = heightOfField;
        sampleRegionSize.x = endOfField;

        asteroidFieldPoints = PoissonDiscSampling.GeneratePoints(radius, sampleRegionSize, samplesBeforeRejection);
    }

    void CreateAsteroidField()
    {
        foreach (Vector2 point in asteroidFieldPoints.asteroidPoints)
        {

            GameObject asteroidClone = Instantiate(asteroid);
            asteroidClone.GetComponent<Asteroid>().GetLocation(point);
            
            asteroidClone.transform.parent = obstacleParent;
        }

    }
}
