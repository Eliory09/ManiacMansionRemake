using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Activates a door transition: teleportation and setting next player location.
/// </summary>
public class ActivateDoorTransition : MonoBehaviour
{
    #region Fields

    [SerializeField] private UnityEvent @event;
    [SerializeField] private Player player;
    [SerializeField] private float distanceFromDoor;
    [SerializeField] private DoorData doorData;

    #endregion

    #region MonoBehaviour

    private void OnMouseDown()
    {
        var distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= distanceFromDoor)
        {
            @event.Invoke();
            GameManager.SetPlayerNextLocation(doorData.GetDoorData(), doorData.GetCameraPosition());
        }
            
    }

    #endregion
    
}
