using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    public DialogueNode firstDialogueNode;

    public void TriggerDialogue()
    {
        DialogueManager.StartDialogue(firstDialogueNode);
    }
}
