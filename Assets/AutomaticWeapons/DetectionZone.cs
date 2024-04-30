using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
   public AWS aws;
   public List<Enemy> detectedEnemies = new List<Enemy>();

   private void OnTriggerEnter(Collider other)
   {
      Enemy enemy = other.GetComponent<Enemy>();
      if (enemy != null)
      {
         detectedEnemies.Add(enemy);
      }
   }

   private void OnTriggerExit(Collider other)
   {
      Enemy enemy = other.GetComponent<Enemy>();
      if (enemy != null)
      {
         detectedEnemies.Remove(enemy);
      }
   }
}
