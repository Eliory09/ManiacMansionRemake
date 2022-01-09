using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Handles the tentacle actions in the 3rd floor.
/// </summary>
public class Tentacle : MonoBehaviour, NPC
{
    #region Fields

    private enum TentacleStatus
    {
        Hungry,
        Thirsty,
        Fine
    }

    private static TentacleStatus Status = TentacleStatus.Hungry;

    [SerializeField] private Vector3 stopFollowingPosition;
    [SerializeField] private Player player;
    [SerializeField] private float xBoundary;
    [SerializeField] private Animator animator;
    [SerializeField] private float speed = 1;
    [SerializeField] private NavMeshSurface2d surface;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private GameStatus status;


    private Vector3 _followLocation;
    private bool _isFollowing = true;
    private static readonly int Distance = Animator.StringToHash("Distance");
    private HashSet<int> foodIds = new HashSet<int>() {12, 9, 13, 19, 20, 26, 23};
    private HashSet<int> drinksIds = new HashSet<int>() {18, 24};
    private int _waxFruitId = 33;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        if (Status == TentacleStatus.Fine)
        {
            EnableWalk();
            Destroy(gameObject);
        }

        else
        {
            Activate();
        }
    }

    private void Start()
    {
        surface.UpdateNavMesh(surface.navMeshData);
        GameManager.SetDangerMusic();
    }

    void Update()
    {
        if (!_isFollowing)
        {
            transform.position = Vector3.MoveTowards(transform.position, stopFollowingPosition, Time.deltaTime * speed);
            UpdateAnimation(new Vector2(stopFollowingPosition.x, stopFollowingPosition.y));
            return;
        }
        
        Vector3 playerLoc = player.transform.position;
        if (playerLoc.x > xBoundary)
        {
            playerLoc.x = xBoundary;
        }

        transform.position = Vector3.MoveTowards(transform.position, playerLoc, Time.deltaTime * speed);
        UpdateAnimation(new Vector2(playerLoc.x, playerLoc.y));
    }

    #endregion


    #region Methods
    
    public void Activate()
    {
        AddToGameStatus();
    }

    public void Reset()
    {
        Status = TentacleStatus.Hungry;
    }

    public void AddToGameStatus()
    {
        status.AddNPC(this);
    }
    
    private void StopFollowing()
    {
        _isFollowing = false;
    }
    
    private void UpdateAnimation(Vector2 followSpot)
    {
        var distance = Vector2.Distance(transform.position, followSpot);
        animator.SetFloat(Distance, distance);
    }

    public void UpdateTentacleStatus()
    {
        var item = GameManager.GetItem();
        var command = GameManager.GetCommand();

        if  (!item || command != Command.Give)
        {
            GameManager.ChangeInteractionTextPlayer("No! Give me what I want!", Color.blue);
            GameManager.ResetInteraction();
            return;
        }
        
        if (Status == TentacleStatus.Hungry)
        {
            if (item.GetId() == _waxFruitId)
            {
                GameManager.ChangeInteractionTextPlayer("Yumm! That's my favourite! Now I'm thirsty.", Color.blue);
                Inventory.RemoveItem(item);
                GameManager.ResetInteraction();
                Status = TentacleStatus.Thirsty;
            }
            else if (foodIds.Contains(item.GetId()))
            {
                foodIds.Remove(item.GetId());
                Inventory.RemoveItem(item);
                GameManager.ResetInteraction();
                GameManager.ChangeInteractionTextPlayer("Yumm! More!", Color.blue);
            }
        }
        else if (Status == TentacleStatus.Thirsty)
        {
            if (drinksIds.Contains(item.GetId()))
            {
                GameManager.ChangeInteractionTextPlayer("Thanks!", Color.blue);
                Inventory.RemoveItem(item);
                GameManager.ResetInteraction();
                Status = TentacleStatus.Fine;
                StopFollowing();
                EnableWalk();
                GameManager.SetDefaultMusic();
            }
        }
    }

    private void EnableWalk()
    {
        Destroy(obstacle);
        surface.UpdateNavMesh(surface.navMeshData);
        GameManager.SetDefaultMusic();
    }

    #endregion
}
