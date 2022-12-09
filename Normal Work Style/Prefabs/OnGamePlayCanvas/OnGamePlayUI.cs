using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace GS.FanstayWorld2D.UI
{
    public class OnGamePlayUI : UIPanel
    {
        public static OnGamePlayUI Instance { get; private set; }
        private Canvas canvas;

        [Header("profile")]
        [SerializeField] private Image profilePic;

        [Header("Health & Magic Bar")]
        [SerializeField] private Image healthBar;
        [SerializeField] private Text healthText;
        [SerializeField] private Image magicBar;
        [SerializeField] private Text magicText;

        [Header("Low Health Warning")]
        [SerializeField] private Image lowHealthWariningImg;
        private bool enableLowHealthWarning = false;

        protected override void OnAwakeCall()
        {
            base.OnAwakeCall();
            Init();
            canvas = GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
        }
        private void Init()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else Destroy(this);
        }

        // Call that function to Set UI Camera
        public void SetUICamera(Camera cam = null)
        {
            canvas.worldCamera = cam == null ? Camera.main : cam;
        }


        # region Health and Magic

        public void UpdateHealthUI(int currentHealth, int maxHealth, float lowHealthPercentage = 0.1f)
        {
            float healthPercent = (float)currentHealth / (float)maxHealth;

            healthBar.fillAmount = healthPercent;

            healthText.text = $"{currentHealth}/{maxHealth}";

            if(healthPercent <= lowHealthPercentage && !enableLowHealthWarning)
            {
                enableLowHealthWarning = true;
                // Enable Low health warining
                LowHealthWariningAnimation();
            }
            else if(healthPercent > lowHealthPercentage || healthPercent <= 0f)
            {
                enableLowHealthWarning = false;
            }

        }

        public void UpdateMagicUI(int currentMagic, int maxMagic)
        {
            float magicPercent = (float)currentMagic / (float)maxMagic;
            magicBar.fillAmount = magicPercent;

            magicText.text = $"{currentMagic}/{maxMagic}";
        }

        public void LowHealthWariningAnimation()
        {
            lowHealthWariningImg.DOFade(0.2f, 0.5f).OnComplete(()=>
            {
                lowHealthWariningImg.DOFade(0,0.5f).OnComplete(()=>
                {
                    if(enableLowHealthWarning) LowHealthWariningAnimation();
                });
            });
        }

        #endregion
    
        #region  Update Profile

        public void UpdateProfilePic(Sprite pic)
        {
            profilePic.sprite = pic;
        }

        #endregion
    
    }
}