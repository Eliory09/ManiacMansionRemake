using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class MechInteractableObject : MonoBehaviour
{
    public MechPlayer player;
    public TextMeshProUGUI commandText;
    public UnityEvent @event;
    public ItemClass data;
    public bool isTriggeredByCommand = false;


    public virtual void ActivateEvent()
    {
        @event.Invoke();
    }

    public bool IsTriggeredByCommand()
    {
        return isTriggeredByCommand;
    }

    private void OnMouseDown()
    {
        var itemPosition = transform.position;
        var playerPosition = player.transform.position;
        var distance = Vector2.Distance(
            itemPosition,
            playerPosition);
        if (distance < 3)
        {
            if (isTriggeredByCommand)
            {
                print("now loading interaction target.");
                MechGameManager.LoadInteractionTarget(this);
            }
            else
                ActivateEvent();
        }
    }
}
