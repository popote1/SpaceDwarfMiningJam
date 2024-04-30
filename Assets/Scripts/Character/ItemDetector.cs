using Ressources;
using Unity.Mathematics;
using UnityEngine;

namespace Character
{
    public class ItemDetector : MonoBehaviour
    {
        private CharacterController _parentCharacter;
        private GameObject _itemDetected;
        private float _timerTake;
        private float _timerDrop;
        private bool _canTake;
        private bool _itemDropped;
        private bool _canDrop;
        private bool _itemTaken;
        private bool _correctMachine;

        private void Awake()
        {
            _parentCharacter = GetComponentInParent<CharacterController>();
            _canTake = true;
            _itemDropped = false;
            _canDrop = false;
            _itemTaken = false;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Item") && !_parentCharacter.carrying) {
                _itemDetected = other.gameObject;
            }
        }
        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Item") && !_parentCharacter.carrying) { _itemDetected = null; }
            if (other.CompareTag("Machine")) { _correctMachine = false; }
        }

        private void OnTriggerStay(Collider other) {
            if (other.CompareTag("Item") && !_parentCharacter.carrying && _canTake) {
                if (other.gameObject == _itemDetected) {
                    if (Input.GetButtonDown("Pick")) {
                        Debug.Log("Picked item");
                        _parentCharacter.carrying = true;
                        _itemDetected.GetComponent<Rigidbody>().isKinematic = true;
                        _itemDetected.GetComponent<CapsuleCollider>().enabled = false;
                        _parentCharacter.itemCarried = _itemDetected;
                        Debug.Log("Barrel of "+ _parentCharacter.itemCarried.GetComponent<BarrelInfos>().resourceType + " carried.");
                        _canTake = false;
                        _itemTaken = true;
                        _canDrop = false;
                        _itemDetected.gameObject.transform.parent = _parentCharacter.itemPickPlacement.transform;
                        _itemDetected.gameObject.transform.position = _parentCharacter.itemPickPlacement.position;
                        _itemDetected.gameObject.transform.rotation = _parentCharacter.itemPickPlacement.rotation;
                        _parentCharacter.animator.SetBool("Carry", true);
                    }
                }
            }

            if (other.CompareTag("Machine") && _parentCharacter.carrying && _canDrop)
            {
                if (other.GetComponent<MachineInfos>().resourceNeeded.ToString() == _parentCharacter.itemCarried.GetComponent<BarrelInfos>().resourceType.ToString())
                {
                    _correctMachine = true;
                    if (Input.GetButtonDown("Pick") && _parentCharacter.carrying && _canDrop && _correctMachine)
                    {
                        Debug.Log("Putting the item in the fuckin machine");
                        other.GetComponent<MachineInfos>().capacityUsed += 1;
                        _parentCharacter.carrying = false;
                        Destroy(_parentCharacter.itemCarried.gameObject);
                        _parentCharacter.itemCarried = null;
                        _itemDropped = true;
                        _parentCharacter.animator.SetBool("Carry", false);
                    }
                }
            }
        }

        private void Update()
        {
            if (_itemTaken) { _timerDrop += Time.deltaTime; }
            if (_timerDrop >= 1) {
                _canDrop = true;
                _itemTaken = false;
                _timerDrop = 0;
            }
            if (Input.GetButtonDown("Pick") && _parentCharacter.carrying && _canDrop && !_correctMachine) {
                Debug.Log("Drop item");
                _parentCharacter.carrying = false;
                Transform itemTransform = _parentCharacter.itemCarried.transform;
                Transform parentTransform = _parentCharacter.transform;
                itemTransform.parent = transform.root;
                itemTransform.rotation = quaternion.Euler(0,0,0);
                _parentCharacter.itemCarried.GetComponent<Rigidbody>().isKinematic = false;
                _parentCharacter.itemCarried.GetComponent<CapsuleCollider>().enabled = true;
                _parentCharacter.itemCarried = null;
                _itemDropped = true;
                _parentCharacter.animator.SetBool("Carry", false);
            }
            if (_itemDropped) { _timerTake += Time.deltaTime; }
            if (_timerTake >= 1) {
                _canTake = true;
                _timerTake = 0;
            }
        }
        
    }
}
