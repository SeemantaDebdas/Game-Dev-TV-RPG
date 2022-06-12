using RPG.Attributes;
using TMPro;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI healthText = null;
        Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            Health target = fighter.GetTarget();
            if (target != null)
                healthText.text = "Enemy: " + target.GetHealthPercentage().ToString();
            else
                healthText.text = "Enemy: N/A";
        }
    }
}
