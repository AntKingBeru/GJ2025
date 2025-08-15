using UnityEngine;

public class Player : Entity
{

    [SerializeField] private string _deathAnimationFlag = "Death";
        
    private GameObject _carriedObject;
    
    //     if (carriedObject == null)
    // {
    //     // Logic to detect and pick up an object (e.g., using OnTriggerStay2D and checking tags)
    //     // For simplicity, let's assume 'objectToPickUp' is already determined
    //     GameObject objectToPickUp = FindObjectOfType<PickableObject>().gameObject; // Example
    //     if (objectToPickUp != null)
    //     {
    //         carriedObject = objectToPickUp;
    //         carriedObject.transform.SetParent(holdPoint);
    //         carriedObject.transform.localPosition = Vector3.zero; // Adjust as needed
    //         // If it has a Rigidbody2D, disable physics
    //         Rigidbody2D rb = carriedObject.GetComponent<Rigidbody2D>();
    //         if (rb != null) rb.isKinematic = true;
    //     }
    // }
    // else
    // {
    //     // Drop the object
    //     Rigidbody2D rb = carriedObject.GetComponent<Rigidbody2D>();
    //     if (rb != null) rb.isKinematic = false;
    //     carriedObject.transform.SetParent(null);
    //     carriedObject = null;
    // }


    // Update is called once per frame
    void Update()
    {
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
            base.Attack();
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeadEnemy"))
        {
            this.PickUp(other.gameObject);
            Debug.Log("Got dead enemy");
        }
        // Debug.Log(col.gameObject.name + " : " + gameObject.name);
        // Debug.Log("Hello, it triggered");
    }

    private void PickUp(GameObject objectToPickUp)
    {
        if (_carriedObject == null)
        {
            // Logic to detect and pick up an object (e.g., using OnTriggerStay2D and checking tags)
            // For simplicity, let's assume 'objectToPickUp' is already determined
            if (objectToPickUp != null)
            {
                _carriedObject = objectToPickUp;
                _carriedObject.transform.SetParent(transform); // My position
                _carriedObject.transform.localPosition = Vector3.zero; // Adjust as needed
                // If it has a Rigidbody2D, disable physics
                Rigidbody2D rb = _carriedObject.GetComponent<Rigidbody2D>();
                if (rb != null) rb.isKinematic = true;
            }
        }
    }

    protected override void OnDeath() {
        Debug.Log("Died!!");
        this.ChangeAnimationFlag(this._deathAnimationFlag, true);
    }
}
