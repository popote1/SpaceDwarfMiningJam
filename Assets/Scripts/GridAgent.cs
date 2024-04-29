using System;
using UnityEngine;

public class GridAgent : MonoBehaviour
{
    [SerializeField] private float _acceleration = 5;
    [SerializeField] private float _maxSpeed = 10;
    
    private MapGenerator _mapGenerator;
    private Rigidbody _rigidbody;
    private Vector3 _direction;

    private float t;
    private void Start() {
        _mapGenerator = MapGenerator.Instance;
        _rigidbody=GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ManageDirectionUpdate();
        ManageMovement();
    }

    private void ManageDirectionUpdate() {
        t += Time.deltaTime;
        if (t >= Metrics.ACTORMOVEUPDATETIME) {
            _direction =_mapGenerator.GetCellFromWorld(transform.position).MoveDir;
            t = 0;
        }
    }

    private void ManageMovement() {
        _rigidbody.AddForce(_acceleration*Time.deltaTime*_direction);
        _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _maxSpeed);
    }
}