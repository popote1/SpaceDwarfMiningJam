using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance;
    
    [SerializeField] private Vector2Int _mapSize = new Vector2Int(50,50);
    [SerializeField] private Cell _prefabCell;
    [SerializeField] private int _cellCicle = 300;
    [SerializeField] private float _cellCicleDelay = 0.01f;
    
    [Space(20),Header("Parameters")]
    [Range(0, 5)] [SerializeField] private float _maxdistanceFactor = 0.77f;
    [Range(0, 2)] [SerializeField] private float _mindistanceFactor = 0;
    [Range(0, 5)] [SerializeField] private float _maxdistanceCentreFactor = 0.54f;
    [Range(0, 2)] [SerializeField] private float _mindistanceCentreFactor = 1.1f;
    [SerializeField] private Vector2 _PerlinOffset;
    [SerializeField] private float _PerlinZoom =0.12f;
    [Range(0, 1)] [SerializeField] private float _threashHold = 0.5f;
    [SerializeField][Range(0, 1)] private float _torusRadius=0.35f;
    [SerializeField] private float _torusThikness = 40;
    [Range(0, 1)] [SerializeField] private float _ressourcesThreachhold = 0.7f;
    [SerializeField][Range(0, 1)] private float _torusSpawnersRadius=0.35f;
    [SerializeField] private float _torusSpawnersThikness = 40;
    [Range(0, 1)] [SerializeField] private float _SpawnersThreachhold = 0.7f;
    private Cell[,] _cells;

    private Cell _pfOrigine;
    private bool _psIsBaking;


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        GenerateMap();
    }
    
    #region Accesors
    
    public Cell GetCenterMap() {
        return _cells[_mapSize.x / 2, _mapSize.y / 2];
    }

    public Cell GetCellFromWorld(Vector3 pos) {
        return GetCell(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.z));
    }

    public List<Cell> GetNeighbors4Straingt(Cell cell) {
        List<Cell> returnList = new List<Cell>();
        Cell neighbor;
        neighbor = GetCell(cell.Coordinate.x - 1, cell.Coordinate.y );
        if(neighbor!=null) returnList.Add(neighbor);
        neighbor = GetCell(cell.Coordinate.x , cell.Coordinate.y -1);
        if(neighbor!=null) returnList.Add(neighbor);
        neighbor = GetCell(cell.Coordinate.x + 1, cell.Coordinate.y );
        if(neighbor!=null) returnList.Add(neighbor);
        neighbor = GetCell(cell.Coordinate.x , cell.Coordinate.y + 1);
        if(neighbor!=null) returnList.Add(neighbor);
        return returnList;
    }
    public List<Cell> GetNeighbors4Diagonal(Cell cell) {
        List<Cell> returnList = new List<Cell>();
        Cell neighbor;
        neighbor = GetCell(cell.Coordinate.x - 1, cell.Coordinate.y + 1);
        if(neighbor!=null) returnList.Add(neighbor);
        neighbor = GetCell(cell.Coordinate.x - 1, cell.Coordinate.y - 1);
        if(neighbor!=null) returnList.Add(neighbor);
        neighbor = GetCell(cell.Coordinate.x + 1, cell.Coordinate.y -1);
        if(neighbor!=null) returnList.Add(neighbor);
        neighbor = GetCell(cell.Coordinate.x + 1, cell.Coordinate.y + 1);
        if(neighbor!=null) returnList.Add(neighbor);
        return returnList;
    }
    
    
    private Cell GetCell(int x, int y) {
        if (x < 0 || x >= _mapSize.x || y < 0 || y >= _mapSize.y) return null;
        return _cells[x, y];
    }

    #endregion

    #region Processors

    public void CalculateFlowField(Cell origin)
    {
        
        if (_psIsBaking) return;
        _pfOrigine = origin;
        Debug.Log("start PathFinding;");
        StartCoroutine("CalculateFlownFieldCo");
        
        /*foreach (var cell in _cells) {
            cell.ResetMoveCost();
        }
        
        origin.MoveCost = 0;
        List<Cell> OpenList = new List<Cell>();
        OpenList.Add(origin);
        
        
        while (OpenList.Count>0) {
            Debug.Log("Cell calculated");
            Cell cell = OpenList[0];
            Debug.Log(" Cell have "+GetNeighbors4Straingt(cell).Count+" cells");
            foreach (var neighbor in GetNeighbors4Straingt(cell)) {
                if (neighbor.MoveCost > cell.CellCost + cell.MoveCost + Metrics.STRAINGHTMOVECOST) {
                    neighbor.MoveCost = cell.CellCost + cell.MoveCost + Metrics.STRAINGHTMOVECOST;
                    neighbor.MoveDir = cell.transform.position - neighbor.transform.position;
                    OpenList.Add(neighbor);
                }
            }
            foreach (var neighbor in GetNeighbors4Diagonal(cell)) {
                if (neighbor.MoveCost > cell.CellCost + cell.MoveCost + Metrics.DIAGONALEMOVECOST) {
                    neighbor.MoveCost = cell.CellCost + cell.MoveCost + Metrics.DIAGONALEMOVECOST;
                    neighbor.MoveDir = cell.transform.position - neighbor.transform.position;
                    OpenList.Add(neighbor);
                }
            }

            OpenList.Remove(cell);
        }
        Debug.Log("PathFinghtdingDone");*/
    }


    IEnumerator CalculateFlownFieldCo() {
        _psIsBaking = true;

        int counter = 0;
        foreach (var cell in _cells) {
            cell.ResetMoveCost();
        }
        
        _pfOrigine.MoveCost = 0;
        List<Cell> OpenList = new List<Cell>();
        OpenList.Add(_pfOrigine);
        
        
        while (OpenList.Count>0) {
            if (counter >= _cellCicle)
            {
               yield return new WaitForSeconds(_cellCicleDelay);
               counter = 0;
            }
            Cell cell = OpenList[0];
            foreach (var neighbor in GetNeighbors4Straingt(cell)) {
                if (neighbor.MoveCost > cell.CellCost + cell.MoveCost + Metrics.STRAINGHTMOVECOST) {
                    neighbor.MoveCost = cell.CellCost + cell.MoveCost + Metrics.STRAINGHTMOVECOST;
                    neighbor.MoveDir = (cell.transform.position - neighbor.transform.position).normalized;
                    OpenList.Add(neighbor);
                }
            }
            foreach (var neighbor in GetNeighbors4Diagonal(cell)) {
                if (neighbor.MoveCost > cell.CellCost + cell.MoveCost + Metrics.DIAGONALEMOVECOST) {
                    neighbor.MoveCost = cell.CellCost + cell.MoveCost + Metrics.DIAGONALEMOVECOST;
                    neighbor.MoveDir = (cell.transform.position - neighbor.transform.position).normalized;
                    OpenList.Add(neighbor);
                }
            }

            OpenList.Remove(cell);
            counter++;
        }
        Debug.Log("PathFinghtdingDone");
        _psIsBaking = false;
        yield return null;
    }
    #endregion
    #region Builder
    private void GenerateMap() {
        _cells = new Cell[_mapSize.x, _mapSize.y];
        for (int x = 0; x < _mapSize.x; x++) {
            for (int z = 0; z < _mapSize.y; z++) {
                _cells[x, z] = Instantiate(_prefabCell, new Vector3(x, 0, z), Quaternion.identity);
                _cells[x, z].transform.SetParent(transform);
                Vector2 pos = new Vector2(x, z);
                float value = GetCircleValue(pos) * GetPerlinValue(x, z) * GetCircleCenterValue(pos);
                _cells[x, z].IsWall = value < _threashHold;
                _cells[x, z].Coordinate = new Vector2Int(x, z);

                if (GetPerlinValue(x, z, _PerlinOffset.x + 100, _PerlinOffset.y + 100, 0.5f) * GetHardTorusValue(pos , _torusRadius, _torusThikness) >
                    _ressourcesThreachhold) {
                    _cells[x, z].Ressouces = Metrics.RESSOURCETYPE.Gaz;
                }
                if (GetPerlinValue(x, z, _PerlinOffset.x + 200, _PerlinOffset.y + 200, 0.5f) * GetHardTorusValue(pos , _torusRadius, _torusThikness) >
                    _ressourcesThreachhold) {
                    _cells[x, z].Ressouces = Metrics.RESSOURCETYPE.Petrole;
                }
                if (GetPerlinValue(x, z, _PerlinOffset.x + 300, _PerlinOffset.y + 300, 0.5f) * GetHardTorusValue(pos , _torusRadius, _torusThikness) >
                    _ressourcesThreachhold) {
                    _cells[x, z].Ressouces = Metrics.RESSOURCETYPE.Mass;
                }
                
                if (GetPerlinValue(x, z, _PerlinOffset.x + 300, _PerlinOffset.y + 300, 0.3f) * GetHardTorusValue(pos , _torusSpawnersRadius, _torusSpawnersThikness) >
                    _SpawnersThreachhold) {
                    _cells[x, z].IsSpwaner = true;
                }

                _cells[x, z].SetRessourcePrefab();
            }
        }
    }

    private float GetPerlinValue(float x , float y) {
        return 1-Mathf.Clamp01(Mathf.PerlinNoise((x+_PerlinOffset.x)*_PerlinZoom, (y+_PerlinOffset.y)*_PerlinZoom));
    }
    private float GetPerlinValue(float x , float y , float xOffset , float yoffSet , float zoom) {
        return 1-Mathf.Clamp01(Mathf.PerlinNoise((x+xOffset)*zoom, (y+yoffSet)*zoom));
    }

    private float GetCircleValue(Vector2 pos)
    {
        float radius = _mapSize.x / 2f;
        Vector2 center = _mapSize / 2;
        float dist = Vector2.Distance(pos, center)*_maxdistanceFactor;
        return 1-( dist / radius-_mindistanceFactor);
    }
    private float GetCircleCenterValue(Vector2 pos)
    {
        float radius = _mapSize.x / 2f;
        Vector2 center = _mapSize / 2;
        float dist = Vector2.Distance(pos, center)*_maxdistanceCentreFactor;
        return 1-( dist / radius-_mindistanceCentreFactor);
    }
    private float GetTorusValue(Vector2 pos) {
        float dist = Vector2.Distance(pos, _mapSize / 2);
        float radius = _mapSize.x / 2f * _torusRadius; 
        if (dist > radius) {
            return 1- (dist-radius)/(_torusThikness/2f);
        }
        return (dist - (radius - (_torusThikness / 2f)) )/ (_torusThikness / 2f);
        
    }
    
    private float GetHardTorusValue(Vector2 pos , float torusRadius , float torusThickness)
    {
        float dist = Vector2.Distance(pos, _mapSize / 2);
        float radius = _mapSize.x / 2f * torusRadius;
        float innerRadius = _mapSize.x / 2f * torusRadius -torusThickness / 2;
        float outerRadius = _mapSize.x / 2f * torusRadius + torusThickness / 2;
        if (innerRadius < dist && dist < outerRadius)
        {
            return 1;
        }
        return 0;
    }

    #endregion
}