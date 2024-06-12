using System;
using DG.Tweening;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class UIButtonFeedBack : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public bool enable;
    [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0,0,1,1);
    [SerializeField] private float _size;
    [SerializeField] private float _duration;

    private Selectable _selectable;

    private void Start() {
        _selectable = GetComponent<Selectable>();
    }

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
        if (!enable||!IsInteractable()) return;
        transform.DOPause();
        transform.DOScale(Vector3.one * _size, _duration).SetEase(_animationCurve);
    }
    private void DoOff()
    {
        if (!enable||!IsInteractable()) return;
        transform.DOPause();
        transform.DOScale(Vector3.one , _duration);
    }


    private bool IsInteractable() {
        if (_selectable != null&&_selectable.interactable) return true;
        return false;
    }
}
