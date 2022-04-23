using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Fighter playerFighter = other.GetComponent<Fighter>();
                playerFighter.EquipWeapon(weapon);
            }
        }
    }
}

