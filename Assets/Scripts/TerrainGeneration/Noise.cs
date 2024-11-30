using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapLength, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapLength];
        
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-1000000, 1000000) + offset.x;
            float offsetZ = prng.Next(-1000000, 1000000) + offset.y;

            octaveOffsets[i] = new Vector2(offsetX, offsetZ);
        }

        // Handling of zero division errors.
        if (scale <= 0.0f)
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfLength = mapLength / 2f;

        for (int z = 0; z < mapLength; z++)
        {
            for(int x = 0; x < mapWidth; x++)
            {
                // Amplitude is how much off an effect the lesser octaves have an impact in overall height values - this is affected by the persistance value.
                float amplitude = 1;
                // Frequency is the level of detail each ocatave passes through to the overall noise height values - this is affected by the lacunarity value.
                float frequency = 1;

                // The noise height is added up over multiple layers - octaves - and the later normalised to allow it to be projected as a value for perlin noise.
                float noiseHeight = 0;

                // Octaves are the layers of Perlin Noise we generate.
                for(int i = 0; i < octaves; i++)
                {
                    // The scale is timesed by the frequency to affect the detail of the respective octave layers, with higher frequency allowing more
                    // finder detail to emerge in the noise.
                    float sampleX = ((float)x - halfLength)/ (scale * frequency) + octaveOffsets[i].x;
                    float sampleZ = ((float)z - halfWidth)/ (scale * frequency) + octaveOffsets[i].y;

                                                                            // Helps to allow negative values of the Perlin Noise.
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleZ) * 2 - 1;

                    // The perlin value is timesed by amplitude to effect how much the other octaves have impact in the overall 
                    // height - i.e. how persistant they are. Lower octaves should propertioanlly have lesser impact.
                    noiseHeight += perlinValue * amplitude;

                    // Per each loop of the octaves, we times the amplitude and frequency by the respective persistance
                    // and lacunarity values to affect the amplitude and the frequency these have per octave layer on the overall height value generated.
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, z] = noiseHeight;
            }
        }

        // Loop through the heightvalues and noramalise due to the previous addition above.
        for (int z = 0; z < mapLength; z++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, z] = Mathf.InverseLerp(maxNoiseHeight, minNoiseHeight, noiseMap[x, z]);
            }
        }

        return noiseMap;
    }
}
