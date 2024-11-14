using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This script is for the intro slide before the tutorial level 

public class Intro : MonoBehaviour
{
    public void Start()
    {
        MusicManager.Instance.PlayMusic("MainMenu");
    }

    public void Continue()
    {
        // Play 3D sound effect when continuing
        SoundManager.Instance.PlaySound3D("Play", transform.position);

        // Load the next scene
        SceneManager.LoadScene("Level0");
    }
}
