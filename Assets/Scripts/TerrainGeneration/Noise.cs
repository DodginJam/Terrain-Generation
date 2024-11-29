using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapLength, float scale)
    {
        float[,] noiseMap = new float[mapWidth, mapLength];
        
        // Handling of zero division errors.
        if (scale <= 0.0f)
        {
            scale = 0.0001f;
        }

        for (int z = 0; z < mapLength; z++)
        {
            for(int x = 0; x < mapWidth; x++)
            {
                float sampleX = (float)x / scale;
                float sampleZ = (float)z / scale;

                float perlinValue = Mathf.PerlinNoise(sampleX, sampleZ);

                noiseMap[x, z] = perlinValue;
            }
        }

        return noiseMap;
    }
}
