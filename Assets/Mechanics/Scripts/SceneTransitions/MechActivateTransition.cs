using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class MechActivateTransition : MonoBehaviour
{
    [SerializeField] private UnityEvent _event;
    private void OnTriggerEnter2D(Collider2D other)
    {
        _event.Invoke();
    }
}
