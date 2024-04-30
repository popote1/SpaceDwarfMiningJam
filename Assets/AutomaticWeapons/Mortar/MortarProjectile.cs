using System;
using UnityEngine;

public class MortarProjectile : MonoBehaviour
{
    public Transform target;
    public Vector3 PosTarget;
    public float speed = 10f;
    public float gravity = -9.81f;
    public float maxHeight = 5f;
    public float LifeTime = 1f;

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

        if (t>=LifeTime) {
            OnEndOfLife();
        }
    }

    private void OnEndOfLife()
    {
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