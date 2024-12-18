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

    public void PassRandomRangeInformationToTerrain(TerrainRange infoToPass, TerrainInformation information)
    {
        information.GridSpacing = UnityEngine.Random.Range(-infoToPass.Information.GridSpacing, infoToPass.Information.GridSpacing);
        information.PerlinScale = UnityEngine.Random.Range(-infoToPass.Information.PerlinScale, infoToPass.Information.PerlinScale);
    }
}
