using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMove : MonoBehaviour
{
    private CharacterController controller;
    //private FixedJoystick joystick;
    private Vector3 velocity;
    private bool isGrounded;
    private float gravity = -9.81f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        //joystick = GameObject.FindWithTag("GameController").GetComponent<FixedJoystick>();
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Ãæµ¹");    
    }

    void FixedUpdate()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        //Vector3 move = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        Vector3 move = new Vector3(0, 0, 0);
        controller.Move(move * Time.deltaTime * 1);

        if (move != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(move);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y += Mathf.Sqrt(1 * -3.0f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
