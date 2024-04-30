
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GamesManager : MonoBehaviour
{
    [SerializeField] private MapGenerator _mapGenerator;

    [Space(10), Header("Spawning")] [SerializeField]
    private bool _doAutoSpawn;
    [SerializeField]private float _SpawnDelay=2;
    [SerializeField]private GridAgent _ennemiesPrefabs;
    

    [Space(20)] [Header("Debug Stuff")] 
    [SerializeField] private GridAgent _prefabGridAgent;
    [SerializeField] private GameObject GreenDebugCube;
    [SerializeField] private GameObject RedDebugCube;
    [SerializeField] private GameObject SelectionPointer;

    [SerializeField] private LayerMask _groundLayerMask;

    private Cell _selectedCell;
    private Camera _camera;
    private float _spawntimer=0;
    
    void Start() {
        _camera = Camera.main;
        
    }

    // Update is called once per frame
    void Update() {
        
        // Debug Stuff
        if( Input.GetButtonDown("Fire1")) ClickOnMap();
        if (Input.GetKeyDown(KeyCode.A) && _selectedCell != null)_mapGenerator.CalculateFlowField(_selectedCell);
        if (Input.GetKeyDown(KeyCode.E) && _selectedCell != null) SpawnGridActor();
        if (Input.GetKeyDown(KeyCode.R) && _selectedCell != null) DebugCell();
        if (Input.GetKeyDown(KeyCode.T) && _selectedCell != null) DoMassExplosion();
        if (Input.GetKeyDown(KeyCode.Y) && _selectedCell != null) DoBurningGround();
        
        //GamesStuff
        ManageEnnemieSpawning();
    }
    
    [ContextMenu("DisplayCenterMap")]
    private void DisplayCenterMap() {
        Cell cell = _mapGenerator.GetCenterMap();
        if (cell != null) {
            Instantiate(RedDebugCube, cell.transform.position, quaternion.identity);
        }
    }

    private void ClickOnMap()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 50, _groundLayerMask))
        {
            Cell selectedCell =_mapGenerator.GetCellFromWorld(hit.point);
            if (selectedCell == null) return;
            _selectedCell = selectedCell;
            SelectionPointer.transform.position = selectedCell.transform.position;

        }
    }

    private void SpawnGridActor() {
        Instantiate(_prefabGridAgent, _selectedCell.transform.position + Vector3.one / 2, Quaternion.identity);
    }

    private void DebugCell()
    {
        Debug.Log("Selected Cell: \n " +
                  "Coordinate ="+_selectedCell.Coordinate+"\n" +
                  "Direction Vec ="+ _selectedCell.MoveDir+"\n" +
                  "MoveCost ="+_selectedCell.MoveCost);
    }

    private void ManageEnnemieSpawning() {
        if (!_doAutoSpawn) return;
        _spawntimer += Time.deltaTime;
        if (_spawntimer >= _SpawnDelay) {
            _spawntimer = 0;
            Cell target = Cell.Spawners[Random.Range(0, Cell.Spawners.Count)];
            Instantiate(_ennemiesPrefabs, target.transform.position, Quaternion.identity);
        }
    }

    private void DoMassExplosion() {
        Debug.Log("DoMassExpolo");
        List<Cell> cells = new List<Cell>();
        cells.AddRange(_mapGenerator.GetNeighbors4Diagonal(_selectedCell));
        cells.AddRange(_mapGenerator.GetNeighbors4Straingt(_selectedCell));
        cells.Add(_selectedCell);
        foreach (var cell in cells) {
            if (cell.Building == null) {
                cell.IsWall = true;
            }
        }
    }
    private void DoBurningGround() {
        Debug.Log("DoMassExpolo");
        List<Cell> cells = new List<Cell>();
        cells.AddRange(_mapGenerator.GetNeighbors4Straingt(_selectedCell));
        cells.Add(_selectedCell);
        foreach (var cell in cells) {
            if (cell.Building == null) {
                cell.SetBurning(10);
            }
        }
    }
}
