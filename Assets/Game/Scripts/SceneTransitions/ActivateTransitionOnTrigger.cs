using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Activates a regular activation on trigger.
/// <example> Loading scenes without player teleportation, activating sound sources. </example>
/// </summary>
public class ActivateTransitionOnTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent @event;
    private void OnTriggerEnter2D(Collider2D other)
    {
        @event.Invoke();
    }
}
