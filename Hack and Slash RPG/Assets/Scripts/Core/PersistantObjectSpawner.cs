using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject persistantObject;

    static bool isSpawned = false;

    private void Awake()
    {
        if (isSpawned) return;

        isSpawned = true;
        
        GameObject persistantObjectSpawn =  Instantiate(persistantObject);
        DontDestroyOnLoad(persistantObjectSpawn);
    }
}
