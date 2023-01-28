using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


public class Defense : MonoBehaviour
{
    public event EventHandler OnShieldPickup;
    public event EventHandler<OnShieldValueChangeEventArgs> OnShieldDestroy;

    public event EventHandler<OnShieldValueChangeEventArgs> OnShieldValueChange;
    public class OnShieldValueChangeEventArgs : EventArgs {public float _shieldCurrentValue; }


    GameObject shield;
    PlayerControls playerControls;
    IDefense iDefense;

    float shieldDestroyValue;
    float shieldCurrentValue;

    private void Update()
    {
        if (shield != null)
        {
            HoldShield();
        }
    }

    private void HoldShield()
    {
        if (playerControls.Player.Shield.IsPressed())
        {
            shield.transform.position = transform.position;
            shield.SetActive(true);

            ShieldValueChange();
            
            DestroyShield();
        }
        else if (shield.activeInHierarchy)
        {
            iDefense.EndDefense();

            ShieldValueChange();

            DestroyShield();
        }
        

    }

    private void ShieldValueChange()
    {
        shieldCurrentValue = iDefense.GetCurrentShieldValue();

        float alphaValue = Mathf.InverseLerp(-0.2f, shieldDestroyValue, shieldCurrentValue);
        OnShieldValueChange?.Invoke(this, new OnShieldValueChangeEventArgs { _shieldCurrentValue = alphaValue });
    }

    private void DestroyShield()
    {
        if (shieldCurrentValue <= 0)
        {

            Destroy(shield);
            shield = null;
            OnShieldDestroy?.Invoke(this, new OnShieldValueChangeEventArgs { _shieldCurrentValue = 0 });
            
        }
    }


    public bool HasShield()
    {
        return shield != null;
    }

    public void DisableControls()
    {
        Destroy(shield);
        OnShieldDestroy?.Invoke(this, new OnShieldValueChangeEventArgs { _shieldCurrentValue = 0 });
        this.enabled = false;
    }

    public void DefensePickup(GameObject _defense)
    {
        shield = Instantiate(_defense, transform);
        iDefense = shield.GetComponent<IDefense>();
        shieldDestroyValue = iDefense.GetShieldDestroyValue();
        ShieldValueChange();
        shield.SetActive(false);

        OnShieldPickup?.Invoke(this, EventArgs.Empty);
    }

    public void GetPlayerControls(PlayerControls pc)
    {
        playerControls = pc;
    }
}
