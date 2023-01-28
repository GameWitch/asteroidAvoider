using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunPellet : MonoBehaviour
{
    [SerializeField] int damageDealt = 7;
    [SerializeField] ParticleSystem explosion;
    float range = 12f;
    Vector3 startingPosition;

    private void Awake()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
        float distance = Vector3.Distance(startingPosition, transform.position);
        if (distance > range)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable objectToTakeDamage = collision.gameObject.GetComponent<IDamageable>();
        if (objectToTakeDamage != null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            objectToTakeDamage.TakeDamage(damageDealt);
        }
        /*
        if (collision.gameObject.tag == "Untagged")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);

            int asteroidsBlasted = PlayerPrefs.GetInt("AsteroidsBlasted", 0);
            asteroidsBlasted++;
            PlayerPrefs.SetInt("AsteroidsBlasted", asteroidsBlasted);

            Destroy(gameObject);
        }
        */
    }
}
