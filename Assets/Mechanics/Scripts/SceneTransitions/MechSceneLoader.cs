using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class MechSceneLoader : MonoBehaviour
{
    [SerializeField] private Animator _transition;
    [SerializeField] private float _time = 1f;

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(Loader(sceneIndex));
    }

    private IEnumerator Loader(int sceneIndex)
    {
        _transition.SetTrigger("SwitchRoom");

        yield return new WaitForSeconds(_time);

        SceneManager.LoadScene(sceneIndex);
    }
}
