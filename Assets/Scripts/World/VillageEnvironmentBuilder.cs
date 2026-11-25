using System.Collections.Generic;
using UnityEngine;

namespace NepalBattleRoyale.World
{
    /// <summary>
    /// Builds a simple terrain-following village road and places buildings
    /// at hand-authored anchors. Assign optimized, original prefabs.
    /// </summary>
    public class VillageEnvironmentBuilder : MonoBehaviour
    {
        [Header("Terrain and road")]
        [SerializeField] private Terrain targetTerrain;
        [SerializeField] private Transform[] roadWaypoints;
        [SerializeField, Min(1f)] private float roadWidth = 5f;
        [SerializeField, Min(0f)] private float roadSurfaceOffset = 0.08f;
        [SerializeField] private Material roadMaterial;
        [SerializeField] private bool addRoadCollider = true;

        [Header("Village buildings")]
        [SerializeField] private Transform[] buildingAnchors;
        [SerializeField] private GameObject[] buildingPrefabs;
        [SerializeField, Range(0f, 20f)] private float maximumBuildingSlope = 12f;
        [SerializeField] private int seed = 2026;

        private const string GeneratedRootName = "Generated Village Environment";

        [ContextMenu("Build Village Environment")]
        public void Build()
        {
            if (targetTerrain == null || targetTerrain.terrainData == null)
            {
                Debug.LogWarning("Assign the generated Terrain before building the village.", this);
                return;
            }

            Clear();
            Transform root = new GameObject(GeneratedRootName).transform;
            root.SetParent(transform, false);

            int roadSegments = BuildRoad(root);
            int buildings = PlaceBuildings(root);

            Debug.Log("Village layout created: " + roadSegments +
                      " road segments and " + buildings + " buildings.", this);
        }

        private int BuildRoad(Transform root)
        {
            if (roadWaypoints == null || roadWaypoints.Length < 2)
                return 0;

            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> triangles = new List<int>();
            float halfWidth = roadWidth * 0.5f;
            int segments = 0;

            for (int i = 0; i < roadWaypoints.Length - 1; i++)
            {
                Transform start = roadWaypoints[i];
                Transform end = roadWaypoints[i + 1];
                if (start == null || end == null) continue;

                Vector3 direction = end.position - start.position;
                direction.y = 0f;
                if (direction.sqrMagnitude < 0.01f) continue;
                direction.Normalize();
                Vector3 side = Vector3.Cross(Vector3.up, direction) * halfWidth;

                Vector3 leftStart = SampleRoadPoint(start.position - side);
                Vector3 rightStart = SampleRoadPoint(start.position + side);
                Vector3 leftEnd = SampleRoadPoint(end.position - side);
                Vector3 rightEnd = SampleRoadPoint(end.position + side);

                int first = vertices.Count;
                vertices.Add(transform.InverseTransformPoint(leftStart));
                vertices.Add(transform.InverseTransformPoint(rightStart));
                vertices.Add(transform.InverseTransformPoint(leftEnd));
                vertices.Add(transform.InverseTransformPoint(rightEnd));

                float length = Vector3.Distance(start.position, end.position) / 5f;
                uvs.Add(new Vector2(0f, 0f));
                uvs.Add(new Vector2(1f, 0f));
                uvs.Add(new Vector2(0f, length));
                uvs.Add(new Vector2(1f, length));

                triangles.Add(first);
                triangles.Add(first + 2);
                triangles.Add(first + 1);
                triangles.Add(first + 1);
                triangles.Add(first + 2);
                triangles.Add(first + 3);
                segments++;
            }

            if (segments == 0) return 0;

            Mesh mesh = new Mesh { name = "Village Road Mesh" };
            mesh.SetVertices(vertices);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            GameObject road = new GameObject("Village Road");
            road.transform.SetParent(root, false);
            MeshFilter filter = road.AddComponent<MeshFilter>();
            filter.sharedMesh = mesh;
            MeshRenderer renderer = road.AddComponent<MeshRenderer>();
            if (roadMaterial != null)
                renderer.sharedMaterial = roadMaterial;

            if (addRoadCollider)
            {
                MeshCollider collider = road.AddComponent<MeshCollider>();
                collider.sharedMesh = mesh;
            }
            return segments;
        }

        private Vector3 SampleRoadPoint(Vector3 worldPosition)
        {
            worldPosition.y = targetTerrain.SampleHeight(worldPosition)
                              + targetTerrain.transform.position.y
                              + roadSurfaceOffset;
            return worldPosition;
        }

        private int PlaceBuildings(Transform root)
        {
            if (buildingAnchors == null || buildingPrefabs == null ||
                buildingPrefabs.Length == 0)
                return 0;

            System.Random random = new System.Random(seed);
            int placed = 0;

            for (int i = 0; i < buildingAnchors.Length; i++)
            {
                Transform anchor = buildingAnchors[i];
                if (anchor == null) continue;

                Vector3 position = anchor.position;
                Vector3 local = targetTerrain.transform.InverseTransformPoint(position);
                Vector3 size = targetTerrain.terrainData.size;
                if (local.x < 0f || local.z < 0f ||
                    local.x > size.x || local.z > size.z)
                    continue;

                float nx = local.x / size.x;
                float nz = local.z / size.z;
                if (targetTerrain.terrainData.GetSteepness(nx, nz) > maximumBuildingSlope)
                    continue;

                GameObject prefab = buildingPrefabs[random.Next(buildingPrefabs.Length)];
                if (prefab == null) continue;

                position.y = targetTerrain.SampleHeight(position)
                             + targetTerrain.transform.position.y;
                GameObject building = Instantiate(prefab, position, anchor.rotation, root);
                building.name = prefab.name + " (Village)";
                placed++;
            }
            return placed;
        }

        [ContextMenu("Clear Village Environment")]
        public void Clear()
        {
            Transform oldRoot = transform.Find(GeneratedRootName);
            if (oldRoot == null) return;

            if (Application.isPlaying)
                Destroy(oldRoot.gameObject);
            else
                DestroyImmediate(oldRoot.gameObject);
        }
    }
}