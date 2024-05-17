using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXGroundSlashLauncher : MonoBehaviour
{
   [SerializeField] private VFXGroundSlashEffect _prefabsEffect;
   [SerializeField] private Transform _firePoint;
   [SerializeField] private float _launchForce;
   [SerializeField] private Camera _camera;
   
   private void Update() {
      if(Input.GetKeyUp(KeyCode.Mouse0)) ManageLaunchEffect();
   }

   private void ManageLaunchEffect() {
      Debug.Log("LaunchEffect");

      Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

      RaycastHit hit;
      if (Physics.Raycast(ray, out hit)) {
         Vector3 dir = (hit.point - transform.position).normalized;
         VFXGroundSlashEffect effect = Instantiate(_prefabsEffect, _firePoint.position, Quaternion.identity);
         effect.transform.forward = dir;
         effect.GetComponent<Rigidbody>().AddForce(dir.normalized*_launchForce, ForceMode.Impulse);
      }

      
   }
}
