using UnityEngine;

// A serializable struct to populat music tracks in the inspector. 
[System.Serializable]
public struct MusicTrack
{
    public string trackName;  // The name of the music track
    public AudioClip clip;    // The AudioClip associated with the track
}

public class MusicLibrary : MonoBehaviour
{
    public MusicTrack[] tracks;  // Array to store the music tracks available in the library


    public AudioClip GetClipFromName(string trackName)
    {
        // Iterate through all tracks to find the one that matches the given track name
        foreach (var track in tracks)
        {
            if (track.trackName == trackName)  // Compare the track name with the input
            {
                return track.clip;  // Return the AudioClip associated with the matched track name
            }
        }

        return null;  // If no match is found, return null
    }
}
