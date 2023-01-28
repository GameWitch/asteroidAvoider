using UnityEngine;

public class PickupHandler : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        switch (tag)
        {
            case "Pickup":
                other.gameObject.GetComponent<IPickUp>().HandlePickup(this.gameObject);
                Destroy(other.gameObject);
                break;
        }
    }
}
