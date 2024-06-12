using UnityEngine;
using UnityEngine.VFX;

public class VFXPlayerControllerDay20 : VFXPlayerConbtrolerBase
{
    [SerializeField] private VisualEffect _vfxRippleMask;
    [SerializeField] private VisualEffect _vfxRippleMaskBigSplash;
    [SerializeField] private VisualEffect _vfxMovingSplash;

    [SerializeField] private float _RippleAmoutOnMaxSpeed=20;
    [SerializeField] private float _RippleAmoutOnMinSpeed=0.5f;
    
    [SerializeField] private float _SplashAmoutOnMaxSpeed=300;
    [SerializeField] private float _SplashAmoutOnMinSpeed=0f;
    [SerializeField] private float _SplashAmmoutOnLanding=1000;

    private bool _wasGrounded;

    protected override void Update() {
        base.Update();

        
        float rippleRate = Mathf.Lerp(_RippleAmoutOnMinSpeed, _RippleAmoutOnMaxSpeed,
            _characterController.velocity.magnitude / _moveSpeed);
        float splashRate =Mathf.Lerp(_SplashAmoutOnMinSpeed, _SplashAmoutOnMaxSpeed,
            _characterController.velocity.magnitude / _moveSpeed);
        if (!_isGrounded)
        {
            rippleRate = 0;
            splashRate = 0;
        }
        
        _vfxRippleMask.SetFloat("SpawnRate", rippleRate);
        _vfxMovingSplash.SetFloat("rate", splashRate);

        if (_wasGrounded == false && _isGrounded) {
            _vfxRippleMaskBigSplash.Play();
            _vfxMovingSplash.SetFloat("rate", _SplashAmmoutOnLanding);
        }

        _wasGrounded = _isGrounded;
    }
}