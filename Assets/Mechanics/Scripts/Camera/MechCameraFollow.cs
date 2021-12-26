using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MechCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
    [Range(1, 10)] public float smoothFactor;
    public Vector3 minValues, maxValues;

    void FixedUpdate ()
    {
        Follow();
    }

    void Follow()
    {
        var playerPosition = player.position + offset;
        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(playerPosition.x, minValues.x, maxValues.x),
            Mathf.Clamp(playerPosition.y, minValues.y, maxValues.y),
            Mathf.Clamp(playerPosition.z, minValues.z, maxValues.z)
        );
        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }
}
