using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] _mats;
    [SerializeField] private Color _colBuildable = Color.green, _colBlocked = Color.red;
    [SerializeField] private float _speed=0.1f;
    private Vector3 _posTarget;

    public void SetBuildable(bool isBuildable) {
        Color col = _colBlocked;
        if (isBuildable) col = _colBuildable;
        foreach (var mat in _mats) {
            mat.material.color = col;

        }
    }

    public void SetBuildingPos(Vector3 pos) {
        _posTarget = pos;
    }

    public void Destroy() {
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _posTarget, _speed*Time.deltaTime);
    }
}
