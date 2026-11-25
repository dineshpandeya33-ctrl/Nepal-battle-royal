using UnityEngine;

namespace NepalBattleRoyale.World
{
    public class RegionMarker : MonoBehaviour
    {
        [SerializeField] private NepalRegionType regionType;
        [SerializeField] private string displayName;

        public NepalRegionType RegionType => regionType;
        public string DisplayName => displayName;

        public void SetRegion(NepalRegionType type, string label)
        {
            regionType = type;
            displayName = label;
            gameObject.name = "Region - " + label;
        }
    }
}
