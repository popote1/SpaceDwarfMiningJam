using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Animator Animator;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Space)) Animator.SetTrigger("Attack");
        if ( Input.GetKeyDown(KeyCode.A)) Animator.SetBool("IsDead", true);
    }
}
