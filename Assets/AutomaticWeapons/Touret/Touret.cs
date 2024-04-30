using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touret : AWS
{

    public float range = 10f;
    public float damage = 10f;
    public float rotationSpeed = 10f;
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
            Vector3 direction = (_targetEnemy.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.up, direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    private void Shoot()
    {
        Vector3 direction = (_targetEnemy.position - transform.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, range))
        {
            if (hit.collider.gameObject == _targetEnemy.gameObject)
            {
                Debug.DrawRay(transform.position, direction * hit.distance, Color.red, 0.1f);
                // _targetEnemy.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    }
}
