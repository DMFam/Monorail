using UnityEngine;

public class Barrel : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 1f;

    // Called when the script instance is being loaded. Initializes the Rigidbody2D component.
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Grabs the Rigidbody2D component attached to the barrel.
    }

    // Triggered when the barrel collides with another object. Handles interactions when hitting the ground.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the barrel hits the ground, apply a force to make it move.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            rb.AddForce(collision.transform.right * speed, ForceMode2D.Impulse); 
            // Moves the barrel to the right with a force.
            // For the barrels to move to the left, I reverse the Y rotation of the platforms
        }
    }
}
