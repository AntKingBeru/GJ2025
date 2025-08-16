using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private GameObject _deadMePrefab;
    private Transform _target;
    private bool _wasInRadius;
    [SerializeField] private float _attackDistanceTrigger = 1.5f;
    
    
    void Start()
    {
        _target = FindFirstObjectByType<Player>().transform;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (GameManager.pause)
        {
            this.ResetMoveAnimation();
            return;
        }
        
        if(this.isHit) {
            this.ResetMoveAnimation();
            this.DisableAttack();
            return;
        }

        if(!this.isAttacking && !this.isDead && this._target != null) {
            this.MoveTowardsPlayer();
            this.CheckAttack();
        }
    }

    public override void TakeDamage(int dmg)
    {
        AudioManager.instance.PlaySound("EnemyBonk");
        base.TakeDamage(dmg);
    }

    private void MoveTowardsPlayer() {
        // Check if a target is assigned to prevent errors
        // Calculate the step to move towards the target
        float step = this.moveSpeed * Time.deltaTime;

        // Move the current object towards the target's position
        transform.position = Vector3.MoveTowards(transform.position, this._target.position, step);

        Vector3 directionToOther = (this._target.position - transform.position).normalized;

        float xValue = directionToOther.x;
        float yValue = directionToOther.y;

        if(Mathf.Abs(yValue) > Mathf.Abs(xValue)) {
            if(yValue > 0) this.lastMoveDirection = Direction.Up;
            else this.lastMoveDirection = Direction.Down;
        }

        else {
            if(xValue > 0) this.lastMoveDirection = Direction.Left;
            else this.lastMoveDirection = Direction.Right;
        }

        if(xValue > 0) this.MoveLeftAnimation();
        else this.MoveRightAnimation();
        
    }

    private void CheckAttack(){
        float distance = Vector2.Distance(transform.position, this._target.position);
        
        if(distance <= this._attackDistanceTrigger) {
            if(!this._wasInRadius) this.Attack();
            this._wasInRadius = true;
        } else this._wasInRadius = false;
    }

    protected override void OnDeath()
    {
        Instantiate(_deadMePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
