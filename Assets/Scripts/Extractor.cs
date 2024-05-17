using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Ressources;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Extractor : MonoBehaviour , IDamageble , IBuildable
{

    [SerializeField] private int _hP;
    [SerializeField] private Metrics.RESSOURCETYPE _ressourcetype;

    [Header("Ressouces"), Space(10)]
    [SerializeField] private float _productionDelay=10;
    [SerializeField] private BarrelInfos _prefabsBarrel;
    [SerializeField] private Transform _ejectionpoint;
    [SerializeField] private float _ejectionPower =2f;
    [Header("BuildingType"), Space(10)]
    [SerializeField] private GameObject _prefabPetrolExtractor;
    [SerializeField] private GameObject _prefabGazlExtractor;
    [SerializeField] private GameObject _prefabMassExtractor;

    private float _productionTimer=0;

    private void Update()
    {
        ManageExtraction();
    }

    public void TakeDamage(int damage) {
        _hP -= damage;
        if (_hP <= 0) Die();
    }

    private void Die() {
        Destroy(gameObject);
    }

    public bool CanBeBuild(Cell cell)
    {
        if (cell.IsWall || cell.IsBurning || cell.Building || cell.Ressouces == Metrics.RESSOURCETYPE.None)
            return false;
        return true;
    }

    public GameObject SelecteBuild(Cell cell)
    {
        return null;
    }

    public void OnBuild(Cell cell) {
        _ressourcetype = cell.Ressouces;
        GameObject buildPrefab;
        switch (_ressourcetype)
        {
            case Metrics.RESSOURCETYPE.None: buildPrefab = _prefabGazlExtractor; break;
            case Metrics.RESSOURCETYPE.Gaz: buildPrefab = _prefabGazlExtractor; break;
            case Metrics.RESSOURCETYPE.Petrole:buildPrefab = _prefabPetrolExtractor; break;
            case Metrics.RESSOURCETYPE.Mass: buildPrefab = _prefabMassExtractor; break;
            default: throw new ArgumentOutOfRangeException();
        }
        Instantiate(buildPrefab, transform.position, quaternion.identity);
    }

    private void ManageExtraction() {
        _productionTimer += Time.deltaTime;
        if (_productionTimer >= _productionDelay) {
            _productionTimer = 0;
            
            BarrelInfos go =Instantiate(_prefabsBarrel, _ejectionpoint.position, Random.rotation);
            go.SetRessourseType(_ressourcetype);
            Vector3 projection = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1))+Vector3.up;
            go.GetComponent<Rigidbody>().AddForce(projection*_ejectionPower,ForceMode.Force);
        }
    }
    
    
    
}
