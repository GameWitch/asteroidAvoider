using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryCleanup : MonoBehaviour
{

    [SerializeField] ParticleSystem explosion;
    [SerializeField] ParticleSystem yAxis;
    [SerializeField] Transform topBottomParticles;

    BoxCollider boundary;

    private void Awake()
    {
        boundary = GetComponent<BoxCollider>();

    }

    private void OnTriggerExit(Collider other)
    {
        string tag = other.gameObject.tag;
        switch (tag)
        {
            case "Untagged":

                Instantiate(explosion, other.gameObject.transform.position, Quaternion.identity);

                Destroy(other.gameObject);
                break;
            case "Shield":
                Destroy(other.gameObject);
                break;
        }
    }


    public void ShapeBoundary(float endOfField, float heightOfField)
    {
        float sizeOffset = 50f;
        float centerOffset = 15f;

        Vector3 size = new Vector3(endOfField + sizeOffset, heightOfField, 15f);
        boundary.size = size;
        boundary.center = new Vector3((endOfField / 2f) + centerOffset, heightOfField / 2f, 0f);

        ParticleSystem clone = Instantiate(yAxis);
        clone.gameObject.transform.position = new Vector3(-6f, 25f, 0f);
        
    }
}
