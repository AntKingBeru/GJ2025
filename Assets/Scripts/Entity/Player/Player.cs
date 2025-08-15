using UnityEngine;

public class Player : Entity
{
    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    //     Debug.Log("BaseClass Start called.");
    // }


    // Update is called once per frame
    void Update()
    {
        // Get input for horizontal and vertical axes
        float horizontalInput = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow keys
        float verticalInput = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow keys

        if(horizontalInput > 0.01) {
            horizontalInput = 1;
        } else if (horizontalInput < -0.01) {
            horizontalInput = -1;
        }

        if(verticalInput > 0.01) {
            verticalInput = 1;
        } else if (verticalInput < -0.01) {
            verticalInput = -1;
        }

        base.Move(horizontalInput, verticalInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            base.Attack();
        }
    }

    protected override void OnDeath() {
        Debug.Log("Died!!");
        Destroy(gameObject);
    }
}
