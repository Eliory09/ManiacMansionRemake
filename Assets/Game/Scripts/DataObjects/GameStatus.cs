using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
///   Game Status save all progress in the current game.
/// </summary>
[CreateAssetMenu(fileName = "new Game Status")]
public class GameStatus : ScriptableObject
{
    #region Fields

        public Dictionary<int, bool> ActivationTable = new Dictionary<int, bool>();
        public HashSet<NPC> npcs = new HashSet<NPC>();

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

    public void ResetNPC()
    {
        foreach (var npc in npcs)
        {
            npc.Reset();
        }
        npcs.Clear();
    }

    public void AddNPC(NPC npc)
    {
        if (npcs.Any(storedNpc => storedNpc.GetType() == npc.GetType()))
        {
            return;
        }

        npcs.Add(npc);
    }

    #endregion
    
}
