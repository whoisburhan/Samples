using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GS.Dialogue;

public class TestDialogue : MonoBehaviour
{
    [SerializeField] private DialogueData conversation;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            DialogueController.Instance.StartConversation(conversation);
        }
    }
}
