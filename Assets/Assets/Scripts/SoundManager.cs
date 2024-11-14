using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Singleton instance of the SoundManager to ensure only one instance is accessible globally
    public static SoundManager Instance;

    // Reference to the SoundLibrary that stores all sound effects
    [SerializeField]
    private SoundLibrary sfxLibrary;

    // 2D audio source used for playing sounds that don't require 3D positioning 
    [SerializeField]
    private AudioSource sfx2DSource;

    private void Awake()
    {
        // Ensure there is only one instance of SoundManager
        if (Instance != null)
        {
            // Destroy duplicate SoundManager instances to maintain a singleton
            Destroy(gameObject);
        }
        else
        {
            // Set this instance as the singleton and prevent it from being destroyed on scene changes
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Play a 3D sound at a specific position in the world using an AudioClip
    public void PlaySound3D(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            // Play the sound at the given world position
            AudioSource.PlayClipAtPoint(clip, pos);
        }
    }

    // Play a 3D sound by name
    public void PlaySound3D(string soundName, Vector3 pos)
    {
        // Retrieve the clip from the sound library and play it at the specified position
        PlaySound3D(sfxLibrary.GetClipFromName(soundName), pos);
    }

    // Play a 2D sound using a sound's name
    public void PlaySound2D(string soundName)
    {
        // Play the sound through the 2D audio source 
        sfx2DSource.PlayOneShot(sfxLibrary.GetClipFromName(soundName));
    }
}
