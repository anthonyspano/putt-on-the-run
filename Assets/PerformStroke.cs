using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerformStroke : MonoBehaviour
{
    private Rigidbody rb;
    bool busy;
    private Camera camera;
    Vector3 cameraDirection;

    public Slider strokeForce;
    public float rotateSpeed;
    public GameObject target;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        camera = Camera.main;
    }

    void Update()
    {
        // when the player clicks the mouse, force will be added to the x,z direction the camera is looking
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            if(!busy && BallIsResting())
            {
                busy = true;
                AttemptStroke();
                
            }
        }

        if(!busy)
        {
            float targetDirection = Input.GetAxis("Horizontal");
            if(targetDirection != 0)
            {
                // rotate the ball accordingly
                var targetPosition = transform.position + new Vector3(0, targetDirection, 0);
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetPosition, rotateSpeed * Time.deltaTime, 0.0f);

                //transform.rotation = Quaternion.LookRotation(newDirection);
                transform.Rotate(0, targetDirection * rotateSpeed * Time.deltaTime, 0);
            }
        }

        
        if(BallIsResting() && IsGrounded())
            rb.velocity = Vector3.zero;


    }

    void AttemptStroke()
    {
        var cameraDirection = camera.transform.forward;
        var strokeDirection = new Vector3(cameraDirection.x, 0 , cameraDirection.z).normalized;
        rb.AddForce(strokeDirection * strokeForce.value, ForceMode.Impulse);
        busy = false;
    }

    bool BallIsResting()
    {
        return rb.velocity.magnitude < 1f;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1f);
    }
}
