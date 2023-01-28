using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{   
    public enum FilterType {  Simple, Ridgid};
    public FilterType filterType;

    [Range(1,8)]
    public int numLayers = 1;
    public float strength = 1;
    public float baseRoughness = 1f;
    public float roughness = 2;
    public float persistance = .5f;
    public float minValue;
    public Vector3 center = Vector3.zero;
}
