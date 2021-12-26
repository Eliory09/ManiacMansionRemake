using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PantryFloor : MonoBehaviour
{
    #region Fields
    
    private static bool _isActivated = false;
    [SerializeField] private Player player;
    
    #endregion

    #region MonoBehaviour

        /// <summary>
        ///   Handles pantry floor one-time actions.
        /// </summary>
        void Start()
        {
            if (_isActivated) return;
            player.Say("WHOOPS!");
            _isActivated = true;
        }

    #endregion
    
}
