
using UnityEngine;

public class RocketProjectile : MonoBehaviour
{
    [SerializeField] int damageDealt = 10;
    [SerializeField] ParticleSystem explosion;
    Transform target;
    Rigidbody rb;
    bool targetAcquired = false;
    float force = 1000f;
    float rotationSpeed = 200f;

    private void Update()
    {
        if (targetAcquired && target != null)

        {
            Vector3 dir = (target.position - transform.position).normalized;
            Quaternion towards = Quaternion.FromToRotation(Vector3.up, dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, towards, rotationSpeed * Time.deltaTime);
          
            rb.AddForce(dir * force * Time.deltaTime);
        }
        else if (targetAcquired && target == null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
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
    }
    public void GetTarget(Transform _target)
    {
        target = _target;
        rb = GetComponent<Rigidbody>();
        targetAcquired = true;
    }


}
