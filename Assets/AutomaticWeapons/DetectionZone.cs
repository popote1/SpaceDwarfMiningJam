using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
   public AWS aws;
   public List<GridAgent> detectedEnemies = new List<GridAgent>();

   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag("Ennemi"))
      {
         detectedEnemies.Add(other.GetComponent<GridAgent>());
      }
      
   }

   private void OnTriggerExit(Collider other) {
      if (other.gameObject.CompareTag("Ennemi"))
      {
         detectedEnemies.Remove(other.GetComponent<GridAgent>());
      }
   }

   public GridAgent GetEnnemi() {
      CheckOfNull();   
      if (detectedEnemies.Count > 0) {
         return detectedEnemies[0];
      }
      return null;
      
   }

   public void CheckOfNull()
   {
      foreach (var zombi in detectedEnemies.ToArray())
      {
         if (zombi == null)
         {
            detectedEnemies.Remove(zombi);
            continue;
         }
         //if( Vector3.Distance(transform.position, zombi.transform.position)>MaxDistance) Zombis.Remove(zombi);
      }
   }
}
