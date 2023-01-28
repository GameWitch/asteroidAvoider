using UnityEngine;
using System;

public class Attack : MonoBehaviour
{
    public event EventHandler<OnAmmoChangeEventArgs> OnWeaponPickup;
    public event EventHandler OnWeaponDestroy;
    public event EventHandler CanFire;
    public event EventHandler CanNotFire;
    public event EventHandler<OnAmmoChangeEventArgs> OnAmmoChange;

    public class OnAmmoChangeEventArgs : EventArgs  {  public int _ammo; }


   // ScoreKeeper scoreKeeper;
    GameObject weaponGameObject;
    IWeapon iWeapon;

    PlayerControls playerControls;
    int ammo = 0;
    bool hasWeapon = false;
    bool runCanFireEvent = true;
    float fireRate;
    float fireRateCounter;

    private void Awake()
    {
      //  scoreKeeper = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreKeeper>();

    }

    void Update()
    {
        if (hasWeapon)
        {
            fireRateCounter += Time.deltaTime;
            if (fireRateCounter >= fireRate)
            {
                if (runCanFireEvent)
                {
                    CanFire?.Invoke(this, EventArgs.Empty);
                    runCanFireEvent = false;
                }
                if (ammo > 0)
                {
                    if (playerControls.Player.Fire.IsPressed())
                    {
                        iWeapon.FireProjectile();

                        fireRateCounter = 0;
                        ammo--;
                        OnAmmoChange?.Invoke(this, new OnAmmoChangeEventArgs { _ammo = ammo });

                        CanNotFire?.Invoke(this, EventArgs.Empty);
                        runCanFireEvent = true;
                    }
                }
                else
                {
                    hasWeapon = false;
                    iWeapon = null;
                    Destroy(weaponGameObject);
                    OnWeaponDestroy?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    public void DisableControls()
    {
        Destroy(weaponGameObject);
        iWeapon = null;
        OnWeaponDestroy?.Invoke(this, EventArgs.Empty);
        this.enabled = false;
    }

    public void WeaponPickup(GameObject weapon)
    {
        hasWeapon = true;

        Destroy(weaponGameObject);
        weaponGameObject = Instantiate(weapon, transform); ;


        // we need to test this where we see if the weapontype is the same abd either add the old ammo with new
        // or we just call the ammo its default value
        // once working we can abstract to function
        WeaponType wt = WeaponType.NONE;
        if (iWeapon != null)
        {
            wt = iWeapon.GetWeaponType();
        }
        iWeapon = weaponGameObject.GetComponent<IWeapon>();        

        if (wt == iWeapon.GetWeaponType())
        {
            ammo += iWeapon.GetAmmo();
        }
        else
        {
            ammo = iWeapon.GetAmmo();
        }

        //scoreKeeper.GetAmmo(ammo);



        fireRate = iWeapon.GetFireRate();
        fireRateCounter = fireRate;

        OnWeaponPickup?.Invoke(this, new OnAmmoChangeEventArgs { _ammo = ammo });

    }

    public void GetPlayerControls(PlayerControls pc)
    {
        playerControls = pc;
    }
}
