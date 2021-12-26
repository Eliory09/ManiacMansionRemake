using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is used to reset the progress already saved when start playing.
/// </summary>
public class ResetProgress : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameStatus gameStatus;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("ProgressReset");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        gameStatus.ResetActivationTable();
    }

    #endregion
    
}
