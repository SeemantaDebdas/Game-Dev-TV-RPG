using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookupTable();

            float[] levels =  lookupTable[characterClass][stat];
            return levels[level - 1];
        }

        public int GetLevel(Stat stat, CharacterClass characterClass)
        {
            BuildLookupTable();
            float[] levels = lookupTable[characterClass][stat];
            return levels.Length;
        }

        private void BuildLookupTable()
        {
            if (lookupTable != null) return;

            lookupTable = new();

            foreach(ProgressionCharacterClass progressionClass in characterClasses)
            {
                Dictionary<Stat, float[]> statLookupTable = new();
                foreach(ProgressionStat progressionStat in progressionClass.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }
                lookupTable[progressionClass.characterClass] = statLookupTable;
            }
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}
