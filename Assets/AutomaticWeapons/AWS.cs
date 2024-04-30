using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWS : MonoBehaviour
{
    // AWS : Automated Weapon System
    // This class will be herited for each AWS ( turret mortars etc.. )

    
    public DetectionZone _detectionZone;
    private float fireRate;
    private float timeSinceLastShot = 0f;

    public float FireRate
    {
        get { return fireRate; }

        protected set { fireRate = value; }
    }
    
    protected virtual void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot > 1 / fireRate)
        {
            timeSinceLastShot = 0;
            AttackEnemies();
        }
        
        ManageOrientation();
    }

    protected virtual void AttackEnemies()
    {
        if (_detectionZone.detectedEnemies.Count > 0)
        {
            foreach (Enemy enemy in _detectionZone.detectedEnemies)
            {
                AttackEnemy(enemy);
            }
        }
        else return;
    }

    protected virtual void AttackEnemy(Enemy enemy)
    {
        
    }

    protected virtual void ManageOrientation()
    {
        
    }
    
}