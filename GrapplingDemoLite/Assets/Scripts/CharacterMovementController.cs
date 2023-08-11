using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    private float currentSpeed = 0;
    public float moveSpeed = 5f;
    public float runSpeed = 10f;

    private Vector3 moveDirection;

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(x, 0, z).normalized;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        Vector3 velocity = moveDirection * currentSpeed * Time.deltaTime;

        transform.position += velocity;
    }

}