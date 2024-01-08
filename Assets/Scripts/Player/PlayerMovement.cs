using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYscale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;

    [Header("FootSteps")]
    [SerializeField] private bool useFootSteps = true;
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float CrouchMultiplier = 1.5f;
    [SerializeField] private float SprintMultiplier = 0.6f;
    [SerializeField] private AudioSource footStepAudioSource = default;
    [SerializeField] private AudioClip[] Grass = default;
    [SerializeField] private AudioClip[] Rock = default;
    private float footStepTimer = 0;
    private bool crouch = false;

    private bool Sprint = false;
    private float GetCurrentOffSet => crouch ? baseStepSpeed * CrouchMultiplier : Sprint ? baseStepSpeed * SprintMultiplier : baseStepSpeed;




    public enum MovementState 
    { 
        walking,
        sprinting,
        crouching,
        air
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startYscale = transform.localScale.y;

        

    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        else if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        else if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYscale, transform.localScale.z);
           
        }
    }
    private void StateHandler()
    {
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
            crouch = true;
        }

        else if(grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
            Sprint = true;
        }

        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
     
        }

        else
        {
            state = MovementState.air;
        }
    }



    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        MyInput();
        SpeedControl();
        StateHandler();

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        if (useFootSteps)
            Handle_Footsteps();
    }


    private void Handle_Footsteps()
    {
        if (!grounded) return;
        if (horizontalInput == 0 && verticalInput == 0) return;

        footStepTimer -= Time.deltaTime;
        if (footStepTimer <= 0)
        {
            footStepAudioSource.pitch = Random.Range(0.9f, 1.1f);
            if(Physics.Raycast(orientation.transform.position, Vector3.down, out RaycastHit hit, 3))
            {
                switch (hit.collider.tag) 
                {
                    case "FootSteps/Grass":
                        footStepAudioSource.PlayOneShot(Grass[Random.Range(0, Grass.Length - 1)]);
                        break;
                    case "FootSteps/Rock":
                        footStepAudioSource.PlayOneShot(Rock[Random.Range(0, Rock.Length - 1)]);
                        break;
                    default:
                        footStepAudioSource.PlayOneShot(Rock[Random.Range(0, Rock.Length - 1)]);
                        break;
                }

            }
            footStepTimer = GetCurrentOffSet;
        }
    }


    private void FixedUpdate()
    {
        MovePlayer();

    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if(!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        rb.useGravity = !OnSlope();
      
    }

    private void SpeedControl()
    {

        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;

        }

        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }


       
    }

    private void Jump()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

  




}
