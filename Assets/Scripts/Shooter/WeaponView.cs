using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Blaster.Weapon
{
    public class WeaponView : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Transform gunPoint;
        [SerializeField] private SpriteRenderer weaponSprite;

        public WeaponController Controller;
        public Transform GunPoint => gunPoint;
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Pointer down on weapon view.");
            if (Controller != null)
            {
                if (Controller.CheckWeaponInTopRow())
                { 
                    //Controller.IsActive = true; 
                    //Debug.Log("Weapon activated."); 
                }// Activate the weapon controller
                
            }
        }
        public void SetColor(Color color)
        {
            weaponSprite.color = color;
        }
      
    }
}