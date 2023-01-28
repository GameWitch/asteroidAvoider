using System;
using System.Collections;
using UnityEngine;

public class SpinBurst : MonoBehaviour, IWeapon
{
    [SerializeField] GameObject projectile;

    [SerializeField ]int projectilesToSpawn = 16;
    [SerializeField] float forceToAdd = 800f;

    int fireIndex = 0;
    int ammo = 100;
    float fireRate = .1f;    

    Vector3[] fireDirections;
    Quaternion initialRotation;

    private void Awake()
    {
        CreateCircleDirectionsArray();
    }

    private void CreateCircleDirectionsArray()
    {
        fireDirections = new Vector3[projectilesToSpawn];
        for (int i = 0; i < projectilesToSpawn; i++)
        {
            float radians = 2 * MathF.PI / projectilesToSpawn * i;
            float y = MathF.Sin(radians);
            float x = Mathf.Cos(radians);

            fireDirections[i] = new Vector3(x, y, 0);
        }
    }

    private Vector3 IncrementX(float incAmount, Vector3 lastDir)
    {
        lastDir.x += incAmount;
        return lastDir;
    }
    private Vector3 IncrementY(float incAmount, Vector3 lastDir)
    {
        lastDir.y += incAmount;
        return lastDir;
    }

    private void CreateSquareDirectionsArray()
    {
        Vector3 startDirection = new Vector3(-1, 1, 0);
        float incrementAmount = 0.5f;
        int sideLength = (int)(2 / incrementAmount);
        fireDirections = new Vector3[sideLength * 4];
        fireDirections[0] = startDirection;

        for (int i = 1; i < sideLength; i++)
        {
            fireDirections[i] = IncrementX(incrementAmount, fireDirections[i - 1]);
        }
        for (int i = sideLength; i < sideLength * 2; i++)
        {
            fireDirections[i] = IncrementY(-incrementAmount, fireDirections[i - 1]);
        }
        for (int i = sideLength * 2; i < sideLength * 3; i++)
        {
            fireDirections[i] = IncrementX(-incrementAmount, fireDirections[i - 1]);
        }
        for (int i = sideLength * 3; i < sideLength * 4; i++)
        {
            fireDirections[i] = IncrementY(incrementAmount, fireDirections[i - 1]);
        }
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void FireProjectile()
    {
        if (fireIndex == fireDirections.Length)
        {
            fireIndex = 0;
        }
        GameObject _proj = Instantiate(projectile, transform.position, initialRotation);
        _proj.GetComponent<Rigidbody>().AddForce(fireDirections[fireIndex] * forceToAdd);
        fireIndex++;
    }
    public WeaponType GetWeaponType()
    {
        return WeaponType.SPINBURST;
    }

    public int GetAmmo()
    {
        return ammo;
    }

    public float GetFireRate()
    {
        return fireRate;
    }
}
