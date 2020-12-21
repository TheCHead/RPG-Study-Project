using UnityEngine;

namespace RPG.Combat
{


    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        enum WeaponType { rightHanded, leftHanded, twoHanded };

        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] WeaponType weaponType;

        [SerializeField] Projectile projectile = null;

        public void Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
        {
            if (weaponPrefab != null)
            {
                Instantiate(weaponPrefab, GetHandTransform(rightHandTransform, leftHandTransform));
            }

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }

        }

        private Transform GetHandTransform(Transform rightHandTransform, Transform leftHandTransform)
        {
            Transform handTransform = null;

            switch (weaponType)
            {
                case WeaponType.rightHanded:
                    handTransform = rightHandTransform;
                    break;
                case WeaponType.leftHanded:
                    handTransform = leftHandTransform;
                    break;
                case WeaponType.twoHanded:
                    break;
                default:
                    break;
            }
            return handTransform;
        }

        public bool HasProjectile()
        {
            return (projectile != null);
        }

        public void LaunchProjectile(GameObject target, Transform rightHandTransform, Transform leftHandTransform)
        {
            if (target == null) { return; }

            Projectile projectileInstance = Instantiate(projectile, GetHandTransform(rightHandTransform, leftHandTransform).position, Quaternion.identity);
            projectileInstance.GetComponent<Projectile>().SetEnemyTarget(target, weaponDamage);
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