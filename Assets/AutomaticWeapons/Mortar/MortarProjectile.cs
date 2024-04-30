using System;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;

public class MortarProjectile : MonoBehaviour
{
    public Vector3 PosTarget;
    public float maxHeight = 5f;
    public float LifeTime = 1f;
    public GameObject ExploisionEffect;

    private float t = 0f;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        
        
        t += Time.deltaTime ;

        Vector3 position = CalculateQuadraticBezierPoint(t/LifeTime, startPosition, CalculateControlPoint(), PosTarget);
        transform.position = position;
        transform.up =
            CalculateQuadraticBezierPoint((t + Time.deltaTime) / LifeTime, startPosition, CalculateControlPoint(),
                PosTarget) - position;

        if (t>=LifeTime) {
            OnEndOfLife();
        }
    }

    private void OnEndOfLife()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 2);
        foreach (var col in cols) {
            if (col.GetComponent<IDamageble>() != null) {
                col.GetComponent<IDamageble>().TakeDamage(10);
            }
        }

        Instantiate(ExploisionEffect, transform.position, quaternion.identity);
        Destroy(gameObject);
    }

    Vector3 CalculateQuadraticBezierPoint(float t, Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        Vector3 pointAB = Vector3.Lerp(pointA, pointB, t);
        Vector3 pointBC = Vector3.Lerp(pointB, pointC, t);
        Vector3 pointABC = Vector3.Lerp(pointAB, pointBC, t);

        return pointABC;
    }

    Vector3 CalculateControlPoint()
    {
        //Vector3 direction = (target.position - startPosition).normalized;
        //Vector3 up = Vector3.up;
        //Vector3 right = Vector3.Cross(up, direction).normalized;
        //Vector3 controlPoint = startPosition + direction * maxHeight + right * maxHeight;
        Vector3 controlPoint = Vector3.Lerp(PosTarget, startPosition, 0.5f);
        controlPoint.y = maxHeight;

        return controlPoint;
    }
}