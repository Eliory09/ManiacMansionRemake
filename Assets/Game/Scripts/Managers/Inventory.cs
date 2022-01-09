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

    private List<ItemClass> _items;
    private static Inventory _shared;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        if (!_shared)
        {
            _shared = this;
            _shared._items = new List<ItemClass>();
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

    #endregion

    #region Methods

    public static void AddItem(ItemClass item)
    {
        _shared._items.Add(item);
        ItemsUIManager.UpdateItemsUI();
    }
    
    public static void RemoveItem(ItemClass item)
    {
        if (!_shared._items.Contains(item)) return;
        _shared._items.Remove(item);
        ItemsUIManager.UpdateItemsUI();
    }

    public static List<ItemClass> GetItems()
    {
        return _shared._items;
    }
    
    public static bool HasItem(int id)
    {
        foreach (var item in _shared._items)
        {
            if (item.GetId() == id)
            {
                return true;
            }
        }
        return false;
    }
    
    public static ItemClass GetItem(int id)
    {
        foreach (var item in _shared._items)
        {
            if (item.GetId() == id)
            {
                return item;
            }
        }

        return null;
    }

    public static void ResetInventory()
    {
        _shared._items = new List<ItemClass>();
        ItemsUIManager.UpdateItemsUI();
    }
    
    public static void DestroyMe()
    {
        Destroy(_shared.gameObject);
    }

    #endregion
    
}
