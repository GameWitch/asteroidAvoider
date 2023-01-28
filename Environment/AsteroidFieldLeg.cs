using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFieldLeg : MonoBehaviour, ILevelLeg
{
    [SerializeField] float radius = 10;
    Vector2 sampleRegionSize;
    int samplesBeforeRejection = 7;

    [SerializeField] ParticleSystem explosion;
    [SerializeField] GameObject asteroid;
    [SerializeField] Transform asteroidsParentTransform;
    [SerializeField] GameObject[] pickups;

    [Header("Walls")]
    [SerializeField] BoxCollider roof;
    [SerializeField] BoxCollider floor;
    [SerializeField] BoxCollider front;
    [SerializeField] BoxCollider back;
    [SerializeField] BoxCollider startCap;
    [SerializeField] BoxCollider endCap;

    AsteroidField asteroidFieldPoints;
    Vector3 scale;
    float width = 50f;
    float height = 50f;
    float buffer = 50;
    float pickupPercent = 0.15f;
    
    Gradient particleGradient;
    Color asteroidColor;

    public void SetupLeg(float distance)
    {

        BuildBoundaries(distance);

        GenerateAsteroidFieldPoints(distance);

        PlacePickups();
        PlaceAsteroidsInField();
        PlaceParticles(distance);

    }
    public ParticleSystem GetExplosion()
    {
        return explosion;
    }

    private void BuildBoundaries(float distance)
    {
        
        // roof and floor
        Vector3 size = roof.size;
        size.x *= distance + buffer;
        size.z = width;

        roof.size = size;
        floor.size = size;

        roof.transform.localPosition = new Vector3(0, height/2, 0);
        floor.transform.localPosition = new Vector3(0, -(height / 2), 0);

        //front and back
        size = front.size;
        size.x = distance + buffer;
        size.y = height;

        front.size = size;
        back.size = size;

        front.transform.localPosition = new Vector3(0, 0, -(width/2));
        back.transform.localPosition = new Vector3(0, 0, width/2);

        // caps
        size = startCap.size;
        size.y = height;
        size.z = width;
        startCap.size = size;
        endCap.size = size;

        float capX = -((distance + buffer) / 2); // we push it back 5 so we dont crash when we turn on the next leg
        startCap.transform.localPosition = new Vector3(capX, 0, 0);
        endCap.transform.localPosition = new Vector3((distance + buffer)/2, 0, 0);
    }




    private void PlaceParticles(float distance)
    {
        TopBottomParticles tbP = GetComponentInChildren<TopBottomParticles>();
        Vector3 newPos = new Vector3(distance / 2, 0, 0f);
        tbP.transform.localPosition = newPos;

        // set the particle colors here
       // tbP.SetParticleColorGradients();
    }

    private void GenerateAsteroidFieldPoints(float distance)
    {
        float asteroidFieldBuffer = 6;
        sampleRegionSize = new Vector2(distance - buffer - asteroidFieldBuffer, height);
        asteroidFieldPoints = PoissonDiscSampling.GeneratePoints(radius, sampleRegionSize, samplesBeforeRejection);

        scale = Vector3.one;
        scale.x *= distance + buffer;
        scale.y = height;
        scale.z = width;
        Vector3 pos = -scale / 2;
        pos.z = 0f;
        pos.x += buffer + asteroidFieldBuffer;
        asteroidsParentTransform.localPosition = pos;
    }

    private void PlacePickups()
    {
        int pickupsToPlace = (int)(asteroidFieldPoints.asteroidPoints.Count * pickupPercent);
        for (int i = 0; i < pickupsToPlace; i++)
        {
            int astIndex = Random.Range(0, asteroidFieldPoints.asteroidPoints.Count);
            Vector3 openPoint = asteroidFieldPoints.asteroidPoints[astIndex];
            asteroidFieldPoints.asteroidPoints.RemoveAt(astIndex);

            int pickIndex = Random.Range(0, pickups.Length);
            GameObject pickupClone = Instantiate(pickups[pickIndex]);
            pickupClone.transform.parent = asteroidsParentTransform;
            pickupClone.transform.localPosition = openPoint;

        }

    }

    private void PlaceAsteroidsInField()
    {
        // we can set the asteroids fit our level generated color scheme here
       // Material astMaterial = asteroid.GetComponent<Material>();
       // astMaterial.color = asteroidColor;

        foreach (Vector2 point in asteroidFieldPoints.asteroidPoints)
        {

            GameObject asteroidClone = Instantiate(asteroid);
            asteroidClone.transform.parent = asteroidsParentTransform;
            asteroidClone.transform.localPosition = point;
            
        }
    }
}
