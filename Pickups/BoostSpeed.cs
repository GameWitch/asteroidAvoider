using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostSpeed : MonoBehaviour, IPickUp
{
    float speedBoost = 500f;

    public void HandlePickup(GameObject player)
    {
        player.GetComponent<Movement>().BoostSpeed(speedBoost);
    }
    public string GetPickupType()
    {
        return this.GetType().ToString();

    }
}
