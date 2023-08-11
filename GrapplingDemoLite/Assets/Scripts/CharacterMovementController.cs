using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    private float currentSpeed = 0;
    public float moveSpeed = 5f;
    public float runSpeed = 10f;

    public float RotationSmoothTime = 0.12f;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;

    private Vector3 moveDirection;

    CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(x, 0, z).normalized;

        Debug.Log(controller.velocity);
        if (controller.velocity != Vector3.zero)
        {
            _targetRotation = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg +
                              Camera.main.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Vector3.zero;
        if (moveDirection != Vector3.zero)
        {
            targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        controller.Move(targetDirection.normalized * (currentSpeed * Time.deltaTime));
    }
}