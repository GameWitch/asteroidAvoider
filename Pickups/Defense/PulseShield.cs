using System;
using UnityEngine;


public class PulseShield : MonoBehaviour, IDefense
{

    [SerializeField] float force = 200f;    
    [SerializeField] ParticleSystem particles;

    SphereCollider sphereCollider;

    int pulses = 3;
    bool runOnPress = true;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if (runOnPress)
        {
            Instantiate(particles, transform.position, Quaternion.identity);
            pulses--;
            runOnPress = false;

        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        
        switch (collision.gameObject.tag)
        {
            case "Untagged":

                Vector3 dir = (collision.GetContact(0).point - transform.position).normalized;
                collision.gameObject.GetComponent<Rigidbody>().AddForce(dir * force);
                sphereCollider.enabled = false;

                break;
        }
    }


    public float GetShieldDestroyValue()
    {
        return pulses;
    }
    
    public float GetCurrentShieldValue()
    {
        return pulses;
    }

    public void EndDefense()
    {
        sphereCollider.enabled = true;        
        runOnPress = true;
        gameObject.SetActive(false);

    }

}
