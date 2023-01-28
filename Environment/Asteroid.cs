using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour, IDealDamage, IDamageable
{
    int damage = 1;
    int health = 1;
    public void GetLocation(Vector3 newPosition)
    {
        transform.localPosition = newPosition;
        transform.localRotation = Random.rotation;
    }
    public void TakeDamage(int _damageTaken)
    {
        health -= _damageTaken;
        if (health <= 0)
        {

            int asteroidsBlasted = PlayerPrefs.GetInt("AsteroidsBlasted", 0);
            asteroidsBlasted++;

            PlayerPrefs.SetInt("AsteroidsBlasted", asteroidsBlasted);

            Destroy(gameObject);
        }
    }

    public int DealDamage()
    {
        return damage;
    }
}
