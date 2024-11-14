using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 1f;   // Delay before the platform starts falling
    [SerializeField] private float destroyDelay = 2f; // Delay before the platform is destroyed after falling

    private bool falling = false; // Tracks whether the platform has started falling

    [SerializeField] private Rigidbody2D rb; // Reference to the platform's Rigidbody2D

    // Triggered when something collides with the platform. Starts the fall if the player lands on it.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (falling)
            return; // Prevents the fall from being triggered multiple times

        // If the player steps on the platform, begin the fall sequence
        if (collision.transform.tag == "Player")
        {
            StartCoroutine(StartFall()); // Starts the coroutine to handle the platform falling
        }
    }

    // Coroutine that handles the platform's fall and destruction
    private IEnumerator StartFall()
    {
        falling = true; // Mark the platform as falling to prevent multiple triggers

        // Wait for the specified fall delay before the platform drops
        yield return new WaitForSeconds(fallDelay);

        // Change the platform's Rigidbody to dynamic so it can fall due to gravity
        rb.bodyType = RigidbodyType2D.Dynamic;

        // Destroy the platform after the specified delay once it starts falling
        Destroy(gameObject, destroyDelay);
    }
}
