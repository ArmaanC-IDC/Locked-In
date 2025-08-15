using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public InputAction moveAction;
    public InputAction lookAction;
    public InputAction upDownAction;

    public float moveSpeed = 10f;
    public float mouseSensitivity = 1f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    #region enable/disable actions
    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        upDownAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        upDownAction.Disable();
    }
    #endregion

    void Update()
    {
        //mouse movement
        Vector2 lookDelta = lookAction.ReadValue<Vector2>();
        rotationX += lookDelta.x * mouseSensitivity;
        rotationY -= lookDelta.y * mouseSensitivity;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f);
        transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);

        // x=left/right, y=forward/back
        Vector2 movement = moveAction.ReadValue<Vector2>();
        float upDown = upDownAction.ReadValue<float>();

        Vector3 move = (transform.forward * movement.y) + (transform.right * movement.x) + (transform.up * upDown);
        move = move.normalized;

        transform.position += move * moveSpeed * Time.deltaTime;

        if (Mouse.current.rightButton.isPressed)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
