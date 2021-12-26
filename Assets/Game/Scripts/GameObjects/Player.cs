using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;


/// <summary>
/// Represent the player.
/// Responsible for player movement and interaction.
/// </summary>
public class Player : MonoBehaviour
{
    #region Enums

    public enum Direction
    {
        Up = 180,
        Right = -90,
        Down = 0,
        Left = 90
    }

    #endregion

    #region Fields

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Direction startDirection;
    [SerializeField] private float clickOffsetYAxis = 0;

    private Vector2 _followSpot;
    private NavMeshAgent _agent;
    private Vector2 _stuckDistanceCheck;
    private TextMeshProUGUI _playerText;
    private static readonly int Distance = Animator.StringToHash("Distance");
    private static readonly int Angle = Animator.StringToHash("Angle");

    #endregion

    #region MonoBehaviour

    void Start()
    {
        _followSpot = transform.position;
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        animator.SetFloat(Angle, (int) startDirection);
        _playerText = GameObject.FindWithTag("PlayerText").GetComponent<TextMeshProUGUI>();
    }
    
    void Update()
    {
        if (UI.IsPointerOverUIElement()) return;
        if (Camera.main is { })
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 cubeRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D cubeHit = Physics2D.Raycast(cubeRay, Vector2.zero);

                if (!cubeHit || cubeHit.collider.gameObject.CompareTag("Door"))
                {
                    WalkTo(mousePosition);
                }
            }
        }

        var distance = Vector2.Distance(transform.position, _followSpot);
        if (distance < 0.01)
        {
            _agent.velocity = Vector3.zero;
        }

        UpdateAnimation();
        AdjustSortingLayer();
    }

    #endregion

    #region Methods

    public void SetDirection(Direction direction)
    {
        animator.SetFloat(Angle, (int) direction);
    }

    public void WalkTo(Vector3 mousePosition)
    {
        _followSpot = new Vector2(mousePosition.x, mousePosition.y + clickOffsetYAxis);
        _agent.SetDestination(_followSpot);
    }

    private void UpdateAnimation()
    {
        var distance = Vector2.Distance(transform.position, _followSpot);
        if (Vector2.Distance(_stuckDistanceCheck, transform.position) == 0)
        {
            animator.SetFloat(Distance, 0f);
            return;
        }

        animator.SetFloat(Distance, distance);
        if (!(distance > 0.01)) return;
        Vector3 direction = -_agent.velocity;
        var position = transform.position;
        var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        animator.SetFloat(Angle, angle);
        _stuckDistanceCheck = position;
    }

    private void AdjustSortingLayer()
    {
        spriteRenderer.sortingOrder = (int) (transform.position.y * -100);
    }

    public void Say(string text)
    {
        CoroutineController.Start(ChangeTextPlayer(text, Color.magenta));
    }

    public IEnumerator ChangeTextPlayer(string text, Color color)
    {
        _playerText.color = color;
        
        _playerText.text = text;

        yield return new WaitForSeconds(2);

        _playerText.text = "";
    }

    #endregion
    
}