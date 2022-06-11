using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour,ISaveable
    {
        [SerializeField] float health = 100f;
        float initialHealth = 0f;

        Animator anim;
        ActionScheduler actionScheduler;
        readonly int DeathTrigger = Animator.StringToHash("DeathTrigger");

        public bool IsDead { get { return isDead; } }
        bool isDead = false;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Start()
        {
            initialHealth = GetComponent<BaseStats>().GetHealth();
            health = initialHealth;
        }

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            
            if (health <= 0)
            {
                Die();
            }
        }

        public float GetHealthPercentage()
        {
            return (health / initialHealth) * 100;
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            anim.SetTrigger(DeathTrigger);
            actionScheduler.CancelCurrentAction();
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float)state;
            if (health <= 0) Die();
        }
    }
}

