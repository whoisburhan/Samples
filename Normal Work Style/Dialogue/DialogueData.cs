using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GS.Dialogue
{
    [CreateAssetMenu(fileName = "Conversation - 0", menuName = "GS/Dialogue", order = 1)]
    public class DialogueData : ScriptableObject
    {
        public List<Segment> segments;
    }

    [Serializable]
    public class Segment
    {
        public DialogueBoxSide dialogueBoxSide;
        public SpeakerInfo speakerInfo;
        public List<string> sentences;
    }

    [Serializable]
    public class SpeakerInfo
    {
        public Sprite speakerImg;
        public string speakerName;
        public SpeakerType speakerType;

    }

    public enum SpeakerType
    {
        Player = 0, NPC, Enemy
    }

    public enum DialogueBoxSide
    {
        DialogueBox_Left, DialogueBox_Right
    }

}