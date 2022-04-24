﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformState : MonoBehaviour
{
    public bool isTransformed;

    private int _transformTime;

    public PowerUp _powerUp;

    [SerializeField] private float _tMoveSpeed;
    [SerializeField] private float _tWalkSpeed;
    [SerializeField] private float _tRunSpeed;

    public float _tJumpHeight = 4f;

    private Vector3 moveDirection;
    Vector3 velocity;

    public Transform groundCheck;
    [SerializeField] bool isGrounded;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float gravity = -9.81f;

    private CharacterController _controller;
    private Animator _anim;

    [SerializeField] GameObject artToDisable = null;
    [SerializeField] GameObject artToEnable = null;

    public bool isJumping;
    public bool isRunning;
    public bool isCharging;
    public bool isAttacking;
    private bool isShooting;

    //public PowerUp _powerUp;
    public int _energy = 0;
    public int currentEnergy;
    public int maxEnergy = 200;

    // Start is called before the first frame update
    void Start()
    {
        isTransformed = false;
        _controller = GetComponent<CharacterController>();
        _anim = GetComponentInChildren<Animator>();
        currentEnergy = _energy;
        //_powerUp.SetMaxEnergy(maxEnergy);
    }

    // Update is called once per frame
    void Update()
    {
        if(_powerUp.slider.value >= 200)
        {
            //_powerUp.SetEnergy(currentEnergy);
            isTransformed = true;
            NewMove();
            Debug.Log("Is Transformed");
        }
        if(_powerUp.slider.value <= 0)
        {
            artToDisable.SetActive(false);
            artToEnable.SetActive(true);
            _powerUp.EnergyReset(currentEnergy);
        }

    }

    public void NewMove()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        float z = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");

        moveDirection = new Vector3(x, 0, z);

        if (isGrounded)
        {
            Debug.Log("Transform state is grounded");

            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                NewWalk();
            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                SuperRun();
            }
            else if (moveDirection == Vector3.zero)
            {
                SuperIdle();
            }

            moveDirection *= _tMoveSpeed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SuperJump();
                Debug.Log("Super Jump");
            }
        }


        _controller.Move(moveDirection * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        _controller.Move(velocity * Time.deltaTime);
    }

    private void SuperIdle()
    {
        _anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
        _anim.SetBool("isRunning", false);
        _anim.SetBool("isJumping", false);
    }

    private void NewWalk()
    {
        _tMoveSpeed = _tWalkSpeed;
        _anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
        _anim.SetBool("isRunning", false);
    }

    private void SuperRun()
    {
        _tMoveSpeed = _tRunSpeed;
        _anim.SetBool("isRunnning", true);
    }

    private void SuperJump()
    {
        velocity.y = Mathf.Sqrt(_tJumpHeight * -2f * gravity);
        _anim.SetBool("isJumping", true);
    }

    /*
    private IEnumerator SuperAttack()
    {
        _anim.SetLayerWeight(_anim.GetLayerIndex("AttackLayer"), 1);
        _anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.9f);
        _anim.SetLayerWeight(_anim.GetLayerIndex("AttackLayer"), 0);
    }
    */
}
