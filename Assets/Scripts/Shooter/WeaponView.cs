using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Blaster.Weapon
{
    public class WeaponView : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Transform _gunPoint;
        [SerializeField] private SpriteRenderer _weaponSprite;
        [SerializeField] private TMPro.TMP_Text _hitText;
        [SerializeField] private ParticleSystem _smokePartilce;
        [SerializeField] private Transform _outTarget;
        [SerializeField] private GameObject _handHelp;
        [SerializeField] protected MeshRenderer _meshRenderer;

        public WeaponController Controller;
        public Transform GunPoint => _gunPoint;
        public Transform OutTarget => _outTarget;
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
            _weaponSprite.color = color;
            _meshRenderer.material.color = color;
        }
        public void SetHitText(string text)
        {
            _hitText.text = text;
        }
        public void PlaySmokeParticle()
        {
            _smokePartilce.Play();
        }
        public void ShowHandHelp(bool show)
        {
            _handHelp.SetActive(show);
        }
    }
}