using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.Attributes
{
    public class ExperiencePointsDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI experiencePointsText = null;
        Experience experience;

        private void Awake()
        {
            experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            experiencePointsText.text = "XP: " + experience.GetExperiencePoints.ToString();
        }
    }
}