using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ChoiceTrigger used to set and make choices of the player
/// throughout the game during dialogues.  
/// </summary>
public class ChoiceTrigger : MonoBehaviour
{
    public DialogueChoicesNode.Choice choice;

    public void SetChoice(DialogueChoicesNode.Choice choice)
    {
        this.choice = choice;
    }

    public void MakeChoice()
    {
        DialogueManager.MakeChoice(choice);
    }
}
