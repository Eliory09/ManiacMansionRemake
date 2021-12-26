using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   Activate the interior of a container (like drawer or fridge).
/// </summary>
public class ActivateInterior : MonoBehaviour
{
    #region Methods

    public void Activate(GameObject obj)
    {
        Command command = GameManager.GetCommand();
        if (command == Command.Open)
        {
            obj.SetActive(true);    
        }
        else if (command == Command.Close)
        {
            obj.SetActive(false);    
        }
    }

    #endregion
    
}
