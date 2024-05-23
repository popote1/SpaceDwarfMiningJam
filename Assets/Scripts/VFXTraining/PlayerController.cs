using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    

    [Range(0,10),SerializeField] private float _moveSpeed=5;
    [SerializeField] private float _gravity=-9.8f;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private LayerMask _layerMaskGround;
    [SerializeField] private float _jumpSpeed=2;
    [SerializeField] private Animator _aniamtor;

    
    [Space(10), Header("Particules")]
    [SerializeField] private float _walkParticuleThreashHold = 0.5f;
    [SerializeField] private ParticleSystem _psWalk;
    [SerializeField] private ParticleSystem _psJump;
    [SerializeField] private ParticleSystem _psLanding;
    [SerializeField] private ParticleSystem _psStartWark;
    

    private CharacterController _characterController;
    private float yVelocity;
    private bool _isGrounded;
    private bool _isWalking ;
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        float xSpeed = Input.GetAxis("Horizontal");
        Vector3 moveVec = Time.deltaTime * _moveSpeed * -xSpeed * Vector3.right;
        //_characterController.Move(Time.deltaTime * _moveSpeed * -xSpeed * Vector3.right);

        
            if (Physics.CheckSphere(_groundChecker.position, 0.4f, _layerMaskGround))
            {
                if (yVelocity <= 0) {
                    yVelocity = -2f;
                    if( _isGrounded==false)SpawnLandingVFX();
                    _isGrounded = true;
                }
            }
            else
            {
                yVelocity += _gravity * Time.deltaTime;
                _isGrounded = false;
            }
        

        if (Input.GetKey(KeyCode.UpArrow)&&_isGrounded)
        {
            yVelocity = Mathf.Sqrt(_jumpSpeed * -2 * _gravity);
            _isGrounded = false;
            SpawnJumpVFX();
        }

        Vector3 veticalVec = Vector3.up * yVelocity * Time.deltaTime;
        _characterController.Move(moveVec+veticalVec);
        
        
        ManageParticule();
        _aniamtor.SetBool("IsGrounded" , _isGrounded);
        _aniamtor.SetFloat("Velocity" , _characterController.velocity.x);
    }

    private void ManageParticule()
    {
        ParticleSystem.EmissionModule emissionModule = _psWalk.emission;
        if (_isGrounded && Mathf.Abs(_characterController.velocity.x) > _walkParticuleThreashHold)
        {
            Debug.Log("Player Walk"+Mathf.Abs(_characterController.velocity.x));
            //if( _psWalk.isStopped) 
            emissionModule.enabled=true;
            if( !_isWalking)SpawnStartWalkVFX(_characterController.velocity.x);
            _isWalking = true;
        }
        else
        {
            Debug.Log("Stop Walk"+Mathf.Abs(_characterController.velocity.x));
            emissionModule.enabled=false;
            _isWalking = false;
        }
        

        
    }

    private void SpawnJumpVFX()
    {
        Instantiate(_psJump, transform.position, quaternion.identity);
    }
    private void SpawnLandingVFX()
    {
        Instantiate(_psLanding, transform.position, quaternion.identity);
    }

    private void SpawnStartWalkVFX(float dir)
    {
        ParticleSystem ps =Instantiate(_psStartWark, transform.position, quaternion.identity);
        ps.transform.right = new Vector3(-dir , 0,0);
    }
}