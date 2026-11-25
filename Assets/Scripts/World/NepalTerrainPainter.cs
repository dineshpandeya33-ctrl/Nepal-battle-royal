using UnityEngine;

namespace NepalBattleRoyale.World
{
    /// <summary>
    /// Builds a procedural five-biome terrain paint pass.
    /// The generated textures are a development fallback; replace them with
    /// scanned/PBR terrain materials for final realistic AAA presentation.
    /// </summary>
    public static class NepalTerrainPainter
    {
        private const int TextureSize = 128;
        private const int AlphaResolution = 256;
        private static readonly string[] LayerNames =
        {
            "Meadow", "Forest", "Rock", "Snow", "Dry Earth"
        };

        public static void Paint(TerrainData data)
        {
            if (data == null) return;

            TerrainLayer[] layers = new TerrainLayer[LayerNames.Length];
            for (int i = 0; i < layers.Length; i++)
            {
                Texture2D texture = BuildTexture(i);
                TerrainLayer layer = new TerrainLayer
                {
                    diffuseTexture = texture,
                    tileSize = new Vector2(i == 2 ? 18f : 12f, i == 2 ? 18f : 12f)
                };
                layers[i] = layer;
            }

            data.terrainLayers = layers;
            data.alphamapResolution = AlphaResolution;
            data.SetAlphamaps(0, 0, BuildWeights(data));
        }

        private static Texture2D BuildTexture(int layerIndex)
        {
            Texture2D texture = new Texture2D(TextureSize, TextureSize,
                TextureFormat.RGB24, true)
            {
                name = LayerNames[layerIndex] + " Procedural Albedo",
                wrapMode = TextureWrapMode.Repeat,
                filterMode = FilterMode.Trilinear
            };

            Color baseColor = GetBaseColor(layerIndex);
            Color[] pixels = new Color[TextureSize * TextureSize];
            for (int y = 0; y < TextureSize; y++)
            for (int x = 0; x < TextureSize; x++)
            {
                float n1 = Mathf.PerlinNoise(x * 0.075f + layerIndex * 17f,
                    y * 0.075f + layerIndex * 31f);
                float n2 = Mathf.PerlinNoise(x * 0.19f + 91f + layerIndex,
                    y * 0.19f + 47f);
                float variation = Mathf.Lerp(0.78f, 1.18f, n1 * 0.7f + n2 * 0.3f);
                float grain = ((x * 17 + y * 31 + layerIndex * 13) % 7) * 0.006f;
                pixels[y * TextureSize + x] = baseColor * (variation + grain);
            }
            texture.SetPixels(pixels);
            texture.Apply(true, false);
            return texture;
        }

        private static Color GetBaseColor(int index)
        {
            switch (index)
            {
                case 0: return new Color(0.30f, 0.37f, 0.20f);
                case 1: return new Color(0.15f, 0.25f, 0.13f);
                case 2: return new Color(0.34f, 0.33f, 0.31f);
                case 3: return new Color(0.78f, 0.81f, 0.84f);
                default: return new Color(0.43f, 0.34f, 0.23f);
            }
        }

        private static float[,,] BuildWeights(TerrainData data)
        {
            int resolution = data.alphamapResolution;
            float[,,] weights = new float[resolution, resolution, LayerNames.Length];

            for (int z = 0; z < resolution; z++)
            for (int x = 0; x < resolution; x++)
            {
                float nx = x / (float)(resolution - 1);
                float nz = z / (float)(resolution - 1);
                float height = data.GetInterpolatedHeight(nx, nz) / data.size.y;
                float slope = data.GetSteepness(nx, nz);
                float dryNoise = Mathf.PerlinNoise(nx * 13f + 4f, nz * 13f + 8f);

                float snow = Mathf.SmoothStep(0.66f, 0.82f, height);
                float rock = Mathf.SmoothStep(18f, 48f, slope) * (1f - snow * 0.8f);
                float dry = (1f - Mathf.SmoothStep(0.20f, 0.38f, height))
                    * Mathf.Lerp(0.25f, 0.75f, dryNoise);
                float forest = Mathf.Clamp01((1f - snow) * (1f - rock) * 0.62f);
                float meadow = Mathf.Clamp01((1f - snow) * (1f - rock) * (1f - dry));

                float total = meadow + forest + rock + snow + dry;
                if (total < 0.0001f) total = 1f;
                weights[z, x, 0] = meadow / total;
                weights[z, x, 1] = forest / total;
                weights[z, x, 2] = rock / total;
                weights[z, x, 3] = snow / total;
                weights[z, x, 4] = dry / total;
            }
            return weights;
        }
    }
}
