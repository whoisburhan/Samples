using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace GS.Dialogue
{
    public class DialogueScript : MonoBehaviour
    {
        #region Primary
        [Header("Primary")]
        [SerializeField] private CanvasGroup dialogueCanvasGroup;
        [SerializeField] private Button primarySkipButton;
        [SerializeField] private float animationDuration = 0.5f;
        #endregion

        #region  Dialogue Box 1
        [Header("Dialogue Box - 1")]
        [SerializeField] private Transform dialogueBox1;
        [Space]
        [SerializeField] private Transform dialogueBox1StartPos;
        [SerializeField] private Transform dialogueBox1EndPos;
        [Space]
        [SerializeField] private Image dialogueBox1SpeakerImg;
        [SerializeField] private Text dialogueBox1SpeakerName;
        [SerializeField] private Image dialgueBox1SpeakerNameHolder;
        [SerializeField] private Text dialogueBox1SpeakerSpeech;
        [SerializeField] private Button dialogueBox1SkipButton;
        [SerializeField] private Image dialogueBox1SkipButtonHint;
        #endregion

        #region  Dialogue Box 2
        [Header("Dialogue Box - 2")]
        [SerializeField] private Transform dialogueBox2;
        [Space]
        [SerializeField] private Transform dialogueBox2StartPos;
        [SerializeField] private Transform dialogueBox2EndPos;
        [Space]
        [SerializeField] private Image dialogueBox2SpeakerImg;
        [SerializeField] private Text dialogueBox2SpeakerName;
        [SerializeField] private Image dialgueBox2SpeakerNameHolder;
        [SerializeField] private Text dialogueBox2SpeakerSpeech;
        [SerializeField] private Button dialogueBox2SkipButton;
        [SerializeField] private Image dialogueBox2SkipButtonHint;
        #endregion

        public Action OnNextButtonPressed;
        public Action OnSkipButtonPressed;
        private void Start()
        {
            InitButtonFunc();
            primarySkipButton.gameObject.SetActive(false);
        }

        private void InitButtonFunc()
        {
            dialogueBox1SkipButton.onClick.AddListener(() => { OnNextButtonPressed?.Invoke(); });
            dialogueBox2SkipButton.onClick.AddListener(() => { OnNextButtonPressed?.Invoke(); });

            primarySkipButton.onClick.AddListener(() => { OnSkipButtonPressed?.Invoke(); });
        }

        #region Dialogue Canvas Animation

        public void ResetDialogueBoxPos()
        {
            dialogueBox1.position = dialogueBox1StartPos.position;
            dialogueBox2.position = dialogueBox2StartPos.position;
        }

        public void StartDialogueAnimation(Action action = null)
        {
            ResetDialogueBoxPos();

            dialogueCanvasGroup.alpha = 1f;
            dialogueCanvasGroup.blocksRaycasts = true;
            dialogueCanvasGroup.interactable = true;

            dialogueBox1.DOMove(dialogueBox1EndPos.position, animationDuration).OnComplete(() =>
            {
                action?.Invoke();
                primarySkipButton.gameObject.SetActive(true);
            });
            dialogueBox2.DOMove(dialogueBox2EndPos.position, animationDuration);
        }

        public void EndDialogueAnimation(Action action = null)
        {
            dialogueBox1.DOMove(dialogueBox1StartPos.position, animationDuration).OnComplete(() =>
            {
                dialogueCanvasGroup.alpha = 0f;
                dialogueCanvasGroup.blocksRaycasts = false;
                dialogueCanvasGroup.interactable = false;

                action?.Invoke();
                primarySkipButton.gameObject.SetActive(false);
            });
            dialogueBox2.DOMove(dialogueBox2StartPos.position, animationDuration);
        }

        #endregion

        #region  Speech Display
        string previousSpeech;
        Text previousSpeekerText;

        public void DisplaySpeech(string speech, DialogueBoxSide dialogueBoxSide)
        {
            switch (dialogueBoxSide)
            {
                case DialogueBoxSide.DialogueBox_Left:
                    DisplaySpeech(speech, dialogueBox1SpeakerSpeech);
                    break;
                case DialogueBoxSide.DialogueBox_Right:
                    DisplaySpeech(speech, dialogueBox2SpeakerSpeech);
                    break;
            }
        }
        private void DisplaySpeech(string speech, Text speekerText)
        {
            Debug.Log($"ENTER view.DisplaySpeech(sentence, dialogueBoxSide);");
            Debug.Log($"{speech}");
            if (previousSpeech != null)
                previousSpeekerText.text = previousSpeech;

            StopAllCoroutines();
            StartCoroutine(WriteSpeech(speech, speekerText));
        }

        private IEnumerator WriteSpeech(string speech, Text speakerText)
        {
            speakerText.text = "";

            previousSpeech = speech;
            previousSpeekerText = speakerText;

            foreach (char letter in speech.ToCharArray())
            {
                speakerText.text += letter;
                yield return null;
            }
        }

        #endregion

        #region Upadate Speaker

        public void UpdateSpeakerInfo(Sprite speakerImg, string speakerName, SpeakerType speakerType, DialogueBoxSide dialogueBoxSide)
        {
            UpdateSpeakerImg(speakerImg, dialogueBoxSide);
            UpdateSpeakerNameAndType(speakerName, speakerType, dialogueBoxSide);
        }

        private void UpdateSpeakerImg(Sprite speakerImg, DialogueBoxSide dialogueBoxSide)
        {
            switch (dialogueBoxSide)
            {
                case DialogueBoxSide.DialogueBox_Left:
                    dialogueBox1SpeakerImg.sprite = speakerImg;
                    break;
                case DialogueBoxSide.DialogueBox_Right:
                    dialogueBox2SpeakerImg.sprite = speakerImg;
                    break;
            }

        }

        private void UpdateSpeakerNameAndType(string speakerName, SpeakerType speakerType, DialogueBoxSide dialogueBoxSide)
        {
            switch (dialogueBoxSide)
            {
                case DialogueBoxSide.DialogueBox_Left:
                    dialogueBox1SpeakerName.text = speakerName;
                    UpdateSpeakerTypeHintBar(speakerType, dialgueBox1SpeakerNameHolder);
                    break;
                case DialogueBoxSide.DialogueBox_Right:
                    dialogueBox2SpeakerName.text = speakerName;
                    UpdateSpeakerTypeHintBar(speakerType, dialgueBox2SpeakerNameHolder);
                    break;
            }
        }

        private void UpdateSpeakerTypeHintBar(SpeakerType speakerType, Image speakerTypeHolder)
        {
            speakerTypeHolder.color = speakerType == SpeakerType.Player ? Color.green : speakerType == SpeakerType.NPC ? Color.white : Color.red; // red when SpeakerType == enemy 
        }

        #endregion

        private void Reset()
        {
            ResetDialogueBoxPos();

            dialogueBox1SpeakerImg.sprite = null;
            dialogueBox1SpeakerName.text = "";
            dialgueBox1SpeakerNameHolder.color = Color.white;
            dialogueBox1SpeakerSpeech.text = "";
            dialogueBox1SkipButton.gameObject.SetActive(false);
            dialogueBox1SkipButtonHint.gameObject.SetActive(false);

            dialogueBox2SpeakerImg.sprite = null;
            dialogueBox2SpeakerName.text = "";
            dialgueBox2SpeakerNameHolder.color = Color.white;
            dialogueBox2SpeakerSpeech.text = "";
            dialogueBox2SkipButton.gameObject.SetActive(false);
            dialogueBox2SkipButtonHint.gameObject.SetActive(false);
        }

    }
}