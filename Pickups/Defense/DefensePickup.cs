using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensePickup : MonoBehaviour, IPickUp
{
    [SerializeField] GameObject[] pickups;

    public void HandlePickup(GameObject player)
    {
        int i = Random.Range(0, pickups.Length);

        player.GetComponent<Defense>().DefensePickup(pickups[i]);
    }

    public string GetPickupType()
    {
        return this.GetType().ToString();
    }

}
