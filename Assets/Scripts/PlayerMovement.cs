using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forceMagnitude = 850f;
    [SerializeField] private float maxVelocity = 6;
    [SerializeField] private float rotationSpeed = 50f;

    private Rigidbody rb;
    private Camera mainCamera;

    private Vector3 movementDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        ProcessInput();
        KeepPlayerOnScreen();
        RotateToFaceVelocity();
    }

    private void RotateToFaceVelocity()
    {
        if (rb.velocity == Vector3.zero) { return; }
        Quaternion targetRotation = Quaternion.LookRotation(rb.velocity, Vector3.back);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void KeepPlayerOnScreen()
    {
        Vector3 newPosition = transform.position;

        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        if (viewportPosition.x > 1) { newPosition.x = -newPosition.x + 0.1f; }
        if (viewportPosition.x < 0) { newPosition.x = -newPosition.x - 0.1f; }
        if (viewportPosition.y > 1) { newPosition.y = -newPosition.y + 0.1f; }
        if (viewportPosition.y < 0) { newPosition.y = -newPosition.y - 0.1f; }

        transform.position = newPosition;
    }

    private void ProcessInput()
    {
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            // Get the touch position and convert it to Screen World Position
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

            // Get the difference and set the z to zero and normalize it
            movementDirection = transform.position - worldPosition;
            movementDirection.z = 0f;
            movementDirection.Normalize();
        }
        else
        {   // no touch no direction
            movementDirection = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        // if no touch then to force added. slows down according to drag
        if (movementDirection == Vector3.zero) { return; }

        // Add force and clamp the max velocity
        rb.AddForce(movementDirection * forceMagnitude * Time.fixedDeltaTime, ForceMode.Force);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }
}
