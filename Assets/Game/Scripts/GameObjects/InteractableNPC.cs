using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///   An interactable object can be activated by certain command.
///   This class handles the actions of an activated object.
/// </summary>
public class InteractableNPC : MonoBehaviour
{
    #region Fields
    
    [SerializeField] private Player player;
    [SerializeField] private UnityEvent @event;
    [SerializeField] private UnityEvent information;
    [SerializeField] private float distanceToActivate = 7;
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
        if (distance < distanceToActivate)
        {
            if (GameManager.GetCommand() == Command.WhatIs)
                ActivateInformation();
            
            else
                ActivateEvent();
        }
    }

    #endregion

    #region Methods

    /// <summary>
    ///   Activate the interactable object event.
    /// </summary>
    protected virtual void ActivateEvent()
    {
        @event.Invoke();
    }

    /// <summary>
    /// Activate the information event, which is triggered from the GameManager by the What is command.
    /// </summary>
    private void ActivateInformation()
    {
        information.Invoke();
    }

    #endregion
}