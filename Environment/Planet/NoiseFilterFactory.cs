using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseFilterFactory
{
    public static INoiseFilter CreateNoiseFilter(NoiseSettings noiseSettings)
    {
        switch (noiseSettings.filterType)
        {
            case NoiseSettings.FilterType.Simple:
                return new NoiseFilter(noiseSettings);
            case NoiseSettings.FilterType.Ridgid:
                return new RigidNoiseFilter(noiseSettings);            
        }
        return null;
    }
}
