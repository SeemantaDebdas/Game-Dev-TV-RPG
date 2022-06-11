using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon;
        [SerializeField] float respawnTime = 5;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Triggering");
                Fighter playerFighter = other.GetComponent<Fighter>();
                playerFighter.EquipWeapon(weapon);
                StartCoroutine(HideForSeconds(respawnTime));
            }
        }

        IEnumerator HideForSeconds(float timeToHide)
        {
            ShowPickUp(false);
            yield return new WaitForSeconds(timeToHide);
            ShowPickUp(true);
        }

        private void ShowPickUp(bool shouldShow)
        {
            GetComponent<SphereCollider>().enabled = shouldShow;
            transform.GetChild(0).gameObject.SetActive(shouldShow);
        }
    }
}

