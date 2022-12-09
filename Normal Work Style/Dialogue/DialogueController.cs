using System.Collections;
using System.Collections.Generic;
using GS.FanstayWorld2D;
using UnityEngine;

namespace GS.Dialogue
{
    [RequireComponent(typeof(DialogueScript))]
    public class DialogueController : MonoBehaviour
    {
        public static DialogueController Instance { get; private set; }
        DialogueScript view;

        private DialogueData currentData;
        private Sprite playerImg;

        #region Unity Func
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this);
            }
        }
        private void OnEnable()
        {
            view = GetComponent<DialogueScript>();

            view.OnNextButtonPressed += UpdateAction;
            view.OnSkipButtonPressed += SkipConversation;

            GameData.OnLoadData += OnDataUpdate;
        }

        private void OnDiable()
        {
            view.OnNextButtonPressed -= UpdateAction;
            view.OnSkipButtonPressed -= SkipConversation;

            GameData.OnLoadData -= OnDataUpdate;
        }
        #endregion

        private void OnDataUpdate(SelectedStoreData data)
        {
            playerImg = data.Outfit.generalInfo.protraitImg;
        }

        //flags
        private int currentDataSegments, currentSegmentSentences, segmentCounter, sentenceCounter;
        public void StartConversation(DialogueData data)
        {

            this.currentDataSegments = data.segments.Count;
            this.currentSegmentSentences = data.segments[0].sentences.Count;
            segmentCounter = 0;
            sentenceCounter = 0;
            currentData = data;

            SpeakerInfo speakerOne = data.segments[0].speakerInfo;
            SpeakerInfo speakerTwo = data.segments[1].speakerInfo;

            var speakerImgOne = speakerOne.speakerType == SpeakerType.Player && playerImg != null ? playerImg : speakerOne.speakerImg;
            var speakerImgTwo = speakerTwo.speakerType == SpeakerType.Player && playerImg != null ? playerImg : speakerTwo.speakerImg;

            view.UpdateSpeakerInfo(speakerImgOne, speakerOne.speakerName, speakerOne.speakerType, data.segments[0].dialogueBoxSide);
            view.UpdateSpeakerInfo(speakerImgTwo, speakerTwo.speakerName, speakerTwo.speakerType, data.segments[1].dialogueBoxSide);

            view.DisplaySpeech("", DialogueBoxSide.DialogueBox_Left);
            view.DisplaySpeech("", DialogueBoxSide.DialogueBox_Right);

            view.StartDialogueAnimation(() =>
            {
                // Deliver First Sentence Here
                UpdateAction();
            });
        }

        public void UpdateAction()
        {
            Debug.Log($"UpdateAction()");
            if (segmentCounter < currentDataSegments)
            {
                if (sentenceCounter < currentSegmentSentences)
                {
                    string sentence = currentData.segments[segmentCounter].sentences[sentenceCounter];
                    DialogueBoxSide dialogueBoxSide = currentData.segments[segmentCounter].dialogueBoxSide;

                    Debug.Log($"Call view.DisplaySpeech(sentence, dialogueBoxSide);");
                    view.DisplaySpeech(sentence, dialogueBoxSide);

                    sentenceCounter++;
                }
                else
                {
                    sentenceCounter = 0;
                    segmentCounter++;

                    if (segmentCounter < currentDataSegments)
                    {
                        currentSegmentSentences = currentData.segments[segmentCounter].sentences.Count;

                        SpeakerInfo speakerOne = currentData.segments[segmentCounter].speakerInfo;
                        var speakerImgOne = speakerOne.speakerType == SpeakerType.Player && playerImg != null ? playerImg : speakerOne.speakerImg;

                        view.UpdateSpeakerInfo(speakerImgOne, speakerOne.speakerName, speakerOne.speakerType, currentData.segments[segmentCounter].dialogueBoxSide);
                    }

                    UpdateAction();
                }
            }
            else
            {
                // End of Conversation
                view.EndDialogueAnimation();
            }
        }

        public void SkipConversation()
        {
            view.EndDialogueAnimation();
        }
    }
}