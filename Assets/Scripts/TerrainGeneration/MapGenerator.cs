using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [field: SerializeField] public int MapWidth
    { get; private set; }
    [field: SerializeField] public int MapLength
    { get; private set; }
    [field: SerializeField] public float NoiseScale
    { get; private set; }
    [field: SerializeField] public int Octaves
    { get; private set; }
    [field: SerializeField, Range(0.0f, 1.0f)] public float Persistance
    { get; private set; }
    [field: SerializeField] public float Lacunarity
    { get; private set; }

    [field: SerializeField] public int Seed
    { get; private set; }
    [field: SerializeField] public Vector2 Offset
    { get; private set; }

    [field: SerializeField] public bool AutoGenerate
    { get; private set; }

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(MapWidth, MapLength, Seed, NoiseScale, Octaves, Persistance, Lacunarity, Offset);

        MapDisplay mapDisplay = GetComponent<MapDisplay>();

        mapDisplay.DrawNoiseMap(noiseMap);
    }

    private void OnValidate()
    {
        if (MapWidth < 1)
        {
            MapWidth = 1;
        }
        if (MapLength < 1)
        { 
            MapLength = 1; 
        }
        if (Octaves < 0)
        {
            Octaves = 0;
        }
    }
}
