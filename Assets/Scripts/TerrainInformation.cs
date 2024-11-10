using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainInformation
{
    public TerrainInformation(int gridLengthX, int gridLengthZ, float gridSpacing, float perlinScale, float OffsetX, float OffsetZ, float gridYHeightRange, float gridYHeightMultiplier, Color terrainColourLow, Color terrainColourHigh, float heightColorChange, bool enableSmoothing)
    {
        this.GridXLength = gridLengthX;
        this.GridZLength = gridLengthZ;
        this.GridSpacing = gridSpacing;
        this.PerlinScale = perlinScale;
        this.OffsetX = OffsetX;
        this.OffsetZ = OffsetZ;
        this.GridYHeightRange = gridYHeightRange;
        this.GridYHeightMultiplier = gridYHeightMultiplier;
        this.TerrainColourLow = terrainColourLow;
        this.TerrainColourHigh = terrainColourHigh;
        this.HeightColorChange = heightColorChange;
        this.EnableSmoothing = enableSmoothing;
    }

    public int GridXLength
    { get; private set; }

    public int GridZLength
    { get; private set; }

    public float GridSpacing
    { get; private set; }

    public float PerlinScale
    { get; private set; }

    public float OffsetX
    { get; private set; }

    public float OffsetZ
    { get; private set; }

    public float GridYHeightRange
    { get; private set; }

    public float GridYHeightMultiplier
    { get; private set; }

    public Color TerrainColourLow
    { get; private set; }

    public Color TerrainColourHigh
    { get; private set; }

    public float HeightColorChange
    { get; private set; }

    public bool EnableSmoothing
    { get; private set; }
}
