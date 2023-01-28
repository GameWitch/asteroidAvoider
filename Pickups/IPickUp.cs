using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickUp
{
    public void HandlePickup(GameObject player);
    public string GetPickupType();
}
