using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manage the game settings and options.
/// Currently handles only the reset game capability.
/// </summary>
public class ControlGame : MonoBehaviour
{
    #region Fields

    [SerializeField] private AudioClip openAudio;
    [SerializeField] private GameStatus status;
    
    private static ControlGame _shared;
    private List<GameObject> _managers;

    #endregion


    #region MonoBehaviour

    private void Awake()
    {
        if (!_shared)
        {
            _shared = this;
            DontDestroyOnLoad(gameObject);
            _shared._managers = new List<GameObject>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            var isInstructionsScene = SceneManager.GetActiveScene().name == "Instructions";
            if (Input.GetKeyDown(KeyCode.R) && !isInstructionsScene)
            {
                var sceneLoader = GameObject.FindWithTag("Loader").GetComponent<SceneLoader>();
                
                sceneLoader.LoadScene(0);
                CoroutineController.Start(RestartGame());
            }

            else if (isInstructionsScene)
            {
                var sceneLoader = GameObject.FindWithTag("Loader").GetComponent<SceneLoader>();

                sceneLoader.LoadScene(1);
                CoroutineController.Start(StartOpeningMusic());
            }
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Adds a manager object to the control script.
    /// Used to manage the re-instantiation and the data reset when resetting the game. 
    /// </summary>
    /// <param name="manager">Manager object.</param>
    public static void AddManager(GameObject manager)
    {
        _shared._managers.Add(manager);
    }

    #endregion


    #region Coroutines

    /// <summary>
    /// Start the opening music when the game starts.
    /// </summary>
    private IEnumerator StartOpeningMusic()
    {
        yield return new WaitForSeconds(1);
        MusicManager.ChangeMusic(openAudio);
    }
    
    /// <summary>
    /// Handles game reset.
    /// </summary>
    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(1);
        MusicManager.ChangeMusic(null);
        MusicManager.SetLoop(false);
        foreach (var manager in _shared._managers)
        {
            Destroy(manager);
        }
        status.ResetActivationTable();
        status.ResetNPC();
    }

    #endregion
    
}
