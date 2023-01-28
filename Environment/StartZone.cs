using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartZone : MonoBehaviour
{
    public void SetUpVector(Vector3 dir)
    {
        Quaternion newUp = Quaternion.FromToRotation(Vector3.up, dir);
        transform.rotation = newUp;
    }
}
