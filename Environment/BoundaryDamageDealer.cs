using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryDamageDealer : MonoBehaviour, IDealDamage
{
    
    int damageToDeal = 1;

    ParticleSystem explosion;

    private void Awake()
    {
        explosion = transform.parent.GetComponent<AsteroidFieldLeg>().GetExplosion();
    }

    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Untagged")
        {
           // Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }
    }


    // this is only used for the endcap but i didnt wanna give it its own seperate script
    // cause im a hack and a fraud *hangLoose-emoji*
    private void OnTriggerEnter(Collider other)
    {
        if (tag == "Untagged")
        {
            // Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
    }

    public int DealDamage()
    {
        return damageToDeal;
    }
}
