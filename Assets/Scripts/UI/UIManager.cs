using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TMP_Text _txtMass, _txtPetrol, _txtGaz;
    void Start()
    {
        GamesManager.Instance.OnRessourceUpdate += UpdateRessources;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateRessources(object sender, EventArgs e ) {
        _txtGaz.text = GamesManager.Instance.Gaz.ToString();
        _txtPetrol.text = GamesManager.Instance.Petrol.ToString();
        _txtMass.text = GamesManager.Instance.Mass.ToString();
       
    } 
}
