using UnityEngine;

public class Player : Entity
{

    [SerializeField] private string _deathAnimationFlag = "Death";
    private string _fixAnimationFlag = "Fix";
    private bool _isFixing;
    private bool _fixInterrupted;
        
    private GameObject _carriedObject;
    private GameObject _currentFix;


    // Update is called once per frame
    void Update()
    {
        if (GameManager.pause)
        {
            this.ResetMoveAnimation();
            return;
        }
        
        if (this._isFixing) return; // Skip iteration while fixing
        
        if(this.isHit) {
            this.ResetMoveAnimation();
            this.DisableAttack();
            return;
        }

        // Get input for horizontal and vertical axes
        float horizontalInput = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow keys
        float verticalInput = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow keys

        if(horizontalInput > 0.01) horizontalInput = 1;
         else if (horizontalInput < -0.01) horizontalInput = -1;

        if(verticalInput > 0.01) verticalInput = 1;
        else if (verticalInput < -0.01) verticalInput = -1;

        base.Move(horizontalInput, verticalInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.instance.PlaySound("Test");
            base.Attack();
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeadEnemy"))
        {
            PickupPipe(other.gameObject);
        }
        else if (other.CompareTag("SpawnPoint") && _carriedObject != null)
        {
            this._isFixing = true;
            this._currentFix = other.gameObject;
            this.ChangeAnimationFlag(this._fixAnimationFlag, true);
        }
    }
    
    private void PickupPipe(GameObject objectToPickUp)
    {
        if (_carriedObject == null)
        {
            // Logic to detect and pick up an object (e.g., using OnTriggerStay2D and checking tags)
            // For simplicity, let's assume 'objectToPickUp' is already determined
            if (objectToPickUp != null)
            {
                _carriedObject = objectToPickUp;
                _carriedObject.transform.SetParent(transform); // My position
                _carriedObject.transform.localPosition = new Vector3(0f, 0.5f, 0f); // Adjust as needed
                // If it has a Rigidbody2D, disable physics
                Rigidbody2D rb = _carriedObject.GetComponent<Rigidbody2D>();
                if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }
    }

    private void DropPipe()
    {
        if (_carriedObject != null)
        {
            // Drop the object
            Rigidbody2D rb = _carriedObject.GetComponent<Rigidbody2D>();
            if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
            _carriedObject.transform.SetParent(null);
            _carriedObject = null;
        }

    }

    public void DisableFixAnimation()
    {
        this.ChangeAnimationFlag(this._fixAnimationFlag, false);
        this._isFixing = false;
        if(_currentFix != null)
        {
            SpawnPoint sp = _currentFix.GetComponent<SpawnPoint>();
            sp.Close();
            this.AddHealth(10);
            Destroy(_carriedObject);
            _carriedObject = null;
        } 
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        // Disabling fix on take damage
        if (this._isFixing)
        {
            this.ChangeAnimationFlag(this._fixAnimationFlag, false);
            this._isFixing = false;
        }

        DropPipe();
    }

    protected override void OnDeath() {
        this.ChangeAnimationFlag(this._deathAnimationFlag, true);
    }
}
