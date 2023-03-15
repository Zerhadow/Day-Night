using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] GameObject lookPoint;

    [SerializeField] float xSensitivity = 10f;
    [SerializeField] float ySensitivity = 10f;

    // Minimum and maximum look angle
    float yMax = 60f;
    float yMin = -60f;

    // Offsets used for camera shake
    private float xOffset = 0f;
    private float yOffset = 0f;

    private float xRotation = 0f;
    private float yRotation = 0f;
    Quaternion originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        originalRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the camera
        xRotation += Input.GetAxis("Mouse X") * xSensitivity;
        yRotation += Input.GetAxis("Mouse Y") * ySensitivity;
        yRotation = Mathf.Clamp(yRotation, yMin, yMax);
        Quaternion xQuaternion = Quaternion.AngleAxis(xRotation, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(yRotation, -Vector3.right);
        transform.position = lookPoint.transform.position;
        transform.localRotation = originalRotation * xQuaternion * yQuaternion;

    }

    public float GetYRotation()
    {
        Vector3 euler = transform.localRotation.eulerAngles;
        return euler.y;
    }

    public Quaternion GetRotation()
    {
        return transform.localRotation;
    }
}
