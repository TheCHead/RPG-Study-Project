using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using System;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;

        // Start is called before the first frame update
        void Start()
        {
            health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            if (health.IsDead()) { return; }

            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            print("nothing to do");
        }



        private bool InteractWithCombat()
        {
                RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

                foreach (RaycastHit hit in hits)
                {
                    CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                    if (target == null) { continue; }

                    GameObject targetGameObject = target.gameObject;

                    if (GetComponent<Fighter>().CanAttack(targetGameObject))
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            GetComponent<Fighter>().Attack(targetGameObject);
                        }
                    return true;
                    }

                    else
                    {
                        continue;
                    }
                }
            return false;
        }


        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

