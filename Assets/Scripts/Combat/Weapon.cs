using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float weaponRange = 2f;

        public void Spawn(Transform handTransform, Animator animator)
        {
            if (weaponPrefab != null)
            {
                Instantiate(weaponPrefab, handTransform);
            }
            animator.runtimeAnimatorController = animatorOverride;
        }

        public float GetWeaponRange()
        {
            return weaponRange;
        }

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }
    }

}