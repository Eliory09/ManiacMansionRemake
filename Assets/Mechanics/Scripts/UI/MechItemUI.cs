using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechItemUI : MonoBehaviour
{
    public ItemClass Data { get; set; }
    
    public void Click()
    {
        MechGameManager.LoadInteractedItem(this);
    }
}
