using System.Collections.Generic;
using UnityEngine;

namespace NepalBattleRoyale.World
{
    /// <summary>
    /// Creates simple static water meshes for a river path and oval lakes.
    /// Waypoint and lake-anchor heights define the water level.
    /// </summary>
    public class WaterFeatureBuilder : MonoBehaviour
    {
        [Header("Shared water appearance")]
        [SerializeField] private Material waterMaterial;
        [SerializeField, Min(0f)] private float surfaceOffset = 0.05f;
        [SerializeField] private bool addWaterColliders;

        [Header("River")]
        [SerializeField] private Transform[] riverWaypoints;
        [SerializeField, Min(1f)] private float riverWidth = 10f;

        [Header("Lakes")]
        [SerializeField] private Transform[] lakeAnchors;
        [SerializeField, Min(3)] private int lakeEdgeSegments = 24;
        [SerializeField] private Vector2 defaultLakeRadius = new Vector2(35f, 22f);

        private const string GeneratedRootName = "Generated Water Features";

        [ContextMenu("Build Water Features")]
        public void Build()
        {
            Clear();
            Transform root = new GameObject(GeneratedRootName).transform;
            root.SetParent(transform, false);

            int riverCount = BuildRiver(root);
            int lakeCount = BuildLakes(root);
            Debug.Log("Water features created: " + riverCount +
                      " river sections and " + lakeCount + " lakes.", this);
        }

        private int BuildRiver(Transform root)
        {
            if (riverWaypoints == null || riverWaypoints.Length < 2)
                return 0;

            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> triangles = new List<int>();
            float halfWidth = riverWidth * 0.5f;
            int sections = 0;
            float distanceAlong = 0f;

            for (int i = 0; i < riverWaypoints.Length - 1; i++)
            {
                Transform a = riverWaypoints[i];
                Transform b = riverWaypoints[i + 1];
                if (a == null || b == null) continue;

                Vector3 direction = b.position - a.position;
                direction.y = 0f;
                float segmentLength = direction.magnitude;
                if (segmentLength < 0.1f) continue;
                direction /= segmentLength;
                Vector3 side = Vector3.Cross(Vector3.up, direction) * halfWidth;

                Vector3 leftA = a.position - side + Vector3.up * surfaceOffset;
                Vector3 rightA = a.position + side + Vector3.up * surfaceOffset;
                Vector3 leftB = b.position - side + Vector3.up * surfaceOffset;
                Vector3 rightB = b.position + side + Vector3.up * surfaceOffset;

                int first = vertices.Count;
                vertices.Add(transform.InverseTransformPoint(leftA));
                vertices.Add(transform.InverseTransformPoint(rightA));
                vertices.Add(transform.InverseTransformPoint(leftB));
                vertices.Add(transform.InverseTransformPoint(rightB));

                float nextDistance = distanceAlong + segmentLength;
                uvs.Add(new Vector2(0f, distanceAlong / 5f));
                uvs.Add(new Vector2(1f, distanceAlong / 5f));
                uvs.Add(new Vector2(0f, nextDistance / 5f));
                uvs.Add(new Vector2(1f, nextDistance / 5f));

                triangles.Add(first);
                triangles.Add(first + 2);
                triangles.Add(first + 1);
                triangles.Add(first + 1);
                triangles.Add(first + 2);
                triangles.Add(first + 3);

                distanceAlong = nextDistance;
                sections++;
            }

            if (sections == 0) return 0;
            CreateWaterObject(root, "River Surface", vertices, uvs, triangles);
            return sections;
        }

        private int BuildLakes(Transform root)
        {
            if (lakeAnchors == null) return 0;

            int created = 0;
            int segments = Mathf.Clamp(lakeEdgeSegments, 3, 128);
            for (int i = 0; i < lakeAnchors.Length; i++)
            {
                Transform anchor = lakeAnchors[i];
                if (anchor == null) continue;

                float radiusX = anchor.localScale.x > 0.01f
                    ? anchor.localScale.x : defaultLakeRadius.x;
                float radiusZ = anchor.localScale.z > 0.01f
                    ? anchor.localScale.z : defaultLakeRadius.y;

                List<Vector3> vertices = new List<Vector3>(segments + 1);
                List<Vector2> uvs = new List<Vector2>(segments + 1);
                List<int> triangles = new List<int>(segments * 3);

                Vector3 center = anchor.position + Vector3.up * surfaceOffset;
                vertices.Add(transform.InverseTransformPoint(center));
                uvs.Add(new Vector2(0.5f, 0.5f));

                for (int j = 0; j < segments; j++)
                {
                    float angle = (j / (float)segments) * Mathf.PI * 2f;
                    float x = Mathf.Cos(angle);
                    float z = Mathf.Sin(angle);
                    Vector3 point = center + new Vector3(x * radiusX, 0f, z * radiusZ);
                    vertices.Add(transform.InverseTransformPoint(point));
                    uvs.Add(new Vector2(0.5f + x * 0.5f, 0.5f + z * 0.5f));
                }

                for (int j = 0; j < segments; j++)
                {
                    int current = j + 1;
                    int next = ((j + 1) % segments) + 1;
                    triangles.Add(0);
                    triangles.Add(current);
                    triangles.Add(next);
                }

                CreateWaterObject(root, "Lake Surface " + (created + 1),
                    vertices, uvs, triangles);
                created++;
            }
            return created;
        }

        private void CreateWaterObject(Transform root, string objectName,
            List<Vector3> vertices, List<Vector2> uvs, List<int> triangles)
        {
            Mesh mesh = new Mesh { name = objectName + " Mesh" };
            mesh.SetVertices(vertices);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            GameObject water = new GameObject(objectName);
            water.transform.SetParent(root, false);
            MeshFilter filter = water.AddComponent<MeshFilter>();
            filter.sharedMesh = mesh;
            MeshRenderer renderer = water.AddComponent<MeshRenderer>();
            if (waterMaterial != null)
                renderer.sharedMaterial = waterMaterial;

            if (addWaterColliders)
            {
                MeshCollider collider = water.AddComponent<MeshCollider>();
                collider.sharedMesh = mesh;
            }
        }

        [ContextMenu("Clear Water Features")]
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