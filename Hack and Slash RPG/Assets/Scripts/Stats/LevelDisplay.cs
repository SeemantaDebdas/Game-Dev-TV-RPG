using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI levelDisplayText = null;
        BaseStats baseStats = null;

        private void Start()
        {
            baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();  
        }

        private void Update()
        {
            levelDisplayText.text = "Levels: "+baseStats.GetLevel().ToString();
        }
    }
}
