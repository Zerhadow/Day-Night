using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Camera cam = null;

    [SerializeField] float maxVelocity = 5f;
    [SerializeField] float timeToMax = 0.25f;

    Rigidbody body;

    float acceleration = 0f;
    Vector3 velocity = Vector3.zero;

    #region Control Inputs
    KeyCode LEFT = KeyCode.A;
    KeyCode RIGHT = KeyCode.D;
    KeyCode UP = KeyCode.W;
    KeyCode DOWN = KeyCode.S;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        acceleration = maxVelocity / timeToMax;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

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
        if (direction == Vector3.zero && velocity.magnitude > 0)
        {
            velocity += velocity.normalized * -acceleration * Time.deltaTime;
            if (velocity.magnitude < 0)
                velocity = Vector3.zero;
        }
        else
        {
            velocity += direction.normalized * acceleration * Time.deltaTime;
            if (velocity.magnitude > maxVelocity)
                velocity = velocity.normalized*maxVelocity;
        }

        // Move the player
        Quaternion xRotation = Quaternion.AngleAxis(cam.GetYRotation(), Vector3.up);
        body.velocity = xRotation * velocity;

    }

}
