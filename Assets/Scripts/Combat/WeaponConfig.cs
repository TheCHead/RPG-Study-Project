using UnityEngine;

namespace RPG.Combat
{


    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        enum WeaponType { rightHanded, leftHanded, twoHanded };

        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapon weaponPrefab = null;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float damagePercentageModifier = 10f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] WeaponType weaponType;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";

        public void Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
        {
            DestroyOldWeapon(rightHandTransform, leftHandTransform);

            if (weaponPrefab != null)
            {
                Weapon weapon = Instantiate(weaponPrefab, GetHandTransform(rightHandTransform, leftHandTransform));
                weapon.gameObject.name = weaponName;
            }

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else
            {
                var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
                if (overrideController != null)
                {
                    animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
                }
            }

        }

        private void DestroyOldWeapon(Transform rightHandTransform, Transform leftHandTransform)
        {
            Transform oldWeapon = rightHandTransform.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHandTransform.Find(weaponName);
            }

            if (oldWeapon == null) return;

            oldWeapon.name = "Destroying";
            Destroy(oldWeapon.gameObject);

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

        public void LaunchProjectile(GameObject instigator, GameObject target, Transform rightHandTransform, Transform leftHandTransform, float calculatedDamage)
        {
            if (target == null) { return; }

            Projectile projectileInstance = Instantiate(projectile, GetHandTransform(rightHandTransform, leftHandTransform).position, Quaternion.identity);
            projectileInstance.GetComponent<Projectile>().SetEnemyTarget(instigator, target, calculatedDamage);
        }

        public float GetWeaponRange()
        {
            return weaponRange;
        }

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }

        public float GetPercentageModifier()
        {
            return damagePercentageModifier;
        }
    }

}