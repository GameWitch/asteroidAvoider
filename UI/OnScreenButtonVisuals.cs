using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnScreenButtonVisuals : MonoBehaviour
{
    [SerializeField] Slider thrustButton;
    [SerializeField] Slider shootButton;
    [SerializeField] Slider shieldButton;
    [SerializeField] Slider leftButton;
    [SerializeField] Slider rightButton;


    Movement movement;
    Attack attack;
    Defense defense;

    private void Awake()
    {
        leftButton.value = 0;
        rightButton.value = 0;
        thrustButton.value = 0;
        shieldButton.gameObject.SetActive(false);
        shootButton.gameObject.SetActive(false);
        
    }

    public void GetControls(Movement move, Attack at, Defense df)
    {

        movement = move;
        attack = at;
        defense = df;

        defense.OnShieldPickup += Defense_OnShieldPickup;
        defense.OnShieldDestroy += Defense_OnShieldDestroy;
        defense.OnShieldValueChange += Defense_OnShieldValueChange;

        attack.OnWeaponPickup += Attack_OnWeaponPickup;
        attack.OnWeaponDestroy += Attack_OnWeaponDestroy;
        attack.CanFire += Attack_CanFire;
        attack.CanNotFire += Attack_CanNotFire;
        attack.OnAmmoChange += Attack_OnAmmoChange;

        movement.OnBoostRotation += Movement_OnBoostRotation;
        movement.OnBoostSpeed += Movement_OnBoostSpeed;
    }

    private void Attack_OnAmmoChange(object sender, Attack.OnAmmoChangeEventArgs e)
    {
        shootButton.value = e._ammo;
    }

    private void Attack_CanNotFire(object sender, System.EventArgs e)
    {
        // maybe do some sprite stuff in here
    }

    private void Attack_CanFire(object sender, System.EventArgs e)
    {
        // sprite stuff in here too dawg
    }

    private void Attack_OnWeaponDestroy(object sender, System.EventArgs e)
    {
        shootButton.gameObject.SetActive(false);
    }

    private void Attack_OnWeaponPickup(object sender, Attack.OnAmmoChangeEventArgs e)
    {
        shootButton.maxValue = e._ammo;
        shootButton.value = e._ammo;
        shootButton.gameObject.SetActive(true);
    }



    private void Defense_OnShieldValueChange(object sender, Defense.OnShieldValueChangeEventArgs e)
    {
        shieldButton.value = e._shieldCurrentValue;
    }
    private void Defense_OnShieldDestroy(object sender, Defense.OnShieldValueChangeEventArgs e)
    {
        shieldButton.value = e._shieldCurrentValue;
        shieldButton.gameObject.SetActive(false);
    }

    private void Defense_OnShieldPickup(object sender, System.EventArgs e)
    {
        shieldButton.gameObject.SetActive(true);
    }



    private void Movement_OnBoostSpeed(object sender, System.EventArgs e)
    {
        thrustButton.value = 1;
    }

    private void Movement_OnBoostRotation(object sender, System.EventArgs e)
    {
        leftButton.value = 1;
        rightButton.value = 1;
    }
}
