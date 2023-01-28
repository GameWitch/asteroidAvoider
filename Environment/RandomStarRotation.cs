using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStarRotation : MonoBehaviour
{
    private void Awake()
    {
        float rando = Random.Range(-180, 180);
        transform.Rotate(Vector3.forward * rando);
    }
}
