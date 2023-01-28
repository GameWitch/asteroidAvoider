using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField]
    bool GeneratePlanetAgain = false;
    [SerializeField] 
    bool isPlanet = false;
    bool planetGenerated = false;

    [SerializeField] ColorSettings colorSettings;
    [SerializeField] ShapeSettings shapeSettings;

    ShapeGenerator shapeGenerator;
    ColorGenerator colorGenerator = new ColorGenerator();
    BiomeGradients biomeGradients;
    PlanetMovement planetMovement;

    [SerializeField, Range(2, 256)] 
    int resolution = 10;



    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;
    int faces = 6;   


    private void Awake()
    {
        if (isPlanet)
        {
            biomeGradients = GetComponent<BiomeGradients>();
            planetMovement = GetComponent<PlanetMovement>();
            planetGenerated = false;
        }

        if (transform.childCount == 0)
        {
            RandomizePlanet();
        }
        // do a for loop on shapesettings.noiselayers and randomize the center for each layer of noise to randomize planet
        // while maintaining settings, could also set each noiselayer's settings randomly within a range 
        // shapeSettings.noiseLayers[i].noiseSettings.center

    }


    private void Update()
    {
        if (GeneratePlanetAgain)
        {
            RandomizePlanet();
            GeneratePlanetAgain = false;
        }
        if (planetGenerated && isPlanet)
        {
            planetMovement.PlanetGenerated();
            planetGenerated = false;
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
        planetGenerated = true;
    }

    void Initialize()
    {
        shapeGenerator = new ShapeGenerator(shapeSettings);
        colorGenerator.UpdateSettings(colorSettings);

        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[faces];

        }

        terrainFaces = new TerrainFace[faces];
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < faces; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    void GenerateMesh()
    {
        foreach(TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
        colorGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }

    void GenerateColors()
    {
        colorGenerator.UpdateColors();
        if (isPlanet)
        {
            RandomizeColorValues();

        }
        foreach (TerrainFace face in terrainFaces)
        {
            face.UpdateUVs(colorGenerator);
        }
    }

    private void RandomizeShapeValues()
    {
        for (int i = 0; i < shapeSettings.noiseLayers.Length; i++)
        {
            RandomCenter(i);
            // colorSettings.biomeColorSettings.biomes[i].gradient
        }
    }

    private void RandomizeColorValues()
    {
        for (int i = 0; i < colorSettings.biomeColorSettings.biomes.Length; i++)
        {
            colorSettings.biomeColorSettings.biomes[i].gradient = biomeGradients.GetRandoBiomeGradient();
        }
        colorSettings.oceanColor = biomeGradients.GetRandoOceanGradient();
    }

    private void RandomCenter(int i)
    {
        float x = Random.Range(-1000, 1000);
        float y = Random.Range(-1000, 1000);
        float z = Random.Range(-1000, 1000);
        shapeSettings.noiseLayers[i].noiseSettings.center = new Vector3(x, y, z);
    }

    private void RandomizePlanet()
    {
        RandomizeShapeValues();
        if (isPlanet)
        {
            RandomizeColorValues();
            shapeSettings.planetRadius = Random.Range(5, 13);

        }
        GeneratePlanet();
    }
}
