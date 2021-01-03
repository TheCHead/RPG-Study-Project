using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;
using RPG.Stats;
using GameDevTV.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        GameObject target = null;
        float baseRange = 1f;
        float baseDamage = 1f;
        [SerializeField] float timeBetweenAttacks = 1f;
        float timeSinceLastAttack = Mathf.Infinity;
        
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;

        [Range(0,1)]
        [SerializeField] float chaseSpeedModifier = 0.6f;

        [SerializeField] WeaponConfig defaultWeapon = null;
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon = null;

        private void Awake()
        {
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }


        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();            
            GetWeaponStats(weapon);
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);

        }

        private void GetWeaponStats(WeaponConfig weapon)
        {
            baseDamage = weapon.GetWeaponDamage();
            baseRange = weapon.GetWeaponRange();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;


            if (target != null)
            {
                if (target.GetComponent<Health>().IsDead())
                {
                    DropTarget();
                }

                else
                {
                    GetComponent<Mover>().MoveTo(target.transform.position, chaseSpeedModifier);
                    if (Vector3.Distance(transform.position, target.transform.position) <= baseRange)
                    {
                        GetComponent<Mover>().StopMoving();
                        // Attack target
                        AttackBehaviour();
                    }
                }
                
            }
            else
            {
                return;
            }
        }

        public bool CanAttack(GameObject target)
        {
            if (target != null && !target.GetComponent<Health>().IsDead())
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public Health GetTarget()
        {
            if (target == null)
            {
                return null;
            }
            return target.GetComponent<Health>();
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                // This will trigger Hit animation event
                GetComponent<Animator>().ResetTrigger("stopAttack");
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0;
            }
        }


        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.gameObject;
        }

        private void DropTarget()
        {
            target = null;
        }

        public void Cancel()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
            DropTarget();
            print("Cancel fighting");

            GetComponent<Mover>().Cancel();
        }

        // Animation Event
        void Hit()
        {
            if (target == null) { return; }

            RunWeaponOnHit();

            target.GetComponent<Health>().TakeDamage(gameObject, GetComponent<BaseStats>().GetStat(Stats.Stats.Damage));
        }

        private void RunWeaponOnHit()
        {
            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }
        }


        // Animation Event
        void Shoot()
        {
            currentWeaponConfig.LaunchProjectile(gameObject, target, rightHandTransform, leftHandTransform, GetComponent<BaseStats>().GetStat(Stats.Stats.Damage));
            RunWeaponOnHit();
        }

        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }

        public IEnumerable<float> GetAdditiveModifier(Stats.Stats stat)
        {
            if (stat == Stats.Stats.Damage)
            {
                yield return currentWeaponConfig.GetWeaponDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stats.Stats stat)
        {
            if (stat == Stats.Stats.Damage)
            {
                yield return currentWeaponConfig.GetPercentageModifier();
            }
        }
    }
}
