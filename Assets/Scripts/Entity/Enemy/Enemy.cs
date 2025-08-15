using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private string _deathAnimationFlag = "Death";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnDeath() {
        this.ChangeAnimationFlag(this._deathAnimationFlag, true);
    }
}
