using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///   An interactable object can be activated by certain command.
///   This class handles the actions of an activated object.
/// </summary>
public class InteractableObject : MonoBehaviour
{
    #region Fields

    public ItemClass data;

    [SerializeField] private Player player;
    [SerializeField] private UnityEvent @event;
    [SerializeField] private UnityEvent information;
    [SerializeField] private bool isTriggeredByCommand = false;
    [SerializeField] private float distanceToActivate = 5;
    [SerializeField] private AudioClip itemAudioClip;
    private TextMeshProUGUI _playerText;

    #endregion

    #region MonoBehaviour

    /// <summary>
    /// Start is acquiring the players text bar (screen top).
    /// </summary>
    private void Start()
    {
        _playerText = GameObject.FindWithTag("PlayerText").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Check if the item is close enough when clicked. If so, it loads it as target.
    /// </summary>
    private void OnMouseDown()
    {
        var itemPosition = transform.position;
        var playerPosition = player.transform.position;
        var distance = Vector2.Distance(
            itemPosition,
            playerPosition);
        if (distance < distanceToActivate || GameManager.GetCommand() == Command.WalkTo)
        {
            if (isTriggeredByCommand)
            {
                GameManager.LoadInteractionTarget(this);
            }
            else
                ActivateEvent(true);
        }
    }

    #endregion

    #region Methods

    /// <summary>
    ///   Activate the interactable object event.
    /// </summary>
    /// <param name="withSound">Determines wheter to play sound during activation.</param>
    public virtual void ActivateEvent(bool withSound)
    {
        @event.Invoke();
        if (itemAudioClip && withSound)
            MusicManager.PlayEffect(itemAudioClip);
    }

    /// <summary>
    ///   True if the item is triggered by a command. If it can be clicked to be activated
    ///   return false.
    /// </summary>
    public bool IsTriggeredByCommand()
    {
        return isTriggeredByCommand;
    }

    /// <summary>
    ///   Enables open and close of objects.
    /// </summary>
    /// <example> Doors, faucet in the kitchen, mailbox in the front yard, etc.</example>
    public void OpenCloseAction()
    {
        Command command = GameManager.GetCommand();
        if (command == Command.Open)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (command == Command.Close)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    
    /// <summary>
    /// Activate the information event, which is triggered from the GameManager by the What is command.
    /// </summary>
    public void ActivateInformation()
    {
        information.Invoke();
    }

    #endregion
}