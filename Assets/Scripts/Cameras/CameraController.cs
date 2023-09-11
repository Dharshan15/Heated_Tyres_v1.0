using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // The player's transform to follow
    public Vector3 offset;   // Offset from the player's position

    private void LateUpdate()
    {
        // Calculate the desired position for the camera
        Vector3 desiredPosition = target.position + offset;

        // Set the camera's position to the desired position without interpolation
        transform.position = desiredPosition;
    }
}
