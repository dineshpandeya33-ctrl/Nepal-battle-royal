using System.Collections.Generic;
using UnityEngine;

namespace NepalBattleRoyale.World
{
    /// <summary>Generates a fictional Nepal-inspired greybox terrain.</summary>
    [ExecuteAlways]
    public class NepalMapGenerator : MonoBehaviour
    {
        [Header("Terrain")]
        [SerializeField, Min(256)] private int heightmapResolution = 513;
        [SerializeField, Min(500f)] private float terrainSize = 4000f;
        [SerializeField, Min(100f)] private float terrainHeight = 900f;
        [SerializeField] private int seed = 2026;
        [SerializeField] private bool generateOnStart;

        [Header("Noise")]
        [SerializeField, Min(0.0001f)] private float baseNoiseScale = 2.4f;
        [SerializeField, Range(0f, 1f)] private float mountainStrength = 0.48f;
        [SerializeField, Range(0f, 1f)] private float detailStrength = 0.08f;

        [Header("Generated content")]
        [SerializeField] private bool createRegionMarkers = true;
        [SerializeField] private float markerHeight = 30f;
        [SerializeField] private bool paintProceduralBiomes = true;

        private readonly List<NepalRegion> regions = new List<NepalRegion>
        {
            new NepalRegion(NepalRegionType.Kathmandu, "Kathmandu Basin", 0.47f, 0.53f, 0.13f, 0.28f),
            new NepalRegion(NepalRegionType.Pokhara, "Pokhara Lake District", 0.30f, 0.48f, 0.12f, 0.34f),
            new NepalRegion(NepalRegionType.Mustang, "Mustang Highlands", 0.20f, 0.76f, 0.16f, 0.62f),
            new NepalRegion(NepalRegionType.Everest, "Everest Range", 0.77f, 0.77f, 0.19f, 0.95f),
            new NepalRegion(NepalRegionType.Chitwan, "Chitwan Forest", 0.55f, 0.28f, 0.14f, 0.12f),
            new NepalRegion(NepalRegionType.Terai, "Terai Plains", 0.28f, 0.16f, 0.18f, 0.04f),
            new NepalRegion(NepalRegionType.Lumbini, "Lumbini Heritage District", 0.12f, 0.30f, 0.10f, 0.06f)
        };

        private void Start()
        {
            if (Application.isPlaying && generateOnStart && GetComponent<Terrain>() == null)
                Generate();
        }

        [ContextMenu("Generate Nepal Map")]
        public void Generate()
        {
            ClearGeneratedChildren();

            int resolution = Mathf.ClosestPowerOfTwo(heightmapResolution - 1) + 1;
            resolution = Mathf.Clamp(resolution, 257, 4097);

            Terrain terrain = GetComponent<Terrain>();
            if (terrain == null) terrain = gameObject.AddComponent<Terrain>();

            TerrainData data = terrain.terrainData;
            if (data == null)
                data = new TerrainData();

            data.heightmapResolution = resolution;
            data.size = new Vector3(terrainSize, terrainHeight, terrainSize);
            data.SetHeights(0, 0, BuildHeightmap(resolution));

            TerrainCollider terrainCollider = GetComponent<TerrainCollider>();
            if (terrainCollider == null) terrainCollider = gameObject.AddComponent<TerrainCollider>();

            terrain.terrainData = data;
            terrainCollider.terrainData = data;
            terrain.drawInstanced = true;
            terrain.heightmapPixelError = 8f;
            terrain.basemapDistance = 1000f;

            if (paintProceduralBiomes)
                NepalTerrainPainter.Paint(data);

            if (createRegionMarkers)
                CreateMarkers(data);

            Debug.Log("Generated Nepal-inspired terrain and procedural biome layers. Replace fallback textures with production PBR assets for final quality.", this);
        }

        private float[,] BuildHeightmap(int resolution)
        {
            float[,] heights = new float[resolution, resolution];
            System.Random random = new System.Random(seed);
            float offsetX = (float)random.NextDouble() * 10000f;
            float offsetZ = (float)random.NextDouble() * 10000f;

            for (int z = 0; z < resolution; z++)
            for (int x = 0; x < resolution; x++)
            {
                float nx = x / (float)(resolution - 1);
                float nz = z / (float)(resolution - 1);
                float broad = Mathf.PerlinNoise(offsetX + nx * baseNoiseScale,
                    offsetZ + nz * baseNoiseScale);
                float detail = Mathf.PerlinNoise(offsetX + nx * baseNoiseScale * 5f,
                    offsetZ + nz * baseNoiseScale * 5f);

                float regionalElevation = 0f;
                float totalInfluence = 0f;
                foreach (NepalRegion region in regions)
                {
                    float influence = region.Influence(nx, nz);
                    regionalElevation += region.elevationBias * influence;
                    totalInfluence += influence;
                }
                if (totalInfluence > 1f) regionalElevation /= totalInfluence;

                float ridge = 1f - Mathf.Abs((broad * 2f) - 1f);
                ridge = Mathf.Pow(ridge, 1.8f);
                float value = 0.08f + broad * 0.18f
                    + ridge * mountainStrength * (0.25f + regionalElevation)
                    + detail * detailStrength;
                float lowland = Mathf.SmoothStep(0f, 0.32f, nz);
                value *= Mathf.Lerp(0.55f, 1f, lowland);
                heights[z, x] = Mathf.Clamp01(value);
            }
            return heights;
        }

        private void CreateMarkers(TerrainData data)
        {
            foreach (NepalRegion region in regions)
            {
                GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                marker.name = "Region - " + region.displayName;
                marker.transform.SetParent(transform, false);
                float y = data.GetInterpolatedHeight(region.centerX, region.centerZ);
                marker.transform.localPosition = new Vector3(
                    region.centerX * terrainSize, y + markerHeight * 0.5f,
                    region.centerZ * terrainSize);
                marker.transform.localScale = new Vector3(18f, markerHeight * 0.5f, 18f);
                marker.AddComponent<RegionMarker>().SetRegion(region.type, region.displayName);
            }
        }

        private void ClearGeneratedChildren()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                GameObject child = transform.GetChild(i).gameObject;
                if (Application.isPlaying) Destroy(child);
                else DestroyImmediate(child);
            }
        }
    }
}
