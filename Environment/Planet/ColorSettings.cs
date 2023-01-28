using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorSettings
{
    public Material planetMaterial;
    public BiomeColorSettings biomeColorSettings;
    public Gradient oceanColor;
    
    
    [System.Serializable]

    public class BiomeColorSettings
    {
        [NonReorderable]
        public Biome[] biomes;
        public NoiseSettings noise;
        public float noiseOffset;
        public float noiseStrength;
        [Range(0,1)]
        public float blendAmount;
        
        [System.Serializable]
        public class Biome
        {
            [Range(0, 1)]
            public float startHeight;
            [Range(0,1)]
            public float tintPercent;
            public Color tint;            
            public Gradient gradient;

        }


    }

}
