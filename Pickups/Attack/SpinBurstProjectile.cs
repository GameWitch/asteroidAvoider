
using UnityEngine;

public class SpinBurstProjectile : MonoBehaviour
{
    [SerializeField] int damageDealt = 5;
    [SerializeField] ParticleSystem explosion;
    private void OnCollisionEnter(Collision collision)
    {
        IDamageable objectToTakeDamage = collision.gameObject.GetComponent<IDamageable>();
        if (objectToTakeDamage != null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            objectToTakeDamage.TakeDamage(damageDealt);
        }
    }
}
