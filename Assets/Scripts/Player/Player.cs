using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField]
    private float _movespeed = 9.0f;
    [SerializeField]
    private float _gravity = 9.8f;
    [SerializeField]
    private float _maxHealth = 100f;
    private float _health;
    [SerializeField]
    private float _jumpSpeed = 10f;
    private float _verticalSpeed = 0f;
    [SerializeField]
    private float _crouchSlamSpeed = 20f;
    [SerializeField]
    private float _jetSpeedMultiplier = 3f;
    [SerializeField]
    private float _verticalJetSpeed = 3f;
    [SerializeField]
    private int _inAirJumps = 1;

    //dash related vars
    [SerializeField]
    private float _dashDistance = 10f;
    private const float _minHeldDuration = 0.2f;
    private float _jetHoldTime = 0f;
    private bool _jetHeld = false;
    private Vector3 _dashDirection;

    //Fuel related vars
    [SerializeField]
    private float _maxFuel = 100f;
    private float _fuelTank;
    [SerializeField]
    private float _fuelRechargeRate = 10f;

    [SerializeField]
    private float _fuelRechargeDelay = 3f;
    [SerializeField]
    private float _jetFuelDrainPerSec = 1f;
    [SerializeField]
    private float _dashFuelDrain = 10f;
    [SerializeField]
    private float _verticalJetCostMult = 2f;
    private bool _fuelAvailable = true;
    private bool _fuelInUse;
    private bool _canRecharge;
    private float _dJumpBoostDrain = 20f;

    private float InstantiationTimer = 0.069f;
    [SerializeField]
    private float _healRate = 10f;
    [SerializeField]
    private float _healFuelCost = 10f;
    
    [SerializeField]
    private GameObject _slamDirtPrefab;
    [SerializeField]
    private GameObject _jetTrailPrefab;
    [SerializeField]
    private GameObject _jumpEffect;
    [SerializeField]
    private GameObject _guns;
    [SerializeField]
    private GameObject _MeleeWeapon;

    private HealthBar _healthbar;
    private FuelBar _fuelBar;

    //Smashstuff :3
    [SerializeField]
    private float _slamRadius = 30f;
    [SerializeField]
    private float _slamPower = 70f;
    private bool _slamUsed = false;
    [SerializeField]
    private float _slamCooldown = 2f;

    public Animator _animator;
    public Slider healthSlider;
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    public DamageEffect dmgfx;

    [SerializeField]
    private DamageEffect _dmgfx;

    // Start is called before the first frame update
    void Start()
    {
        _animator = this.GetComponent<Animator>();

        _healthbar = GameObject.Find("SliderHealth").GetComponent<HealthBar>();
        _fuelBar = GameObject.Find("SliderFuel").GetComponent<FuelBar>();

        _health = _maxHealth;
        _fuelTank = _maxFuel;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _controller = GetComponent<CharacterController>();
        _dmgfx = GameObject.Find("DamageFx").GetComponent<DamageEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("CURRENT FUEL: " + _fuelTank + " ANd health = " + _health);
        //FuelUI.instance.UpdateFuel((int)_fuelTank, (int)_maxFuel);
        //shoot

        UpdateHealthBar();
        UpdateFuelBar();
        
        
            if (Input.GetMouseButton(0))
            {                           
                bool isShootingPressed = true;
                _animator.SetBool("isshooting", isShootingPressed);                              
            }
            else
            {
                bool isShootingPressed = false;
                _animator.SetBool("isshooting", isShootingPressed);
            }

            if (Input.GetMouseButtonDown(1))
            {
                _animator.SetTrigger("swing");
            }
          


        
        CalculateMovement();
        FuelCheck();

        if (Input.GetKey(KeyCode.Q) && _fuelAvailable && _health < _maxHealth)
        {
            FuelHeal();
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            _fuelInUse = false;
            StartCoroutine(FuelCooling());
        }

        //Fuel recharging
        if (_canRecharge == true && !_fuelInUse)
        {
            _fuelTank += _fuelRechargeRate * Time.deltaTime;

        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (_fuelTank > _maxFuel)
        {
            _fuelTank = _maxFuel;
        }

        if (_health <= 0)
        {
            SceneManager.LoadScene(3);
        }

    }

    public void AddAmmo()
    {
        Weapon weapon = this.GetComponentInChildren<Weapon>();
        Weapon2 weapon2 = this.GetComponentInChildren<Weapon2>();
        weapon.AddAmmo();
        weapon2.AddAmmo();
    }

    void SlamAttack()
    {
        var cloneDirt = Instantiate(_slamDirtPrefab, transform.position, _slamDirtPrefab.transform.rotation);
        Destroy(cloneDirt, 2.5f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, _slamRadius);

        foreach (Collider nearbyObject in colliders)
        {
            RichAI rAI = nearbyObject.GetComponent<RichAI>();
            if (rAI != null)
            {
                rAI.Pounded();
            }
        }

        StartCoroutine(SlamCooldown());
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //creating movement and gravity functions
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        Vector3 velocity = direction * _movespeed;
        velocity.y -= _gravity;

        //taking global space values and changing them into local
        velocity = transform.TransformDirection(velocity);

        //some ugly dash logic code:
        _dashDirection = new Vector3(horizontalInput * _dashDistance, 0, verticalInput * _dashDistance);
        _dashDirection = transform.TransformDirection(_dashDirection);


        //Jump and doublejump
        if (_controller.isGrounded)
        {
            _inAirJumps = 1;
            _verticalSpeed = -1;
            if (_controller.isGrounded && Input.GetButtonDown("Jump"))
            {
                _verticalSpeed = _jumpSpeed;
            }
        }
        else if (Input.GetButtonDown("Jump"))
        {
            if (_inAirJumps == 1)
            {
                _verticalSpeed = _jumpSpeed + 1;
                _inAirJumps--;
                Instantiate(_jumpEffect, transform.position, _jumpEffect.transform.rotation);
            }
            else if (_fuelAvailable && _inAirJumps == 0)
            {
                if (Input.GetButton("Jump") && _fuelTank > _dJumpBoostDrain)
                {
                    _verticalSpeed = _jumpSpeed + 1;
                    _fuelTank -= _dJumpBoostDrain;
                    _canRecharge = false;
                    Instantiate(_jumpEffect, transform.position, _jumpEffect.transform.rotation);
                    StartCoroutine(FuelCooling());
                }
            }
        }


        //slamming/groundpound
        if (Input.GetKey(KeyCode.C) && !_slamUsed)
        {
            Vector3 slamDirection = Camera.main.transform.forward * 2;
            slamDirection += Vector3.down;
            _controller.Move(slamDirection * _crouchSlamSpeed * Time.deltaTime);

            if (_controller.isGrounded)
            {
                Debug.Log("SLAM!");
                SlamAttack();
                _slamUsed = true;
                return;
            }
        }
        else
        {
            transform.position = transform.position;
        }

        //Jetting and dashing
        if (_fuelAvailable == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _movespeed *= _jetSpeedMultiplier;
                _jetHoldTime = Time.timeSinceLevelLoad;
                _fuelInUse = true;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                if (!_jetHeld && _fuelTank > _dashFuelDrain)
                {
                    _controller.Move(_dashDirection);
                    _fuelTank -= _dashFuelDrain;
                    _fuelInUse = true;
                }

                _fuelInUse = false;
                _jetHeld = false;
                SetDefaultSpeed();
                StartCoroutine(FuelCooling());
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                InstantiationTimer -= Time.deltaTime;
                if (InstantiationTimer <= 0)
                {
                    Instantiate(_jetTrailPrefab, transform.position, Quaternion.identity);
                    InstantiationTimer = 0.069f;
                }

                _canRecharge = false;
                _fuelTank -= _jetFuelDrainPerSec * Time.deltaTime;
                if (Time.timeSinceLevelLoad - _jetHoldTime > _minHeldDuration)
                {
                    _jetHeld = true;
                }
            }
        }

        _verticalSpeed -= _gravity * Time.deltaTime;
        velocity.y = _verticalSpeed;

        _controller.Move(velocity * Time.deltaTime);

        if (Input.GetKey(KeyCode.W))
        {
            bool isWalkingPressed = true;
            _animator.SetBool("Iswalking", isWalkingPressed);
        }
        else
        {
            bool isWalkingPressed = false;
            _animator.SetBool("Iswalking", isWalkingPressed);
        }

         
   }

    void FuelCheck()
    {
        if (_fuelTank < 0)
        {
            _fuelTank = 0;
            _fuelAvailable = false;
            SetDefaultSpeed();
            StartCoroutine(FuelCooling());
        }
        else
        {
            _fuelAvailable = true;
        }
    }
    void SetDefaultSpeed()
    {
        _movespeed = 9;
    }

    IEnumerator FuelCooling()
    {
        yield return new WaitForSeconds(_fuelRechargeDelay);
        _canRecharge = true;
    }

    IEnumerator SlamCooldown()
    {
        yield return new WaitForSeconds(_slamCooldown);
        _slamUsed = false;
    }

    public void HitFountain()
    {
        _controller.Move(Vector3.up * 500 * Time.deltaTime);
    }

    public void TakeDamage(float amount)
    {
        _health -= amount;
        _dmgfx.TakeDamage();
    }

    void UpdateHealthBar()
    {
        _healthbar.SetValue(_health);
    }

    void UpdateFuelBar()
    {
        _fuelBar.SetValue(_fuelTank);
    }

    public void FuelHeal()
    {
        _fuelTank -= _healFuelCost * Time.deltaTime;
        _health += _healRate * Time.deltaTime;
        _fuelInUse = true;
        _canRecharge = false;
    }
}