using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touret : AWS
{

    public float range = 10f;
    public int damage = 10;
    public float rotationSpeed = 10f;
    public Transform TurretHead;
    public ParticleSystem PSShoot;
    public GameObject ImpactPrefab;
    private Transform _targetEnemy;

    private void Start()
    {
        FireRate = 1;
    }
    
    

    protected override void AttackEnemy(GridAgent enemy)
    {
        _targetEnemy = enemy.transform;
        ManageOrientation();
        Shoot();
    }

    protected override void ManageOrientation()
    {
        if (_targetEnemy != null)
        {
            Vector3 direction = (_targetEnemy.position - TurretHead.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.up, direction);
            TurretHead.rotation = Quaternion.RotateTowards(TurretHead.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    private void Shoot()
    {
        if( PSShoot!=null) PSShoot.Play();
        Vector3 direction = (_targetEnemy.position - TurretHead.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(TurretHead.position, direction, out hit, range)) {
            Instantiate(ImpactPrefab, hit.point, Quaternion.identity);
            if (hit.collider.gameObject.GetComponent<IDamageble>()!=null) {
                hit.collider.gameObject.GetComponent<IDamageble>().TakeDamage(damage);
            }
            
        }
    }
}
