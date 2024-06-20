using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private TrailRenderer _trailRenderer;
    private Transform aimTransform;

    [Header("----------Movement----------")]
    [SerializeField]
    private float movementSpeed = 10f;
    [SerializeField]
    private float dashSpeed = 50f;
    [SerializeField]
    private float dashDuration = 0.5f;




    [Header("----------Muzzle Point----------")]
    [SerializeField]
    private GameObject _muzzleFlash;
    [SerializeField]
    private Transform _muzzlePoint;

    private Animator _animator;
    private Vector2 _movement;
    private Vector2 _mousePosition;
    private Vector2 lookDir;
    private float _playerSpeed;
    private bool _canDash = false;
    private Vector3 _aimDir;
    private bool _isShoot = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _trailRenderer = GetComponent<TrailRenderer>();
        aimTransform = transform.Find("Aim");
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerSpeed = movementSpeed;
        _muzzleFlash.SetActive(false);
    }

    private void FixedUpdate()
    {
        PlayerDirection();
        PlayerRotation();
        AimWeapon();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Create animation that can rotate character in 2D
        var velocity = _rb.velocity;
        if (velocity.magnitude > 0.1f)
        {
            _animator.SetBool("IsRunning", true);
        }
        else
        {
            _animator.SetBool("IsRunning", false);
        }

        _animator.SetFloat("Horizontal", lookDir.x);
        _animator.SetFloat("Vertical", lookDir.y);

        
    }

    private void PlayerDirection()
    {
        //Move Character by WASD
        _rb.velocity = _movement * _playerSpeed;
    }

    private void PlayerRotation()
    {
        //Rotate Character smoothly
        lookDir = _mousePosition - _rb.position;
        lookDir.Normalize();
    }

    public void MovementInput(Vector2 input)
    {
        //Set input to movement
        _movement = input;
    }

    public void AimRotation(Vector2 mousePosition)
    {
        //Set input by mouse position to rotate player
        _mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    public void Dashing()
    {
        //Check Dashing for player
        if(!_canDash)
        {
            _animator.SetBool("IsDash", true);
            _canDash = true; //If player ready to dash, so start coroutine
            StartCoroutine(DashingTime());
        }
        else
        {
            return;
        }
        
    }

    private IEnumerator DashingTime()
    {
        _playerSpeed = dashSpeed; //Change normal speed to dash speed to give speed to player a little bit time 
        _trailRenderer.emitting = true; //When player dash, Line trail will appear
        yield return new WaitForSeconds(dashDuration);
        _trailRenderer.emitting = false;
        _playerSpeed = movementSpeed; //Set speed back to normal
        _animator.SetBool("IsDash", false);
        _canDash = false;
    }

    private void AimWeapon()
    {
        //Set weapon aim with cursor
        _aimDir = _mousePosition - _rb.position;
        _aimDir.Normalize();
        float angle = Mathf.Atan2(_aimDir.y, _aimDir.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        //Check degree of weapon to flip
        Vector3 aimLocalScale = Vector3.one;
        if(angle > 90 || angle < -90)
        {
            aimLocalScale.y = -1f;
        }
        else
        {
            aimLocalScale.x = +1f;
        }
        aimTransform.localScale = aimLocalScale;
    }

    public void WeaponShoot()
    {
        if(!_isShoot)
        {
            _isShoot = true; //If player can shoot, start coroutine
            StartCoroutine(MuzzleFlashActive());
        }
        else
        {
            return;
        }
    }

    public IEnumerator MuzzleFlashActive()
    {
        //Gun fire by left click
        _muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _muzzleFlash.SetActive(false);
        _isShoot = false;
    }
}
