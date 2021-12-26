using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MechItemsUIManager : MonoBehaviour
{
    private static MechItemsUIManager _shared;
    private int _page;
    private int _totalPages;

    private void Awake()
    {
        _shared = this;
    }

    private void Start()
    {
        _shared._page = 0;
        _shared._totalPages = ((MechInventory.GetItems().Count) / 5) + 1;
        UpdateItemsUI();
    }
    
    public static void UpdateItemsUI()
    {
        var i = 0;
        var items = MechInventory.GetItems();
        foreach (Transform child in _shared.transform)
        {
            if (items.Count > 5 * _shared._page + i)
            {
                child.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = items[5 * _shared._page + i].GetTitle();
                child.gameObject.GetComponent<MechItemUI>().Data = items[5 * _shared._page + i];
            }

            else
            {
                child.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "";
                child.gameObject.GetComponent<MechItemUI>().Data = null;
            }
            i++;
        }
    }

    public static void NextPage()
    {
        if (_shared._page == _shared._totalPages)
            _shared._page = 1;
        else
            _shared._page++;
    }
    
    public static void PreviousPage()
    {
        if (_shared._page == 1)
            _shared._page = _shared._totalPages;
        else
            _shared._page--;
    }

}
