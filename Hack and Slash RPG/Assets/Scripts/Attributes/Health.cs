using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour,ISaveable
    {
        float health = -1f;

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
            if(health < 0f)//if health has not been restored using the Restore Function down below
                health = GetComponent<BaseStats>().GetStat(Stat.Health); 
        }

        public void TakeDamage(float damage, GameObject instigator)
        {
            health = Mathf.Max(health - damage, 0);
            
            if (health <= 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }


        public float GetHealthPercentage()
        {
            return (health / GetComponent<BaseStats>().GetStat(Stat.Health)) * 100;
        }

        private void Die()
        {
            if (isDead) return;

            if (anim == null)
            {
                Debug.LogError($"{name}'s animator does not appear to be assigned, re-aquiring it");
                anim = GetComponent<Animator>();
            }
            if (actionScheduler == null)
            {
                Debug.LogError($"{name}'s action scheduler not assigned, re-aquiring it");
                actionScheduler = GetComponent<ActionScheduler>();
            }
                
            isDead = true;
            anim.SetTrigger(DeathTrigger);
            actionScheduler.CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience instigatorExperience = instigator.GetComponent<Experience>();

            if (instigatorExperience == null) return;
            instigatorExperience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
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

