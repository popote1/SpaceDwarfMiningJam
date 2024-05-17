using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ressources
{
    public class BarrelInfos : MonoBehaviour , IDamageble
    {
        [SerializeField] private MeshRenderer mesh;

        [SerializeField] private Color _gazColor = Color.green;
        [SerializeField] private Color _petrolColor = Color.black;
        [SerializeField] private Color _MassColor = Color.blue;
        [SerializeField] private GameObject _exploisionPrefab;
        
        [FormerlySerializedAs("_resourceType")] [SerializeField]
        public Metrics.RESSOURCETYPE resourceType;

        public Metrics.RESSOURCETYPE Resource
        {
            get => resourceType;
            set => resourceType = value;
        }

        private void Awake() {
            mesh = GetComponent<MeshRenderer>();
        }

        public void SetRessourseType(Metrics.RESSOURCETYPE ressourcetype) {
            resourceType = ressourcetype;
            switch (resourceType) {
                case Metrics.RESSOURCETYPE.None:mesh.material.color = Color.magenta; break;
                case Metrics.RESSOURCETYPE.Gaz: mesh.material.color = _gazColor;break;
                case Metrics.RESSOURCETYPE.Petrole: mesh.material.color = _petrolColor; break;
                case Metrics.RESSOURCETYPE.Mass: mesh.material.color = _MassColor; break;
                default: throw new ArgumentOutOfRangeException(nameof(ressourcetype), ressourcetype, null);
            }
        }


        public void TakeDamage(int damage) {
            Instantiate(_exploisionPrefab, transform.position, Quaternion.identity);
            switch (resourceType) {
                case Metrics.RESSOURCETYPE.None:
                    break;
                case Metrics.RESSOURCETYPE.Gaz:
                    break;
                case Metrics.RESSOURCETYPE.Petrole:
                    GamesManager.Instance.DoBurningGround(transform.position);
                    break;
                case Metrics.RESSOURCETYPE.Mass:
                    GamesManager.Instance.DoMassExplosion(transform.position);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Destroy(gameObject);
        }
        
    }
}
