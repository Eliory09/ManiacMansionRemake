using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ReturnFromCutscene : MonoBehaviour
{
    [SerializeField] private UnityEvent @event;
    [SerializeField] private int currentSceneIndex;

    private void Awake()
    {
        @event.Invoke();
        SceneManager.UnloadSceneAsync(currentSceneIndex);
    }
}
