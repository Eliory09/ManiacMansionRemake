using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///   Represent data of doors.
/// </summary>
[CreateAssetMenu(fileName = "new Door Data")]
public class DoorData : ScriptableObject
{
    #region Fields

    [Serializable]
        public class DoorLocationData
        {
            public Vector2 location;
            public Player.Direction direction;
        }
    
        [SerializeField] private int doorID;
        [SerializeField] private DoorLocationData doorData;
        [SerializeField] private Vector3 cameraPosition;

    #endregion

    #region Methods

    public int GetID()
        {
            return doorID;
        }
    
        public DoorLocationData GetDoorData()
        {
            return doorData;
        }
        
        public Vector3 GetCameraPosition()
        {
            return cameraPosition;
        }

    #endregion
    
}
