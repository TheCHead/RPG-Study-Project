using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save file";

        IEnumerator Start()
        {
            FindObjectOfType<Fader>().FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return FindObjectOfType<Fader>().FadeIn();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
            print($"saving to { defaultSaveFile }");
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
            print($"loading from { defaultSaveFile }");
        }


    }
}
