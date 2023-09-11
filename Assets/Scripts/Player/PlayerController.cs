
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Car settings")]
    public float driftFactor = 0.95f;
    public float accelerationFactor = 30.0f;
    public float turnFactor = 3.5f;
    public float maxSpeed = 20;

    //scoring 
    public float driftScoreMultiplier = 10.0f;
    public float maxDriftAngle = 30.0f;
    private float currentDriftScore = 0;

    float accelerationInput = 0;
    float steeringInput = 0;

    float rotationAngle = 0;
    float velocityVsUp = 0;

    public TextMeshProUGUI scoreText;
    private float score = 0;
    private GameManager gameManager;
    public TextMeshProUGUI winText;

    public bool won = false;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    void FixedUpdate()
    {
        ApplyEngineForce();
        KillOrthogonalVelocity();
        ApplySteering();

        if (gameManager.isGameOver)
        {
            scoreText.text = "0";
        }
        CalculateDriftScore();
        UpdateScoreText();
    }

    void ApplyEngineForce()
    {
        //calculate how much forward we are going in terms of the direction of our velocity 
        velocityVsUp = Vector2.Dot(transform.up, rb.velocity);
        if (velocityVsUp > maxSpeed && accelerationInput > 0)
            return;

        if (velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0)
            return;

        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
            return;

        //apply drag if there is no acceleration 
        if (accelerationInput == 0)
            rb.drag = Mathf.Lerp(rb.drag, 3.0f, Time.fixedDeltaTime * 3);
        else rb.drag = 0;
        //create force for the engine
        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;
        //apply force and push the car forward
        rb.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteering()
    {
        //stop turning at rest position 
        float minSpeedBeforeAllowTurningFactor = (rb.velocity.magnitude / 8);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        //rotation
        rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowTurningFactor;

        //apply steering by rotating the car object 
        rb.MoveRotation(rotationAngle);
    }

    void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);

        rb.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    float GetLateralVelocity()
    {
        return Vector2.Dot(transform.right, rb.velocity);
    }
    public bool IsTireScreeching(out float lateralVelocity, out bool isDrifting)
    {
        lateralVelocity = GetLateralVelocity();
        isDrifting = false;

        if (Input.touchCount > 0)
        {
            isDrifting = true;
            return true;
        }
        return false;
    }

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "win")
        {
            if (SceneManager.GetActiveScene().buildIndex < 5)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else if (winText != null)
            {
                winText.gameObject.SetActive(true);
                gameManager.isGameOver = true;
                won = true;
                Time.timeScale = 0;
            }
        }
    }
    void CalculateDriftScore()
    {
        float lateralVelocity;
        bool isDrifting;
        bool tireScreeching = IsTireScreeching(out lateralVelocity, out isDrifting);



        // Only increase score if the player is actively drifting
        if (tireScreeching && isDrifting)
        {
            // Calculate drift angle based on lateral velocity and maxDriftAngle
            float driftAngle = Mathf.Abs(lateralVelocity) / maxSpeed * maxDriftAngle;

            // Calculate drift score based on drift angle and multiplier
            float driftScore = driftAngle * driftScoreMultiplier / 1000;

            // Update current drift score by adding the calculated drift score
            currentDriftScore += driftScore;
        }
    }

    void UpdateScoreText()
    {
        // Update the score display text
        if (!gameManager.isGameOver)
        {
            score = Mathf.RoundToInt(currentDriftScore);
            scoreText.text = score.ToString();
        }
    }
}