using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    
    public static MusicManager Instance;

    [SerializeField]
    private MusicLibrary musicLibrary;  // Reference to the MusicLibrary script, which holds available music tracks
    [SerializeField]
    private AudioSource musicSource;    // The AudioSource that plays the music

    private void Awake()
    {
        // Ensure only one instance of MusicManager exists 
        if (Instance != null)
        {
            Destroy(gameObject);  // Destroy duplicate MusicManager instances
        }
        else
        {
            Instance = this;  // Assign the current instance
            DontDestroyOnLoad(gameObject);  // Keep MusicManager across scenes
        }
    }

    // Method to play a specific music track, with an optional fade duration 
    public void PlayMusic(string trackName, float fadeDuration = 0.5f)
    {
        // Start a coroutine to handle crossfading to the new music track
        StartCoroutine(AnimateMusicCrossfade(musicLibrary.GetClipFromName(trackName), fadeDuration));
    }

    // Coroutine to fade out the current track and fade in the new one
    IEnumerator AnimateMusicCrossfade(AudioClip nextTrack, float fadeDuration = 0.5f)
    {
        float percent = 0;

        // Gradually fade out the current track
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;  // Increment percent based on time
            musicSource.volume = Mathf.Lerp(1f, 0, percent);  // Lerp decreases volume from 1 to 0
            yield return null;  
        }

        // Switch to the new track and start playing it
        musicSource.clip = nextTrack;
        musicSource.Play();

        percent = 0;

        // Gradually fade in the new track
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;  // Increment percent for fading in
            musicSource.volume = Mathf.Lerp(0, 1f, percent);  // Lerp increases volume from 0 to 1
            yield return null;  // Wait for the next frame
        }
    }
}
