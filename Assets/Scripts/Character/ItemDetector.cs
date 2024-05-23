using Ressources;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Character
{
    public class ItemDetector : MonoBehaviour {
        private CharacterController _parentCharacter;
        private GameObject _itemDetected;
        private float _timerTake;
        private float _timerDrop;
        private bool _canTake;
        private bool _itemDropped;
        private bool _canDrop;
        private bool _itemTaken;
        private bool _correctMachine;
        

        private void Awake() {
            _parentCharacter = GetComponentInParent<CharacterController>();
            _canTake = true;
            _itemDropped = false;
            _canDrop = false;
            _itemTaken = false;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Item") && !_parentCharacter.Carrying) {
                _itemDetected = other.gameObject;
            }
        }
        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Item") && !_parentCharacter.Carrying) { _itemDetected = null; }
            if (other.CompareTag("Machine")) { _correctMachine = false; }
        }

        private void OnTriggerStay(Collider other) {
            if (other.CompareTag("Item") && !_parentCharacter.Carrying && _canTake) {
                if (other.GetComponent<BarrelInfos>()!=null) {
                    if (Input.GetButtonDown("Pick")) {
                        
                        PlayPickupSound();
                        
                        
                        Debug.Log("Picked item");
                        BarrelInfos barrel = other.GetComponent<BarrelInfos>();
                        barrel.StartCarring(_parentCharacter.itemPickPlacement.transform);
                        _parentCharacter.itemCarried = barrel;
                        
                        Debug.Log("Barrel of "+ _parentCharacter.itemCarried.GetComponent<BarrelInfos>().resourceType + " carried.");
                        
                        _canTake = false;
                        _itemTaken = true;
                        _canDrop = false;
                        _parentCharacter.animator.SetBool("Carry", true);
                    }
                }
            }

            if (other.CompareTag("Machine") && _parentCharacter.Carrying && _canDrop)
            {
                Debug.Log("Try to put into machine");
                MachineInfos machineInfos = other.GetComponent<MachineInfos>();
                if (machineInfos.CanDropRessources(_parentCharacter.itemCarried.Resource))
                {
                    _correctMachine = true;
                    if (Input.GetButtonDown("Pick") && _parentCharacter.Carrying && _canDrop && _correctMachine)
                    {
                        int randomClipChoose = Random.Range(0, _parentCharacter.carryDropSounds.Count);
                        _parentCharacter.audioSource.clip = _parentCharacter.carryDropSounds[randomClipChoose];
                        _parentCharacter.audioSource.Play();
                        
                        
                        Debug.Log("Putting the item in the fuckin machine");
                        other.GetComponent<MachineInfos>().capacityUsed += 1;
                        other.GetComponent<MachineInfos>().DropRessources(_parentCharacter.itemCarried.Resource);
                        Destroy(_parentCharacter.itemCarried.gameObject);
                        _parentCharacter.itemCarried = null;
                        _itemDropped = true;
                        _parentCharacter.animator.SetBool("Carry", false);
                    }
                }
                else
                {
                    Debug.Log("Machine can't tack this resource");
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
            if (Input.GetButtonDown("Pick") && _parentCharacter.Carrying && _canDrop && !_correctMachine) {
                int randomClipChoose = Random.Range(0, _parentCharacter.carryDropSounds.Count);
                _parentCharacter.audioSource.clip = _parentCharacter.carryDropSounds[randomClipChoose];
                _parentCharacter.audioSource.Play();
                Debug.Log("Drop item");
                
                
                _parentCharacter.itemCarried.EndCarring();
                //Transform itemTransform = _parentCharacter.itemCarried.transform;
                //Transform parentTransform = _parentCharacter.transform;
                //itemTransform.parent = transform.root;
                //itemTransform.rotation = quaternion.Euler(0,0,0);
                //_parentCharacter.itemCarried.GetComponent<Rigidbody>().isKinematic = false;
                //_parentCharacter.itemCarried.GetComponent<CapsuleCollider>().enabled = true;
                //_parentCharacter.itemCarried = null;
                _itemDropped = true;
                _parentCharacter.animator.SetBool("Carry", false);
            }
            if (_itemDropped) { _timerTake += Time.deltaTime; }
            if (_timerTake >= 1) {
                _canTake = true;
                _timerTake = 0;
            }
        }

        private void PlayPickupSound() {
            int randomClipChoose = Random.Range(0, _parentCharacter.carryDropSounds.Count);
            _parentCharacter.audioSource.clip = _parentCharacter.carryDropSounds[randomClipChoose];
            _parentCharacter.audioSource.Play();
        }
    }
}
