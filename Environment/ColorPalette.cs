
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ColorPalette", order = 1)]
public class ColorPalette : ScriptableObject
{
    public Gradient background;
    public Gradient borderParticles;
    public Gradient planetLand;
    public Gradient planetOcean;
    public Gradient asteroids;
}
