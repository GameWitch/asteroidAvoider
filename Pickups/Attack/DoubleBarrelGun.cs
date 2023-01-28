using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBarrelGun : MonoBehaviour, IWeapon
{
    [SerializeField] GameObject projectile;
    [SerializeField] float forceToApply = 4000f;
    float fireRate = 0.1f;
    int ammo = 100;
    bool barrelSwitch = true;

    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public WeaponType GetWeaponType()
    {
        return WeaponType.LASER;
    }
    public int GetAmmo()
    {
        return ammo;
    }


    public float GetFireRate()
    {
        return fireRate;
    }

    public void FireProjectile()
    {
        Vector3 barrel;
        if (barrelSwitch)
        {
            barrel = transform.localPosition;
            barrel.x -= 0.5f;
            barrelSwitch = !barrelSwitch;
        }
        else
        {
            barrel = transform.localPosition;
            barrel.x += 0.5f;
            barrelSwitch = !barrelSwitch;
        }

        
        GameObject lazer = Instantiate(projectile, transform.TransformPoint(barrel), transform.rotation);
        lazer.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * forceToApply);
        return;
    }
}
