using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   Real time adjustment of the gameObject sorting layer, based on it's y-position.
/// </summary>
public class AdjustSortingLayer : MonoBehaviour
{
    #region Fields

    [SerializeField] private SpriteRenderer _spriteRenderer;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        _spriteRenderer.sortingOrder = (int) (transform.position.y * -100);
    }

    #endregion
}