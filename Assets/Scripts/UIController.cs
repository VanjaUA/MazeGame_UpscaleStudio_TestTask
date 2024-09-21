using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    //Singleton pattern
    public static UIController Instance { get; private set; }

    [SerializeField] private GameObject winScreen, loseScreen, pauseScreen;
    [SerializeField] private TextMeshProUGUI winScreenTimeText;

    [SerializeField] private TextMeshProUGUI keysText;

    [SerializeField] private TextMeshProUGUI timerText;
    private float timer = 0f;

    private bool gameEnded = false;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        CursorHandle();

        UpdateTimer();
    }

    public void TogglePause() 
    {
        //Method to toggle pause window
        if (gameEnded)
        {
            return;
        }

        if (pauseScreen.activeInHierarchy)
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void UpdateTimer() 
    {
        //Method to update timer text in correct format
        timer += Time.deltaTime;

        System.TimeSpan timeToDisplay = System.TimeSpan.FromSeconds(timer);
        timerText.text = timeToDisplay.Minutes.ToString("00") + ":" + timeToDisplay.Seconds.ToString("00");
    }

    private void CursorHandle()
    {
        //Method to correct handle cursor on screen
        if (gameEnded)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.None && pauseScreen.activeInHierarchy == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void UpdateKeysText(int keysAmount) 
    {
        //Method to update key counter text
        keysText.text = keysAmount.ToString();
    }

    public void QuitGame() 
    {
        //Method to Quit game
        //Debug log for Unity Engine
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void RestartGame() 
    {
        //Method to restart game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoseGame()
    {
        //Method to correct handling of losing a game
        pauseScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        gameEnded = true;
        Time.timeScale = 0f;
        loseScreen.SetActive(true);
    }

    public void WinGame() 
    {
        //Method to correct handling of winning a game
        winScreenTimeText.text = timerText.text;
        pauseScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        gameEnded = true;
        Time.timeScale = 0f;
        winScreen.SetActive(true);
    }
}
