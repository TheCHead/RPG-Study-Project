using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
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
                }
            }
            else
            {
                return;
            }
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
        }

        public void DropTarget()
        {
            target = null;
        }
    }
}
