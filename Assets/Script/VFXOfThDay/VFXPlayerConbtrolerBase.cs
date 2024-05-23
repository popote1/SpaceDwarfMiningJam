using UnityEngine;

public class VFXPlayerConbtrolerBase : MonoBehaviour
{
    [Range(0,10),SerializeField] protected float _moveSpeed=5;
    [SerializeField] protected float _gravity=-9.8f;
    [SerializeField] protected Transform _groundChecker;
    [SerializeField] protected LayerMask _layerMaskGround;
    [SerializeField] protected float _jumpSpeed=2;
    //[SerializeField] protected Animator _aniamtor;

    
    //[Space(10), Header("Particules")]
    //[SerializeField] private float _walkParticuleThreashHold = 0.5f;
    //[SerializeField] private ParticleSystem _psWalk;
    //[SerializeField] private ParticleSystem _psJump;
    //[SerializeField] private ParticleSystem _psLanding;
    //[SerializeField] private ParticleSystem _psStartWark;
    

    protected CharacterController _characterController;
    protected float yVelocity;
    protected bool _isGrounded;
    protected bool _isWalking ;
    void Start() {
        _characterController = GetComponent<CharacterController>();
    }

    
    protected virtual void Update()
    {
        float xSpeed = Input.GetAxis("Horizontal");
        Vector3 moveVec = Time.deltaTime * _moveSpeed * -xSpeed * Vector3.right;
        //_characterController.Move(Time.deltaTime * _moveSpeed * -xSpeed * Vector3.right);

        
        if (Physics.CheckSphere(_groundChecker.position, 0.4f, _layerMaskGround))
        {
            if (yVelocity <= 0) {
                yVelocity = -2f;
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
        }

        Vector3 veticalVec = Vector3.up * yVelocity * Time.deltaTime;
        _characterController.Move(moveVec+veticalVec);
        
        
        //_aniamtor.SetBool("IsGrounded" , _isGrounded);
        //_aniamtor.SetFloat("Velocity" , _characterController.velocity.x);
    }
}