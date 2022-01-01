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
        var obj = GameObject.FindGameObjectsWithTag(tag);

        if (obj.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
