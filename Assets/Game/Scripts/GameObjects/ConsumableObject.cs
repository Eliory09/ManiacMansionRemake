using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   Interactable object that can be consumed/carried.
/// </summary>
public class ConsumableObject : InteractableObject
{
    #region Methods

    public override void ActivateEvent(bool withSound)
    {
        base.ActivateEvent(true);
        AddItemToInventory();
    }

    private void AddItemToInventory()
    {
        Inventory.AddItem(data);
        Destroy(gameObject);
    }

    #endregion
}