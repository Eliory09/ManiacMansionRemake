using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// UI inventory manager.
/// </summary>
public class ItemsUIManager : MonoBehaviour
{
    #region Fields

    private static ItemsUIManager _shared;
    private int _page;
    private int _totalPages;

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

    private void Start()
    {
        ControlGame.AddManager(gameObject);
        _shared._page = 0;
        _shared._totalPages = ((Inventory.GetItems().Count) / 4) + 1;
        UpdateItemsUI();
    }

    #endregion

    #region Methods

    public static void UpdateItemsUI()
    {
        var i = 0;
        var items = Inventory.GetItems();
        var uiInventory = GameObject.FindWithTag("UI Inventory");
        foreach (Transform child in uiInventory.transform)
        {
            if (items.Count > 4 * _shared._page + i)
            {
                child.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = items[4 * _shared._page + i].GetTitle();
                child.gameObject.GetComponent<ItemUI>().Data = items[4 * _shared._page + i];
            }

            else
            {
                child.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "";
                child.gameObject.GetComponent<ItemUI>().Data = null;
            }
            i++;
        }

        _shared._totalPages = Math.Max(items.Count - 1, 0) / 4;
    }

    public static void NextPage()
    {
        if (_shared._page == _shared._totalPages)
            _shared._page = 0;
        else
            _shared._page++;
        UpdateItemsUI();
    }
    
    public static void PreviousPage()
    {
        if (_shared._page == 0)
            _shared._page = _shared._totalPages;
        else
            _shared._page--;
        UpdateItemsUI();
    }

    public static void SetDisable()
    {
        _shared.gameObject.SetActive(false);
    }
    
    public static void SetEnable()
    {
        _shared.gameObject.SetActive(true);
    }

    public static void DestroyMe()
    {
        Destroy(_shared.gameObject);
    }
    
    
    #endregion
}
