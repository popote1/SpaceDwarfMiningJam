using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public static List<Cell> Spawners = new List<Cell>();
    
    [SerializeField] private GameObject _wallobject;
    [SerializeField] private GameObject _prefabsGaz;
    [SerializeField] private GameObject _prefabspetrol;
    [SerializeField] private GameObject _prefabsMass;
    [SerializeField] private GameObject _prefabsSpawner;
    [SerializeField] private GameObject _prefabsBurningZone;
    [SerializeField] private GameObject _ressouces;
    //[SerializeField] private GameObject _burningeffectPrefab;
    [SerializeField] private BurningCell _burningeffectPrefab;
    
    public GameObject Building;
    public Metrics.RESSOURCETYPE Ressouces;
    
    [NonSerialized]public int MoveCost;
    [NonSerialized]public  Vector3 MoveDir;
    [NonSerialized] public bool IsSpwaner;
    

    

    public bool IsWall {
        get => _iswall;
        set {
            _iswall = value;
            if (_wallobject!=null)_wallobject.SetActive(value);
        }
    }
    public bool IsBurning {
        get {
            if (_burningCell==null) return false;
            return _burningCell.IsBurning;
        }
         private set { }
    }
    
    
    public int CellCost {
        get {
            if (IsWall) return Metrics.WALLMOVECOST;
            return Metrics.GROUNDMOVECOST;
        }
    }

     
    public Vector2Int Coordinate; 
    
    private bool _iswall;
    private BurningCell _burningCell;

    public void ResetMoveCost ()=> MoveCost = int.MaxValue;

    

    public void SetRessourcePrefab(){
        switch (Ressouces) {
            case Metrics.RESSOURCETYPE.None:
                break;
            case Metrics.RESSOURCETYPE.Gaz:
                _ressouces = Instantiate(_prefabsGaz, transform.position, Quaternion.identity);
                break;
            case Metrics.RESSOURCETYPE.Petrole:
                _ressouces = Instantiate(_prefabspetrol, transform.position, Quaternion.identity);
                break;
            case Metrics.RESSOURCETYPE.Mass:
                _ressouces = Instantiate(_prefabsMass, transform.position, Quaternion.identity);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (IsSpwaner) {
            IsWall = false;
            Spawners.Add(this);
            _ressouces = Instantiate(_prefabsSpawner, transform.position, Quaternion.identity);
        }

        if (_ressouces != null) _ressouces.transform.SetParent(transform);
    }
    public void SetBurning(float time)
    {
        if (_burningCell != null) {
            _burningCell.SetBurning(time);
            return;
        }
        _burningCell = Instantiate(_burningeffectPrefab, transform.position, quaternion.identity);
        _burningCell.transform.SetParent(transform);
        _burningCell.SetBurning(time);
    }
    
}