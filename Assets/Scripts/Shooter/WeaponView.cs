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
        [SerializeField] private TMPro.TMP_Text hitText;
        [SerializeField] private ParticleSystem smokePartilce;
        [SerializeField] private Transform outTarget;
        [SerializeField] private GameObject handHelp;

        public WeaponController Controller;
        public Transform GunPoint => gunPoint;
        public Transform OutTarget => outTarget;
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Pointer down on weapon view.");
            if (Controller != null)
            {
                ShowHandHelp(false);
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
        public void SetHitText(string text)
        {
            hitText.text = text;
        }
        public void PlaySmokeParticle()
        {
            smokePartilce.Play();
        }
        public void ShowHandHelp(bool show)
        {
            handHelp.SetActive(show);
        }
    }
}