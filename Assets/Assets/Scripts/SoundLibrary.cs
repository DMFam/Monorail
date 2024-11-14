using UnityEngine;

// A serializable struct to populat sound effects similar to music tracks in the inspector. 
[System.Serializable]
public struct SoundEffect
{
    // The unique identifier for the sound group 
    public string groupID;

    // An array of AudioClips for the specific sound group
    public AudioClip[] clips;
}

public class SoundLibrary : MonoBehaviour
{
    // Array of sound effects, each having a group ID and multiple AudioClips
    public SoundEffect[] soundEffects;

    // Function to retrieve a random AudioClip from a sound group by its groupID
    public AudioClip GetClipFromName(string name)
    {
        // Loop through all the sound effects to find a matching groupID
        foreach (var soundEffect in soundEffects)
        {
            // If the groupID matches the requested name, return a random clip from the group
            if (soundEffect.groupID == name)
            {
                return soundEffect.clips[Random.Range(0, soundEffect.clips.Length)];
            }
        }

        // Return null if no matching groupID is found
        return null;
    }
}
