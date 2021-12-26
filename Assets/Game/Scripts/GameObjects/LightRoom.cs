using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   Activates room light once it turned on.
/// </summary>
public class LightRoom : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] renderers;
    private static bool _isActivated = false;

    private void Awake()
    {
        if (_isActivated) return;
        foreach (var spriteRenderer in renderers)
        {
            spriteRenderer.color =
                spriteRenderer.gameObject.CompareTag("Player") ? new Color(0.3f, 0.3f, 0.3f) : Color.black;
        }
    }

    public void Activate()
    {
        foreach (var spriteRenderer in renderers)
        {
            spriteRenderer.color = Color.white;
        }

        _isActivated = true;
    }
}
