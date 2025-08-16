using UnityEngine;
using System.Collections;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 1f;

    [SerializeField] private int _damage = 10;

    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private string _horizontalAnimationFlag = "default";
    [SerializeField] private string _upAnimationFlag = "default";
    [SerializeField] private string _downAnimationFlag = "default";
    [SerializeField] private string _attackHorizontalAnimationFlag = "default";
    [SerializeField] private string _attackUpAnimationFlag = "default";
    [SerializeField] private string _attackDownAnimationFlag = "default";
    [SerializeField] private string _hitAnimationFlag = "default";
    [SerializeField] protected float attackSize = 1.5f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] protected float castDistance = 0.5f; // How far the box is cast
    private int _health;
    protected bool isAttacking = false;
    protected bool isHit = false;
    public bool isDead
    {
        get => this._health <= 0;
    }
    
    public float currentHealthPercent {
        get => (float)this._health / (float)this._maxHealth;
    }

    protected enum Direction 
    {
        Right,
        Left,
        Up,
        Down
    }
    protected Direction lastMoveDirection = Direction.Right;
    protected Vector2 lastMoveVector;

    private SpriteRenderer _spriteRenderer; // For left/right/up/down move

    // For all animations
    protected Animator _animator;

    void Awake()
    {
        // Getting animator
        this._animator = GetComponent<Animator>();

        // Getting sprite renderer
        this._spriteRenderer = GetComponent<SpriteRenderer>();
        
        this._health = this._maxHealth;

    }

    protected void AddHealth(int amount)
    {
        this._health = Mathf.Min(this._maxHealth, this._health + amount);
    }

    protected void Move(float horizontal, float vertical) {
        // Create a movement vector based on input
        Vector2 movement = new Vector2(horizontal, vertical);
        lastMoveVector = movement;

        if(this.isAttacking) {
            // animator.SetBool(this.verticalAnimationFlag, false);
            this.ChangeAnimationFlag(this._horizontalAnimationFlag, false);
            return;
        }

        if (horizontal > 0)  {
            this.lastMoveDirection = Direction.Right;
            this.MoveRightAnimation();
        }
        else if (horizontal < 0) {
            this.lastMoveDirection = Direction.Left;
            this.MoveLeftAnimation();
        }
        else {
            this.ChangeAnimationFlag(this._horizontalAnimationFlag, false);
        }

        if (vertical > 0) {
            this.lastMoveDirection = Direction.Up;
            this.MoveUpAnimation();
        }
        else if (vertical < 0) {
            this.lastMoveDirection = Direction.Down;
            this.MoveDownAnimation();
        }
        else {
            this.ChangeAnimationFlag(this._upAnimationFlag, false);
            this.ChangeAnimationFlag(this._downAnimationFlag, false);
        }

        // Normalize the vector to prevent faster diagonal movement
        if (movement.magnitude > 1f) movement.Normalize();

        // Apply movement scaled by speed and time.deltaTime for frame-rate independence
        transform.Translate(movement * this.moveSpeed * Time.deltaTime);
    }
    
    // ================ Helper function for all movement start ================

    protected void ResetMoveAnimation() {
        this.ChangeAnimationFlag(this._upAnimationFlag, false);
        this.ChangeAnimationFlag(this._downAnimationFlag, false);
        this.ChangeAnimationFlag(this._horizontalAnimationFlag, false);
    }

    protected void MoveRightAnimation() {
        this.ChangeAnimationFlag(this._upAnimationFlag, false);
        this.ChangeAnimationFlag(this._downAnimationFlag, false);
        this.ChangeAnimationFlag(this._horizontalAnimationFlag, true);
        this._spriteRenderer.flipX = false; // Regular orientation
    }

    protected void MoveLeftAnimation() {
        this.ChangeAnimationFlag(this._upAnimationFlag, false);
        this.ChangeAnimationFlag(this._downAnimationFlag, false);
        this.ChangeAnimationFlag(this._horizontalAnimationFlag, true);
        this._spriteRenderer.flipX = true; // Flip orientation
    }

    private void MoveUpAnimation() {
        this.ChangeAnimationFlag(this._horizontalAnimationFlag, false);
        this.ChangeAnimationFlag(this._downAnimationFlag, false);
        this.ChangeAnimationFlag(this._upAnimationFlag, true);
    }

    private void MoveDownAnimation() {
        this.ChangeAnimationFlag(this._horizontalAnimationFlag, false);
        this.ChangeAnimationFlag(this._upAnimationFlag, false);
        this.ChangeAnimationFlag(this._downAnimationFlag, true);
    }

    // ================ Helper functions for attack ================

    private void AttackHorizontalAnimation() {
        this.ResetMoveAnimation();
        this.ChangeAnimationFlag(this._attackUpAnimationFlag, false);
        this.ChangeAnimationFlag(this._attackDownAnimationFlag, false);
        this.ChangeAnimationFlag(this._attackHorizontalAnimationFlag, true);
    }

    private void AttackDownAnimation() {
        this.ResetMoveAnimation();
        this.ChangeAnimationFlag(this._attackHorizontalAnimationFlag, false);
        this.ChangeAnimationFlag(this._attackUpAnimationFlag, false);
        this.ChangeAnimationFlag(this._attackDownAnimationFlag, true);
    }

    private void AttackUpAnimation() {
        this.ResetMoveAnimation();
        this.ChangeAnimationFlag(this._attackHorizontalAnimationFlag, false);
        this.ChangeAnimationFlag(this._attackDownAnimationFlag, false);
        this.ChangeAnimationFlag(this._attackUpAnimationFlag, true);
    }

    public void DisableAttack() {
         this.ChangeAnimationFlag(this._attackHorizontalAnimationFlag, false);
         this.ChangeAnimationFlag(this._attackUpAnimationFlag, false);
         this.ChangeAnimationFlag(this._attackDownAnimationFlag, false);
         this.isAttacking = false;
    }

    // Getting animation name becuase be are generic, yay
    protected void Attack() {
        this.isAttacking = true;

        switch(this.lastMoveDirection) {
            case Direction.Right:
                this.AttackHorizontalAnimation();
                break;
            case Direction.Left:
                this.AttackHorizontalAnimation();
                break;
            case Direction.Up:
                this.AttackUpAnimation();
                break;
            case Direction.Down:
                this.AttackDownAnimation();
                break;
            default:
                this.AttackHorizontalAnimation();
                break;
        }
       
    }

    private void CheckIfAttackHit() {
        Vector2 boxSize = new Vector2(this.castDistance, 0.5f);
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, 0f, lastMoveVector, this.castDistance, this._layerMask);

        if (hit.collider != null) {
            // Getting game object
            GameObject hitGameObject = hit.collider.gameObject;

            if(hitGameObject.CompareTag("Enemy") || hitGameObject.CompareTag("Player")) {
                Entity entity = hitGameObject.GetComponent<Entity>();
                if(!entity.isDead) entity.TakeDamage(this._damage);
                this.DisableAttack();
            } 
        }
    }

    public virtual void TakeDamage(int dmg) {
        this._health -= dmg;
        this.isHit = true;

        if(this.isDead) {
            this.OnDeath();
        } else {
            this.ChangeAnimationFlag(this._hitAnimationFlag, true);
        }
    }

    public void DisableHitAnimation() {
        this.ChangeAnimationFlag(this._hitAnimationFlag, false);
        this.isHit = false;
    }

    // Changing animation flag if not default
    protected void ChangeAnimationFlag(string flagName, bool on) {
        if(flagName != "default") {
            this._animator.SetBool(flagName, on);
        }
    }
    
    protected abstract void OnDeath();
}
