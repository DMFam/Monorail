using UnityEngine;

public class Spawner : MonoBehaviour
{
    // The prefab to spawn
    public GameObject prefab;

    // Minimum and maximum time between spawns
    public float minTime = 2f;

    public float maxTime = 4f;

    // Called when the script is enabled 
    private void OnEnable()
    {
        // Invoke the Spawn method after 'minTime' seconds
        Invoke(nameof(Spawn), minTime);
    }

    // Called when the script is disabled (or when the GameObject is deactivated)
    private void OnDisable()
    {
        // Cancel any pending Invoke calls when the spawner is disabled
        CancelInvoke();
    }

    
    private void Spawn()
    {
        // Instantiate the prefab at the spawner's position with no rotation and allow the platform to control the force
        Instantiate(prefab, transform.position, Quaternion.identity);

        // Schedule the next spawn with a random delay between the minimum and maximum time
        Invoke(nameof(Spawn), Random.Range(minTime, maxTime));
    }
}
