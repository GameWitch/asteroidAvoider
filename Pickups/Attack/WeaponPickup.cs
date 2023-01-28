using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour, IPickUp
{
    [SerializeField] GameObject[] pickups;

    public void HandlePickup(GameObject player)
    {
        int i = Random.Range(0, pickups.Length);

        player.GetComponent<Attack>().WeaponPickup(pickups[i]);
    }


    public string GetPickupType()
    {
        return this.GetType().ToString();

    }
}    
