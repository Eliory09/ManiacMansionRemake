using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image charImage;

    public Animator animator;

    public Button choiceButton;
    public GameObject choicesFrame;
    public Button continueButton;

    public Image background;
    public Animator bgAnimator;

    public Queue<string> sentences;
    private static DialogueManager _shared;
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");
    private DialogueNode _currentDialogueNode;
    private List<Button> choiceButtons;

    void Start()
    {
        _shared = this;
        _shared.choiceButtons = new List<Button>();
    }

    public static void StartDialogue(DialogueNode firstDialogueNode)
    {
        GameManager.FreezeSceneInDialogue();
        _shared.animator.SetBool(IsOpen, true);
        _shared.background.gameObject.SetActive(true);
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
        // _shared.dialogueText.text = _shared._currentDialogueNode.sentence;
        CoroutineController.StopAll();
        CoroutineController.Start(TypeSentence(_shared._currentDialogueNode.sentence));
        _shared.charImage.sprite = _shared._currentDialogueNode.charImage;
        if (_shared._currentDialogueNode is DialogueChoicesNode currNode)
        {
            foreach (var choice in currNode.choices)
            {
                Button button = Instantiate(_shared.choiceButton, Vector3.zero, Quaternion.identity);
                _shared.choiceButtons.Add(button);
                button.GetComponentInChildren<TextMeshProUGUI>().text = choice.sentence;
                button.transform.SetParent(_shared.choicesFrame.transform, false);
                button.gameObject.SetActive(true);
                button.GetComponent<ChoiceTrigger>().SetChoice(choice);
            }

            _shared._currentDialogueNode = null;
            _shared.continueButton.gameObject.SetActive(false);
        }
        else
        {
            _shared.continueButton.gameObject.SetActive(true);
            _shared._currentDialogueNode = _shared._currentDialogueNode.next;
        }
        
    }
    
    private static void EndDialog()
    {
        _shared.animator.SetBool(IsOpen, false);
        GameManager.UnfreezeSceneInDialogue();
        _shared.bgAnimator.SetBool(IsOpen, false);
        CoroutineController.Start(DisableBG(0.25f));
    }

    public static void MakeChoice(DialogueChoicesNode.Choice choice)
    {
        _shared._currentDialogueNode = choice.node;
        foreach (var button in _shared.choiceButtons)
        {
            Destroy(button.gameObject);
        }
        _shared.choiceButtons.Clear();
        DisplayNextSentence();
    }
    
    private static IEnumerator DisableBG(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _shared.background.gameObject.SetActive(false);
    }

    private static IEnumerator TypeSentence(string sentence)
    {
        _shared.dialogueText.text = "";
        foreach (var letter in sentence.ToCharArray())
        {
            _shared.dialogueText.text += letter;
            yield return null;
        }
    }
}
