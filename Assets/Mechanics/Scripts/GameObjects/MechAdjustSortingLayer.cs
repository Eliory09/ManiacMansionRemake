using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechAdjustSortingLayer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer.sortingOrder = (int) (transform.position.y * -100);
    }
}
