using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;

    
    [SerializeField] private float _tMoveSpeed;
    [SerializeField] private float _tWalkSpeed;
    [SerializeField] private float _tRunSpeed;
    
    [SerializeField] private float _flySpeed;


    [SerializeField] ParticleSystem _chargeParticle = null;
    [SerializeField] AudioSource _chargeAudio = null;
    [SerializeField] AudioClip _chargeSFX = null;

    [SerializeField] ParticleSystem _superParticle = null;

    [SerializeField] AudioSource _transformAudio = null;
    [SerializeField] AudioClip _SuperSFX = null;

    public GameObject artToDisable = null;
    public GameObject artToEnable = null;

    public float _tJumpHeight = 5f;

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

    public bool isJumping;
    public bool isRunning;
    public bool isCharging;
    public bool isAttacking;
    private bool isShooting;
    public bool isFlying;

    public bool notTransformed;

    //[SerializeField] private float timerSpeed = 2f;

    private float elapsed;

    public Image fill;
    public Gradient gradient;

    public PowerUp _powerUp;
    public float _energy = 0;
    public int currentEnergy;
    public int maxEnergy = 200;

    public bool isTransformed;

    private int _transformTime;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponentInChildren<Animator>();
        //GameObject.Find("SuperChar(1)");
        //currentEnergy = _energy;
        _superParticle.Stop();
        isTransformed = false;
        notTransformed = true;
        //_powerUp.SetMaxEnergy(maxEnergy);
    }

    // Update is called once per frame
    private void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.P))
        {
            //StartCoroutine(Attack());
            Kick();
            _anim.SetBool("isAttacking", true);
            Debug.Log("Attack");
        }
        /*
        if (Input.GetKeyDown(KeyCode.O) && isTransformed)
        {
            Projectile();
            _anim.SetBool("isShooting", true);
            Debug.Log("Projectile Shot");
        }
        */
            if (Input.GetButton("Fire1") && isGrounded)
        {
            PlayChargeSound();
            ChargeParticle();
            GainEnergy(1);
            _powerUp.SetEnergy(currentEnergy);
            _anim.SetBool("isCharging", true);
            Charge();
            //_powerUp.SetMaxEnergy(100);
        }
            else if(!Input.GetButton("Fire1") && isGrounded)
        {
            //NotCharging();
        }
        /*
        if(_powerUp.slider.value >= maxEnergy)
        {
            notTransformed = false;
            _transformState.isTransformed = true;
            _transformState.Move();
            //Move().enabled(false);
        }
        */
        if (_powerUp.slider.value >= 200 & notTransformed)
        {
            _superParticle.Play();
            GameObject.Find("SuperChar(1)");
            //NewMove();
            //_powerUp.SetEnergy(currentEnergy);
            notTransformed = false;
            isTransformed = true;
           
            //_anim.SetBool("isTransformed", true);

            //_powerUp.EnergyReset(currentEnergy);

            PlayTransformSound();

            artToDisable.SetActive(false);
            artToEnable.SetActive(true);

            // EnergyDecrease(10);
            //_powerUp.slider.value(currentEnergy);
            _powerUp.SetEnergy(currentEnergy);
            //LoseEnergy(1);
            Debug.Log("Is Transformed");
        }
        if(_powerUp.slider.value <= 100)
        {
            notTransformed = true;
            isTransformed = false;

            artToDisable.SetActive(true);
            artToEnable.SetActive(false);
            _superParticle.Stop();
            _chargeParticle.Stop();
            Move();

            if (Input.GetButton("Fire1") && isGrounded)
            {
                PlayChargeSound();
                ChargeParticle();
                GainEnergy(1);
                _powerUp.SetEnergy(currentEnergy);
                _anim.SetBool("isCharging", true);
                Charge();
                //_powerUp.SetMaxEnergy(100);
            }
            /*
            Move();
            isTransformed = false;
            notTransformed = true;


            artToDisable.SetActive(true);
            artToEnable.SetActive(false);

            GainEnergy(1);
            Charge();
            _powerUp.SetEnergy(currentEnergy);
            //artToDisable.SetActive(true);
            */
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
            _anim = GetComponentInChildren<Animator>();
            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
                _anim.SetBool("isRunning", false);
            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                Run();
                _anim.SetBool("isRunning", true);
            }
            else if (moveDirection == Vector3.zero)
            {
                Idle();
                _anim.SetBool("isRunning", false);
                _anim.SetBool("isJumping", false);
            }

            moveDirection *= _moveSpeed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
                Debug.Log("Jumped");
                _anim.SetBool("isJumping", true);
            }
            else if (isTransformed == true)
            {
                //_anim = GetComponentInChildren<Animator>();
                NewMove();
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
        //_anim.SetFloat("Run", 1, 0.1f, Time.deltaTime);
        //_anim.SetBool("Run",true);
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        //_anim.SetFloat("Height", 0.5f, 0.1f, Time.deltaTime);

    }

    private void Kick()
    {
        _anim.SetBool("isAttacking", true);
    }
    /*
    private void Projectile()
    {
        _anim.SetBool("isShooting", true);
    }
    */
    public void NewMove()
    {
        _anim = GetComponentInChildren<Animator>();
        if (isTransformed)
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
                if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
                {
                    NewWalk();
                    //_anim.SetBool("isTransformed", true);

                    _anim.SetBool("isFlying", false);
                    Debug.Log("Ninja Run");
                }
                else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
                {
                    _anim.SetBool("isFlying", true);
                    //gravity = 0;
                    Fly();
                    //_anim.SetBool("isTransformed", true);                   
                    Debug.Log("Flying");
                }
                else if (moveDirection == Vector3.zero)
                {
                    SuperIdle();
                    //_anim.SetBool("isTransformed", true);
                    _anim.SetBool("isFlying", false);
                    //_anim.SetBool("isJumping", false);
                }

                moveDirection *= _tMoveSpeed;

            }


            _controller.Move(moveDirection * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;
            _controller.Move(velocity * Time.deltaTime);
        }
    }

    private void SuperIdle()
    {
        _anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
        //_anim.SetBool("isFlying", false);
        //_anim.SetBool("isJumping", false);
    }

    private void NewWalk()
    {
        _tMoveSpeed = _tWalkSpeed;
        _anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
        //_anim.SetBool("isFlying", false);
    }

    private void Fly()
    {
        _tMoveSpeed = _flySpeed;
        //velocity.y = Mathf.Sqrt(_tJumpHeight * -2f * 1);
        //_anim.SetFloat("SuperSpeed", 0.5f, 0.1f, Time.deltaTime);
        //_anim.SetBool("isFlying", true);
        //_anim.SetBool("isRunning", true);
    }

    /*
    private IEnumerator Attack()
    {
        _anim.SetLayerWeight(_anim.GetLayerIndex("AttackLayer"), 1);
        _anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.9f);
        _anim.SetLayerWeight(_anim.GetLayerIndex("AttackLayer"), 0);
    }
    */
    private IEnumerator Charge()
    {
        //_anim.SetLayerWeight(_anim.GetLayerIndex("AttackLayer"), 1);
        //_anim.SetTrigger("Charge");
        _anim.SetBool("isCharging",true);

        yield return new WaitForSeconds(0.9f);
        //_anim.SetLayerWeight(_anim.GetLayerIndex("AttackLayer"), 0);
    }

    public void GainEnergy(int gain)
    {
        currentEnergy += gain;

        _powerUp.SetEnergy(currentEnergy);
    }


    /*
    private void SuperRun()
    {
        _tMoveSpeed = _tRunSpeed;
        _anim.SetBool("isRunning",true);
    }
   
    private void SuperJump()
    {
        velocity.y = Mathf.Sqrt(_tJumpHeight * -2f * gravity);
        _anim.SetBool("isJumping", true);
    }
    */
    /*
    private void EnergyDecrease(float power)
    {
        if (_powerUp.slider.value >= 200 && isTransformed)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= timerSpeed)
            {
                elapsed = 0f;
                //_powerUp.slider.value--;
                _powerUp.slider.value = _energy;

                _energy -= power;
                fill.fillAmount = _energy / 100f;

                //_powerUp.SetEnergy(currentEnergy);

                Debug.Log("Energy is dropping");
            }
        }
    }
    */
    
    public void LoseEnergy(int loss)
    {
        currentEnergy -= loss;

        _powerUp.SetEnergy(currentEnergy);
    }

    /*
    public void SetMinEnergy(int energy)
    {
        slider.maxValue = energy;
        slider.value = energy;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetEnergy(int energy)
    {
        slider.value = energy;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
    */

    void ChargeParticle()
    {
        _chargeParticle.Play();
    }

    void TransformParticle()
    {
        _superParticle.Play();
    }

    void PlayChargeSound()
    {
        _chargeAudio.clip = _chargeSFX;
        _chargeAudio?.Play();
    }

    void PlayTransformSound()
    {
        _transformAudio.clip = _SuperSFX;
        _transformAudio?.Play();
    }

}
