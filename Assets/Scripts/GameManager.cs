using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Properties")]
    public float gameDuration = 60.0f; // Duration of the game in seconds
    public int initialRockCount = 4; // Initial number of rocks
    public float spawnRangeX = 10.0f; // X-axis range for random rock spawn
    public float spawnRangeY = 5.0f;  // Y-axis range for random rock spawn
    public GameObject rockPrefab; // Prefab for the rock
    public Transform rockParent; // Parent transform for instantiated rocks

    [Header("Components")]
    [SerializeField] AudioSource audioSource; // Audio source for playing sounds
    [SerializeField] AudioClip throwClip; // Sound clip for throwing rocks

    private int rockCount; // Current number of rocks available
    private float timer; // Remaining time for the game
    private int levelCount; // Current game level
    private Coroutine timerCoroutine; // Coroutine reference for the timer

    private void Awake()
    {
        // Singleton initialization
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        timer = gameDuration; // Initialize the timer
        levelCount = 0; // Initialize the level count
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        InitializeGame(); // Initialize the game
    }

    private void InitializeGame()
    {
        rockCount = initialRockCount + levelCount; // Set the rock count based on level
        UIManager.instance.UpdateRockScore(rockCount); // Update UI with current rock count
        SpawnRocks(); // Spawn rocks in the scene
        timerCoroutine = StartCoroutine(TimerCoroutine(gameDuration)); // Start the timer coroutine
    }

    private void SpawnRocks()
    {
        for (int i = 0; i < rockCount; i++)
        {
            Instantiate(rockPrefab, GetRandomSpawnPosition(), Quaternion.identity, rockParent);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(Random.Range(-spawnRangeX, spawnRangeX), Random.Range(-spawnRangeY, spawnRangeY), 0);
    }

    public void ThrowRock()
    {
        audioSource.PlayOneShot(throwClip); // Play the throw sound
        rockCount--; // Decrement rock count
        if (rockCount <= 0 && timer > 0)
        {
            UIManager.instance.Winner(); // Display winner UI if no rocks left and time > 0
            return;
        }
        UIManager.instance.UpdateRockScore(rockCount); // Update UI with remaining rock count
       
    }

    public void IncreaseLevel()
    {
        levelCount++; // Increase the level count
        InitializeGame(); // Reinitialize the game for the next level
    }

    IEnumerator TimerCoroutine(float time)
    {
        while (time > 0)
        {
            string min = ((int)time / 60).ToString("00"); // Format minutes as 00
            string sec = (time % 60).ToString("00"); // Format seconds as 00
            UIManager.instance.UpdateTimer(min + ":" + sec); // Update UI with formatted time
            time -= 1; // Decrement time
            yield return new WaitForSeconds(1); // Wait for 1 second
        }
        UIManager.instance.GameOver(); // Display game over UI when time is up
    }
}
