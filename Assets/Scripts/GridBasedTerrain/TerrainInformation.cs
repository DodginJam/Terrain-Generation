using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TerrainInformation
{
    public TerrainInformation(int gridLengthX, int gridLengthZ, float gridSpacing, float perlinScale, float OffsetX, float OffsetZ, float gridYHeightRange, float gridYHeightMultiplier, Gradient terrainGradient, float heightColorChange, bool enableSmoothing, Vector3 position, Material terrainMaterial, int seed, int octaves, float persistance, float lacunarity, Vector2 octaveOffset, AnimationCurve terrainCurve, bool colourLockToHeight, NormalizeMode normalizeMode)
    {
        this.GridXLength = gridLengthX;
        this.GridZLength = gridLengthZ;
        this.GridSpacing = gridSpacing;
        this.PerlinScale = perlinScale;
        this.OffsetX = OffsetX;
        this.OffsetZ = OffsetZ;
        this.GridYHeightRange = gridYHeightRange;
        this.GridYHeightMultiplier = gridYHeightMultiplier;
        this.TerrainGradient = terrainGradient;
        this.EnableSmoothing = enableSmoothing;
        this.Position = position;
        this.TerrainMaterial = terrainMaterial;
        this.Seed = seed;
        this.Octaves = octaves;
        this.Persistance = persistance;
        this.Lacunarity = lacunarity;
        this.OctaveOffset = octaveOffset;
        this.TerrainCurve = terrainCurve;
        this.ColourLockToHeight = colourLockToHeight;
        this.HeightNormalisation = normalizeMode;

        // HeightColourChange values is locked to GridHeight value if the bool is true.
        if (!colourLockToHeight)
        {
            this.HeightColorChange = heightColorChange;
        }
        else
        {
            this.HeightColorChange = gridYHeightRange;
        }
    }

    [SerializeField, Range(1, 2000), Tooltip("The length of how many vertices's in the X direction.")]
    private int gridXLength;
    /// <summary>
    /// The length of how many vertices's in the X direction.
    /// </summary>
    [SerializeField]
    public int GridXLength
    { 
        get { return gridXLength; }
        set 
        {
            gridXLength = Mathf.Clamp(value, 1, 2000);
        } 
    }

    [SerializeField, Range(1, 2000), Tooltip("The length of how many vertices's in the Z direction.")]
    private int gridZLength;
    /// <summary>
    /// The length of how many vertices's in the Z direction.
    /// </summary>
    [SerializeField]
    public int GridZLength
    {
        get { return gridZLength; }
        set
        {
            gridZLength = Mathf.Clamp(value, 1, 2000);
        }
    }

    /// <summary>
    /// GridSpacing determines the distance between the vertices's - essentially the resolution of the grid.
    /// </summary>
    [field: SerializeField, Tooltip("The distance between the vertices of the mesh")]
    public float GridSpacing
    { get; set; }

    /// <summary>
    /// PerlinScale changes the detail or resolution of the perlin noise being used in application to the height of the terrain.
    /// </summary>
    [field: SerializeField, Tooltip("The scale of the perlin noise being used in application to the height of the terrain"), Range(0.01f, 300f)]
    public float PerlinScale
    { get; set; }

    /// <summary>
    /// Offset the PerlinNoise data being applied to the mesh via the X axis.
    /// </summary>
    [field: SerializeField, Tooltip("Offset the PerlinNoise data being applied to the mesh via the X axis.")]
    public float OffsetX
    { get; set; }

    /// <summary>
    /// Offset the PerlinNoise data being applied to the mesh via the Z axis.
    /// </summary>
    [field: SerializeField, Tooltip("Offset the PerlinNoise data being applied to the mesh via the Z axis.")]
    public float OffsetZ
    { get; set; }

    /// <summary>
    /// The maximum height value that can be produced by the generated Mesh Y vertices - pre-smoothing.
    /// </summary>
    [field: SerializeField, Tooltip("The maximum height value that can be produced by the generated Mesh Y vertices")]
    public float GridYHeightRange
    { get; set; }

    /// <summary>
    /// Multiplier tied to the height range - currently unused.
    /// </summary>
    public float GridYHeightMultiplier
    { get; set; }

    /// <summary>
    /// Gradient colours that are applied to the Mesh - these values are mapped out onto the mesh with colour determnined by height of Y vertices.
    /// </summary>
    [field: SerializeField, Tooltip("Gradient colours that are applied to the Mesh - these values are mapped out onto the mesh with colour determnined by height of Y vertices.\r\n")] public Gradient TerrainGradient
    { get; set; }

    /// <summary>
    /// The height at which the mid-point of the gradient occurs.
    /// </summary>
    [field: SerializeField, Tooltip("Change the height value at which the gradient colours are applied to the mesh.")]
    public float HeightColorChange
    { get; set; }

    /// <summary>
    /// Allows one pass of smoothing to be applied to the terrain.
    /// </summary>
    [field: SerializeField, Tooltip("Allow height smoothing of the meshes vertices.")]
    public bool EnableSmoothing
    { get; set; }

    /// <summary>
    /// The position of the Mesh in the world.
    /// </summary>
    [field: SerializeField, Tooltip("The position of the Mesh in the world")]
    public Vector3 Position
    { get; set; }

    /// <summary>
    /// The material of the Mesh.
    /// </summary>
    [field: SerializeField, Tooltip("The material to apply to the mesh - ideally a Shader Graph shader")]
    public Material TerrainMaterial
    { get; set; }

    /// <summary>
    /// The seed here is fed into a new initialisation of the System.Random class to provide reproducable outcomes.
    /// Provides offsets per octave layer to be randomised -  too be re-looked at its application.
    /// </summary>
    [field: SerializeField, Tooltip("Provides offsets per octave layer to be randomised - too be re-looked at its application")] public int Seed
    { get; set; }

    [SerializeField, Range(1, 20)] 
    private int octaves;
    public int Octaves
    {
        get { return octaves; }
        set
        {
            octaves = Mathf.Clamp(value, 1, 20);
        }
    }
    [SerializeField, Range(0f, 1f)]
    private float persistance;
    public float Persistance
    {
        get { return persistance; }
        set
        {
            persistance = Mathf.Clamp(value, 0f, 1f);
        }
    }

    [SerializeField, Range(0.1f, 5.0f)]
    private float lacunarity;
    public float Lacunarity
    {
        get { return lacunarity; }
        set
        {
            lacunarity = Mathf.Clamp(value, 0.1f, 5.0f);
        }
    }
    [field: SerializeField] public Vector2 OctaveOffset
    { get; set; }

    /// <summary>
    /// Animation curve will affect the normalised height value before being multiplied by the Height Range to alllow modified terrain in certain ranges of the normalised height scale.
    /// </summary>
    [field: SerializeField, Tooltip("Animation curve will affect the normalised height value before being multiplied by the Height Range to alllow modified terrain in certain ranges of the normalised height scale.")] 
    public AnimationCurve TerrainCurve
    { get; set; }

    public bool ColourLockToHeight
    { get; set; }

    [Serializable]
    public enum NormalizeMode
    {
        Local,
        Global
    }

    [field: SerializeField]
    public NormalizeMode HeightNormalisation
    { get; set; } = NormalizeMode.Global;
}
