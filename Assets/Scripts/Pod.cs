using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pod : MonoBehaviour ,IDamageble
{

    [SerializeField] private int _hp =50;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        _hp -= damage;
        if (_hp <= 0)
        {
            Debug.Log("GameOver");
        }
    }
}
