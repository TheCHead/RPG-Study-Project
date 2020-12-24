using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Resources;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
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

        [SerializeField] Weapon defaultWeapon = null;
        Weapon currentWeapon = null;



        private void Start()
        {
            //if (GetComponent<Health>().IsDead()) return;

            if (currentWeapon != null) return;

            EquipWeapon(defaultWeapon);
        }


        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);

            GetWeaponStats(weapon);
        }

        private void GetWeaponStats(Weapon weapon)
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
            target.GetComponent<Health>().TakeDamage(baseDamage);
        }


        // Animation Event
        void Shoot()
        {
            currentWeapon.LaunchProjectile(target, rightHandTransform, leftHandTransform);
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}
