using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
    public ItemClass Data { get; set; }
    
    public void Click()
    {
        if (transform.GetChild(0).GetComponent<TextMeshProUGUI>().text != "")
            GameManager.LoadInteractedItem(this);
    }
}
