using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        GameObject target;
        float baseRange = 1f;
        float baseDamage = 1f;
        [SerializeField] float timeBetweenAttacks = 1f;
        float timeSinceLastAttack = Mathf.Infinity;
        
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;

        [SerializeField] Weapon defaultWeapon = null;
        Weapon currentWeapon = null;

        [Range(0,1)]
        [SerializeField] float chaseSpeedModifier = 0.6f;


        private void Start()
        {
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

    }
}
