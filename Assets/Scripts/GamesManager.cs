
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GamesManager : MonoBehaviour
{
    public static GamesManager Instance;

    public event EventHandler OnRessourceUpdate;
    public event EventHandler<string> OnErrorMessage;
    [SerializeField] private MapGenerator _mapGenerator;

    [Space(10), Header("Spawning")] [SerializeField]
    private bool _doAutoSpawn;

    [SerializeField] private float _SpawnDelay = 2;
    [SerializeField] private GridAgent _ennemiesPrefabs;

    [FormerlySerializedAs("_ExtractorController")] [Space(10), Header("ConstrunctionMode")] [SerializeField]
    public Extractor _Extractorprefab;

    [SerializeField] public AWS _mortarPrefab;
    [SerializeField] public AWS _turretPrefab;
    [SerializeField] public AWS _FlameThrowerPrefab;
    [SerializeField] private BuildingInfoStruct[] _buildingInfoStructs;



    [Space(20)] [Header("Debug Stuff")] [SerializeField]
    private GridAgent _prefabGridAgent;

    [SerializeField] private GameObject GreenDebugCube;
    [SerializeField] private GameObject RedDebugCube;
    [SerializeField] private GameObject SelectionPointer;

    [SerializeField] private LayerMask _groundLayerMask;

    private Cell _selectedCell;
    private Camera _camera;
    private float _spawntimer = 0;
    private bool _isInContructionMode;
    private BuildingGhost _contructionghost;
    private IBuildable _selectedconstruction;
    private int _selectionConstructionId = -1;
    private GameObject _selectedGoconstruction;


    private int _mass=5;
    private int _petrol;
    private int _gaz;

    public int Mass { get => _mass;}
    public int Petrol { get => _petrol;}
    public int Gaz { get => _gaz;}


private void Awake() {
        Instance = this;
    }

    void Start() {
        _camera = Camera.main;
        
    }

    // Update is called once per frame
    void Update() {
        
        // Debug Stuff
        if( Input.GetButtonDown("Fire")) ClickOnMap();
        if (Input.GetKeyDown(KeyCode.A) && _selectedCell != null)_mapGenerator.CalculateFlowField(_selectedCell);
        if (Input.GetKeyDown(KeyCode.E) && _selectedCell != null) SpawnGridActor();
        if (Input.GetKeyDown(KeyCode.R) && _selectedCell != null) DebugCell();
        if (Input.GetKeyDown(KeyCode.T) && _selectedCell != null) DoMassExplosion();
        if (Input.GetKeyDown(KeyCode.Y) && _selectedCell != null) DoBurningGround();
        if (Input.GetKeyDown(KeyCode.Alpha1) ) TryToBuild(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) ) TryToBuild(1);
        if (Input.GetKeyDown(KeyCode.Alpha3) ) TryToBuild(2);
        if (Input.GetKeyDown(KeyCode.Alpha4) ) TryToBuild(3);
        //if (Input.GetKeyDown(KeyCode.U) ) StartContructionMode(_Extractorprefab,_Extractorprefab.gameObject);
        //if (Input.GetKeyDown(KeyCode.I) ) StartContructionMode(_mortarPrefab,_mortarPrefab.gameObject);
        //if (Input.GetKeyDown(KeyCode.O) ) StartContructionMode(_turretPrefab,_turretPrefab.gameObject);
        //if (Input.GetKeyDown(KeyCode.P) ) StartContructionMode(_FlameThrowerPrefab,_FlameThrowerPrefab.gameObject);
        if( Input.GetButtonDown("Fire")&&_isInContructionMode)ManageContruction();

        //GamesStuff
        ManageEnnemieSpawning();
        ManagerContructionMode();
        
    }

    public void ChangeMass(int value) {
        _mass += value;
        OnRessourceUpdate?.Invoke(this, EventArgs.Empty);
    }
    public void ChangePetrol(int value) {
        _petrol += value;
        OnRessourceUpdate?.Invoke(this, EventArgs.Empty);
    }
    public void ChangeGaz(int value) {
        _gaz += value;
        OnRessourceUpdate?.Invoke(this, EventArgs.Empty);
    }
    
    [ContextMenu("DisplayCenterMap")]
    private void DisplayCenterMap() {
        Cell cell = _mapGenerator.GetCenterMap();
        if (cell != null) {
            Instantiate(RedDebugCube, cell.transform.position, quaternion.identity);
        }
    }

    private void ClickOnMap() {
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

    public void DoMassExplosion(Vector3 pos) {
        List<Cell> cells = new List<Cell>();
        Cell origin = _mapGenerator.GetCellFromWorld(pos);
        cells.AddRange(_mapGenerator.GetNeighbors4Diagonal(origin));
        cells.AddRange(_mapGenerator.GetNeighbors4Straingt(origin));
        cells.Add(origin);
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
    public void DoBurningGround(Vector3 pos) {
        Debug.Log("DoMassExpolo");
        List<Cell> cells = new List<Cell>();
        Cell origin = _mapGenerator.GetCellFromWorld(pos);
        cells.AddRange(_mapGenerator.GetNeighbors4Straingt(origin));
        cells.Add(origin);
        foreach (var cell in cells) {
            if (cell.Building == null) {
                cell.SetBurning(10);
            }
        }
    }
    private void StartContructionMode(IBuildable buildable,GameObject go, int id, Vector3 startPos )
    {
        if (!_isInContructionMode) {
            _contructionghost = Instantiate(_buildingInfoStructs[id]._buildingGhost);
            _contructionghost.transform.position = startPos;
            _isInContructionMode = true;
            _selectedconstruction = buildable;
            _selectedGoconstruction = go;
            _selectionConstructionId = id;
            return;
        }
        _contructionghost.Destroy();
        _isInContructionMode = false;
        _selectedconstruction = null;
        _selectedGoconstruction = null;
        _selectionConstructionId = -1;
    }

    private void ManagerContructionMode()
    {
        if (!_isInContructionMode) return;
        RaycastHit hit;
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 50, _groundLayerMask)) {
            Cell selectedCell =_mapGenerator.GetCellFromWorld(hit.point);
            //if (selectedCell == null|| selectedCell.IsWall ||selectedCell.Building!=null) return;
            //_contructionghost.transform.position = selectedCell.transform.position;
            _contructionghost.SetBuildable(_selectedconstruction.CanBeBuild(selectedCell));
            _contructionghost.SetBuildingPos(selectedCell.transform.position);
            //if (_selectedconstruction.CanBeBuild(selectedCell)) {
            //    _contructionghost.transform.position = selectedCell.transform.position;
            //}
        }
    }

    public void ManageContruction() {
        RaycastHit hit;
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 50, _groundLayerMask))
        { 
            Cell selectedCell =_mapGenerator.GetCellFromWorld(hit.point);
            if (! _selectedconstruction.CanBeBuild(selectedCell)) return;
            
            GameObject extra =Instantiate(_selectedGoconstruction, selectedCell.transform.position, Quaternion.identity);
            extra.GetComponent<IBuildable>().OnBuild(_selectedCell);

            BuildingInfoStruct info = _buildingInfoStructs[_selectionConstructionId];
            ChangeGaz(-info.Gaz);
            ChangeMass(-info.Mass);
            ChangePetrol(-info.Petrol);
            
            selectedCell.Building = extra.gameObject;
            StartContructionMode(null, null, -1, Vector3.zero);
        }
    }

    public void TryToBuild(int id) {
        if (CanBuildBuilding(id)) {
            StartContructionMode(
                _buildingInfoStructs[id]._prefabBuilding.GetComponent<IBuildable>()
                ,_buildingInfoStructs[id]._prefabBuilding , id, GetMouseWorldPos());
            return;
        }
        OnErrorMessage?.Invoke(this,"PAS ASSEZ DE RESSOURCE");
        //Debug.Log("Can't Build");
    }

    public bool CanBuildBuilding(int id) {
        if (_buildingInfoStructs == null || _buildingInfoStructs.Length <= id) return false;
        if (_buildingInfoStructs[id].Mass <= _mass
            && _buildingInfoStructs[id].Gaz <= _gaz
            && _buildingInfoStructs[id].Petrol <= _petrol
            && _buildingInfoStructs[id]._prefabBuilding != null) return true;
        return false;
    }

    private Vector3 GetMouseWorldPos() {
        RaycastHit hit;
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 50, _groundLayerMask)) {
            return hit.point;
        }
        return Vector3.zero;
        
    }
}
