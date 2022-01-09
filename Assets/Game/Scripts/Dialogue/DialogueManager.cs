using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The dialogue manager handles all the dialogue data, generates the dialogue GUI and handles
/// choice taking and Actions executing during a conversation. 
/// </summary>
public class DialogueManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image charImage;
    [SerializeField] private Animator animator;
    [SerializeField] private Button choiceButton;
    [SerializeField] private GameObject choicesFrame;
    [SerializeField] private Button continueButton;
    [SerializeField] private Image background;
    [SerializeField] private Animator bgAnimator;
    private event Action RegularEventsOnContinue;
    private event Action ContinueAction;
    private static DialogueManager _shared;
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");
    private DialogueNode _nextDialogueNode;
    private DialogueNode _currentDialogueNode;
    private DialogueChoicesNode.Choice _currentChoice;
    private List<Button> choiceButtons;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        _shared = this;
        _shared.choiceButtons = new List<Button>();
    }

    #endregion


    #region Methods

    /// <summary>
    /// Starts a the dialogue logic and handles GUI.
    /// </summary>
    public static void StartDialogue(DialogueNode firstDialogueNode)
    {
        GameManager.FreezeSceneInDialogue();
        _shared.animator.SetBool(IsOpen, true);
        _shared.background.gameObject.SetActive(true);
        _shared._nextDialogueNode = firstDialogueNode;

        DisplayNextSentence();
    }

    /// <summary>
    /// Show the next dialogue text. Handles multiple choices buttons instantiation. 
    /// </summary>
    public static void DisplayNextSentence()
    {
        if (_shared._nextDialogueNode == null)
        {
            EndDialog();
            return;
        }
        
        _shared.nameText.text = _shared._nextDialogueNode.charName;
        CoroutineController.StopAll();
        CoroutineController.Start(TypeSentence(_shared._nextDialogueNode.sentence));
        _shared.charImage.sprite = _shared._nextDialogueNode.charImage;
        
        // Choices.
        if (_shared._nextDialogueNode is DialogueChoicesNode currNode)
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

            _shared._nextDialogueNode = null;
            _shared.continueButton.gameObject.SetActive(false);
        }
        
        // Regular sentence.
        else
        {
            _shared.continueButton.gameObject.SetActive(true);
            _shared._currentDialogueNode = _shared._nextDialogueNode;
            _shared._nextDialogueNode = _shared._nextDialogueNode.next;
        }
        
    }
    
    /// <summary>
    /// Handles the end of a dialogue. Closes the dialogue box and resets all the manager data.
    /// </summary>
    private static void EndDialog()
    {
        _shared.animator.SetBool(IsOpen, false);
        GameManager.UnfreezeSceneInDialogue();
        _shared.bgAnimator.SetBool(IsOpen, false);
        CoroutineController.Start(DisableBG(0.25f));
        _shared.RegularEventsOnContinue = null;
    }

    /// <summary>
    /// Update the manager with the player last decision, and handles GUI.
    /// </summary>
    public static void MakeChoice(DialogueChoicesNode.Choice choice)
    {
        SetCurrentChoice(choice);
        ActivateContinueAction();
        _shared._nextDialogueNode = choice.node;
        foreach (var button in _shared.choiceButtons)
        {
            Destroy(button.gameObject);
        }
        _shared.choiceButtons.Clear();
        _shared._currentChoice = null;
        DisplayNextSentence();
    }

    /// <summary>
    /// Returns the player's last choice.
    /// </summary>
    public static DialogueChoicesNode.Choice GetCurrentChoice()
    {
        return _shared._currentChoice;
    }

    /// <summary>
    /// Returns the player current DialogueNode (which currently appears on screen).
    /// </summary>
    public static DialogueNode GetCurrentNode()
    {
        return _shared._currentDialogueNode;
    }
    
    /// <summary>
    /// Set the player's last choice.
    /// </summary>
    public static void SetCurrentChoice(DialogueChoicesNode.Choice choice)
    {
        _shared._currentChoice = choice;
    }
    
    /// <summary>
    /// Set an action happens in a specific DialogueNode.
    /// This actions happens once in a while, unlike "Regular Actions On Continue Button" which
    /// happens on every press on continue.
    /// </summary>
    public static void SetActionToContinueButton(Action func)
    {
        _shared.ContinueAction = func;
    }

    /// <summary>
    /// Activates all the actions need to be executed in a continue button press (special and regular).
    /// </summary>
    public static void ActivateContinueAction()
    {
        _shared.RegularEventsOnContinue?.Invoke();
        if (_shared.ContinueAction == null) return;
        _shared.ContinueAction.Invoke();
        _shared.ContinueAction = null;
    }

    /// <summary>
    /// Add a regular event on continue press, which will happen on each continue press during the conversation.
    /// </summary>
    public static void AddRegularEventToContinueButton(Action func)
    {
        _shared.RegularEventsOnContinue += func;
    }
    
    /// <summary>
    /// Remove a regular event from the event listener.
    /// Regular action will happen on each continue press during the conversation.
    /// </summary>
    public static void RemoveRegularEventToContinueButton(Action func)
    {
        _shared.RegularEventsOnContinue -= func;
    }

    #endregion

    #region Coroutines

    /// <summary>
    /// Disables the dark background of a conversation.
    /// </summary>
    private static IEnumerator DisableBG(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _shared.background.gameObject.SetActive(false);
    }

    /// <summary>
    /// Sentence typing effect in the dialogue box.
    /// </summary>
    private static IEnumerator TypeSentence(string sentence)
    {
        _shared.dialogueText.text = "";
        foreach (var letter in sentence.ToCharArray())
        {
            _shared.dialogueText.text += letter;
            yield return null;
        }
    }


    #endregion

}
