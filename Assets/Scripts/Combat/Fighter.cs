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
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        float timeSinceLastAttack = 0;
        [SerializeField] float baseDamage = 5f;

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;


            if (target != null)
            {
                GetComponent<Mover>().MoveTo(target.position);
                if (Vector3.Distance(transform.position, target.position) <= weaponRange)
                {
                    GetComponent<Mover>().StopMoving();
                    // Attack target
                    AttackBehaviour();
                }
            }
            else
            {
                return;
            }
        }

        private void AttackBehaviour()
        {
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                // This will trigger Hit animation event
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0;
            }
        }


        public void Attack(CombatTarget combatTarget)
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
            DropTarget();
            print("Cancel fighting");
        }

        // Animation Event
        void Hit()
        {
            print("Get that you smooth monster!");
            target.GetComponent<Health>().TakeDamage(baseDamage);
        }
    }
}
