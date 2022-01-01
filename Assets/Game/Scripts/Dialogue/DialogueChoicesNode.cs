using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
