using UnityEngine;
using UnityEngine.UI;

namespace GS.FanstayWorld2D.Enemy
{
    public class HealthBarBehaviour : MonoBehaviour, IHealthBarBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Image fillImg;
        [SerializeField] private Color low;
        [SerializeField] private Color high;
        [SerializeField] private Vector3 offset;


        public void UpdateHealthInUI(float health, float maxHealth)
        {
            slider.gameObject.SetActive(health < maxHealth && health > 0);
            slider.value = health;
            slider.maxValue = maxHealth;

            fillImg.color = Color.Lerp(low, high, slider.normalizedValue);
        }

        private void Update()
        {
            slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
        }
    }
}