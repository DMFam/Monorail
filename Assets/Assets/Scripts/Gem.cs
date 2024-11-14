using UnityEngine;

public class Gem : MonoBehaviour
{
    // This script will handle gem collection and destruction
    public void Collect()
    {
        // Play the sound at the gem's position
        SoundManager.Instance.PlaySound2D("Gem");

        // Destroy the gem object when collected
        Destroy(gameObject);
    }
}