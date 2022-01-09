using System;
using UnityEngine;

/// <summary>
/// Dialogue data objects.
/// Stores information about the choice of the player and the DialogueNode
/// that it leads to in the conversation.
/// </summary>

[CreateAssetMenu(fileName = "new Dialogue Choices Line")]
public class DialogueChoicesNode : DialogueNode
{
    [Serializable]
    public class Choice
    {
        public string sentence;
        public DialogueNode node;
    }
    public Choice[] choices;
}
