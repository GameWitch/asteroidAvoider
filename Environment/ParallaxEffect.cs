using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] float parallaxValue = 0.95f;

    Camera cam;


    private void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        ApplyParallax();
    }

    void ApplyParallax()
    {
        Vector3 newPos = cam.transform.position * parallaxValue;
        newPos.z = 45;
        transform.position = newPos;
    }
}
