using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Saving;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        private bool isPlayed = false;

        

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && !isPlayed)
            {
                GetComponent<PlayableDirector>().Play();
                isPlayed = true;
            }
            
        }

        public object CaptureState()
        {
            return isPlayed;
        }

        public void RestoreState(object state)
        {
            isPlayed = (bool)state;
        }

    }

}



