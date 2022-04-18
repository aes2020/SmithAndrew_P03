using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;

    public float jumpHeight = 3f;

    private Vector3 moveDirection;
    Vector3 velocity;

    public Transform groundCheck;
    [SerializeField] bool isGrounded;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float gravity = -9.81f;

    private CharacterController _controller;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        float z = Input.GetAxis("Vertical");

        moveDirection = new Vector3(0, 0, z);

        if(isGrounded)
        {
            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                Run();
            }
            else if (moveDirection == Vector3.zero)
            {
                Idle();
            }

            moveDirection *= _moveSpeed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
                Debug.Log("Jumped");
            }
        }
               

        _controller.Move(moveDirection * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        _controller.Move(velocity * Time.deltaTime);
    }

    private void Idle()
    {

    }

    private void Walk()
    {
        _moveSpeed = _walkSpeed;
    }

    private void Run()
    {
        _moveSpeed = _runSpeed;
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
}
