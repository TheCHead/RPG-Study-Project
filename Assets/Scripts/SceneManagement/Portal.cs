using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Control;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }


        [SerializeField] int sceneToLoad;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] DestinationIdentifier targetDestination;
        [SerializeField] float fadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }


        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);

            GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = false;
            //fade out
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut();
            print("faded out");

            //save current level
            FindObjectOfType<SavingWrapper>().Save();
            print("saved");

            //load new scene and spawn player
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            //AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

            //while (!asyncLoad.isDone)
            //{
            //    yield return null;
            //}

            print("scene loaded");

            GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = false;


            //load current level
            FindObjectOfType<SavingWrapper>().Load();


            Portal otherPortal = GetOtherPortal();
            TeleportPlayerToPortal(otherPortal);

            FindObjectOfType<SavingWrapper>().Save();


            // fade in
            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn();


            GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = true;

            Destroy(gameObject);
        }

        private void TeleportPlayerToPortal(Portal portal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(portal.spawnPoint.position);
            player.transform.rotation = portal.spawnPoint.rotation;
        }

        private Portal GetOtherPortal()
        {
            Portal[] portals = FindObjectsOfType<Portal>();

            foreach (Portal portal in portals)
            {
                if (portal.destination == targetDestination)
                {
                    return portal;
                }
            }

            return null;
        }
    }
}


