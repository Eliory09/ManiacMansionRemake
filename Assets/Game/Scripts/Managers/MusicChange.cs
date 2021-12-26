using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicChange : MonoBehaviour
{
    [SerializeField] private UnityEvent @event;
    [SerializeField] private AudioClip defaultMusic;
    void Awake()
    {
        @event.Invoke();
    }

    private void OnDisable()
    {
        MusicManager.ChangeMusic(defaultMusic);
    }
}
