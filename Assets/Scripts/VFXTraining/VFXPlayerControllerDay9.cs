using UnityEngine;

public class VFXPlayerControllerDay9 : VFXPlayerConbtrolerBase
{
    [SerializeField] protected Animator _aniamtor;
    protected override void Update()
    {
        base.Update();
        _aniamtor.SetBool("IsGrounded" , _isGrounded);
        _aniamtor.SetFloat("Velocity" , _characterController.velocity.x);
    }
}