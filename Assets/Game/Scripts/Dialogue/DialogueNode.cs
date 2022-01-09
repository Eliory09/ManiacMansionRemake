using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// DialogueNode is a part of the dialogue system, which is implemented as linked list
/// of DialogueNode objects, which contains data of the current sentence, character talking (and image) and
/// also the next conversation line.
/// </summary>
/// 
[CreateAssetMenu(fileName = "new Dialogue Line")]
public class DialogueNode: ScriptableObject
{
    public string charName;
    public Sprite charImage;
    
    [TextArea(3, 10)]
    public string sentence;

    public DialogueNode next;

    public string GetSentence()
    {
        return sentence;
    }
}
