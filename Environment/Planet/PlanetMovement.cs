
using UnityEngine;

public class PlanetMovement : MonoBehaviour
{
    [SerializeField] Vector3 rotation;
    [SerializeField] Vector3 startPosition;
    [SerializeField, Range(0,1)] 
    float parallaxValue = 1;

    Camera cam;

    bool planetGenerated = false;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (planetGenerated)
        {
            transform.Rotate(rotation*Time.deltaTime);

            ParallaxEffect();
        }
    }

    void ParallaxEffect()
    {
        Vector3 newPos = new Vector3(cam.transform.position.x + 20, cam.transform.position.y, 40) * parallaxValue;
        transform.position = newPos;
    }

    public void PlanetGenerated()
    {
        planetGenerated = true;
    }
}
