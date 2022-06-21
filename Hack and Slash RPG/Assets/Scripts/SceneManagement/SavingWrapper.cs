using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] float fadeOutTime = 0.5f;
        const string defaultSaveFile = "Save";

        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();

            fader.FadeCompletely();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return fader.FadeOut(fadeOutTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
                Save();
            else if (Input.GetKeyDown(KeyCode.L))
                Load();
            else if(Input.GetKeyDown(KeyCode.Delete))
                Delete();
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}

