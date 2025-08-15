using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private string _deathAnimationFlag = "Death";
    [SerializeField] private Transform _target;
    private bool wasInRadius = false;

    // Update is called once per frame
    void Update()
    {
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
        float distance = Vector3.Distance(transform.position, this._target.position);

        if(distance < this.castDistance) {
            if(!this.wasInRadius) this.Attack();
            this.wasInRadius = true;
        } else this.wasInRadius = false;
    }

    protected override void OnDeath() {
        this.ChangeAnimationFlag(this._deathAnimationFlag, true);
    }
}
