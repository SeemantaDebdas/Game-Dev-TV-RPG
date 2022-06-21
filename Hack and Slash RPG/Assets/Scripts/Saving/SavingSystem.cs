using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        public void Delete(string saveFile)
        {
            File.Delete(saveFile);
        }

        public IEnumerator LoadLastScene(string saveFile)
        {
            //get the state
            Dictionary<string, object> state = LoadFile(saveFile);

            if (state.ContainsKey("lastSceneBuildIndex"))
            {
                //load the last scene
                int sceneBuildIdx = (int)state["lastSceneBuildIndex"];
                if (sceneBuildIdx != SceneManager.GetActiveScene().buildIndex)
                    yield return SceneManager.LoadSceneAsync(sceneBuildIdx);
            }
            
            RestoreState(state);
        }

        private void SaveFile(string saveFile, object state)
        {
            string savePath = GetPathFromSaveFile(saveFile);
            print("Saving at: " + savePath);

            using (FileStream stream = File.Open(savePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string loadPath = GetPathFromSaveFile(saveFile);
            if (!File.Exists(loadPath))
            {
                return new Dictionary<string, object>();
            }

            //if file already exists, return this
            using (FileStream stream = File.Open(loadPath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SavableEntity savableEntity in FindObjectsOfType<SavableEntity>())
            {
                string id = savableEntity.GetUniqueIdentifier();
                if (state.ContainsKey(id))
                    savableEntity.RestoreState(state[id]);
            }
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (SavableEntity savableEntity in FindObjectsOfType<SavableEntity>())
            {
                state[savableEntity.GetUniqueIdentifier()] = savableEntity.CaptureState();
            }

            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}

