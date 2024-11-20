using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TerrainInformation
{
    public TerrainInformation(int gridLengthX, int gridLengthZ, float gridSpacing, float perlinScale, float OffsetX, float OffsetZ, float gridYHeightRange, float gridYHeightMultiplier, Color terrainColourLow, Color terrainColourHigh, float heightColorChange, bool enableSmoothing, Vector3 position)
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
        this.Position = position;
    }

    /// <summary>
    /// The length of how many vertices's in the X direction.
    /// </summary>
    [field: SerializeField]
    public int GridXLength
    { get; private set; }
    /// <summary>
    /// The length of how many vertices's in the Z direction.
    /// </summary>
    [field: SerializeField]
    public int GridZLength
    { get; private set; }
    /// <summary>
    /// GridSpacing determines the distance between the vertices's - essentially the resolution of the grid.
    /// </summary>
    [field: SerializeField]
    public float GridSpacing
    { get; private set; }
    /// <summary>
    /// PerlinScale changes the detail or resolution of the perlin noise being used in application to the height of the terrain.
    /// </summary>
    [field: SerializeField]
    public float PerlinScale
    { get; private set; }
    [field: SerializeField]
    public float OffsetX
    { get; private set; }
    [field: SerializeField]
    public float OffsetZ
    { get; private set; }
    /// <summary>
    /// Real height range pre-smoothing.
    /// </summary>
    [field: SerializeField]
    public float GridYHeightRange
    { get; private set; }
    /// <summary>
    /// Multiplier tied to the height range.
    /// </summary>
    [field: SerializeField]
    public float GridYHeightMultiplier
    { get; private set; }
    [field: SerializeField]
    public Color TerrainColourLow
    { get; private set; }
    [field: SerializeField]
    public Color TerrainColourHigh
    { get; private set; }
    [field: SerializeField]
    public float HeightColorChange
    { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    [field: SerializeField]
    public bool EnableSmoothing
    { get; private set; }
    [field: SerializeField]
    public Vector3 Position
    { get; private set; }
}
