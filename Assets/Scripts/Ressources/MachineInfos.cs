using UnityEngine;
using UnityEngine.Serialization;

namespace Ressources
{
    public class MachineInfos : MonoBehaviour
    {
        public enum ResourceNeeded
        {
            Petrol,
            Mass,
            Gaz
        }

        [SerializeField]
        public ResourceNeeded resourceNeeded;

        public ResourceNeeded Resource
        {
            get => resourceNeeded;
            set => resourceNeeded = value;
        }

        public int capacityUsed = 0;
    }
}

