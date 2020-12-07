using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        Transform target;
        float baseRange = 1f;
        float baseDamage = 1f;
        [SerializeField] float timeBetweenAttacks = 1f;
        float timeSinceLastAttack = Mathf.Infinity;
        
        [SerializeField] Transform handTransform = null;

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
            weapon.Spawn(handTransform, animator);

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
                    GetComponent<Mover>().MoveTo(target.position, chaseSpeedModifier);
                    if (Vector3.Distance(transform.position, target.position) <= baseRange)
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
            transform.LookAt(target);
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
            target = combatTarget.transform;
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


            print("Get that you smooth monster!");
            target.GetComponent<Health>().TakeDamage(baseDamage);
        }
    }
}
