using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;


public class UIButtonFeedBack : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public bool enable;
    [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0,0,1,1);
    [SerializeField] private float _size;
    [SerializeField] private float _duration;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        DoOn();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DoOff();
    }

    private void DoOn()
    {
        if (!enable) return;
        transform.DOPause();
        transform.DOScale(Vector3.one * _size, _duration).SetEase(_animationCurve);
    }
    private void DoOff()
    {
        if (!enable) return;
        transform.DOPause();
        transform.DOScale(Vector3.one , _duration);
    }
}
