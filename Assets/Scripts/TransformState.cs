using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformState : MonoBehaviour
{
    public bool isTransformed;

    private int _transformTime;

    public PowerUp _powerUp;

    [SerializeField] AudioSource _transformAudio = null;
    [SerializeField] AudioClip _SuperSFX = null;

    [SerializeField] private float _tMoveSpeed;
    [SerializeField] private float _tWalkSpeed;
    [SerializeField] private float _tRunSpeed;

    [SerializeField] private float _flySpeed;

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

        _anim.SetFloat("SuperSpeed", 0.5f, 0.1f, Time.deltaTime);
        _anim.SetBool("isTransformed", true);

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
            //isFlying = true;
            //_anim.SetBool("isFlying", true);
            //_anim.SetFloat("Fly", 0.5f, 0.1f, Time.deltaTime);
            //_anim.SetBool("isTransformed", true);


            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                NewWalk();
            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                Fly();
            }
            else if (moveDirection == Vector3.zero)
            {
                SuperIdle();
            }

            moveDirection *= _tMoveSpeed;
            /*
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SuperJump();
                Debug.Log("Super Jump");
            }
            */

        }

        _controller.Move(moveDirection * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        _controller.Move(velocity * Time.deltaTime);
    }

    private void SuperIdle()
    {
        _anim.SetFloat("SuperSpeed", 0, 0.1f, Time.deltaTime);
        _anim.SetBool("isFlying", false);
        //_anim.SetBool("isJumping", false);
    }

    private void NewWalk()
    {
        _tMoveSpeed = _tWalkSpeed;
        _anim.SetFloat("SuperSpeed", 0.5f, 0.1f, Time.deltaTime);
        _anim.SetBool("isFlying", false);
    }

    private void Fly()
    {
        _tMoveSpeed = _flySpeed;
        _anim.SetFloat("SuperSpeed", 0.5f, 0.1f, Time.deltaTime);
        _anim.SetBool("isFlying", true);
    }

    void PlayTransformSound()
    {
        _transformAudio.clip = _SuperSFX;
        _transformAudio?.Play();
    }
}
