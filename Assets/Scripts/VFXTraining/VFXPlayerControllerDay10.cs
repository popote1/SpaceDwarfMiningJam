using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.VFX;

public class VFXPlayerControllerDay10 : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private CinemachineImpulseSource _impulseSource;
    [SerializeField] private CinemachineImpulseSource _impulseSource2;
    [SerializeField] private CinemachineImpulseSource _impulseSource3;

    [SerializeField] private Transform[] _groundSlashOrigin;
    [SerializeField] private float _launchForce = 10;
    [SerializeField] private CinemachineVirtualCameraBase _virtualCameraBase;
    [Space(10), Header("VFX")]
    [SerializeField] public VisualEffect _vfxAttack;
    [SerializeField] public VisualEffect _vfxSlash1;
    [SerializeField] public VisualEffect _vfxSlash2;
    [SerializeField] public VisualEffect _vfxSlash3;
    [SerializeField] public VisualEffect _vfxCasting1;
    [SerializeField] private VFXGroundSlashEffect _prefabsEffect;


    
    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown(KeyCode.Q))_animator.SetTrigger("Attack");
        if( Input.GetKeyDown(KeyCode.W))_animator.SetTrigger("Slash");
        if( Input.GetKeyDown(KeyCode.E))_animator.SetTrigger("Casting");
    }

    public void PlayAttackVFX() {
        _impulseSource.GenerateImpulse();
        _vfxAttack.Play();
    }
    public void PlaySlashVFX1() {
        _impulseSource.GenerateImpulse();
        _vfxSlash1.Play();
    }
    public void PlaySlashVFX2() {
        _impulseSource.GenerateImpulse();
        _vfxSlash2.Play();
    }
    public void PlaySlashVFX3() {
        _impulseSource.GenerateImpulse();
        _vfxSlash3.Play();
    }

    public void PlayStartCast() {
        _virtualCameraBase.gameObject.SetActive(true);
    }
    public void PlayCastingVFX1() {
        _impulseSource2.GenerateImpulse();
        _vfxCasting1.Play();
    }
    
    public void PlayCastingVFX2() {
        _impulseSource3.GenerateImpulse();
        
        foreach (var _firePoint in _groundSlashOrigin) {
            VFXGroundSlashEffect effect = Instantiate(_prefabsEffect, _firePoint.position, Quaternion.identity);
            effect.transform.forward = _firePoint.forward;
            effect.GetComponent<Rigidbody>().AddForce(_firePoint.forward*_launchForce, ForceMode.Impulse);
        }
    }

    public void PlayEndCasting()
    {
        _virtualCameraBase.gameObject.SetActive(false);
    }
}
