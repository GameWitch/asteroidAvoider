using UnityEngine;

public class Shotgun : MonoBehaviour, IWeapon
{
    [SerializeField] GameObject projectile;

    Vector3[] cardinalDirecctions =
        new Vector3[5] {new Vector3(-0.5f,1,0), new Vector3(-0.25f, 1, 0),
        new Vector3(0,1,0), new Vector3(0.25f,1,0), new Vector3(0.5f,1,0)};
    
    int ammo = 100;
    float forceToAdd = 3500f;
    float fireRate = 1f;

    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public void FireProjectile()
    {
        for (int i = 0; i < cardinalDirecctions.Length; i++)
        {

            Vector3 fireDirection = transform.TransformDirection(cardinalDirecctions[i]);
            GameObject pellet = Instantiate(projectile, transform.position, transform.rotation);
            pellet.GetComponent<Rigidbody>().AddForce(fireDirection * forceToAdd);
        }
    }

    public WeaponType GetWeaponType()
    {
        return WeaponType.SHOTGUN;
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
