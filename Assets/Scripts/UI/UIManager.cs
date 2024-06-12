using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TMP_Text _txtMass, _txtPetrol, _txtGaz;

    [Space(10)] [Header("ConstructionButton"), SerializeField]
    private Button[] _bpConstruction;


    [Space(10)]
    [Header("Annonser"), SerializeField] 
    private TMP_Text _txtAnnoncer;
    [SerializeField]private CanvasGroup _canvasGroupAnnoncer;
    [SerializeField]private float _annoncerLifeTime =3f;
    [SerializeField]private AnimationCurve _annoncerLifeCurve = AnimationCurve.EaseInOut(0,1,1,0);
    void Start()
    {
        GamesManager.Instance.OnRessourceUpdate += UpdateRessources;
        GamesManager.Instance.OnErrorMessage+=GamesManager_DisplayAnnoncer;
    }

    private void GamesManager_DisplayAnnoncer(object sender, string e) => DisplayAnnoncer(e);
    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateButtonsInteractibility() {
        for (int i = 0; i < _bpConstruction.Length; i++) {
            _bpConstruction[i].interactable =GamesManager.Instance.CanBuildBuilding(i);
        }
    }
    public void UpdateRessources(object sender, EventArgs e ) {
        _txtGaz.text = GamesManager.Instance.Gaz.ToString();
        _txtPetrol.text = GamesManager.Instance.Petrol.ToString();
        _txtMass.text = GamesManager.Instance.Mass.ToString();
        
        UpdateButtonsInteractibility();
    }

    public void DisplayAnnoncer(string text) {
        _canvasGroupAnnoncer.DOPause();
        _txtAnnoncer.text = text;
        _canvasGroupAnnoncer.alpha = 1;
        _canvasGroupAnnoncer.DOFade(0, _annoncerLifeTime).SetEase(_annoncerLifeCurve);
    }
    

    [ContextMenu("Test Annoncer")]
    private void TestAnnoncer()
    {
        DisplayAnnoncer("Test");
    }
}
