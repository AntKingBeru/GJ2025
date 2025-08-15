using UnityEngine;
using System.Collections;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1f;

    [SerializeField] private int _health = 100;

    [SerializeField] private int _damage = 10;

    // [SerializeField] protected string idleAnimationFlag = "idle";
    [SerializeField] private string _horizontalAnimationFlag = "default";
    [SerializeField] private string _upAnimationFlag = "default";
    [SerializeField] private string _downAnimationFlag = "default";
    [SerializeField] private string _attackHorizontalAnimationFlag = "default";
    [SerializeField] private string _attackUpAnimationFlag = "default";
    [SerializeField] private string _attackDownAnimationFlag = "default";
    [SerializeField] private string _hitAnimationFlag = "default";
    [SerializeField] private float _attackSize = 1.5f;
    [SerializeField] private LayerMask _layerMask;

    private bool _isAttacking = false;

    enum Direction 
    {
        Right,
        Left,
        Up,
        Down
    }
    private Direction _lastMoveDirection = Direction.Right;

    private SpriteRenderer _spriteRenderer; // For left/right/up/down move

    // For all animations
    protected Animator _animator;

    void Awake()
    {
        // Getting animator
        _animator = GetComponent<Animator>();

        // Getting sprite renderer
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }

    protected void Move(float horizontal, float vertical) {
        // Create a movement vector based on input
        Vector2 movement = new Vector2(horizontal, vertical);

        if(this._isAttacking) {
            // animator.SetBool(this.verticalAnimationFlag, false);
            this.ChangeAnimationFlag(this._horizontalAnimationFlag, false);
            return;
        }

        // Calcuate animation to active

        if (horizontal > 0)  {
            this._lastMoveDirection = Direction.Right;
            this.MoveRightAnimation();
        }
        else if (horizontal < 0) {
            this._lastMoveDirection = Direction.Left;
            this.MoveLeftAnimation();
        }
        else {
            this.ChangeAnimationFlag(this._horizontalAnimationFlag, false);
        }

        if (vertical > 0) {
            this._lastMoveDirection = Direction.Up;
            this.MoveUpAnimation();
        }
        else if (vertical < 0) {
            this._lastMoveDirection = Direction.Down;
            this.MoveDownAnimation();
        }
        else {
            this.ChangeAnimationFlag(this._upAnimationFlag, false);
            this.ChangeAnimationFlag(this._downAnimationFlag, false);
        }

        // Normalize the vector to prevent faster diagonal movement
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        // Apply movement scaled by speed and time.deltaTime for frame-rate independence
        transform.Translate(movement * _moveSpeed * Time.deltaTime);
    }
    
    // ================ Helper function for all movement start ================

    private void ResetMoveAnimation() {
        this.ChangeAnimationFlag(this._horizontalAnimationFlag, false);
        this.ChangeAnimationFlag(this._upAnimationFlag, false);
        this.ChangeAnimationFlag(this._downAnimationFlag, false);
        // this._spriteRenderer.flipX = false; // Flip orientation
    }

    private void MoveRightAnimation() {
        this.ChangeAnimationFlag(this._horizontalAnimationFlag, true);
        this.ChangeAnimationFlag(this._upAnimationFlag, false);
        this.ChangeAnimationFlag(this._downAnimationFlag, false);
        this._spriteRenderer.flipX = false; // Flip orientation
    }

    private void MoveLeftAnimation() {
        this.ChangeAnimationFlag(this._horizontalAnimationFlag, true);
        this.ChangeAnimationFlag(this._upAnimationFlag, false);
        this.ChangeAnimationFlag(this._downAnimationFlag, false);
        this._spriteRenderer.flipX = true; // Flip orientation
    }

    private void MoveUpAnimation() {
        this.ChangeAnimationFlag(this._upAnimationFlag, true);
        this.ChangeAnimationFlag(this._horizontalAnimationFlag, false);
        this.ChangeAnimationFlag(this._downAnimationFlag, false);
    }

    private void MoveDownAnimation() {
        this.ChangeAnimationFlag(this._downAnimationFlag, true);
        this.ChangeAnimationFlag(this._horizontalAnimationFlag, false);
        this.ChangeAnimationFlag(this._upAnimationFlag, false);
    }

    // ================ Helper functions for attack ================

    private void AttackHorizontalAnimation() {
        this.ResetMoveAnimation();
        this.ChangeAnimationFlag(this._attackHorizontalAnimationFlag, true);
        this.ChangeAnimationFlag(this._attackUpAnimationFlag, false);
        this.ChangeAnimationFlag(this._attackDownAnimationFlag, false);
    }

    private void AttackDownAnimation() {
        this.ResetMoveAnimation();
        this.ChangeAnimationFlag(this._attackDownAnimationFlag, true);
        this.ChangeAnimationFlag(this._attackHorizontalAnimationFlag, false);
        this.ChangeAnimationFlag(this._attackUpAnimationFlag, false);
    }

    private void AttackUpAnimation() {
        this.ResetMoveAnimation();
        this.ChangeAnimationFlag(this._attackUpAnimationFlag, true);
        this.ChangeAnimationFlag(this._attackHorizontalAnimationFlag, false);
        this.ChangeAnimationFlag(this._attackDownAnimationFlag, false);
    }

    public void DisableAttack() {
         this.ChangeAnimationFlag(this._attackHorizontalAnimationFlag, false);
         this.ChangeAnimationFlag(this._attackUpAnimationFlag, false);
         this.ChangeAnimationFlag(this._attackDownAnimationFlag, false);
         this._isAttacking = false;
    }

    // Getting animation name becuase be are generic, yay
    protected void Attack() {
        this._isAttacking = true;

        float castDistance = 1f; // How far the box is cast
        Vector2 boxSize = new Vector2(this._attackSize, this._attackSize);

        Vector2 direction = transform.right; // default

        switch(this._lastMoveDirection) {
            case Direction.Right:
                this.AttackHorizontalAnimation();
                direction = transform.right;
                break;
            case Direction.Left:
                this.AttackHorizontalAnimation();
                direction = -transform.right;
                break;
            case Direction.Up:
                this.AttackUpAnimation();
                direction = transform.up;
                break;
            case Direction.Down:
                this.AttackDownAnimation();
                direction = -transform.up;
                break;
            default:
                this.AttackHorizontalAnimation();
                direction = transform.right;
                break;
        }
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, 0f, direction, castDistance, this._layerMask);
         if (hit.collider != null) {
            // Getting game object
            GameObject hitGameObject = hit.collider.gameObject;

            if(hitGameObject.CompareTag("Enemy") || hitGameObject.CompareTag("Player")) {
                Entity entity = hitGameObject.GetComponent<Entity>();
                entity.TakeDamage(this._damage);
            } 
         }
    }

    public void TakeDamage(int dmg) {
        this._health -= dmg;

        if(this._health <= 0) {
            this.OnDeath();
        } else {
            this.ChangeAnimationFlag(this._hitAnimationFlag, true);
        }
        
    }

    public void DisableHitAnimation() {
        this.ChangeAnimationFlag(this._hitAnimationFlag, false);
    }

    // Changing animcation flag if not default
    protected void ChangeAnimationFlag(string flagName, bool flag) {
        if(flagName != "default") {
            this._animator.SetBool(flagName, flag);
        }
    }

    protected abstract void OnDeath();
}
