using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeGradients : MonoBehaviour
{
    [SerializeField] Gradient[] biomeGradients;
    [SerializeField] Gradient[] oceanGradients;

    public Gradient GetRandoBiomeGradient()
    {
        int i = Random.Range(0, biomeGradients.Length);
        return biomeGradients[i];
    }

    public Gradient GetRandoOceanGradient()
    {
        int i = Random.Range(0, oceanGradients.Length);
        return oceanGradients[i];
    }
}
