using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///   Activates dark room light once it the light switch is turned on.
/// </summary>
public class LightRoom : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] renderers;
    private static HashSet<int> _litSceneIndexes = new HashSet<int>();

    private void Awake()
    {
        if (_litSceneIndexes.Contains(SceneManager.GetActiveScene().buildIndex)) return;
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

        _litSceneIndexes.Add(SceneManager.GetActiveScene().buildIndex);
    }
}
