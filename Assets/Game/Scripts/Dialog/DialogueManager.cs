using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image charImage;

    public Animator animator;
    
    public Queue<string> sentences;
    private static DialogueManager _shared;
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");
    private DialogueNode _currentDialogueNode;

    void Start()
    {
        _shared = this;
        _shared.sentences = new Queue<string>();
    }

    public static void StartDialogue(DialogueNode firstDialogueNode)
    {
        _shared.animator.SetBool(IsOpen, true);
        _shared._currentDialogueNode = firstDialogueNode;

        DisplayNextSentence();
    }

    public static void DisplayNextSentence()
    {
        if (_shared._currentDialogueNode == null)
        {
            EndDialog();
            return;
        }

        _shared.nameText.text = _shared._currentDialogueNode.charName;
        _shared.dialogueText.text = _shared._currentDialogueNode.sentence;
        _shared.charImage.sprite = _shared._currentDialogueNode.charImage;
        _shared._currentDialogueNode = _shared._currentDialogueNode.next;
    }
    
    private static void EndDialog()
    {
        _shared.animator.SetBool(IsOpen, false);
        Debug.Log("End of conversation");
    }
}
