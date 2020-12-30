
using RPG.Control;
using RPG.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{

    [RequireComponent(typeof(Health))]

    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public bool HandleRaycast(PlayerController controller)
        {
            if (!controller.GetComponent<Fighter>().CanAttack(this.gameObject))
            {
                return false;
            }

            if (Input.GetMouseButton(0))
            {
                controller.GetComponent<Fighter>().Attack(this.gameObject);
            }
            //SetCursor(CursorType.Combat);
            return true;
        }
    }
}
