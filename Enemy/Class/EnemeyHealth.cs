using UnityEngine;

namespace GS.FanstayWorld2D.Enemy
{
   public class EnemeyHealth : MonoBehaviour, IHealth
    {
        private IHealthBarBehaviour healthUI;

        private int health, maxHealth;

        public EnemeyHealth(HealthBarBehaviour healthUI)
        {
            this.healthUI = healthUI;
        }

        public void Init(HealthBarBehaviour healthUI)
        {
           this.healthUI = healthUI; 
        }

        public void SetHealth(int health, int maxHealth)
        {
            this.health = health;
            this.maxHealth = maxHealth;
            healthUI.SetHealth(health, maxHealth);
        }

        public bool UpdateHealth(int amount)
        {
            if (health > 0)
            {
                health -= amount;
                healthUI.SetHealth(health, maxHealth);
            }
            return health > 0;
        }
    }
}