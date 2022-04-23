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

        public float WeaponDamage { get { return weaponDamage; } }
        public float WeaponRange { get { return weaponRange; } }

        public void SpawnWeapon(Transform handTransform, Animator anim)
        {
            if(weaponPrefab != null)
                Instantiate(weaponPrefab, handTransform);
            if(weaponAnimatorOverride != null)
                anim.runtimeAnimatorController = weaponAnimatorOverride; 
        }
    }
}