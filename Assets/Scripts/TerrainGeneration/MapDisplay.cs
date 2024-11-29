using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    [field: SerializeField]
    public Renderer TextureRenderer
    { get; private set; }

    public void DrawNoiseMap(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int length = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width, length);

        Color[] colorsMap = new Color[width * length];

        int counter = 0;
        for(int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                colorsMap[counter] = Color.Lerp(Color.black, Color.white, noiseMap[x, z]);
                counter++;
            }
        }

        texture.SetPixels(colorsMap);
        texture.Apply();

        TextureRenderer.sharedMaterial.mainTexture = texture;

        TextureRenderer.transform.localScale = new Vector3(width, 1, length);
    }
}
