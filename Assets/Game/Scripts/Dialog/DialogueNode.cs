using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Dialogue Line")]
public class DialogueNode: ScriptableObject
{
    public string charName;
    public Sprite charImage;
    
    [TextArea(3, 10)]
    public string sentence;

    public DialogueNode next;
}
