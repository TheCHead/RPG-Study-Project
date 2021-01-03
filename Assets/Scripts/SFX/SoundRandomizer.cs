using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SFX
{
    public class SoundRandomizer : MonoBehaviour
    {
        [SerializeField] AudioClip[] soundsToRandomize;
        

        public void PlayRandomSFX()
        {
            int randIndex = Random.Range(0, soundsToRandomize.Length - 1);
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = soundsToRandomize[randIndex];
            audioSource.pitch = Random.Range(0.7f, 1f);
            audioSource.Play();
        }
    }
}


