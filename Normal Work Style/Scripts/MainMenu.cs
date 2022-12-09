using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GS.FanstayWorld2D.UI
{
    public class MainMenu : UIPanel
    {
        [Header("Buttons")]
        [SerializeField] private Button resumeBtn;
        [SerializeField] private Button statesBtn, profileBtn, storeBtn, settingsBtn, exitBtn;

        [Header("Main Menu Panels")]
        [SerializeField] private UIPanel statesPanel;
        [SerializeField] private UIPanel profilePanel;
        [SerializeField] private UIPanel storePanel;
        [SerializeField] private UIPanel settingsPanel;

        private UIPanel activePanel;

        #region Ovveride Base Func
        protected override void OnAwakeCall()
        {
            base.OnAwakeCall();
        }

        protected override void OnStartCall()
        {
            base.OnStartCall();
            activePanel = statesPanel;

            InitBtns();     //Init All Main Menu Buttons
        }

        protected override void OnUpdateCall()
        {
            base.OnUpdateCall();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ShowPanel();
                Time.timeScale = 0f;
            }
        }

        protected override void OnShowPanel()
        {
            base.OnShowPanel();
            UpdateActivePanel(statesPanel, activePanel);
        }
        #endregion


        private void InitBtns()
        {
            // Resume Btn
            resumeBtn.onClick.AddListener(() =>
            {
                HidePanel();
                Time.timeScale = 1f;
            });

            statesBtn.onClick.AddListener(() => UpdateActivePanel(statesPanel, activePanel));
            profileBtn.onClick.AddListener(() => UpdateActivePanel(profilePanel, activePanel));
            storeBtn.onClick.AddListener(() => UpdateActivePanel(storePanel, activePanel));
            settingsBtn.onClick.AddListener(() => UpdateActivePanel(settingsPanel, activePanel));

            // Exit Btn
        }


        #region Panel Activation
        private void UpdateActivePanel(UIPanel nextPanel, UIPanel currentPanel)
        {
            if (nextPanel != currentPanel)
            {
                DeActivatePanel(currentPanel);
                ActivatePanel(nextPanel);
                activePanel = nextPanel;
            }
        }

        private void ActivatePanel(UIPanel panel)
        {
            panel.ShowPanel();
        }

        private void DeActivatePanel(UIPanel panel)
        {
            panel.HidePanel();
        }

        #endregion

    }
}