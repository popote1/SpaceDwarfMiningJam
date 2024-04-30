using System;
using Unity.Mathematics;
using UnityEngine;

namespace Character
{
    public class ItemDetector : MonoBehaviour
    {
        private CharacterController _parentCharacter;
        private GameObject _itemDetected;
        private bool _inItemZone;
        private float _timerTake;
        private float _timerDrop;
        private bool _canTake;
        private bool _itemDropped;
        private bool _canDrop;
        private bool _itemTaken;

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
                _inItemZone = true;
            }
        }
        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Item") && !_parentCharacter.carrying) {
                _itemDetected = null;
                _inItemZone = false;
            }
        }

        private void OnTriggerStay(Collider other) {
            if (other.CompareTag("Item") && !_parentCharacter.carrying && _canTake) {
                if (other.gameObject == _itemDetected) {
                    if (Input.GetButtonDown("Pick")) {
                        Debug.Log("Picked item");
                        _parentCharacter.animator.SetBool("Carry", true);
                        _parentCharacter.carrying = true;
                        _parentCharacter.itemCarried = _itemDetected;
                        _itemDetected.GetComponent<Rigidbody>().isKinematic = true;
                        _itemDetected.GetComponent<CapsuleCollider>().enabled = false;
                        _canTake = false;
                        _itemTaken = true;
                        _canDrop = false;
                        _itemDetected.gameObject.transform.parent = _parentCharacter.itemPickPlacement.transform;
                        _itemDetected.gameObject.transform.position = _parentCharacter.itemPickPlacement.position;
                        _itemDetected.gameObject.transform.rotation = _parentCharacter.itemPickPlacement.rotation;
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
            if (Input.GetButtonDown("Pick") && _parentCharacter.carrying && _canDrop) {
                Debug.Log("Drop item");
                _parentCharacter.animator.SetBool("Carry", false);
                _parentCharacter.carrying = false;
                Transform itemTransform = _parentCharacter.itemCarried.transform;
                Transform parentTransform = _parentCharacter.transform;
                itemTransform.parent = transform.root;
                itemTransform.rotation = quaternion.Euler(0,0,0);
                _parentCharacter.itemCarried.GetComponent<Rigidbody>().isKinematic = false;
                _parentCharacter.itemCarried.GetComponent<CapsuleCollider>().enabled = true;
                _parentCharacter.itemCarried = null;
                _itemDropped = true;
            }
            if (_itemDropped) { _timerTake += Time.deltaTime; }
            if (_timerTake >= 1) {
                _canTake = true;
                _timerTake = 0;
            }
        }
        
    }
}
