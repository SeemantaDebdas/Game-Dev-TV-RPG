using RPG.Attributes;
using System;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/New Weapon")]
    public class Weapon:ScriptableObject
    {
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] AnimatorOverrideController weaponAnimatorOverride = null;
        [SerializeField] float weaponRange;
        [SerializeField] float weaponDamage;
        [SerializeField] bool isRightHanded = false;
        [SerializeField] Projectile projectile = null;
        const string weaponName = "Weapon";

        public float WeaponDamage { get { return weaponDamage; } }
        public float WeaponRange { get { return weaponRange; } }
        public bool HasProjectile { get { return projectile != null; } }   

        public void SpawnWeapon(Transform rightHand,Transform leftHand, Animator anim)
        {
            //Destroy old Weapon before equipping new weapon
            DestroyOldWeapon(rightHand, leftHand);

            Transform handTransform = isRightHanded ? rightHand : leftHand;

            if(weaponPrefab != null)
            {
                GameObject weaponSpawn = Instantiate(weaponPrefab, handTransform);
                weaponSpawn.name = weaponName;
            }

            var overrideController = anim.runtimeAnimatorController as AnimatorOverrideController;
            if (weaponAnimatorOverride != null)
                anim.runtimeAnimatorController = weaponAnimatorOverride;
            else if (overrideController != null)
                anim.runtimeAnimatorController = overrideController.runtimeAnimatorController;
        }
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Transform handTransform = isRightHanded ? rightHand : leftHand;

            Projectile projectileSpawn = Instantiate(projectile, handTransform.position, Quaternion.identity);
            projectileSpawn.SetTraget(target, weaponDamage);
        }
        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform weapon = rightHand.Find(weaponName);
            if(weapon == null)
                weapon = leftHand.Find(weaponName);
            if (weapon == null) return;

            #region Why change weapon name
            ///Weapon name needs to be changed in case
            ///before destroying the current weapon, the
            ///new weapon is equipped. Both named "Weapon"
            ///There's a chance of the new weapon getting destroyed
            #endregion
            weapon.name = "GARBAGE";
            Destroy(weapon.gameObject);
        }

    }
}