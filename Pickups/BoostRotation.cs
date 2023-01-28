using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostRotation : MonoBehaviour, IPickUp
{
    float rotationBoost = 125;


    public void HandlePickup(GameObject player)
    {
        player.GetComponent<Movement>().BoostRotation(rotationBoost);
    }

    public string GetPickupType()
    {
        return this.GetType().ToString();

    }
}
