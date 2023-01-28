using UnityEngine;
public interface IWeapon
{
    public GameObject GetGameObject();
    public WeaponType GetWeaponType();
    public int GetAmmo();

    public float GetFireRate();

    public void FireProjectile();

}
