using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

public class VFXDay9sequencer : MonoBehaviour
{


    [SerializeField] private float _duration = 2;
    [SerializeField] private bool _doAnimation;

    [Space(5), Header("LineRenderer")] [SerializeField]
    private TrailRenderer _trailRenderer;
    [SerializeField] private float _trSize = 0.5f;
    [SerializeField] private AnimationCurve _trAnimationCurve;
    [Space(5), Header("VFXGraph")] [SerializeField]
    private VisualEffect VisualEffect;

    [Space(5), Header("PostProcess")] 
    [SerializeField] private Volume _volume;
    [SerializeField] private float _lensDistortionScale = 1f;
    [SerializeField] private AnimationCurve _lensDistortionAnimationCurve;
    [SerializeField] private float _vignetteIntencityScale = 1f;
    [SerializeField] private AnimationCurve _vignetteIntencityAnimationCurve;



    private float _timer;
    private Vignette _vignette;
    private LensDistortion _lensDistortion;


    private void Start() {
        _volume.profile.TryGet(out _vignette);
        _volume.profile.TryGet(out _lensDistortion);
    }

    private void Update() {
        ManageAnimation();
    }

    private void ManageAnimation() {
        if (!_doAnimation) return;
        float t = _timer / _duration;
        _timer += Time.deltaTime;
        
        _trailRenderer.time = _trSize*_trAnimationCurve.Evaluate(t);
        
        ManagePostProcess(t);
        
        if (_timer >= _duration) {
            _doAnimation = false;
            _trailRenderer.time = 0;
        }
    }

    public void StartAnimation() {
        _timer = 0;
        _doAnimation = true;
        VisualEffect.Play();
    }

    private void ManagePostProcess(float t)
    {
        _vignette.intensity.value = _vignetteIntencityScale * _vignetteIntencityAnimationCurve.Evaluate(t);
        _lensDistortion.intensity.value = _lensDistortionScale * _lensDistortionAnimationCurve.Evaluate(t);
    }
}
