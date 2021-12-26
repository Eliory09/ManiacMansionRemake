using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


/// <summary>
/// Scene loader used to load scenes and activate a transition.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    #region Fields

    [SerializeField] private Animator transition;
    [SerializeField] private float time = 1f;
    private static readonly int SwitchRoom = Animator.StringToHash("SwitchRoom");
    private static readonly int EndTransition = Animator.StringToHash("EndTransition");

    #endregion
    
    #region Methods

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(Loader(sceneIndex));
    }

    public void LoadSceneAdditive(int sceneIndex)
    {
        StartCoroutine(LoaderAdditive(sceneIndex));
    }

    private IEnumerator Loader(int sceneIndex)
    {
        transition.SetTrigger(SwitchRoom);

        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(sceneIndex);
    }
    
    private IEnumerator LoaderAdditive(int sceneIndex)
    {
        transition.SetTrigger(SwitchRoom);

        yield return new WaitForSeconds(time);
        
        ItemsUIManager.SetDisable();

        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
        
        transition.SetTrigger(EndTransition);
    }

    #endregion
    
}
