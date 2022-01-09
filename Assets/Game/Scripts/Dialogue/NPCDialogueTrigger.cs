using UnityEngine;

/// <summary>
/// Triggers the NPC dialogue.
/// </summary>
public class NPCDialogueTrigger : MonoBehaviour
{
    public DialogueNode firstDialogueNode;

    public void TriggerDialogue()
    {
        DialogueManager.StartDialogue(firstDialogueNode);
    }
}
