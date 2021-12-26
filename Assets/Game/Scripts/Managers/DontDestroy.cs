using System.Linq;
using UnityEngine;

/// <summary>
/// Prevent destruction of object.
/// CURRENTLY NOT IN USE.
/// </summary>
public class DontDestroy : MonoBehaviour
{
    private void Awake()
    {
        var objs = GameObject.FindGameObjectsWithTag("GameController");

        if (objs.Length > 6)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
