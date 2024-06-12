using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class VFXDay21Sequencer : MonoBehaviour
{
    public Material mat;

    [SerializeField] private float _animationTime;
    [Header("Fade")] [SerializeField] private AnimationCurve _fadeAnimationCurve;
    [Header("Gradiant")] [SerializeField] private AnimationCurve _GradiantPowerAnimationCurve;
    [Header("Gradiant")] [SerializeField] private AnimationCurve _GradiantScaleAnimationCurve;

    private float _timer;
    private bool _doAnim;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( _doAnim)ManagerNaim();
    }

    [ContextMenu("DoTransition")]
    private void DoTransition()
    {
        Rest();
        _doAnim = true;
    }

    private void ManagerNaim()
    {
        _timer += Time.deltaTime;

        float t = _timer / _animationTime;
        
        mat.SetFloat("_FadePower",_fadeAnimationCurve.Evaluate(t));
        mat.SetFloat("_GradianPower",_GradiantPowerAnimationCurve.Evaluate(t));

        if (_timer >= _animationTime) {
            //Rest();
        }
    }

    [ContextMenu("Rest")]
    private void Rest()
    {
        _timer = 0;
        _doAnim = false;
        
        mat.SetFloat("_FadePower",0);
        mat.SetFloat("_GradianPower",0);
    }
}
