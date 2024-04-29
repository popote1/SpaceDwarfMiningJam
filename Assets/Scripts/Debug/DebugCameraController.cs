using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCameraController : MonoBehaviour
{

    public float speed = 5;
    

    // Update is called once per frame
    void Update() {
        Vector3 moveVec = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position += speed * Time.deltaTime*moveVec;
    }
}
