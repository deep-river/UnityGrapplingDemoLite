using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    private float currentSpeed = 0;
    public float moveSpeed = 5f;
    public float runSpeed = 10f;

    public float RotationSmoothTime = 0.12f;
    private float targetRotation = 0.0f;
    private float rotationVelocity;

    private Vector3 moveDirection;

    private Vector3 gravity = new Vector3(0, -15f, 0);

    private CharacterController controller;

    [HideInInspector] public bool freeze = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!freeze)
        {
            Move();
        }
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(x, 0, z).normalized;
        if (controller.velocity != Vector3.zero)
        {
            targetRotation = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Vector3.zero;
        if (moveDirection != Vector3.zero)
        {
            targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        controller.Move(targetDirection.normalized * (currentSpeed * Time.deltaTime) + gravity * Time.deltaTime);
    }

    public IEnumerator LaunchToPosition(Vector3 targetPos, Vector3 offset, float travelDuration)
    {
        float startTime = Time.time;

        while (Time.time < startTime + travelDuration)
        {
            float t = (Time.time - startTime) / travelDuration;

            // gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPos + offset, t);
            gameObject.transform.position = Vector3.LerpUnclamped(gameObject.transform.position, targetPos + offset, Mathf.SmoothStep(0, 1, t));

            yield return null;
        }
        // gameObject.transform.position = targetPos;
    }
}