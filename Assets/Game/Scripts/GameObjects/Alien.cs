using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Handles the conversation details and specifications with the Alien at the mansion entrance.
/// </summary>
public class Alien : MonoBehaviour, NPC
{

    #region Fields

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject qMark;
    [SerializeField] private AudioClip troubleMusic;
    [SerializeField] private AudioClip regularMusic;
    [SerializeField] private DialogueNode arrestChoice;
    [SerializeField] private DialogueNode meteorChoice;
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private Player player;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private GameStatus status;

    private bool found;
    private static bool _visited;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        if (_visited)
            Destroy(gameObject);
        AddToGameStatus();
    }

    private void Update()
    {
        // Find the player within radius.
        if (!found)
        {
            if (Vector3.Distance(transform.position, player.gameObject.transform.position) <= 3f)
            {
                found = true;
                _visited = true;
                Activate();
            }
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Activates the conversation with the alien.
    /// </summary>
    public void Activate()
    {
        GameManager.FreezeSceneInDialogue();
        MusicManager.ChangeMusic(troubleMusic);
        DialogueManager.AddRegularEventToContinueButton(CheckCurrentNode);
        CoroutineController.Start(ActivateDialogue(1));
    }

    /// <summary>
    /// Reset conversation with alien.
    /// </summary>
    public void Reset()
    {
        _visited = false;
    }
    
    public void AddToGameStatus()
    {
        status.AddNPC(this);
    }

    /// <summary>
    /// Checks the current dialogue choice and act accordingly.
    /// </summary>
    private void CheckCurrentNode()
    {
        CheckDecision(DialogueManager.GetCurrentNode());
    }

    /// <summary>
    /// Handles the player's choice of being arrested or fooling the Alien.
    /// </summary>
    private void CheckDecision(DialogueNode node)
    {
        if (node == arrestChoice)
        {
            DialogueManager.SetActionToContinueButton(ActivateArrest);
        }
        else if (node == meteorChoice)
        {
            DialogueManager.SetActionToContinueButton(ActivateWalkToGate);
        }
    }

    /// <summary>
    /// Activates the walk to gate decision.
    /// </summary>
    private void ActivateWalkToGate()
    {
        // NPCDialogueTrigger trigger = gameObject.GetComponent<NPCDialogueTrigger>();
        // trigger.firstDialogueNode = afterTalking;
        // trigger.TriggerDialogue();
        print("walk to gate");
        playableDirector.Play();
        spriteRenderer.sortingOrder = 400;
        CoroutineController.Start(UnFreezeGame((float) playableDirector.duration));
    }

    /// <summary>
    /// Activates the arrest decision.
    /// </summary>
    private void ActivateArrest()
    {
        // NPCDialogueTrigger trigger = gameObject.GetComponent<NPCDialogueTrigger>();
        // trigger.firstDialogueNode = afterTalking;
        // trigger.TriggerDialogue();
        MusicManager.ChangeMusic(regularMusic);
        sceneLoader.LoadScene(18);
    }
    #endregion

    #region Coroutines

    /// <summary>
    /// Activates the dialogue with the alien.
    /// </summary>
    private IEnumerator ActivateDialogue(float seconds)
    {
        qMark.SetActive(true);
        yield return new WaitForSeconds(seconds);
        qMark.SetActive(false);
        gameObject.GetComponent<NPCDialogueTrigger>().TriggerDialogue();
    }
    
    /// <summary>
    /// Unfreeze the game then the conversation ends.
    /// </summary>
    private IEnumerator UnFreezeGame(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GameManager.UnfreezeSceneInDialogue();
        MusicManager.ChangeMusic(regularMusic);

    }

    #endregion
}
