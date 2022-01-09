using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


/// <summary>
/// Handles the return from a sudden cutscene.
/// Used to close an additive cutscene of the Edinsons wandering the house.
/// </summary>
public class ReturnFromCutscene : MonoBehaviour
{
    #region Fields

    [SerializeField] private UnityEvent @event;
    [SerializeField] private int currentSceneIndex;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        @event.Invoke();
        GameManager.UnfreezeSceneInDialogue();
        GameManager.UnFreezeEdinsonsScene();
        SceneManager.UnloadSceneAsync(currentSceneIndex);
    }

    #endregion
}
