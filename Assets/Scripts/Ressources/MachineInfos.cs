using UnityEngine;
using UnityEngine.Serialization;

namespace Ressources
{
    public class MachineInfos : MonoBehaviour
    {
        

        [SerializeField]
        public Metrics.RESSOURCETYPE resourceNeeded;

        public Metrics.RESSOURCETYPE Resource
        {
            get => resourceNeeded;
            set => resourceNeeded = value;
        }

        public int capacityUsed = 0;

        public bool CanDropRessources(Metrics.RESSOURCETYPE type) {
            if (resourceNeeded == Metrics.RESSOURCETYPE.Everything) return true;
            return resourceNeeded == type;
        }
        
        public virtual void  DropRessources(Metrics.RESSOURCETYPE type) {
            
        }
    }
}

