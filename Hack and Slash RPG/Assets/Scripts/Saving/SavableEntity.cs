using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SavableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";
        static Dictionary<string, SavableEntity> globalLookup = new Dictionary<string,SavableEntity>();
        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach(ISaveable savable in GetComponents<ISaveable>())
            {
                //------------GetType() returns the type of script implementing the Interface----------
                state[savable.GetType().ToString()] = savable.CaptureState();
            }
            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach(ISaveable savable in GetComponents<ISaveable>())
            {
                if (stateDict.ContainsKey(savable.GetType().ToString()))
                    savable.RestoreState(stateDict[savable.GetType().ToString()]);
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.isPlaying) return;
            //either of two checks work. Checking if the game object is in prefab mode or not
            if (gameObject.scene.name == null || gameObject.scene.rootCount == 0) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;


            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            { 
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookup[property.stringValue] = this;
        }
#endif
        private bool IsUnique(string property)
        {
            if (!globalLookup.ContainsKey(property)) return true;

            if (globalLookup[property] == this) return true;

            #region Why this check?
            /*-----------------------
             * When we load another scene, the objects get destroyed
             * and so does there UIDs. But the dictionary still has those
             * UIDs. This check will remove those UIDs and make new ones
             ------------------------*/
            #endregion
            if (globalLookup[property] == null)
            {
                globalLookup.Remove(property);
                return true;
            }

            #region Why this check?
            /*-----------------------
             * When we change the value of UIDs in the editor,
             * this check will ensure that a new UID gets inserted
             * in it's place
             ------------------------*/
            #endregion
            if(globalLookup[property].GetUniqueIdentifier() != property)
            {
                globalLookup.Remove(property);
                return true;
            }

            return false;
        }
    }
}


