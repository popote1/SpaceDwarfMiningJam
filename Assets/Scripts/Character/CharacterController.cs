using System.Collections.Generic;
using UnityEngine;

namespace Character {
    public class CharacterController : MonoBehaviour, IDamageble
    {
        public Rigidbody rigidbody;
        public float speed;
        public Animator animator;
        public bool running;
        public bool shooting;
        public bool carrying;
        public GameObject itemCarried;
        public float shootCooldown;
        private float _shootCooldownTimer;
        public float rotationSpeed = 10f;
        public float shootDistance = 50f;
        public LayerMask shootLayerMask;
        public GameObject hitEffectPrefab;
        public List<ParticleSystem> shootParticleSystems = new List<ParticleSystem>();
        public GameObject particleSystems;
        public Transform particleTransformPoint;
        public Transform itemPickPlacement;
        

        private void FixedUpdate() {
            Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Vector3 destination = new Vector3(move.x, 0, move.y);
            if (Input.GetButton("Run")) { running = true; }
            else { running = false; }
            if (running) {  if (!shooting) rigidbody.MovePosition(rigidbody.position + destination * (speed * 1.5f) * Time.fixedDeltaTime); }
            else { if (!shooting) rigidbody.MovePosition(rigidbody.position + destination * speed * Time.fixedDeltaTime); }
            if (move != Vector2.zero & !shooting) {
                animator.SetBool("Walk", !running);
                animator.SetBool("Run", running);
                Vector3 targetDirection = new Vector3(destination.x, 0, destination.z);
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            } else {
                animator.SetBool("Walk", false);
                animator.SetBool("Run", false);
                running = false;
            }
            if (Input.GetButtonDown("Fire") && !shooting && !carrying) {
                animator.SetTrigger("Shoot");
                foreach (ParticleSystem ps in shootParticleSystems) { ps.Play(); }
                particleSystems.transform.position = particleTransformPoint.position;
                particleSystems.transform.rotation = particleTransformPoint.rotation;
                shooting = true;
                _shootCooldownTimer = 0;
            }
            if (shooting && !carrying) {
                if (_shootCooldownTimer >= shootCooldown) { _shootCooldownTimer = 0; shooting = false; }
                _shootCooldownTimer += Time.deltaTime;
                Vector3 shootDirection = transform.forward;
                Vector3 playerMidHeight = new Vector3(transform.position.x, 1, transform.position.z);
                RaycastHit hit;
                if (Physics.Raycast(playerMidHeight, shootDirection, out hit, shootDistance, shootLayerMask)) {
                    Debug.DrawRay(playerMidHeight, shootDirection * hit.distance, Color.red, 1f);
                    Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                } else { Debug.DrawRay(playerMidHeight, shootDirection * shootDistance, Color.green, 1f); }
            }

            if (carrying)
            {
                if (Input.GetButtonDown("Pick"))
                {
                    
                }
            }
        }

        public void TakeDamage(int damage) {
            
        }
    }
}