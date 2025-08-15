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
    [SerializeField] private LayerMask layerMask;

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

    // For all animations
    private Animator animator;

    void Awake()
    {
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
        // else animator.SetBool(this.verticalAnimationFlag, false);


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

        float castDistance = 1f; // How far the box is cast
        Vector2 boxSize = new Vector2(this.attackSize, this.attackSize);

        Vector2 direction = transform.right; // default

        switch(lastMoveDirection) {
            case Direction.Right:
                Debug.Log("Right attack");
                direction = transform.right;
                break;
            case Direction.Left:
                Debug.Log("Left attack");
                direction = -transform.right;
                break;
            case Direction.Up:
                Debug.Log("Up attack");
                direction = transform.up;
                break;
            case Direction.Down:
                Debug.Log("Down attack");
                direction = -transform.up;
                break;
            default:
                Debug.Log("Default attack, right");
                direction = transform.right;
                break;
        }
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, 0f, direction, castDistance, this.layerMask);
         if (hit.collider != null) {
            // Getting game object
            GameObject hitGameObject = hit.collider.gameObject;

            if(hitGameObject.CompareTag("Enemy") || hitGameObject.CompareTag("Player")) {
                Entity entity = hitGameObject.GetComponent<Entity>();
                entity.TakeDamage(this.damage);
            } 
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
