using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MechInventory : MonoBehaviour
{
    private List<ItemClass> _items = new List<ItemClass>();
    private static MechInventory _shared;

    private void Start()
    {
        _shared = this;
    }

    public static void AddItem(ItemClass item)
    {
        _shared._items.Add(item);
        MechItemsUIManager.UpdateItemsUI();
    }
    
    public static void RemoveItem(ItemClass item)
    {
        // Need to check if item is in inventory.
        _shared._items.Remove(item);
        MechItemsUIManager.UpdateItemsUI();
    }

    public static List<ItemClass> GetItems()
    {
        return _shared._items;
    }
}
