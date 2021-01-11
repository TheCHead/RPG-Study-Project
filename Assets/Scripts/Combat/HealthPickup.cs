using RPG.Attributes;
using RPG.Control;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class HealthPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] float healthRestore = 0;
        [SerializeField] AudioClip soundFX = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                other.gameObject.GetComponent<Health>().Heal(healthRestore);
                AudioSource.PlayClipAtPoint(soundFX, transform.position);
                Destroy(gameObject);
            }
        }

        public bool HandleRaycast(PlayerController controller)
        {
            if (Input.GetMouseButtonDown(0))
            {
                print("It's a health pickup!");
                controller.GetComponent<Mover>().MoveTo(transform.position, controller.GetSpeedModifier());
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}


