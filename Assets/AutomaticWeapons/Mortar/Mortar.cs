using UnityEngine;

public class Mortar : AWS
{
    public Transform Canon;
    public MortarProjectile mortarPrefab;
    public float ProjectilHeight = 10f;
    public float ProjectileMaxLifeTime = 1f;
    public float MaxEnnemyDistance = 25f;

    public ParticleSystem detonationParticle;

    public void Start()
    {
        FireRate = 2f;
    }

    protected override void AttackEnemy(GridAgent enemy) {
        Debug.Log("Fire");

        MortarProjectile mortarProjectile = Instantiate(mortarPrefab, transform.position, Quaternion.identity);
        mortarProjectile.maxHeight = ProjectilHeight;
        mortarProjectile.PosTarget = enemy.transform.position;
        mortarProjectile.LifeTime = calculateBulletLifeTime(enemy.transform.position);
    }

    protected override void ManageOrientation()
    {
        //if (_detectionZone.detectedEnemies.Count <= 0) return;
        //if (_detectionZone.detectedEnemies[0] == null) return;

        if (_currenttarget == null) return;
            
        

        Canon.up = CalculateControlPoint(transform.position, _currenttarget.transform.position)
                   - transform.position;
    }

    private float calculateBulletLifeTime(Vector3 pos)
    {
        float dist = Vector3.Distance(transform.position, pos);
        float t = dist / MaxEnnemyDistance;
        return Mathf.Lerp(0.1f, ProjectileMaxLifeTime, t);
    }

    // Vector3 CalculateQuadraticBezierPoint(float t, Vector3 pointA, Vector3 pointB, Vector3 pointC)
    // {
    //     Vector3 pointAB = Vector3.Lerp(pointA, pointB, t);
    //     Vector3 pointBC = Vector3.Lerp(pointB, pointC, t);
    //     Vector3 pointABC = Vector3.Lerp(pointAB, pointBC, t);
    //
    //     return pointABC;
    // }

    Vector3 CalculateControlPoint(Vector3 StartPos, Vector3 PosTarget)
    {
        Vector3 controlPoint = Vector3.Lerp(PosTarget, StartPos, 0.5f);
        controlPoint.y = ProjectilHeight;

        return controlPoint;
    }
}