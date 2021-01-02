using RPG.Control;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField] float projectileSpeed = 5f;
        GameObject enemyTarget = null;
        float damage;
        [SerializeField] float projectileDamageModifier = 1f;
        [SerializeField] float projectileLifetime = 8f;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitEffect = null;

        GameObject myInstigator = null;

        private void Start()
        {
            DestroyOnTime();
            transform.LookAt(GetAimLocation());
        }

        // Update is called once per frame
        void Update()
        {
            if (isHoming && !enemyTarget.GetComponent<Health>().IsDead())
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCollider = enemyTarget.GetComponent<CapsuleCollider>();
            if (targetCollider != null)
            {
                return enemyTarget.transform.position + Vector3.up * (targetCollider.height / 2);
            }
            
            return enemyTarget.transform.position;
        }

        public void SetEnemyTarget(GameObject instigator, GameObject enemy, float damageToDeal)
        {
            this.enemyTarget = enemy;
            this.damage = damageToDeal * projectileDamageModifier;
            myInstigator = instigator;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == enemyTarget.gameObject)
            {
                if (other.GetComponent<Health>().IsDead()) return;

                other.gameObject.GetComponent<Health>().TakeDamage(myInstigator, damage);

                if (other.GetComponent<AIController>() != null)
                {
                    other.GetComponent<AIController>().SetTriggered();
                }

                projectileSpeed = 0f;

                if (hitEffect != null)
                {
                    GameObject hitParticle = Instantiate(hitEffect, transform.position, Quaternion.identity);
                    Destroy(hitParticle, 1f);
                }

                if (transform.Find("Head"))
                {
                    Destroy(transform.Find("Head").gameObject);
                }
                Destroy(gameObject, 2f);
            }
        }

        private void DestroyOnTime()
        {
            Destroy(gameObject, projectileLifetime);
        }

    }
}


