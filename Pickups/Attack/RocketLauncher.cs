using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour, IWeapon
{
    [SerializeField] GameObject rocket;
    LineRenderer line;

    Vector3[] points = new Vector3[2];
    Vector3[] empty = new Vector3[2] { Vector3.zero, Vector3.zero };

    Dictionary<int, Transform> transformIDs = new Dictionary<int, Transform>();

    float fireTimer = 0;
    float timeToFire = 2.1f;
    float range = 15f;
    float fireRate = 5f;
    float internalFireRate = 1f;
    float internalFireTimer = 0;
    int ammo = 25;
    int layerMask = 1 << 7;
    bool fire = false;
    
    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.widthMultiplier = 0.2f;
        line.startColor = Color.white;
        line.SetPositions(empty);

        layerMask = ~layerMask;
    }
    private void FixedUpdate()
    {
        if (fire)
        {
            // this timer defines how long we have to mark targets and fire at them
            fireTimer += Time.deltaTime;
            if (fireTimer > timeToFire)
            {               
                fire = false;
                fireTimer = 0f;
                line.SetPositions(empty);
                return;
            }


            // this target defines how long in between actually firing at all the targets we mark and store in dictionary
            internalFireTimer += Time.deltaTime;
            if (internalFireTimer > internalFireRate)
            {
                foreach (KeyValuePair<int, Transform> entry in transformIDs)
                {

                    Instantiate(rocket, transform.position, transform.rotation)
                        .GetComponent<RocketProjectile>().GetTarget(entry.Value);    
                    
                    
                }
                
                // reset the timer and dictionary
                internalFireTimer = 0f;
                transformIDs.Clear();

                // out here we can play a big fire all rockets SFX
            }


            // here we raycast, draw lines and mark targets to fire at
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.up, out hit, range, layerMask))
            {
                SetLinePoints(hit.distance);

                if (hit.collider.tag == "Untagged" && !transformIDs.ContainsKey(hit.colliderInstanceID))
                {
                    transformIDs.Add(hit.colliderInstanceID, hit.collider.transform);
                    // playa  boop here to signal target marked
                }
            }
            else
            {
                SetLinePoints(range);
            }                
            
            
        }
    }
    private void SetLinePoints(float range)
    {
        points[0] = transform.position;
        points[1] = transform.position + transform.TransformDirection(Vector3.up) * range;
        line.SetPositions(points);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public WeaponType GetWeaponType()
    {
        return WeaponType.ROCKETLAUNCHER;
    }
    public int GetAmmo()
    {
        return ammo;
    }

    public float GetFireRate()
    {
        return fireRate;
    }

    public void FireProjectile()
    {
        fire = true;
        
    }
}
