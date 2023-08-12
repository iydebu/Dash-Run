using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Singleton for the UIManager
    public static UIManager instance;

    // UI Panels
    [Header("Panels")]
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _winnerPanel;
    [SerializeField] private GameObject _gamePanel;

    // Animated Characters for different game states
    [Header("Animated Characters")]
    [SerializeField] private GameObject[] _animatedCharacters;

    // UI Components
    [Header("Components")]
    [SerializeField] private TMP_Text _rockText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private GameObject _canvasBG;

    // Game management objects
    [Header("Scene Objects")]
    [SerializeField] private GameObject _gameManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartGame();
    }

    // Deactivates all UI panels
    private void CloseAllPanels()
    {
        _startPanel.SetActive(false);
        _gameOverPanel.SetActive(false);
        _winnerPanel.SetActive(false);
        _gamePanel.SetActive(false);
    }

    // Deactivates all animated characters
    private void DisableCharacter()
    {
        foreach (GameObject character in _animatedCharacters)
        {
            character.SetActive(false);
        }
    }

    /// <summary>
    /// Updates the displayed rock score.
    /// </summary>
    public void UpdateRockScore(int score)
    {
        _rockText.text = "Rocks: " + score.ToString();
    }

    /// <summary>
    /// Updates the displayed timer.
    /// </summary>
    public void UpdateTimer(string time)
    {
        _timeText.text = time;
    }

    /// <summary>
    /// Sets up the start game UI.
    /// </summary>
    public void StartGame()
    {
        _canvasBG.SetActive(true);
        CloseAllPanels();
        _startPanel.SetActive(true);
        DisableCharacter();
        _animatedCharacters[0].SetActive(true);
    }

    /// <summary>
    /// Sets up the game over UI.
    /// </summary>
    public void GameOver()
    {
        _canvasBG.SetActive(true);
        CloseAllPanels();
        _gameOverPanel.SetActive(true);
        DisableCharacter();
        _animatedCharacters[1].SetActive(true);
    }

    /// <summary>
    /// Sets up the winner UI.
    /// </summary>
    public void Winner()
    {
        _canvasBG.SetActive(true);
        CloseAllPanels();
        _winnerPanel.SetActive(true);
        DisableCharacter();
        _animatedCharacters[2].SetActive(true);
    }

    /// <summary>
    /// Initializes the game for play.
    /// </summary>
    public void PlayGame()
    {
        CloseAllPanels();
        DisableCharacter();
        _canvasBG.SetActive(false);
        _gamePanel.SetActive(true);
        _gameManager.SetActive(true);
    }

    /// <summary>
    /// Restarts the game by reloading the scene.
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Progresses to the next level.
    /// </summary>
    public void NextLevel()
    {
        _canvasBG.SetActive(false);
        CloseAllPanels();
        DisableCharacter();
        _gamePanel.SetActive(true);
        GameManager.Instance.IncreaseLevel();
    }
}
