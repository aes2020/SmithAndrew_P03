﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;

    [SerializeField] private float _tMoveSpeed;
    [SerializeField] private float _tWalkSpeed;
    [SerializeField] private float _tRunSpeed;

    public float _tJumpHeight = 4f;

    public float jumpHeight = 3f;

    private Vector3 moveDirection;
    Vector3 velocity;

    public Transform groundCheck;
    [SerializeField] bool isGrounded;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float gravity = -9.81f;

    private CharacterController _controller;
    private Animator _anim;

    public TransformState _transformState;

    public bool notTransformed;

    public PowerUp _powerUp;
    public int _energy = 0;
    public int currentEnergy;
    public int maxEnergy = 200;

    public bool isTransformed;

    private int _transformTime;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponentInChildren<Animator>();
        currentEnergy = _energy;
        isTransformed = false;
        notTransformed = true;
        //_powerUp.SetMaxEnergy(maxEnergy);
    }

    // Update is called once per frame
    private void Update()
    {
        Move();

        if(Input.GetKeyDown(KeyCode.P))
        {
           StartCoroutine(Attack());
        }

        if (Input.GetButton("Submit") && isGrounded)
        {
            GainEnergy(1);
            _powerUp.SetEnergy(currentEnergy);

            Charge();
            //_powerUp.SetMaxEnergy(100);
        }
        
        if(_powerUp.slider.value >= maxEnergy)
        {
            notTransformed = false;
            _transformState.isTransformed = true;
            _transformState.Move();
            //Move().enabled(false);
        }
        
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        float z = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");

        moveDirection = new Vector3(x, 0, z);

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
        _anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    private void Walk()
    {
        _moveSpeed = _walkSpeed;
        _anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }

    private void Run()
    {
        _moveSpeed = _runSpeed;
        //_anim.SetBool("Run", 1, 0.1f, Time.deltaTime);
        _anim.SetBool("Run",true);
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        _anim.SetFloat("Height", 0.5f, 0.1f, Time.deltaTime);

    }

    private IEnumerator Attack()
    {
        _anim.SetLayerWeight(_anim.GetLayerIndex("AttackLayer"), 1);
        _anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.9f);
        _anim.SetLayerWeight(_anim.GetLayerIndex("AttackLayer"), 0);
    }

    private IEnumerator Charge()
    {
        //_anim.SetLayerWeight(_anim.GetLayerIndex("AttackLayer"), 1);
        _anim.SetTrigger("Charge");

        yield return new WaitForSeconds(0.9f);
        //_anim.SetLayerWeight(_anim.GetLayerIndex("AttackLayer"), 0);
    }

    public void GainEnergy(int gain)
    {
        currentEnergy += gain;

        _powerUp.SetEnergy(currentEnergy);
    }

    
}
