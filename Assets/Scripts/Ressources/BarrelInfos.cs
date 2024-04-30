using UnityEngine;
using UnityEngine.Serialization;

namespace Ressources
{
    public class BarrelInfos : MonoBehaviour
    {
        public enum ResourceType
        {
            Petrol,
            Mass,
            Gaz
        }

        [FormerlySerializedAs("_resourceType")] [SerializeField]
        public ResourceType resourceType;

        public ResourceType Resource
        {
            get => resourceType;
            set => resourceType = value;
        }
    }
}
