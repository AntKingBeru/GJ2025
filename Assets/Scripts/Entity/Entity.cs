using UnityEngine;
using System.Collections;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    [SerializeField] private int health = 100;

    [SerializeField] private int damage = 10;

    // [SerializeField] protected string idleAnimationFlag = "idle";
    [SerializeField] private string horizontalAnimationFlag = "horizontal";
    [SerializeField] private string verticalAnimationFlag = "vertical";
    [SerializeField] private string attackAnimationFlag = "attack";

    [SerializeField] private float attackSize = 1.5f;

    private bool isAttacking = false;

    enum Direction 
    {
        Right,
        Left,
        Up,
        Down
    }
    private Direction lastMoveDirection = Direction.Right;

    private SpriteRenderer spriteRenderer; // For left/right/up/down move

    // Weapon collider to get hit right
    private BoxCollider2D weaponCollider;

    // For all animations
    private Animator animator;

    void Awake()
    {
        weaponCollider = GetComponentInChildren<BoxCollider2D>();
        weaponCollider.enabled = false; // Start with collider disabled

        // Getting animator
        animator = GetComponent<Animator>();

        // Getting sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    protected void Move(float horizontal, float vertical) {
        // Create a movement vector based on input
        Vector2 movement = new Vector2(horizontal, vertical);

        if(this.isAttacking) {
            // animator.SetBool(this.verticalAnimationFlag, false);
            animator.SetBool(this.horizontalAnimationFlag, false);
            return;
        }

        if (horizontal > 0) this.OnMoveRight();
        else if (horizontal < 0) this.OnMoveLeft();
        else animator.SetBool(this.horizontalAnimationFlag, false);

        if (vertical > 0) this.OnMoveUp();
        else if (vertical < 0) this.OnMoveDown();
        else animator.SetBool(this.verticalAnimationFlag, false);


        // Normalize the vector to prevent faster diagonal movement
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        // Apply movement scaled by speed and time.deltaTime for frame-rate independence
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
    
    // ================ Helper function for all movement start ================

    // Helper function for left movement
    private void OnMoveLeft() {
        animator.SetBool(this.horizontalAnimationFlag, true);
        spriteRenderer.flipX = true; // Flip orientation
        this.lastMoveDirection = Direction.Left;
    }

    
    // Helper function for left movement
    private void OnMoveRight() {
        animator.SetBool(this.horizontalAnimationFlag, true);
        spriteRenderer.flipX = false; // Original orientation
        this.lastMoveDirection = Direction.Right;
    }

    // Helper function for left movement
    private void OnMoveUp() {
        this.lastMoveDirection = Direction.Up;
    }

    // Helper function for left movement
    private void OnMoveDown() {
        this.lastMoveDirection = Direction.Down;
    }

    // ================ Helper functions for attack ================

    public void DisableAttack() {
         animator.SetBool(this.attackAnimationFlag, false);
         this.isAttacking = false;
    }

    // Getting animation name becuase be are generic, yay
    protected void Attack() {
        this.isAttacking = true;

        animator.SetBool(this.attackAnimationFlag, true);

        switch(lastMoveDirection) {
            case Direction.Right:
                Debug.Log("Right attack");
                break;
            case Direction.Left:
                Debug.Log("Left attack");
                break;
            case Direction.Up:
                Debug.Log("Up attack");
                break;
            case Direction.Down:
                Debug.Log("Down attack");
                break;
            default:
                Debug.Log("Default attack, right");
                break;
        }
        // SquereCast


        // Debug.Log("Triggered");
        // if (other.CompareTag("Enemy")) // Check if the collided object is an enemy

        // weaponCollider.enabled = false; // Removing weapon collider

        // // animator.Play(this.idleAnimation, 0, 0f);
    }

    // For when attack succeed
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered");
        if (other.CompareTag("Enemy")) // Check if the collided object is an enemy
        {
            // Example: Call a method on the enemy to take damage
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(this.damage);
            }
            // Optional: Disable collider after first hit to prevent multiple hits per swing
            weaponCollider.enabled = false; 
        }

        else if (other.CompareTag("Player")) { // Check if collided with player
            // Example: Call a method on the enemy to take damage
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(this.damage);
            }
            // Optional: Disable collider after first hit to prevent multiple hits per swing
            weaponCollider.enabled = false; 
        } 
    }

    public void TakeDamage(int dmg) {
        this.health -= dmg;

        if(this.health <= 0) {
            this.OnDeath();
            Destroy(gameObject);
        }
        
    }

    protected abstract void OnDeath();
}
