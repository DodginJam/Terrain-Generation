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

    [field: SerializeField] public bool AutoGenerate
    { get; private set; }

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(MapWidth, MapLength, NoiseScale);

        MapDisplay mapDisplay = GetComponent<MapDisplay>();

        mapDisplay.DrawNoiseMap(noiseMap);
    }
}
