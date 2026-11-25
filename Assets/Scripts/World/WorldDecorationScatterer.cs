using System.Collections.Generic;
using UnityEngine;

namespace NepalBattleRoyale.World
{
    /// <summary>
    /// Places a bounded number of decorative prefabs on a Terrain.
    /// Keep this on a separate GameObject from NepalMapGenerator so map
    /// regeneration does not remove the decoration controller.
    /// </summary>
    public class WorldDecorationScatterer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Terrain targetTerrain;
        [SerializeField] private GameObject[] decorationPrefabs;
        [SerializeField] private Transform[] keepClearAround;

        [Header("Placement")]
        [SerializeField, Min(0)] private int instanceCount = 250;
        [SerializeField, Min(0f)] private float minimumSpacing = 8f;
        [SerializeField, Min(0f)] private float keepClearRadius = 18f;
        [SerializeField, Range(0f, 60f)] private float maximumSlope = 28f;
        [SerializeField, Min(0f)] private float edgeMargin = 20f;
        [SerializeField] private Vector2 uniformScaleRange = new Vector2(0.85f, 1.2f);
        [SerializeField] private int seed = 2026;

        [Header("Mobile limits")]
        [SerializeField, Min(1)] private int maximumAttemptsPerInstance = 30;

        private Transform generatedRoot;

        [ContextMenu("Scatter Decorations")]
        public void Scatter()
        {
            if (targetTerrain == null || targetTerrain.terrainData == null)
            {
                Debug.LogWarning("WorldDecorationScatterer needs a Terrain reference.", this);
                return;
            }

            if (decorationPrefabs == null || decorationPrefabs.Length == 0)
            {
                Debug.LogWarning("Assign at least one decoration prefab.", this);
                return;
            }

            ClearDecorations();
            generatedRoot = new GameObject("Generated World Decorations").transform;
            generatedRoot.SetParent(transform, false);

            TerrainData data = targetTerrain.terrainData;
            Vector3 terrainSize = data.size;
            float marginX = Mathf.Clamp(edgeMargin, 0f, terrainSize.x * 0.45f);
            float marginZ = Mathf.Clamp(edgeMargin, 0f, terrainSize.z * 0.45f);
            float minSpacingSqr = minimumSpacing * minimumSpacing;
            float keepClearSqr = keepClearRadius * keepClearRadius;
            Vector2 scaleRange = new Vector2(
                Mathf.Max(0.01f, Mathf.Min(uniformScaleRange.x, uniformScaleRange.y)),
                Mathf.Max(0.01f, Mathf.Max(uniformScaleRange.x, uniformScaleRange.y)));

            System.Random random = new System.Random(seed);
            List<Vector2> acceptedPositions = new List<Vector2>(instanceCount);
            int attempts = 0;
            int maxAttempts = Mathf.Max(1, instanceCount) *
                              Mathf.Max(1, maximumAttemptsPerInstance);

            while (acceptedPositions.Count < instanceCount && attempts < maxAttempts)
            {
                attempts++;

                float localX = Mathf.Lerp(marginX, terrainSize.x - marginX,
                    (float)random.NextDouble());
                float localZ = Mathf.Lerp(marginZ, terrainSize.z - marginZ,
                    (float)random.NextDouble());

                float normalizedX = localX / terrainSize.x;
                float normalizedZ = localZ / terrainSize.z;
                Vector3 worldPosition = targetTerrain.transform.position +
                    new Vector3(localX, 0f, localZ);

                Vector3 normal = data.GetInterpolatedNormal(normalizedX, normalizedZ);
                if (Vector3.Angle(normal, Vector3.up) > maximumSlope)
                    continue;

                Vector2 candidate = new Vector2(worldPosition.x, worldPosition.z);
                if (IsTooClose(candidate, acceptedPositions, minSpacingSqr))
                    continue;
                if (IsNearProtectedPoint(candidate, keepClearSqr))
                    continue;

                GameObject prefab = decorationPrefabs[random.Next(decorationPrefabs.Length)];
                if (prefab == null)
                    continue;

                worldPosition.y = targetTerrain.SampleHeight(worldPosition)
                                  + targetTerrain.transform.position.y;

                float yaw = (float)random.NextDouble() * 360f;
                float scale = Mathf.Lerp(scaleRange.x, scaleRange.y,
                    (float)random.NextDouble());

                GameObject instance = Instantiate(prefab, worldPosition,
                    Quaternion.Euler(0f, yaw, 0f), generatedRoot);
                instance.name = prefab.name + " (Scattered)";
                instance.transform.localScale *= scale;
                acceptedPositions.Add(candidate);
            }

            Debug.Log("Placed " + acceptedPositions.Count +
                      " decoration objects after " + attempts + " placement attempts.", this);
        }

        [ContextMenu("Clear Decorations")]
        public void ClearDecorations()
        {
            Transform oldRoot = transform.Find("Generated World Decorations");
            if (oldRoot == null) return;

            if (Application.isPlaying)
                Destroy(oldRoot.gameObject);
            else
                DestroyImmediate(oldRoot.gameObject);

            generatedRoot = null;
        }

        private bool IsTooClose(Vector2 candidate, List<Vector2> accepted, float spacingSqr)
        {
            for (int i = 0; i < accepted.Count; i++)
            {
                if ((candidate - accepted[i]).sqrMagnitude < spacingSqr)
                    return true;
            }
            return false;
        }

        private bool IsNearProtectedPoint(Vector2 candidate, float radiusSqr)
        {
            if (keepClearAround == null) return false;

            for (int i = 0; i < keepClearAround.Length; i++)
            {
                Transform point = keepClearAround[i];
                if (point == null) continue;

                Vector2 protectedPosition = new Vector2(point.position.x, point.position.z);
                if ((candidate - protectedPosition).sqrMagnitude < radiusSqr)
                    return true;
            }
            return false;
        }
    }
}