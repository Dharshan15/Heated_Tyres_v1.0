using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    private PlayerController playerController;

    public bool isGameOver = false;
    void Start()
    {
        Time.timeScale = 1;
        playerController = FindObjectOfType<PlayerController>();
    }

    
    void Update()
    {
        
    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameOver = true;
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        if (playerController.won)
            SceneManager.LoadScene(1);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
