using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GamesManager : MonoBehaviour
{
    [SerializeField] private MapGenerator _mapGenerator;




    [Space(20)] [Header("Debug Stuff")] 
    [SerializeField] private GridAgent _prefabGridAgent;
    [SerializeField] private GameObject GreenDebugCube;
    [SerializeField] private GameObject RedDebugCube;
    [SerializeField] private GameObject SelectionPointer;

    [SerializeField] private LayerMask _groundLayerMask;

    private Cell _selectedCell;


    private Camera _camera;
    void Start() {
        _camera = Camera.main;
        
    }

    // Update is called once per frame
    void Update() {
        if( Input.GetButtonDown("Fire1")) ClickOnMap();
        if (Input.GetKeyDown(KeyCode.A) && _selectedCell != null)_mapGenerator.CalculateFlowField(_selectedCell);
        if (Input.GetKeyDown(KeyCode.E) && _selectedCell != null) SpawnGridActor();
        if (Input.GetKeyDown(KeyCode.R) && _selectedCell != null) DebugCell();
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
}
