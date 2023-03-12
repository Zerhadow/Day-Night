using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Camera cam = null;
    [SerializeField] Transform groundCheck = null;

    [SerializeField] float maxVelocity = 5f;
    [SerializeField] float timeToMax = 0.25f;
    [SerializeField] float jumpHeight = 1f;
    [SerializeField] float jumpDuration = 0.25f;

    CharacterController controller;

    float gravity = 0f;
    float acceleration = 0f;
    public Vector3 velocity2 = Vector3.zero;
    Vector3 velocityZ = Vector3.zero;
    Vector3 jumpVector = Vector3.zero;

    LayerMask groundMask;
    float groundCheckDist = 0.42f;
    bool grounded = false;
    bool canJump = false;

    #region Control Inputs
    KeyCode LEFT = KeyCode.A;
    KeyCode RIGHT = KeyCode.D;
    KeyCode UP = KeyCode.W;
    KeyCode DOWN = KeyCode.S;
    KeyCode JUMP = KeyCode.Space;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        float tempDuration = jumpDuration / 2;
        groundMask = LayerMask.GetMask("Ground");
        controller = GetComponent<CharacterController>();
        acceleration = maxVelocity / timeToMax;
        gravity = 2 * jumpHeight / (tempDuration * tempDuration);
        jumpVector = new Vector3(0, 2 * jumpHeight / tempDuration, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        Vector3 direction = Vector3.zero;

        // Get inputs
        if (Input.GetKey(UP))
        {
            //Debug.Log("FORWARD");
            direction.z = direction.z + 1;
        }
        if (Input.GetKey(DOWN))
        {
            direction.z = direction.z - 1;
        }
        if (Input.GetKey(LEFT))
        {
            direction.x = direction.x - 1;
        }
        if (Input.GetKey(RIGHT))
        {
            direction.x = direction.x + 1;
        }

        // Change player's velocity based off inputs
        if (direction == Vector3.zero && velocity2.magnitude > 0)
        {
            velocity2 += velocity2.normalized * -acceleration * Time.deltaTime;
            if (velocity2.magnitude < 0.005)
                velocity2 = Vector3.zero;
        }
        else
        {
            velocity2 += direction.normalized * acceleration * Time.deltaTime;
            if (velocity2.magnitude > maxVelocity)
                velocity2 = velocity2.normalized*maxVelocity;
        }

        // Move the player
        Quaternion xRotation = Quaternion.AngleAxis(cam.GetYRotation(), Vector3.up);
        controller.Move(xRotation * velocity2 * Time.deltaTime);

    }

    void Jump()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundCheckDist, groundMask);
        canJump = Physics.CheckSphere(groundCheck.position, groundCheckDist + 0.05f, groundMask);

        if (Input.GetKey(JUMP) && canJump)
        {
            velocityZ = jumpVector;
        }
        else
        {
            if (!grounded)
            {
                velocityZ += Vector3.down * gravity * Time.deltaTime;
            }
            else
            {
                velocityZ = Vector3.zero;
            }
        }

        controller.Move(velocityZ * Time.deltaTime);
    }
}
