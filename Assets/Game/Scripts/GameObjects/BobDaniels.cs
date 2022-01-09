using System.Collections;
using UnityEngine;

/// <summary>
/// Handles the conversation details and specifications with Bob Daniels, the salesman at the dungeon.
/// </summary>
public class BobDaniels : MonoBehaviour, NPC
{
    #region Fields

    private enum State
    {
        FirstTalkNoMoney,
        SecondTalkNoMoney,
        AfterTalking
    }
    
    private static State _state = State.FirstTalkNoMoney;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite bobFront;
    [SerializeField] private GameObject qMark;
    [SerializeField] private DialogueNode secondDialogueDontHaveMoney;
    [SerializeField] private DialogueNode afterTalking;
    [SerializeField] private DialogueNode payChoiceWithMoney;
    [SerializeField] private DialogueNode payChoiceNoMoney;
    [SerializeField] private DialogueNode payItemChoiceDialogueNode;
    [SerializeField] private int moneyID;
    [SerializeField] private AudioClip itemSound;
    [SerializeField] private GameObject brick;
    [SerializeField] private GameStatus status;
    
    #endregion

    #region MonoBehaviour

    public void Awake()
    {
        AddToGameStatus();
        switch (_state)
        {
            case State.FirstTalkNoMoney:
                break;
            case State.SecondTalkNoMoney:
            case State.AfterTalking:
                spriteRenderer.sprite = bobFront;
                break;
        }
    }
    
    #endregion

    #region Methods

    /// <summary>
    /// Activates the conversation.
    /// </summary>
    public void Activate()
    {
        DialogueManager.AddRegularEventToContinueButton(CheckCurrentNode);
        switch (_state)
        {
            case State.FirstTalkNoMoney:
                spriteRenderer.sprite = bobFront;
                CoroutineController.Start(ActivateDialogue(1));
                break;
            case State.SecondTalkNoMoney:
                ActivateSecondDialogue();
                break;
            case State.AfterTalking:
                ActivateAfterTalking();
                break;
        }
    }

    /// <summary>
    /// Resets bob's conversation and status.
    /// </summary>
    public void Reset()
    {
        _state = State.FirstTalkNoMoney;
    }

    public void AddToGameStatus()
    {
        status.AddNPC(this);
    }

    /// <summary>
    /// Activates the dialogue with Bob after the player spoke to him once.
    /// </summary>
    private void ActivateSecondDialogue()
    {
        NPCDialogueTrigger trigger = gameObject.GetComponent<NPCDialogueTrigger>();
        trigger.firstDialogueNode = secondDialogueDontHaveMoney;
        trigger.TriggerDialogue();
    }

    /// <summary>
    /// Checks the current node the player is in during the conversation.
    /// </summary>
    private void CheckCurrentNode()
    {
        DialogueChoicesNode.Choice choice = DialogueManager.GetCurrentChoice();
        
        if (choice == null) return;
        if (choice.node == payChoiceWithMoney || choice.node == payChoiceNoMoney)
            CheckForItem(DialogueManager.GetCurrentChoice());
    }

    /// <summary>
    /// Check if the player has the item that is needed in order
    /// to reveal the secret of how to get out of the dungeon.
    /// </summary>
    private void CheckForItem(DialogueChoicesNode.Choice choice)
    {
        if (Inventory.HasItem(moneyID))
        {
            choice.node = payChoiceWithMoney;
            MusicManager.PlayEffect(itemSound);
            Inventory.RemoveItem(Inventory.GetItem(moneyID));
            _state = State.AfterTalking;
            brick.SetActive(true);
        }
        else
        {
            choice.node = payChoiceNoMoney;
        }
        DialogueManager.SetCurrentChoice(choice);
    }

    /// <summary>
    /// Activates the dialogue with Bob after the player revealed the secret.
    /// </summary>
    private void ActivateAfterTalking()
    {
        NPCDialogueTrigger trigger = gameObject.GetComponent<NPCDialogueTrigger>();
        trigger.firstDialogueNode = afterTalking;
        trigger.TriggerDialogue();
    }
    
    #endregion

    #region Coroutines

    /// <summary>
    /// Handles conversation activation.
    /// </summary>
    private IEnumerator ActivateDialogue(float seconds)
    {
        qMark.SetActive(true);
        yield return new WaitForSeconds(seconds);
        qMark.SetActive(false);
        gameObject.GetComponent<NPCDialogueTrigger>().TriggerDialogue();
        _state = State.SecondTalkNoMoney;
    }

    #endregion
    
    
}
