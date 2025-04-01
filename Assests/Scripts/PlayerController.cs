using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject gameOverPanel; // Assign in inspector
    
    // Tag to find health text object in scene
    public string healthTextTag = "HealthText";
    private TextMesh healthText;
    
    private float speed = 15.0f;
    private float turnSpeed = 40.0f;
    private float horizontalInput;
    private float verticalInput;
    
    private bool isGameOver = false;
    
    // Health properties
    public int maxHealth = 100;
    private int currentHealth;
    public int damageTakenPerHit = 25;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PlayerController initialized");
        
        // Hide game over panel at start
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
            Debug.Log("Game over panel hidden");
        }
        else
        {
            Debug.LogWarning("Game over panel is not assigned!");
        }
        
        // Find health text by tag
        GameObject healthTextObject = GameObject.FindWithTag(healthTextTag);
        if (healthTextObject != null)
        {
            healthText = healthTextObject.GetComponent<TextMesh>();
            Debug.Log("Found health text by tag: " + healthTextTag);
        }
        else
        {
            Debug.LogWarning("Could not find health text object with tag: " + healthTextTag);
        }
            
        // Initialize health
        currentHealth = maxHealth;
        Debug.Log("Health initialized to: " + currentHealth);
        
        // Update health display
        UpdateHealthDisplay();
    }

    // The rest of the methods stay the same
    void Update()
    {
        // Don't process input if game is over
        if (isGameOver)
        {
            // Check for space key to restart
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Space key pressed - restarting game");
                RestartGame();
            }
            return;
        }
            
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        //Move truck forward
        transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput);

        //truck rotation + Horizontal movement
        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);

        // Check if the car falls off the road
        if (transform.position.y < -5)
        {
            Debug.Log("Car fell off road at position: " + transform.position);
            GameOver();
        }
    }
    
    // Update the health display
    void UpdateHealthDisplay()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth.ToString();
            Debug.Log("Health display updated: " + currentHealth);
        }
        else
        {
            Debug.LogWarning("Health text is not assigned or found by tag!");
        }
    }
    
    // Called when this object collides with another collider
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name + " (Tag: " + collision.gameObject.tag + ")");
        
        // Check if we hit an obstacle
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit obstacle: " + collision.gameObject.name);
            TakeDamage(damageTakenPerHit);
            
            // Destroy the obstacle to ensure each collision is counted only once
            Destroy(collision.gameObject);
            Debug.Log("Destroyed obstacle: " + collision.gameObject.name);
        }
    }
    
    // Handle taking damage
    void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Took " + damageAmount + " damage. Health now: " + currentHealth);
        UpdateHealthDisplay();
        
        // Check if health is depleted
        if (currentHealth <= 0)
        {
            Debug.Log("Health depleted - game over");
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over triggered!");
        isGameOver = true;
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Debug.Log("Game over panel activated");
        }
        else
        {
            Debug.LogError("Game Over Panel is null!");
        }
        
        Time.timeScale = 0;
        Debug.Log("Time scale set to 0");
    }
    
    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        Time.timeScale = 1;
        Debug.Log("Time scale reset to 1");
        
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}