﻿using System;
using UnityEngine;

public class GridAgent : MonoBehaviour
{
    [SerializeField] private float _acceleration = 5;
    [SerializeField] private float _maxSpeed = 10;

    [SerializeField] private float _delayToDestoyWall=5;
    [SerializeField] private Cell _collidedCell;

    [Space(10), Header("HP")] [SerializeField]
    private int _hP = 10;
    [SerializeField]private bool _isBurning;
    [SerializeField] private GameObject _ps_Burning;
    
    private MapGenerator _mapGenerator;
    private Rigidbody _rigidbody;
    private Vector3 _direction;
    private float _wallDestructionTimer;

    private float t;
    private float _burningTimer=0;
    private void Start() {
        _mapGenerator = MapGenerator.Instance;
        _rigidbody=GetComponent<Rigidbody>();
    }

    private void Update() {
        ManageDirectionUpdate();
        ManageMovement();
        ManageWallDestruction();
        ManageBurning();
    }

    private void ManageDirectionUpdate() {
        t += Time.deltaTime;
        if (t >= Metrics.ACTORMOVEUPDATETIME)
        {
            Cell currentCell = _mapGenerator.GetCellFromWorld(transform.position);
            _direction =currentCell.MoveDir;
            if (currentCell.IsBurning) {
                _isBurning = true;
            }
            t = 0;
        }
    }

    private void ManageMovement() {
        _rigidbody.AddForce(_acceleration*Time.deltaTime*_direction);
        _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _maxSpeed);
        transform.forward = _rigidbody.velocity;
    }

    private void ManageWallDestruction() {
        if (_collidedCell != null && _collidedCell.IsWall) {
            _wallDestructionTimer += Time.deltaTime;
            if (_wallDestructionTimer >= _delayToDestoyWall) {
                _collidedCell.IsWall = false;
                _collidedCell = null;
            }
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.transform.CompareTag("Cell")&&_mapGenerator.GetCellFromWorld(other.transform.position).IsWall) {
            _collidedCell = _mapGenerator.GetCellFromWorld(other.transform.position);
            _wallDestructionTimer = 0;
        }
        
    }
    private void OnCollisionExit(Collision other) {
        if (other.transform.CompareTag("Cell")&&_mapGenerator.GetCellFromWorld(other.transform.position)==_collidedCell) {
            _collidedCell = null;
        }
    }

    private void ManageBurning() {
        if (!_isBurning) return;
        _ps_Burning.SetActive(true);
        _burningTimer += Time.deltaTime;
        if (_burningTimer > Metrics.BURININGTICKDELAY) {
            TakeDamage(Metrics.BURININGTICKDAMAGE);
            _burningTimer = 0;
        }
    }

    public void TakeDamage(int damage) {
        _hP -= damage;
        if (_hP <= 0) {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}