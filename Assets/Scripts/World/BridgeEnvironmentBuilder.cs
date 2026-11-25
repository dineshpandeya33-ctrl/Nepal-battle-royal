using System.Collections.Generic;
using UnityEngine;

namespace NepalBattleRoyale.World
{
    /// <summary>
    /// Builds a simple static pedestrian bridge deck along ordered centerline waypoints.
    /// This is environment geometry only; it does not implement combat or vehicle systems.
    /// </summary>
    public class BridgeEnvironmentBuilder : MonoBehaviour
    {
        [Header("Bridge path")]
        [SerializeField] private Transform[] waypoints;
        [SerializeField, Min(1f)] private float deckWidth = 4f;
        [SerializeField] private float deckHeightOffset = 0.15f;
        [SerializeField] private Material deckMaterial;
        [SerializeField] private bool addDeckCollider = true;

        [Header("Optional side rails")]
        [SerializeField] private bool buildSideRails = true;
        [SerializeField, Min(0.2f)] private float railHeight = 1.1f;
        [SerializeField, Min(0.05f)] private float railThickness = 0.12f;
        [SerializeField] private Material railMaterial;

        private const string GeneratedRootName = "Generated Bridge";

        [ContextMenu("Build Bridge")]
        public void Build()
        {
            Clear();

            if (waypoints == null || waypoints.Length < 2)
            {
                Debug.LogWarning("Bridge needs at least two ordered waypoints.", this);
                return;
            }

            Transform root = new GameObject(GeneratedRootName).transform;
            root.SetParent(transform, false);

            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> triangles = new List<int>();
            float halfWidth = deckWidth * 0.5f;
            float distanceAlong = 0f;
            int validSegments = 0;

            for (int i = 0; i < waypoints.Length - 1; i++)
            {
                Transform a = waypoints[i];
                Transform b = waypoints[i + 1];
                if (a == null || b == null) continue;

                Vector3 direction = b.position - a.position;
                direction.y = 0f;
                float length = direction.magnitude;
                if (length < 0.1f) continue;

                direction /= length;
                Vector3 side = Vector3.Cross(Vector3.up, direction) * halfWidth;
                Vector3 leftA = a.position - side + Vector3.up * deckHeightOffset;
                Vector3 rightA = a.position + side + Vector3.up * deckHeightOffset;
                Vector3 leftB = b.position - side + Vector3.up * deckHeightOffset;
                Vector3 rightB = b.position + side + Vector3.up * deckHeightOffset;

                int first = vertices.Count;
                vertices.Add(transform.InverseTransformPoint(leftA));
                vertices.Add(transform.InverseTransformPoint(rightA));
                vertices.Add(transform.InverseTransformPoint(leftB));
                vertices.Add(transform.InverseTransformPoint(rightB));

                float nextDistance = distanceAlong + length;
                uvs.Add(new Vector2(0f, distanceAlong / 2f));
                uvs.Add(new Vector2(1f, distanceAlong / 2f));
                uvs.Add(new Vector2(0f, nextDistance / 2f));
                uvs.Add(new Vector2(1f, nextDistance / 2f));

                // Winding order produces upward-facing deck normals.
                triangles.Add(first);
                triangles.Add(first + 2);
                triangles.Add(first + 1);
                triangles.Add(first + 1);
                triangles.Add(first + 2);
                triangles.Add(first + 3);

                if (buildSideRails)
                {
                    BuildRailSegment(leftA, leftB, "Left Rail " + i, length);
                    BuildRailSegment(rightA, rightB, "Right Rail " + i, length);
                }

                distanceAlong = nextDistance;
                validSegments++;
            }

            if (validSegments == 0)
            {
                Debug.LogWarning("No valid bridge segments were found.", this);
                Clear();
                return;
            }

            Mesh mesh = new Mesh { name = "Bridge Deck Mesh" };
            mesh.SetVertices(vertices);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            GameObject deck = new GameObject("Bridge Deck");
            deck.transform.SetParent(root, false);
            MeshFilter filter = deck.AddComponent<MeshFilter>();
            filter.sharedMesh = mesh;
            MeshRenderer renderer = deck.AddComponent<MeshRenderer>();
            if (deckMaterial != null) renderer.sharedMaterial = deckMaterial;

            if (addDeckCollider)
            {
                MeshCollider collider = deck.AddComponent<MeshCollider>();
                collider.sharedMesh = mesh;
            }

            Debug.Log("Bridge created with " + validSegments + " deck sections.", this);
        }

        private void BuildRailSegment(Vector3 start, Vector3 end, string objectName, float length)
        {
            Transform root = transform.Find(GeneratedRootName);
            if (root == null)
            {
                root = new GameObject(GeneratedRootName).transform;
                root.SetParent(transform, false);
            }

            Vector3 center = (start + end) * 0.5f + Vector3.up * (railHeight * 0.5f);
            GameObject rail = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rail.name = objectName;
            rail.transform.SetParent(root, true);
            rail.transform.position = center;
            rail.transform.rotation = Quaternion.LookRotation((end - start).normalized, Vector3.up);
            rail.transform.localScale = new Vector3(railThickness, railHeight, length + railThickness);
            MeshRenderer renderer = rail.GetComponent<MeshRenderer>();
            if (railMaterial != null) renderer.sharedMaterial = railMaterial;
        }

        [ContextMenu("Clear Bridge")]
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
