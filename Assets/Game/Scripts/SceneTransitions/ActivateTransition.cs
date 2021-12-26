using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Activates a regular activation.
/// <example> Loading scenes without player teleportation, activating sound sources. </example>
/// </summary>
public class ActivateTransition : MonoBehaviour
{
    [SerializeField] private UnityEvent @event;

    private void Awake()
    {
        @event.Invoke();
    }
}
