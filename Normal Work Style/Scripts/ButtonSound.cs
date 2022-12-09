using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GS.AudioAsset;

namespace GS.FanstayWorld2D.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonSound : MonoBehaviour
    {
        private Button button;
        private AudioSourceScript audioSource;
        [SerializeField] private List<AudioName> audioName;

        private void Awake()
        {
            button = GetComponent<Button>();
            audioSource = GetComponent<AudioSourceScript>();
            if (audioSource == null) audioSource = transform.parent.GetComponent<AudioSourceScript>();
            if (audioSource == null) audioSource = transform.parent.parent.GetComponent<AudioSourceScript>();
            if (audioSource == null) audioSource = transform.parent.parent.parent.GetComponent<AudioSourceScript>();
            if (audioSource == null) audioSource = transform.parent.parent.parent.parent.GetComponent<AudioSourceScript>();
        }

        private void Start()
        {
            button.onClick.AddListener(() => PlayAudio());
        }

        private void PlayAudio()
        {
            if (audioName.Count > 0)
            {
                int chosenAudio = Random.Range(0, audioName.Count);
                audioSource.Play(AudioManager.Instance.GetAudioClip(audioName[chosenAudio]));
            }
        }
    }
}