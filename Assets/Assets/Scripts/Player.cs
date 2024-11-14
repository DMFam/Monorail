using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections;  // For using IEnumerator

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;  // Reference to the player's sprite renderer
    public Sprite[] runSprites;  // Array to hold sprites for running animation
    public Sprite climbSprite;  // Sprite to display when climbing
    private int spriteIndex;  // Index for cycling through run sprites

    private Rigidbody2D rb;  // Player's rigidbody for physics-based movement
    private CapsuleCollider2D capsuleCollider;  // Capsule collider for collision detection

    private readonly Collider2D[] overlaps = new Collider2D[4];  // Array to hold overlapping objects
    private Vector2 direction;  // Movement direction of the player

    private bool grounded;  // Checks if the player is on the ground
    private bool climbing;  // Checks if the player is climbing

    public float moveSpeed = 3f;  // Player's horizontal movement speed
    public float jumpStrength = 4f;  // Force applied when the player jumps

    // Threshold below which the player falls off the map
    private float fallThreshold = -10f;
    private bool isReloading = false;  // Used to prevent multiple level reloads

    private void Awake()
    {
        // Get components for sprite rendering, physics, and collision
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Starts sprite animation
    private void OnEnable()
    {
        InvokeRepeating(nameof(AnimateSprite), 1f / 12f, 1f / 12f);  // Calls AnimateSprite 12 times per second
    }

    // Stops sprite animation
    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Update()
    {
        // Check if the player is grounded or climbing
        CheckCollision();

        // Set the movement direction based on input
        SetDirection();

        // Check if the player has fallen below the fall threshold
        if (transform.position.y < fallThreshold && !isReloading)
        {
            StartCoroutine(ReloadLevelAfterDelay());
        }
    }

    // Checks if the player is grounded or climbing
    private void CheckCollision()
    {
        grounded = false;
        climbing = false;

        float skinWidth = 0.1f;

        // Calculate a box size slightly larger than the player's capsule collider to handle collisions fluidly
        Vector2 size = capsuleCollider.bounds.size;
        size.y += skinWidth;
        size.x /= 2f;

        // Check for collisions with ground or ladder
        int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, overlaps);
        for (int i = 0; i < amount; i++)
        {
            GameObject hit = overlaps[i].gameObject;

            // If the player collides with the ground
            if (hit.layer == LayerMask.NameToLayer("Ground"))
            {
                // Determine if the player is grounded
                grounded = hit.transform.position.y < (transform.position.y - 0.5f + skinWidth);
                Physics2D.IgnoreCollision(overlaps[i], capsuleCollider, !grounded);
            }
            // If the player collides with a ladder
            else if (hit.layer == LayerMask.NameToLayer("Ladder"))
            {
                climbing = true;
            }
        }
    }

    // Handles player movement based on input
    private void SetDirection()
    {
        // If the player is climbing, adjust the vertical direction
        if (climbing)
        {
            direction.y = Input.GetAxis("Vertical") * moveSpeed;
        }
        // If the player is grounded and presses jump, apply jump strength
        else if (grounded && Input.GetButtonDown("Jump"))
        {
            direction = Vector2.up * jumpStrength;
        }
        // Apply gravity when not climbing or jumping
        else
        {
            direction += Physics2D.gravity * Time.deltaTime;
        }

        // Set horizontal movement
        direction.x = Input.GetAxis("Horizontal") * moveSpeed;

        // Limit downward velocity if grounded to make jumping feel more fluid
        if (grounded)
        {
            direction.y = Mathf.Max(direction.y, -1f);
        }

        // Flip the player's sprite based on movement direction
        if (direction.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;  // Face right
        }
        else if (direction.x < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);  // Face left
        }
    }

    // Handles physics updates for player movement
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * Time.fixedDeltaTime);  // Apply movement to the player
    }

    // Handles sprite animation based on movement
    private void AnimateSprite()
    {
        if (climbing)
        {
            // Show climb sprite when climbing
            spriteRenderer.sprite = climbSprite;
        }
        else if (direction.x != 0f)
        {
            // Animate run sprites when moving
            spriteIndex++;
            if (spriteIndex >= runSprites.Length)
            {
                spriteIndex = 0;
            }
            spriteRenderer.sprite = runSprites[spriteIndex];
        }
    }

    // Handles collision with specific objects like objectives or obstacles
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player collides with an objective, move to the next level
        if (collision.gameObject.CompareTag("Objective"))
        {
            GameManager.Instance.LevelComplete();

            // Load the next level
            int nextLevelIndex = GameManager.Instance.level;
            SceneManager.LoadScene("Level" + nextLevelIndex);
        }
        // If the player collides with an obstacle, fail the level
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            enabled = false;  // Disable the player script
            GameManager.Instance.LevelFailed();
        }
    }

    // Coroutine to reload the level after a delay when the player falls
    private IEnumerator ReloadLevelAfterDelay()
    {
        isReloading = true;  // Prevent multiple reloads
        yield return new WaitForSeconds(0.5f);  // Wait before reloading
        GameManager.Instance.LevelFailed();
        isReloading = false;
    }

    // Handles gem collection
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the player collects a gem
        if (other.gameObject.CompareTag("Gem"))
        {
            // Notify GameManager to increment the gem count
            GameManager.Instance.CollectGem();
            
            // Play sound when gem is collected
            SoundManager.Instance.PlaySound3D("Gem", transform.position);

            // Destroy the gem object
            Destroy(other.gameObject);
        }
    }
}
