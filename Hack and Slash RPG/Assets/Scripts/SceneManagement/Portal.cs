using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A,B,C,D,E
        }


        [SerializeField] private DestinationIdentifier identifier;
        [SerializeField] int sceneIdx = -1;
        [SerializeField] Transform spawnPointTransform;

        [Header("Fader")]
        [SerializeField] float fadeInTime = 0.5f;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeWaitTime = 0.5f;

        private void Awake()
        {
            if (spawnPointTransform == null)
                spawnPointTransform = transform.GetChild(0);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(Transition());
            }
        }

        IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeIn(fadeInTime);

            //saving state before loading another scene
            FindObjectOfType<SavingWrapper>().Save();

            yield return SceneManager.LoadSceneAsync(sceneIdx);

            //loading state after scene load is complete
            FindObjectOfType<SavingWrapper>().Load();

            Portal portal = GetOtherPortal();
            UpdatePlayerPosition(portal);

            FindObjectOfType<SavingWrapper>().Save();

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeOut(fadeOutTime);

            Destroy(gameObject);
        }

        private void UpdatePlayerPosition(Portal portal)
        {
            if (portal == null) return;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = portal.spawnPointTransform.position;
            player.transform.rotation = portal.spawnPointTransform.localRotation;
        }

        private Portal GetOtherPortal()
        {
            Portal targetPortal = null;
            Portal[] portals =  FindObjectsOfType<Portal>();
            for(int i = 0; i < portals.Length; i++)
            {
                if (portals[i] == this) continue;
                if (portals[i].identifier == this.identifier)
                    targetPortal = portals[i];
            }

            return targetPortal;
        }
    }

}
