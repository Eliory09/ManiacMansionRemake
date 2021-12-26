using System;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///   Represent data of items (interactable and consumable).
/// </summary>
/// <param name="effect">Sound effect to play</param>
[CreateAssetMenu(fileName = "new Item Data")]
public class ItemClass : ScriptableObject
{
    #region Fields

    [Serializable]
    public class Interaction
    {
        public Command command;
        public int interactedItemId;
        public int targetItemId;
    }

    public enum InteractionOption
    {
        CommandTarget,
        CommandItemTarget,
        CommandTargetItem,
    };

    [SerializeField] private UnityEvent @event;
    [SerializeField] private InteractionOption interactionType;
    [SerializeField] private Interaction[] supportedInteractions;

    [SerializeField] private int id;
    [SerializeField] private string title;

    #endregion

    #region Methods

    public int GetId()
    {
        return id;
    }

    public string GetTitle()
    {
        return title;
    }

    public InteractionOption GetInteractionType()
    {
        return interactionType;
    }

    public bool IsCommandLegal(List<int> phrase)
    {
        switch (interactionType)
        {
            case InteractionOption.CommandTarget:
                foreach (var t in supportedInteractions)
                {
                    if (
                        phrase.Count == 2
                        && phrase[0] == (int) t.command
                        && phrase[1] == t.targetItemId
                    )
                        return true;
                }

                break;
            case InteractionOption.CommandItemTarget:
                foreach (var t in supportedInteractions)
                {
                    if (
                        phrase.Count == 3
                        && phrase[0] == (int) t.command
                        && phrase[1] == t.interactedItemId
                        && phrase[2] == t.targetItemId
                    )
                        return true;
                }

                break;
            case InteractionOption.CommandTargetItem:
                foreach (var t in supportedInteractions)
                {
                    if (
                        phrase.Count == 3
                        && phrase[0] == (int) t.command
                        && phrase[1] == t.targetItemId
                        && phrase[2] == t.interactedItemId
                    )
                        return true;
                }

                break;
        }

        return false;
    }

    public void ActivateEvent()
    {
        @event.Invoke();
    }

    #endregion
}