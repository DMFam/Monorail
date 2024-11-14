using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

//This script is based on a GameManager I found through a tutorial and edited as needed for my game
// Ensures this script runs early and only one instance exists globally 
[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private const int NUM_LEVELS = 2; 
    public int level { get; private set; } = 0; 
    public int lives { get; private set; } = 3; 
    public int score { get; private set; } = 0;

    public int gemCount; // Tracks collected gems
    public TextMeshProUGUI gemText; // UI reference for gem count

    // Initializes Singleton and persists GameManager across scenes
    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject); // Destroy if duplicate
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        SceneManager.sceneLoaded += OnSceneLoaded; 
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null; // Clear instance if destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unregister scene event
    }

    // Starts background music and sets initial level and gem count UI
    private void Start()
    {
        MusicManager.Instance.PlayMusic("Game");
        level = SceneManager.GetActiveScene().buildIndex + 1;
        UpdateGemText(); 
    }

    // Updates the gem text in the UI
    private void Update()
    {
        UpdateGemText();
    }

    // Updates gem text to display current count
    private void UpdateGemText()
    {
        if (gemText != null) {
            gemText.text = "Gems Collected: " + gemCount.ToString();
        }
    }

    // Called when a new scene is loaded to update gem text
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (gemText == null)
            gemText = GameObject.Find("Gem Text")?.GetComponent<TextMeshProUGUI>(); // Assign if null

        if (gemText != null) {
            UpdateGemText(); // Update UI after reassigning
        } else {
            Debug.LogError("Gem Text not found!");
        }
    }

    // Resets game variables and starts at the first level
    private void NewGame()
    {
        lives = 3;
        score = 0;
        gemCount = 0;
        LoadLevel(1);
    }

    // Loads a specified level with a transition effect
    private void LoadLevel(int level)
    {
        this.level = level;

        if (level > NUM_LEVELS) {
            LoadLevel(1); // Restart if all levels are completed
            return;
        }

        Camera camera = Camera.main;
        if (camera != null) camera.cullingMask = 0; // Simple transition effect

        Invoke(nameof(LoadScene), 1f); // Delay scene load
    }

    // Reloads the current scene
    private void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Awards score and loads the next level
    public void LevelComplete()
    {
        score += 1000;
        LoadLevel(level + 1);
    }

    // Decreases lives; resets game if lives reach zero, otherwise reloads the level
    public void LevelFailed()
    {
        lives--;

        if (lives <= 0) {
            NewGame();
        } else {
            LoadLevel(level);
        }
    }

    // Increments gem count and updates UI when a gem is collected
    public void CollectGem()
    {
        gemCount++;
        UpdateGemText();
    }
}
