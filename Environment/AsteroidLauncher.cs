using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidLauncher : MonoBehaviour
{
    [SerializeField] GameObject asteroid;
    float launchForce;
    float timeToFire;
    float countDown = 0;
    Transform playerTransform;
    
    Vector3 offset = new Vector3(35, 0, 0);

    void Awake()
    {
        timeToFire = Random.Range(7, 15);

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (playerTransform != null)
        {            
            transform.position = new Vector3(playerTransform.position.x + offset.x, playerTransform.position.y + offset.y, offset.z);
            
            countDown += Time.deltaTime;
            if (countDown >= timeToFire)
            {                
                FireAtPlayer();
                countDown = 0f;
                timeToFire = Random.Range(7, 15);
            }
        }

    }
    void FireAtPlayer()
    {
        Vector3 dir = (playerTransform.position - transform.position).normalized;
        launchForce = Random.Range(200, 500);
        GameObject clone = Instantiate(asteroid);
        clone.GetComponent<Asteroid>().GetLocation(transform.position);
        clone.GetComponent<Rigidbody>().AddRelativeForce(dir * launchForce);
    }

    public void SetOffset(Vector3 _offset)
    {
        offset = _offset;
    }
    public void GetAsteroidGameObject(GameObject _asteroid)
    {
        asteroid = _asteroid;
    }


}
