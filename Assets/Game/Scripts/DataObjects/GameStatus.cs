using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
///   Game Status save all progress in the current game.
/// </summary>
[CreateAssetMenu(fileName = "new Game Status")]
public class GameStatus : ScriptableObject
{
    #region Fields

        public Dictionary<int, bool> ActivationTable = new Dictionary<int, bool>();
        
    #endregion

    #region Methods

    public void AddToActivationTable(int key, bool value)
        {
            ActivationTable[key] = value;
        }
    
    public void ResetActivationTable()
    {
        ActivationTable = new Dictionary<int, bool>();
    }

    #endregion
    
}
