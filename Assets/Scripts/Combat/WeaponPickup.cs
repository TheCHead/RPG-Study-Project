using RPG.Combat;
using RPG.Control;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weaponPickup;
        [SerializeField] float respawnTime = 5f;
        [SerializeField] AudioClip soundFX = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                AudioSource.PlayClipAtPoint(soundFX, transform.position);
                Pickup(other.GetComponent<Fighter>());
            }
        }

        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(weaponPickup);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            HidePickup();
            yield return new WaitForSeconds(seconds);
            RespawnPickup();
        }

        private void RespawnPickup()
        {
            transform.GetComponent<BoxCollider>().enabled = true;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                child.gameObject.SetActive(true);
            }
        }

        private void HidePickup()
        {
            GetComponent<BoxCollider>().enabled = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                child.gameObject.SetActive(false);
            }
        }

        public bool HandleRaycast(PlayerController controller)
        {
            if (Input.GetMouseButtonDown(0))
            {
                controller.GetComponent<Mover>().MoveTo(transform.position, controller.GetSpeedModifier());
                //Pickup(controller.GetComponent<Fighter>());
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }

}

