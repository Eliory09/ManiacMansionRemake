using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MechPlayer2 : MonoBehaviour
{
    enum Directions
    {
        Up = 180,
        Right = -90,
        Down = 0,
        Left = 90
    }
    
    
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Directions startDirection;
    [SerializeField] private float clickOffsetYAxis = 0;

    private Vector2 _followSpot;
    private NavMeshAgent _agent;
    private Vector2 _stuckDistanceCheck;
    private Queue<Vector2> _cornersQueue;

    private void Awake()
    {
        _followSpot = transform.position;
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _animator.SetFloat("Angle", (int) startDirection);
        _agent.angularSpeed = 0;
    }

    void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            _followSpot = new Vector2(mousePosition.x, mousePosition.y + clickOffsetYAxis);
            SetPlayerDestination(mousePosition);
        }
        _agent.SetDestination(new Vector3(_followSpot.x, _followSpot.y, transform.position.z));
        UpdateAnimation();
        AdjustSortingLayer();
    }
    
    private void UpdateAnimation()
    {
        var distance = Vector2.Distance(transform.position, _followSpot);
        if (Vector2.Distance(_stuckDistanceCheck, transform.position) == 0)
        {
            _animator.SetFloat("Distance", 0f);
            return;
        }

        _animator.SetFloat("Distance", distance);
        if (distance > 0.01)
        {
            var position = transform.position;
            var direction = position - new Vector3(_followSpot.x, _followSpot.y, position.z);
            var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            _animator.SetFloat("Angle", angle);
            _stuckDistanceCheck = position;
        }
    }

    private void AdjustSortingLayer()
    {
        _spriteRenderer.sortingOrder = (int) (transform.position.y * -100);
    }
    
    private void SetPlayerDestination(Vector3 mousePosition)
    {
        _followSpot = new Vector2(mousePosition.x, mousePosition.y + clickOffsetYAxis);
        NavMeshPath path = new NavMeshPath();
        _agent.CalculatePath(new Vector3(_followSpot.x, _followSpot.y, transform.position.z), path);
        _cornersQueue = new Queue<Vector2>();
        foreach (var pathCorner in path.corners)
        {
            print(pathCorner);
            _cornersQueue.Enqueue(new Vector2(pathCorner.x, pathCorner.y));
        }
    }
}