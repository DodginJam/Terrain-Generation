using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCurveOptions : MonoBehaviour
{
    [Serializable]
    public class TerrainRange
    {
        public TerrainRange(TerrainInformation info, float gridSpacingRange, float perlinScaleRange, float heightRange, float heightColourAppliedRange, float octaveRange, float persistanceRange, float lacunarityRange)
        {
            this.Information = info;
            this.GridSpacingRange = gridSpacingRange;
            this.PerlinScaleRange = perlinScaleRange;
            this.HeightRange = heightRange;
            this.HeightColourAppliedRange = heightColourAppliedRange;
            this.OctaveRange = octaveRange;
            this.PersistanceRange = persistanceRange;
            this.LacunarityRange = lacunarityRange;
        }

        [field: SerializeField]
        public TerrainInformation Information
        { get; private set; }

        [field: SerializeField, Header("Spacing for Random.Range on either side of respective Terrain value")]
        public float GridSpacingRange
        { get; private set; }
        [field: SerializeField]
        public float PerlinScaleRange
        { get; private set; }
        [field: SerializeField]
        public float HeightRange
        { get; private set; }
        [field: SerializeField]
        public float HeightColourAppliedRange
        { get; private set; }
        [field: SerializeField]
        public float OctaveRange
        { get; private set; }
        [field: SerializeField]
        public float PersistanceRange
        { get; private set; }
        [field: SerializeField]
        public float LacunarityRange
        { get; private set; }

    }

    [field: SerializeField]
    public TerrainRange RollingHills
    {  get; private set; }

    [field: SerializeField]
    public TerrainRange Mountains
    { get; private set; }

    [field: SerializeField]
    public TerrainRange DesertDunes
    { get; private set; }

    [field: SerializeField]
    public TerrainRange RockyArid
    { get; private set; }

    [field: SerializeField]
    public TerrainRange IslandsFlat
    { get; private set; }

    [field: SerializeField]
    public TerrainRange IslandsHills
    { get; private set; }

    [field: SerializeField]
    public TerrainRange Canyons
    { get; private set; }

    [field: SerializeField]
    public TerrainRange WildCard
    { get; private set; }

    public TerrainInformation ParseTerrainRangeIntoInformation(TerrainRange infoToPass)
    {
        TerrainInformation newInformation = new TerrainInformation
            (
                infoToPass.Information.GridXLength,
                infoToPass.Information.GridZLength,
                infoToPass.Information.GridSpacing,
                infoToPass.Information.PerlinScale += UnityEngine.Random.Range(-infoToPass.PerlinScaleRange, infoToPass.PerlinScaleRange),
                infoToPass.Information.OffsetX,
                infoToPass.Information.OffsetZ,
                infoToPass.Information.GridYHeightRange += UnityEngine.Random.Range(-infoToPass.HeightRange, infoToPass.HeightRange),
                infoToPass.Information.GridYHeightMultiplier,
                infoToPass.Information.TerrainGradient,
                infoToPass.Information.HeightColorChange += UnityEngine.Random.Range(-infoToPass.HeightColourAppliedRange, infoToPass.HeightColourAppliedRange),
                infoToPass.Information.EnableSmoothing,
                infoToPass.Information.Position,
                infoToPass.Information.TerrainMaterial,
                infoToPass.Information.Seed,
                infoToPass.Information.Octaves += UnityEngine.Random.Range((int)-infoToPass.OctaveRange, (int)infoToPass.OctaveRange + 1),
                infoToPass.Information.Persistance += UnityEngine.Random.Range(-infoToPass.PersistanceRange, infoToPass.PersistanceRange),
                infoToPass.Information.Lacunarity += UnityEngine.Random.Range(-infoToPass.LacunarityRange, infoToPass.LacunarityRange),
                infoToPass.Information.OctaveOffset,
                infoToPass.Information.TerrainCurve,
                infoToPass.Information.ColourLockToHeight,
                infoToPass.Information.HeightNormalisation
            );

        /*
        // Allowing for randomness within the pre-arranged ranges.
        newInformation.GridSpacing += UnityEngine.Random.Range(-infoToPass.GridSpacingRange, infoToPass.GridSpacingRange);
        newInformation.PerlinScale += UnityEngine.Random.Range(-infoToPass.PerlinScaleRange, infoToPass.PerlinScaleRange);
        newInformation.GridYHeightRange += UnityEngine.Random.Range(-infoToPass.HeightRange, infoToPass.HeightRange);
        newInformation.HeightColorChange += UnityEngine.Random.Range(-infoToPass.HeightColourAppliedRange, infoToPass.HeightColourAppliedRange);
        newInformation.Octaves += Mathf.Clamp(UnityEngine.Random.Range((int)-infoToPass.OctaveRange, (int)infoToPass.OctaveRange + 1), 1, 20);
        newInformation.Persistance += Mathf.Clamp(UnityEngine.Random.Range(-infoToPass.PersistanceRange, infoToPass.PersistanceRange), 0.01f, 20);
        newInformation.Lacunarity += Mathf.Clamp(UnityEngine.Random.Range(-infoToPass.LacunarityRange, infoToPass.LacunarityRange), 0.01f, 20);
        */

        return newInformation;
    }
}
