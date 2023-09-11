using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        Vector2 inputVector = Vector2.zero;

        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            float touchX = touch.position.x;

            // Determine whether to move left or right based on touch position
            if (touchX < Screen.width * 0.5f)
            {
                inputVector.x = -1f; // Move left
            }
            else
            {
                inputVector.x = 1f; // Move right
            }
        }

        // Always move forward automatically
        inputVector.y = 1f;

        playerController.SetInputVector(inputVector);
    }
}
