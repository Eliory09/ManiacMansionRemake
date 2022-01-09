using UnityEngine;

/// <summary>
/// Activates an item.
/// Used to activate an item when it is not directly activated.
/// EXAMPLE: When a door is opened on the other side, it should stay open although
/// it wasn't directly opened from the other side.
/// </summary>
public class ActivateItem: MonoBehaviour
{
    public GameStatus status;
    
    /// <summary>
    /// Activate the item by updating the GameStatus scriptable object activation table.
    /// </summary>
    public void Activate(int itemID)
    {
        status.AddToActivationTable(itemID, true);
    }
}
