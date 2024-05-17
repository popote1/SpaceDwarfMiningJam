using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : AWS
{
    public float range = 10f;
    public int damage = 10;
    public float rotationSpeed = 10f;
    public Transform TurretHead;
    public ParticleSystem PSShoot;
    public GameObject ImpactPrefab;
    public float BurningTime =10f;
    public Transform FirePoint;
    public LineRenderer LRFire;
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
        if (_targetEnemy != null) {
            Vector3 direction = (_targetEnemy.position - TurretHead.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.up, direction);
            TurretHead.rotation = Quaternion.RotateTowards(TurretHead.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            LRFire.enabled = true;
            LRFire.positionCount = 10;
            LRFire.SetPositions(
                GetFireCurbe(FirePoint.position, _targetEnemy.position, 
                    GetBezierKey(FirePoint.position, _targetEnemy.position, 0.5f), 10));
            return;
        }

        LRFire.enabled = false;
    }
    
    private void Shoot() {
        if( PSShoot!=null) PSShoot.Play();
        Vector3 direction = (_targetEnemy.position - TurretHead.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(TurretHead.position, direction, out hit, range)) {
            Instantiate(ImpactPrefab, hit.point, Quaternion.identity);
            foreach (var cell in MapGenerator.Instance.GetCrossCellsFromWorldPos(hit.point)) {
                cell.SetBurning(BurningTime);
            }
            if (hit.collider.gameObject.GetComponent<IDamageble>()!=null) {
                hit.collider.gameObject.GetComponent<IDamageble>().TakeDamage(damage);
            }
            
        }
    }

    private Vector3[] GetFireCurbe(Vector3 pos1 , Vector3 pos2 , Vector3 key ,int nombres ) {
        Vector3[] points = new Vector3[nombres];
        for (int i = 0; i < nombres; i++) {
            points[i] = Metrics.BezierPoint(pos1, pos2, key, i/(float)nombres);
        }
        return points;
    }

    private Vector3 GetBezierKey(Vector3 pos1 , Vector3 pos2 ,float yOffSet) {
        return Vector3.Lerp(pos1, pos2, 0.5f) + Vector3.up * yOffSet;
    }
}
