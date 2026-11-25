using System;
using UnityEngine;

namespace NepalBattleRoyale.World
{
    public enum NepalRegionType
    {
        Kathmandu,
        Pokhara,
        Mustang,
        Everest,
        Chitwan,
        Terai,
        Lumbini
    }

    [Serializable]
    public class NepalRegion
    {
        public NepalRegionType type;
        public string displayName;
        [Range(0f, 1f)] public float centerX;
        [Range(0f, 1f)] public float centerZ;
        [Range(0.02f, 0.5f)] public float radius;
        [Range(0f, 1f)] public float elevationBias;

        public NepalRegion(NepalRegionType type, string displayName, float centerX,
            float centerZ, float radius, float elevationBias)
        {
            this.type = type;
            this.displayName = displayName;
            this.centerX = centerX;
            this.centerZ = centerZ;
            this.radius = radius;
            this.elevationBias = elevationBias;
        }

        public float Influence(float x, float z)
        {
            float dx = x - centerX;
            float dz = z - centerZ;
            float distance = Mathf.Sqrt(dx * dx + dz * dz);
            return 1f - Mathf.SmoothStep(0f, radius, distance);
        }
    }
}
