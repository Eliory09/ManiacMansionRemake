using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MechGameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI commandText;

    private static MechGameManager _shared;
    private List<int> _phrase = new List<int>();
    private bool _commandLoaded;
    private MechCommand _command;
    private ItemClass _itemToActivate;
    private MechInteractableObject _interactableToActivate;
    private MechItemUI _interactionItem;
    void Awake()
    {
        _shared = this;
        _shared._command = MechCommand.None;
    }

    private void Update()
    {
        if( Input.GetMouseButtonDown(0) )
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (!hit && !MechUI.IsPointerOverUIElement())
            {
                ResetInteraction();
            }
        }
    }

    public static void LoadCommand(MechCommand command)
    {
        ResetInteraction();
        _shared._phrase.Add((int) command);
        print("command loaded: " + command);
        _shared._commandLoaded = true;
        _shared._command = command;
        PrintPhrase();
        ChangeInteractionTextUI();
    }

    private static void PrintPhrase()
    {
        foreach (var num in _shared._phrase)
        {
            print(num);
        }
    }

    public static void LoadInteractedItem(MechItemUI item)
    {
        if (_shared._commandLoaded)
        {
            _shared._phrase.Add(item.Data.GetId());
            _shared._interactionItem = item;
            print("interacted item loaded: " + item);
            PrintPhrase();
            ChangeInteractionTextUI();
            if (_shared._phrase.Count != 3) return;
            ActivateInteraction();
            ResetInteraction();
        }
        print("failed");
        print(_shared._commandLoaded);
        print(_shared._command);
        PrintPhrase();
    }
    
    public static void LoadInteractionTarget(MechInteractableObject item)
    {
        if (item.gameObject.CompareTag("Consumable") && !item.IsTriggeredByCommand())
        {
            item.ActivateEvent();
            ResetInteraction();
            return;
        }
        
        if (_shared._commandLoaded)
        {
            _shared._phrase.Add(item.data.GetId());
            _shared._itemToActivate = item.data;
            _shared._interactableToActivate = item;
            print("Interaction item loaded: " + item.data.GetTitle());
            PrintPhrase();
            ChangeInteractionTextUI();
            if(ActivateInteraction())
                ResetInteraction();
            print(_shared._commandLoaded);
        }
        else
        {
            print("failed");
            ResetInteraction();
        }
    }

    public static void ResetInteraction()
    {
        print("is reseting");
        _shared._commandLoaded = false;
        _shared._phrase.Clear();
        _shared._itemToActivate = null;
        _shared._interactableToActivate = null;
        _shared._command = MechCommand.None;
        _shared._interactionItem = null;
        ChangeInteractionTextUI();
    }

    public static bool ActivateInteraction()
    {
        print("ActivateInteraction started...");
        print(_shared._itemToActivate.GetInteractionType().ToString());
        if (_shared._itemToActivate.IsCommandLegal(_shared._phrase))
        {
            print("Checked and the command is legal.");
            _shared._interactableToActivate.ActivateEvent();
            if (_shared._interactionItem)
            {
                MechInventory.RemoveItem(_shared._interactionItem.Data);
            }
            return true;
        }
        print("failed");
        return false;
    }
    
    public static void ChangeInteractionTextUI()
    {
        string text = "";
        switch (_shared._commandLoaded)
        {
            case false:
                _shared.commandText.text = text;
                return;
            case true:
                text += _shared._command.ToString();
                break;
        }

        if (_shared._itemToActivate && _shared._interactionItem)
        {
            text += HandleTextUIChange3Parameters();
        }
        else if (_shared._itemToActivate || _shared._interactionItem)
        {
            text += HandleTextUIChange2Parameters();
        }

        _shared.commandText.text = text.ToLower().FirstCharacterToUpper();
    }

    private static string HandleTextUIChange3Parameters()
    {
        var text = _shared._itemToActivate.GetInteractionType() switch
        {
            ItemClass.InteractionOption.CommandTargetItem => 
                $" {_shared._itemToActivate.GetTitle()} with {_shared._interactionItem}",
            ItemClass.InteractionOption.CommandItemTarget => 
                $" {_shared._interactionItem.Data.GetTitle()} on {_shared._interactionItem}",
            _ => throw new ArgumentOutOfRangeException()
        };
        return text;
    }

    private static string HandleTextUIChange2Parameters()
    {
        var text = "";
        if (_shared._interactionItem && !_shared._itemToActivate)
        {
            return $" {_shared._interactionItem.Data.GetTitle()} on";
        }
        text = _shared._itemToActivate.GetInteractionType() switch
        {
            ItemClass.InteractionOption.CommandTargetItem => $" {_shared._itemToActivate.GetTitle()} with",
            ItemClass.InteractionOption.CommandTarget => $" {_shared._itemToActivate.GetTitle()}",
            _ => throw new ArgumentOutOfRangeException()
        };
        return text;
    }
}
