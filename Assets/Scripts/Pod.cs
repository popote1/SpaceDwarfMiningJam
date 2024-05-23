using System;
using System.Collections;
using System.Collections.Generic;
using Ressources;
using UnityEngine;

public class Pod : MachineInfos ,IDamageble
{

    [SerializeField] private int _hp =50;
    public bool CanDropRessources(Metrics.RESSOURCETYPE type) {
        if (resourceNeeded == Metrics.RESSOURCETYPE.Everything) return true;
        return resourceNeeded == type;
    }
    public override void  DropRessources(Metrics.RESSOURCETYPE type) {
            Debug.Log("Ressources dorped"+ type);
            switch (type) {
                case Metrics.RESSOURCETYPE.None: break; 
                case Metrics.RESSOURCETYPE.Gaz: GamesManager.Instance.ChangeGaz(5); break;
                case Metrics.RESSOURCETYPE.Petrole: GamesManager.Instance.ChangePetrol(5); break;
                case Metrics.RESSOURCETYPE.Mass: GamesManager.Instance.ChangeMass(5); break;
                case Metrics.RESSOURCETYPE.Everything: break; default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
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
