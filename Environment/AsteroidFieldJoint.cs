using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFieldJoint : MonoBehaviour
{
    [SerializeField] GameObject asteroidLauncher;
    [SerializeField] GameObject arrow;
    GameObject lastLeg;
    GameObject nextLeg;

    Vector3 direction;
    bool doOnce = true;
    
    public void GetLastAndNextLeg(GameObject last, GameObject next)
    {

        lastLeg = last;
        nextLeg = next;
        direction = (nextLeg.transform.position - transform.position).normalized;

        arrow = Instantiate(arrow);
        arrow.transform.position = transform.position;
        arrow.transform.parent = transform;

        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, direction);
        arrow.transform.rotation = rotation;

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            if (lastLeg) lastLeg.SetActive(false);

            nextLeg.SetActive(true);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            
            if (lastLeg) Destroy(lastLeg);
            if (doOnce)
            {
                Instantiate(asteroidLauncher);
                doOnce = false;
            }
        }
    }
}
