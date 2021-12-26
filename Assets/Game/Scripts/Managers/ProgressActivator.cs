using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates progress of the current scene during gameplay.
/// </summary>
public class ProgressActivator : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameStatus gameStatus;
    [SerializeField] private InteractableObject[] registeredItems;
    private Dictionary<int, InteractableObject> _itemsDict;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        _itemsDict = new Dictionary<int, InteractableObject>();
        for (int i = 0; i < registeredItems.Length; i++)
        {
            _itemsDict[registeredItems[i].data.GetId()] = registeredItems[i];
        }
        
        foreach (var kv in gameStatus.ActivationTable)
        {
            if (_itemsDict.ContainsKey(kv.Key) && gameStatus.ActivationTable[kv.Key])
            {
                if(_itemsDict[kv.Key].CompareTag("Consumable"))
                {
                    Destroy(_itemsDict[kv.Key].gameObject);
                }
                else
                {
                    _itemsDict[kv.Key].ActivateEvent(false);
                }
            }
        }
    }

    #endregion
    
}
