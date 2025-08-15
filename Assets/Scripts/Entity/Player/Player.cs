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

        base.Move(horizontalInput, verticalInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            base.Attack();
        }
    }

    protected override void OnDeath() {
        Debug.Log("Died!!");
    }
}
