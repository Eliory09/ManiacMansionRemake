using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
