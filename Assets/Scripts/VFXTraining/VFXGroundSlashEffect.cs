using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class VFXGroundSlashEffect : MonoBehaviour
{


    [SerializeField] private float _speed;
    [SerializeField] private float _slowDownFactor;
    [SerializeField] private float _detectingDistance;
    [SerializeField] private float _destroyDelay;


    private Rigidbody _rb;
    private bool stopped;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null) Debug.Log("No Rigidbody", this);
        else StartCoroutine(SlowDown());
        
        Destroy(gameObject, _destroyDelay);


    }

    private void FixedUpdate()
    {
        if (stopped) return;
        
        RaycastHit hit;
        Vector3 distance = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

        if (Physics.Raycast(distance, transform.TransformDirection(-Vector3.up), out hit, _detectingDistance)) {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }
    }

    IEnumerator SlowDown()
    {
        float t = 1;
        while (t > 0)
        {
            _rb.velocity = Vector3.Lerp(Vector3.zero, _rb.velocity,t);
            t -= _slowDownFactor;
            yield return new WaitForSeconds(0.1f);
        }

        stopped = true;
    }
}
