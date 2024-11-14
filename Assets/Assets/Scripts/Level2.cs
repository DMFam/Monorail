using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This script is for the final scene in the game
public class Level2 : MonoBehaviour
{
    public void Start()
    {
       MusicManager.Instance.PlayMusic("Victory");
    }

    public void Return()
    {
        // Play 3D sound effect when continuing
        SoundManager.Instance.PlaySound3D("Play", transform.position);

        // Load the next scene
        SceneManager.LoadScene("MainMenu");
    }
}
