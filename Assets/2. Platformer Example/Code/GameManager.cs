using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Enum for example game states
[Serializable]  // serializable so it can be viewed in the inspector
public enum GameState
{
    SplashScreen,
    Options,
    Paused,
    Running,
    GameOver
}

// GameManager is a persistent singleton class that manages the game
public sealed class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState CurrentGameState;  // current game state
    public bool GameRunning = false;    // is the game running?
    public float PlayerScore = 10;      // example player score variable
    public float PlayerHealth = 100;    // example player health variable
    
    // TextMeshProUGUI object to display the player score
    public TextMeshProUGUI healthText;   // TextMeshProUGUI object to display the player score
    public TextMeshProUGUI scoreText;    // TextMeshProUGUI object to display the player health
    public AudioClip ambientMusic;      // music asset to be added to the variable in the inspector
    public AudioClip heavyRain;     // music asset to be added to the variable in the inspector

    #region Standard Unity Methods
    void Awake()
    {
        // Ensure that there is only one instance of GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        Debug.Log("GameManager is running");    // delete this line after testing
        DontDestroyOnLoad(gameObject);          // persist between scenes   
        ChangeGameState(GameState.Running);
    }

    void Update()
    {
        // Check the current game state
        switch (CurrentGameState)
        {
            case GameState.SplashScreen:
                SplashScreen();
                break;
            case GameState.Options:
                Options();
                break;
            case GameState.Paused:
                Paused();
                break;
            case GameState.Running:
                Running();
                break;
            case GameState.GameOver:
                GameOver();
                break;
        }
    } // end of update
    #endregion

    private void SplashScreen()
    {
        GameRunning = false;
    }
    private void Options()
    {
        GameRunning = false;
    }
    private void Paused()
    {
        GameRunning = false;
    }
    private void Running()
    {
        if (!GameRunning) 
        {

            GameRunning = true;
            //AudioSystem.Instance.PlayMusic(ambientMusic, 0.1f);
            if (AudioSystem.Instance != null)
            {
                AudioSystem.Instance.PlayMusic(ambientMusic, 0.1f);
            }
            else
            {
                Debug.LogWarning("AudioSystem instance is null. Cannot play music.");
            }
            if (AudioSystem.Instance != null)
            {
                AudioSystem.Instance.PlayWeather(heavyRain, 0.5f);
            }
            else
            {
                Debug.LogWarning("AudioSystem instance is null. Cannot play weather.");
            }
            Debug.Log("Game is running");
            // Example of loading a scene by name
            // This will work if the gameplay scene is called GamePlay and it is added to the build settings
            // But note that it is bad practice to load scenes with a string name rather than a scene index or variable
            
            // // load Unity scene if not already loaded
            // if (!SceneManager.GetSceneByName("GamePlay").isLoaded)
            // {
            //    SceneManager.LoadScene("GamePlay");
            // } 
        }
        else // game is running
        {
            // update player health in the UI
           if (healthText != null && scoreText != null)
            {
                healthText.text = "Health: " + PlayerHealth.ToString();
            }
            if (scoreText != null)
            {
                // update player score in the UI
                scoreText.text = "Score: " + PlayerScore.ToString();
            }
            
            

        }
    }
        private void GameOver()
    {
        GameRunning = false;
    }

    // Method to change the game state
    public void ChangeGameState(GameState newGameState)
    {
        CurrentGameState = newGameState;
    }
    // Public static method to get the singleton instance
}
