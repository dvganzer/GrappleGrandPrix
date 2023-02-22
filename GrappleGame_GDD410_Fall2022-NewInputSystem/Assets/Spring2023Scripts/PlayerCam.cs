using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerCam : MonoBehaviour
{
    [Header("UI")]
    [SerializedField] public Text SenseText;

    [Header("Camera")]
    //Sensitivty Variables 
    public float sensX;
    public float sensY;

    public Transform orientation;
    [SerializedField] public Vector2 cameraInput = Vector2.zero;
    //Camera Roation Variables
    float xRotation;
    float yRotation;

    private void Start()
    {
        //Locks Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        //Gets InputSystem for Controller and Mouse
        float mouseX = cameraInput.x * Time.deltaTime * sensX;
        float mouseY = cameraInput.y * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;       
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        //Rotates Camera and Orientation of Player

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

    }

    public void OnCamera(InputAction.CallbackContext context)
    {
        cameraInput = context.ReadValue<Vector2>();
    }

    public void SenseUp(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            sensX += .1f;
            sensY += .1f;
            SenseText.text = "SENSITIVITY:" + sensX * 10;
        }

    }
    public void SenseDown(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            sensX -= .1f;
            sensY -= .1f;
            SenseText.text = "SENSITIVITY:" + sensX * 10;
        }

    }
}
