using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    [Header("Movement")] 
    public float moveSpeed;
    
    public float groundDrag;
    
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool _readyToJump;
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask whatIsGround;

    public Transform orientation;
    
    private float _horizontalInput;
    private float _verticalInput;

    private Vector3 _moveDirection;
    
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        
        _readyToJump = true;
    }

    private void Update()
    {
        UpdateInput();
        SpeedControl();
        
        PlayerManager.Instance.isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround); 
        if (PlayerManager.Instance.isGrounded)
        {
            print("grounded");
            _rb.drag = groundDrag;
        }
        else
        {
            print("not grounded");
            _rb.drag = 0;
        }

        if (PlayerManager.Instance.isGrounded)
        {
            PlayerManager.Instance.isJumping = false;
            print("not jump");
        }
        
        if (_rb.velocity.y <= -12f)
        {
            PlayerManager.Instance.isFalling = true;
            print("falling");
        }
        else
        {
            PlayerManager.Instance.isFalling = false;
            print("not falling");
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void UpdateInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(KeyCode.Space) && _readyToJump && PlayerManager.Instance.isGrounded)
        {
            _readyToJump = false;
            
            Jump();
            PlayerManager.Instance.isJumping = true;
            print("jumping");
            
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;

        // on ground
        if (PlayerManager.Instance.isGrounded)
        {
            _rb.AddForce(_moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        // in air
        else if (!PlayerManager.Instance.isGrounded)
        {
            _rb.AddForce(_moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        
        // limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    
    private void ResetJump()
    {
        _readyToJump = true;
    }

    public bool GetGrounded()
    {
        return PlayerManager.Instance.isGrounded;
    }

    public bool GetReadyToJump()
    {
        return _readyToJump;
    }

    public String GetVelocity()
    {
        return _rb.velocity.ToString();
    }
}
