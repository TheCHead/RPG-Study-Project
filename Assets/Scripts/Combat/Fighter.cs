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

        private void Update()
        {
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
            GetComponent<Animator>().SetTrigger("attack");
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
        }
    }
}
