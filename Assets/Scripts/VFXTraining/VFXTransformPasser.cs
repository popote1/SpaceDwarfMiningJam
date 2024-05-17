using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXTransformPasser : MonoBehaviour
{

    public VisualEffect VFXGraph;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        VFXGraph.SetVector3("Target", transform.position); 
    }
}
