﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;
    public float jumpHeight;
    public float gravity = -9.81f;

    public float turnSmoothTime = 0.1f;
    private bool moving;
    private bool jumping;

    float turnSmoothVelocity;
    Vector3 velocity;
    bool IsGrounded;

    public Transform GroundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    // Update is called once per frame
    void Update()
    {
        IsGrounded = Physics.CheckSphere(GroundCheck.position, groundDistance, groundMask);

        if (IsGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (IsGrounded)
        {
            if (Input.GetButtonDown("Jump") && IsGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }
            jumping = false;
        }
        if (IsGrounded == false)
        {
            jumping = true;
        }

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            moving = true;
        }
        else
        {
            moving = false;
        }
    }

    public bool isMoving()
    {
        return moving;
    }

    public bool isJumping()
    {
        return jumping;
    }
}
