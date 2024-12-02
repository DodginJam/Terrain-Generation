using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TerrainInformation
{
    public TerrainInformation(int gridLengthX, int gridLengthZ, float gridSpacing, float perlinScale, float OffsetX, float OffsetZ, float gridYHeightRange, float gridYHeightMultiplier, Color terrainColourLow, Color terrainColourHigh, Gradient terrainGradient, float heightColorChange, bool enableSmoothing, Vector3 position, Material terrainMaterial, int seed, int octaves, float persistance, float lacunarity, Vector2 octaveOffset)
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
        this.TerrainGradient = terrainGradient;
        this.HeightColorChange = heightColorChange;
        this.EnableSmoothing = enableSmoothing;
        this.Position = position;
        this.TerrainMaterial = terrainMaterial;
        this.Seed = seed;
        this.Octaves = octaves;
        this.Persistance = persistance;
        this.Lacunarity = lacunarity;
        this.OctaveOffset = octaveOffset;
    }

    [SerializeField, Range(0, 2000)]
    private int gridXLength;
    /// <summary>
    /// The length of how many vertices's in the X direction.
    /// </summary>
    [SerializeField]
    public int GridXLength
    { 
        get { return gridXLength; }
        private set 
        {
            gridXLength = Mathf.Clamp(value, 0, 2000);
        } 
    }

    [SerializeField, Range(0, 2000)]
    private int gridZLength;
    /// <summary>
    /// The length of how many vertices's in the Z direction.
    /// </summary>
    [SerializeField]
    public int GridZLength
    {
        get { return gridZLength; }
        private set
        {
            gridZLength = Mathf.Clamp(value, 0, 2000);
        }
    }

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
    /// <summary>
    /// The lower color in the singular gradiant applied to the terrain. Redundant.
    /// </summary>
    [field: SerializeField]
    public Color TerrainColourLow
    { get; private set; }
    /// <summary>
    /// The higher color in the singular gradiant applied to the terrain. Redundant.
    /// </summary>
    [field: SerializeField]
    public Color TerrainColourHigh
    { get; private set; }
    /// <summary>
    /// Gradient for terrain colouring.
    /// </summary>
    [field: SerializeField]
    public Gradient TerrainGradient
    { get; private set; }
    /// <summary>
    /// The height at which the mid-point of the gradient occurs. Redundant.
    /// </summary>
    [field: SerializeField]
    public float HeightColorChange
    { get; private set; }
    /// <summary>
    /// Allows one pass of smoothing to be applied to the terrain.
    /// </summary>
    [field: SerializeField]
    public bool EnableSmoothing
    { get; private set; }
    /// <summary>
    /// The position of the Mesh in the world.
    /// </summary>
    [field: SerializeField]
    public Vector3 Position
    { get; private set; }
    /// <summary>
    /// The material of the Mesh.
    /// </summary>
    [field: SerializeField]
    public Material TerrainMaterial
    { get; private set; }

    [field: SerializeField] public int Seed
    { get; private set; }
    [field: SerializeField] public int Octaves
    { get; private set; }
    [SerializeField, Range(0f, 1f)]
    private float persistance;
    /// <summary>
    /// The length of how many vertices's in the Z direction.
    /// </summary>
    [SerializeField]
    public float Persistance
    {
        get { return persistance; }
        private set
        {
            persistance = Mathf.Clamp(value, 0f, 1f);
        }
    }

    
    [field: SerializeField] public float Lacunarity
    { get; private set; }
    [field: SerializeField] public Vector2 OctaveOffset
    { get; private set; }
}
