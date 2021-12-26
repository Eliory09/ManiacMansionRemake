using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechConsumableObject : MechInteractableObject
{

    public override void ActivateEvent()
    {
        base.ActivateEvent();
        AddItemToInventory();
    }

    private void AddItemToInventory()
    {
        MechInventory.AddItem(data);
        Destroy(gameObject);
    }
}
