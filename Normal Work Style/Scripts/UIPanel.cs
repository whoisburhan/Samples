using System;
using System.Security.Claims;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.FanstayWorld2D.UI
{
    public interface IPanel
    {
        public void ShowPanel();
        public void HidePanel();
    }

    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanel : MonoBehaviour, IPanel
    {
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            OnAwakeCall();
        }

        private void Start() => OnStartCall();
        private void Update() => OnUpdateCall();
        private void OnEnable() => OnEnableCall();
        private void OnDisable() => OnDisableCall();

        #region 
        protected virtual void OnAwakeCall() { }

        protected virtual void OnStartCall() { }
        protected virtual void OnUpdateCall() { }
        protected virtual void OnEnableCall() { }
        protected virtual void OnDisableCall() { }
        protected virtual void OnShowPanel() { }
        protected virtual void OnHidePanel() { }

        #endregion

        #region Hide & Show Panel
        public void ShowPanel()
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            OnShowPanel();
        }

        public void HidePanel()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            OnHidePanel();
        }

        #endregion
    }
}