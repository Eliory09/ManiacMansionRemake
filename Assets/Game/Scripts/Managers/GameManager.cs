using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


/// <summary>
/// Game Manager. Controls main interaction options of the user with the game.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameStatus gameStatus;
    [SerializeField] private AudioClip defaultMusic;
    [SerializeField] private AudioClip dangerMusic;
    private static Vector2 _nextPlayerLocation;
    private static Vector3 _nextCameraPosition;
    private static Player.Direction _nextPlayerDirection;
    private static bool _changeLocation = false;
    private float _timeToActivateEdinsonScene;
    private bool _isEdinsonTimerOn;
    private AudioClip _currentMusic;
    private TextMeshProUGUI _commandText;
    private Player _player;
    private static GameManager _shared;
    private List<int> _phrase = new List<int>();
    private bool _commandLoaded;
    private Command _command;
    private ItemClass _itemToActivate;
    private InteractableObject _interactableToActivate;
    private ItemUI _interactionItem;
    private bool _isFreeze;
    private int _seconds;
    private NavMeshSurface2d _surface;

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        if (!_shared)
        {
            _shared = this;
            _shared._command = Command.None;
            GameObject player = GameObject.FindWithTag("Player");
            if (player)
                _shared._player = player.GetComponent<Player>();
            GameObject commandText = GameObject.FindWithTag("CommandText");
            if (commandText)
                _shared._commandText = commandText.GetComponent<TextMeshProUGUI>();
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
            ResetInteraction();
            SetTimerForEdinsonMove();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        ControlGame.AddManager(gameObject);
    }

    private void Update()
    {
        if (_shared._isFreeze) return;
        if (_shared._isEdinsonTimerOn)
        {
            _shared._timeToActivateEdinsonScene -= Time.deltaTime;
            if (_shared._seconds - (int) _shared._timeToActivateEdinsonScene >= 1)
            {
                _shared._seconds = (int) _shared._timeToActivateEdinsonScene - 1;
            }
            if (_shared._timeToActivateEdinsonScene <= 0)
            {
                _shared._isFreeze = true;
                ActivateEdinsonScene();
            }
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (Camera.main is { })
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if (!hit && !UI.IsPointerOverUIElement())
                {
                    ResetInteraction();
                }
            }
        }
    }

    #endregion

    #region Methods

    public static void SetTimerForEdinsonMove()
    {
        _shared._timeToActivateEdinsonScene = Random.Range(300, 360);
        _shared._isEdinsonTimerOn = true;
        _shared._seconds = (int) _shared._timeToActivateEdinsonScene;
    }
    
    public static void SetPlayerNextLocation(DoorData.DoorLocationData data, Vector3 nextCameraPosition)
    {
        _nextPlayerLocation = data.location;
        _nextPlayerDirection = data.direction;
        _nextCameraPosition = nextCameraPosition;
        _changeLocation = true;
    }

    public static void LoadCommand(Command command)
    {
        ResetInteraction();
        _shared._phrase.Add((int) command);
        _shared._commandLoaded = true;
        _shared._command = command;
        ChangeInteractionTextUI();
    }
    
    public static void LoadInteractedItem(ItemUI item)
    {
        if (_shared._commandLoaded)
        {
            _shared._phrase.Add(item.Data.GetId());
            _shared._interactionItem = item;
            ChangeInteractionTextUI();
            if (_shared._phrase.Count != 3) return;
            bool isActivated = ActivateInteraction();
            if (_shared._itemToActivate == null) return;
            if (!isActivated)
            {
                ChangeInteractionTextPlayer("I can't do that.", Color.magenta);
            }
            ResetInteraction();
        }
    }

    public static void LoadInteractionTarget(InteractableObject item)
    {
        if (item.gameObject.CompareTag("Consumable") && !item.IsTriggeredByCommand())
        {
            item.ActivateEvent(true);
            ResetInteraction();
            return;
        }

        if (_shared._commandLoaded)
        {
            if (_shared._command == Command.WalkTo)
            {
                GameObject.FindWithTag("Player").GetComponent<Player>().WalkTo(
                    Camera.main.ScreenToWorldPoint(Input.mousePosition));
                ResetInteraction();
                return;
            }

            if (_shared._command == Command.WhatIs)
            {
                item.ActivateInformation();
                ResetInteraction();
                return;
            }

            _shared._phrase.Add(item.data.GetId());
            _shared._itemToActivate = item.data;
            _shared._interactableToActivate = item;
            ChangeInteractionTextUI();
            bool isActivated = ActivateInteraction();
            if (isActivated)
            {
                ResetInteraction();
                return;
            }
            if (_shared._interactionItem == null) return;
            ChangeInteractionTextPlayer("I can't do that.", Color.magenta);
            ResetInteraction();

        }
        else
        {
            ChangeInteractionTextPlayer(item.data.GetTitle(), Color.magenta);
            ResetInteraction();
        }
    }

    public static InteractableObject GetTarget()
    {
        return _shared._interactableToActivate;
    }
    
    public static ItemClass GetItem()
    {
        return !_shared._interactionItem ? null : _shared._interactionItem.Data;
    }

    public static Command GetCommand()
    {
        return _shared._command;
    }

    public static void ResetInteraction()
    {
        _shared._commandLoaded = false;
        _shared._phrase.Clear();
        _shared._itemToActivate = null;
        _shared._interactableToActivate = null;
        _shared._command = Command.None;
        _shared._interactionItem = null;
        ChangeInteractionTextUI();
    }

    public static void SetDefaultMusic()
    {
        if (!_shared._currentMusic || _shared._currentMusic.name != _shared.defaultMusic.name)
        {
            MusicManager.ChangeMusic(_shared.defaultMusic);
            _shared._currentMusic = _shared.defaultMusic;
        }
    }
    
    public static void SetDangerMusic()
    {
        if (!_shared._currentMusic || _shared._currentMusic.name != _shared.dangerMusic.name)
        {
            MusicManager.ChangeMusic(_shared.dangerMusic);
            _shared._currentMusic = _shared.dangerMusic;
        }
    }

    public static void FreezeSceneInDialogue()
    {
        _shared._surface = GameObject.FindWithTag("NavMesh").GetComponent<NavMeshSurface2d>();
        _shared._surface.enabled = false;
    }
    
    public static void UnfreezeSceneInDialogue()
    {
        _shared._surface.enabled = true;
    }

    private static void ActivateEdinsonScene()
    {
        SceneLoader loader = GameObject.FindWithTag("Loader").GetComponent<SceneLoader>();
        _shared._isEdinsonTimerOn = false;
        FreezeSceneInDialogue();
        loader.LoadSceneAdditive(17);
    }

    public static void UnFreezeEdinsonsScene()
    {
        _shared._isFreeze = false;
        ItemsUIManager.SetEnable();
        SetTimerForEdinsonMove();
    }

    private static void SetPlayerPosition(Vector2 newPosition, Player.Direction direction)
    {
        _shared._player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _shared._player.transform.position =
            new Vector3(newPosition.x, newPosition.y, -10);
        _shared._player.SetDirection(direction);
    }
    
    private static bool ActivateInteraction()
    {
        if (_shared._itemToActivate.IsCommandLegal(_shared._phrase))
        {
            _shared._interactableToActivate.ActivateEvent(true);
            if (_shared._interactionItem)
            {
                Inventory.RemoveItem(_shared._interactionItem.Data);
            }

            _shared.gameStatus.AddToActivationTable(
                _shared._itemToActivate.GetId(),
                _shared._interactableToActivate);
            return true;
        }
        return false;
    }

    public static void ChangeInteractionTextPlayer(string text, Color color)
    {
        CoroutineController.Start(_shared._player.ChangeTextPlayer(text, color));
    }

    private static void ChangeInteractionTextUI()
    {
        string text = "";
        switch (_shared._commandLoaded)
        {
            case false:
                _shared._commandText.text = text;
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

        _shared._commandText.text = text.ToLower().FirstCharacterToUpper();
    }

    private static string HandleTextUIChange3Parameters()
    {
        var text = _shared._itemToActivate.GetInteractionType() switch
        {
            ItemClass.InteractionOption.CommandTargetItem =>
                $" {_shared._itemToActivate.GetTitle()} with {_shared._interactionItem.Data.GetTitle()}",
            ItemClass.InteractionOption.CommandItemTarget =>
                $" {_shared._interactionItem.Data.GetTitle()} on {_shared._interactionItem.Data.GetTitle()}",
            ItemClass.InteractionOption.CommandTarget =>
                $" {_shared._interactionItem.Data.GetTitle()} on {_shared._interactionItem.Data.GetTitle()}",
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
    
    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_changeLocation)
        {
            SetPlayerPosition(_nextPlayerLocation, _nextPlayerDirection);
            if (Camera.main is { }) Camera.main.gameObject.transform.position = _nextCameraPosition;
            _changeLocation = false;
        }
    }
    
    public static void DestroyMe()
    {
        Destroy(_shared.gameObject);
    }

    #endregion
}