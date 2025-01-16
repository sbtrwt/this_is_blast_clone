using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Blaster.Weapon
{
    public class WeaponView : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Transform gunPoint;

        public WeaponController Controller;
        public Transform GunPoint => gunPoint;
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Pointer down on weapon view.");
            if (Controller != null)
            {
                Controller.IsActive = true; // Activate the weapon controller
                Debug.Log("Weapon activated.");
            }
        }
      
    }
}