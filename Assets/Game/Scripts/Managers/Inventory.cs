using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Inventory manager. Stores items and interacts with UI.
/// </summary>
public class Inventory : MonoBehaviour
{
    #region Fields

    private List<ItemClass> _items = new List<ItemClass>();
    private static Inventory _shared;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        if (!_shared)
        {
            _shared = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Methods

    public static void AddItem(ItemClass item)
    {
        _shared._items.Add(item);
        ItemsUIManager.UpdateItemsUI();
    }
    
    public static void RemoveItem(ItemClass item)
    {
        // Need to check if item is in inventory.
        _shared._items.Remove(item);
        ItemsUIManager.UpdateItemsUI();
    }

    public static List<ItemClass> GetItems()
    {
        return _shared._items;
    }

    #endregion
    
}
