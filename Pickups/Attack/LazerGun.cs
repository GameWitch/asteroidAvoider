using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerGun : MonoBehaviour, IWeapon
{
    AudioController audioControl;
    [SerializeField] GameObject projectile;
    [SerializeField] float forceToApply = 250f;
    float fireRate = 0.25f;
    int ammo = 100;

    private void Start()
    {
        audioControl = GameObject.FindGameObjectWithTag("Core").GetComponent<AudioController>();
    }

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
        audioControl.PlayAudio(AudioType.MP_00);
        GameObject lazer = Instantiate(projectile, transform.position, transform.rotation);
        lazer.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * forceToApply);
        return;
    }
}
