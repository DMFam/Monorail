using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;  // Allows access to editor-specific functionality which lets me "quit" the game
#endif
public class MainMenu : MonoBehaviour
{
    // Plays the main menu music when the main menu scene starts
    public void Start()
    {
        MusicManager.Instance.PlayMusic("MainMenu");
    }

    
    public void Play()
    {
        SceneManager.LoadScene("Intro");  // Load the Intro scene
    }

    
    public void Quit()
    {
        Debug.Log("Game is quitting");  // Log a message to confirm quit action
        
        Application.Quit();  // Closes the application (works in builds only)

        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;  // Stops play mode in the Unity Editor
        #endif
    }
}
