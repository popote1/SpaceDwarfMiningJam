using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public static List<Cell> Spawners = new List<Cell>();
    
    [SerializeField] private GameObject _wallobject;
    [SerializeField] private GameObject _prefabsGaz;
    [SerializeField] private GameObject _prefabspetrol;
    [SerializeField] private GameObject _prefabsMass;
    [SerializeField] private GameObject _prefabsSpawner;
    [SerializeField] private GameObject _ressouces;
    
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
    
    public enum  RessourceType {
        None, Gaz,Pertrole, Mass
    }

    public RessourceType Ressouces;

    public int CellCost {
        get {
            if (IsWall) return Metrics.WALLMOVECOST;
            return Metrics.GROUNDMOVECOST;
        }
    }

     
    public Vector2Int Coordinate; 

    private bool _iswall;

    public void ResetMoveCost ()=> MoveCost = int.MaxValue;

    public void SetRessourcePrefab()
    {
        switch (Ressouces)
        {
            case RessourceType.None:
                break;
            case RessourceType.Gaz:
                _ressouces = Instantiate(_prefabsGaz, transform.position, Quaternion.identity);
                break;
            case RessourceType.Pertrole:
                _ressouces = Instantiate(_prefabspetrol, transform.position, Quaternion.identity);
                break;
            case RessourceType.Mass:
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
    }
}