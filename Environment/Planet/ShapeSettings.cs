using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShapeSettings
{
    public float planetRadius;
    [NonReorderable]
    public NoiseLayer[] noiseLayers;
    
    [System.Serializable]
    public struct NoiseLayer
    {   
        public bool enabled;
        public bool useFirstLayerAsMask;
        public NoiseSettings noiseSettings;
        
    }

}
