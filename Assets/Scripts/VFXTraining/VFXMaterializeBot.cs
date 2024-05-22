using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class VFXMaterializeBot : MonoBehaviour
{
    [SerializeField]private  Animator _animator;

    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer1;
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer2;
    [SerializeField] private VisualEffect _vfxGraph;
    [SerializeField] private Light _light;
     private Material _mat1;
     private Material _mat2;

    [Space(10), Header("Parameters")]
    [SerializeField] private float _duration;
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private AnimationCurve _shadeerCurve;
    [SerializeField] private AnimationCurve _shaderColorCurve;
    [SerializeField] private AnimationCurve _LightCurveCurve;
    [SerializeField][ColorUsageAttribute(true,true,0f,8f,0.125f,3f)] 
    private Color _colorMaterialization;
    [SerializeField][ColorUsageAttribute(true,true,0f,8f,0.125f,3f)] 
    private Color _colorMat1;
    [SerializeField][ColorUsageAttribute(true,true,0f,8f,0.125f,3f)] 
    private Color _colorMat2;

    [SerializeField] private bool _doAnim;
    private float _timer=0;


    private void Start() {
        _mat1 = _skinnedMeshRenderer1.material;
        _mat2 = _skinnedMeshRenderer2.material;
    }

    void Update()
    {
        ManageAnimation();
    }

    private void ManageAnimation() {
        if (!_doAnim) return;
        _timer += Time.deltaTime;

        float t = _timer / _duration;
        _animator.SetFloat("Blend",_animationCurve.Evaluate(t));
        _mat1.SetFloat("_Clip", _shadeerCurve.Evaluate(t));
        _mat2.SetFloat("_Clip", _shadeerCurve.Evaluate(t));
        _mat1.SetColor("_Color", Color.Lerp(_colorMaterialization, _colorMat1, _shaderColorCurve.Evaluate(t)));
        _mat2.SetColor("_Color", Color.Lerp(_colorMaterialization, _colorMat2, _shaderColorCurve.Evaluate(t)));
        _light.intensity = _LightCurveCurve.Evaluate(t);
        Debug.Log("Light value ="+_LightCurveCurve.Evaluate(t));
        if (t >= 1) {
            _doAnim = false;
        }
    }

    [ContextMenu("RestartAnime")]
    private void RestartAnim() {
        _timer = 0;
        _doAnim = true;
        _vfxGraph.Reinit();
    }
}
