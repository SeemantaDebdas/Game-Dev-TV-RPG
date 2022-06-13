using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression;

        //public float GetHealth()
        //{
        //    return progression.GetHealth(characterClass, startingLevel);
        //}

        //public float GetExperience()
        //{
        //    return 30f;
        //}

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, startingLevel);
        }
    }
}
