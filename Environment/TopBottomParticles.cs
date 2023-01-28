using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBottomParticles : MonoBehaviour
{
    ParticleSystem[] particles;
    private void Awake()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
        SetParticleRotation();        
    }


    private void SetParticleRotation()
    {
        
        foreach (ParticleSystem par in particles)
        {
            var main = par.main;
            main.startRotation = -transform.eulerAngles.z * Mathf.Deg2Rad;
        }
    }

    public void SetParticleColorGradients(Gradient gradient)
    {
        foreach (ParticleSystem par in particles)
        {
            var main = par.main;
            main.startColor = gradient;
        }
    }

}
