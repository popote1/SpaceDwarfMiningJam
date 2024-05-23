using System.Collections.Generic;
using Ressources;
using UnityEditor;
using UnityEngine;

namespace Character {
    public class CharacterController : MonoBehaviour, IDamageble
    {
        [Header("Move and animations")]
        public Rigidbody rigidbody;
        public Animator animator;
        public float speed;
        public bool running;
        
        [Header("Carrying")]
        public BarrelInfos itemCarried;
        public Transform itemPickPlacement;
        
        [Header("Shoot")]
        public bool shooting;
        public float shootCooldown;
        private float _shootCooldownTimer;
        public float rotationSpeed = 10f;
        public float shootDistance = 50f;
        public LayerMask shootLayerMask;
        
        [Header("Particles")]
        public GameObject particleSystems;
        public Transform particleTransformPoint;
        public List<ParticleSystem> shootParticleSystems = new List<ParticleSystem>();
        
        [Header("Sounds Shits")]
        public AudioSource audioSource;
        public List<AudioClip> buildingSounds = new List<AudioClip>();
        public List<AudioClip> carryDropSounds = new List<AudioClip>();
        public List<AudioClip> damageSounds = new List<AudioClip>();
        public List<AudioClip> movingSounds = new List<AudioClip>();
        public AudioSource gunAudioSource;
        
        [Header("Misc")]
        public GameObject hitEffectPrefab;

        public bool Carrying {
            get => itemCarried != null;
        }

        
        private void FixedUpdate() {
            Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Vector3 destination = new Vector3(move.x, 0, move.y);
            if (Input.GetButton("Run")) { running = true; }
            else { running = false; }
            if (running) {  if (!shooting) rigidbody.MovePosition(rigidbody.position + destination * (speed * 1.5f) * Time.fixedDeltaTime); }
            else { if (!shooting) rigidbody.MovePosition(rigidbody.position + destination * speed * Time.fixedDeltaTime); }
            if (move != Vector2.zero & !shooting) {
                int randomClipSound = Random.Range(0, 450);
                if (randomClipSound < 5)
                {
                    int randomClipChoose = Random.Range(0, movingSounds.Count);
                    audioSource.clip = movingSounds[randomClipChoose];
                    audioSource.Play();
                }
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
            if (Input.GetButtonDown("Fire") && !shooting && !Carrying) {
                animator.SetTrigger("Shoot");
                gunAudioSource.Play();
                particleSystems.transform.position = particleTransformPoint.position;
                particleSystems.transform.rotation = particleTransformPoint.rotation;
                shooting = true;
                _shootCooldownTimer = 0;
            }
            if (shooting && !Carrying) {
                if (_shootCooldownTimer >= shootCooldown) { _shootCooldownTimer = 0; shooting = false; }
                if (_shootCooldownTimer is >= 0.55f and <= 0.6f) { foreach (ParticleSystem ps in shootParticleSystems) { ps.Play(); }}
                _shootCooldownTimer += Time.deltaTime;
                Vector3 shootDirection = transform.forward;
                Vector3 playerMidHeight = new Vector3(transform.position.x, 1, transform.position.z);
                RaycastHit hit;
                if (Physics.Raycast(playerMidHeight, shootDirection, out hit, shootDistance, shootLayerMask)) {
                    Debug.DrawRay(playerMidHeight, shootDirection * hit.distance, Color.red, 1f);
                    Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                } else { Debug.DrawRay(playerMidHeight, shootDirection * shootDistance, Color.green, 1f); }
            }

            
        }

        public void TakeDamage(int damage)
        {
            int randomClipChoose = Random.Range(0, damageSounds.Count);
            audioSource.clip = damageSounds[randomClipChoose];
            audioSource.Play();
        }
    }
}