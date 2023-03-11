using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Camera cam = null;

    [SerializeField] float topSpeed = 5f;
    [SerializeField] float timeToTopSpeed = 0.25f;

    Rigidbody body;

    float acceleration = 10f;

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
    }

    // Update is called once per frame
    void Update()
    {
        Move();

    }

    void Move()
    {
        Vector3 direction = Vector3.zero;

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

        //Debug.Log(body.rotation);

        Quaternion xRotation = Quaternion.AngleAxis(cam.GetYRotation(), Vector3.up);
        body.velocity = xRotation * direction.normalized * topSpeed;

    }

}
