using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWS : MonoBehaviour, IBuildable
{
    // AWS : Automated Weapon System
    // This class will be herited for each AWS ( turret mortars etc.. )

    
    public DetectionZone _detectionZone;
    private float fireRate;
    private float timeSinceLastShot = 0f;
    protected GridAgent _currenttarget;

    public float FireRate
    {
        get { return fireRate; }

        protected set { fireRate = value; }
    }
    
    protected virtual void Update()
    {
        CheckForTarget();
        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot >  fireRate) {
            timeSinceLastShot = 0;
            AttackEnemies();
        }
        
        ManageOrientation();
    }

    protected virtual void AttackEnemies() {
        if(_currenttarget==null) return;
        AttackEnemy(_currenttarget);
    }

    protected virtual void AttackEnemy(GridAgent enemy)
    {
        
    }

    protected virtual void ManageOrientation()
    {
        
    }

    protected void CheckForTarget() {
        if( _currenttarget!=null) return;
        if (_detectionZone.GetEnnemi() != null) {
            _currenttarget = _detectionZone.GetEnnemi();
        }
    }

    public bool CanBeBuild(Cell cell) {
        if (cell == null || cell.Building != null || cell.Ressouces != Metrics.RESSOURCETYPE.None ||
            cell.IsWall|| cell.IsBurning) return false;
        return true;
    }

    public GameObject SelecteBuild(Cell cell) {
        return null;
    }

    public void OnBuild(Cell cell) {
    }
}