using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    
    public class Fader : MonoBehaviour
    {
        [SerializeField] float fadeOutTime = 2f;
        [SerializeField] float fadeInTime = 1f;

        CanvasGroup canvasGroup = null;
        Coroutine activeFade = null;

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        public Coroutine FadeOut()
        {
            // Cancel running coroutines
            if (activeFade != null)
            {
                StopCoroutine(activeFade);
            }
            // Fade out
            activeFade = StartCoroutine(FadeOutRoutine());
            return activeFade;
        }
        
        private IEnumerator FadeOutRoutine()
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / fadeOutTime;

                yield return null;
            }
        }

        public Coroutine FadeIn()
        {
            // Cancel running coroutines
            if (activeFade != null)
            {
                StopCoroutine(activeFade);
            }
            // Fade out
            activeFade = StartCoroutine(FadeinRoutine());
            return activeFade;

        }

        private IEnumerator FadeinRoutine()
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / fadeInTime;

                yield return null;
            }
        }


        private void Start()
        {
            
        }

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

    }
}

