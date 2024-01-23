using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    [Header("NewMove")]
    [Tooltip("The amount of force applied to the player whilst walking")][SerializeField] private float walkMoveForce;
    [Tooltip("The amount of force applied to the player whilst sprinting")][SerializeField] private float sprintMoveForce;
    private float moveForce;

    [Tooltip("Controls how quickly the player accelerates")][SerializeField] AnimationCurve moveSpeedCurve;
    [Tooltip("How quickly the player rotates when changing direction")][SerializeField] private float rotationSpeed;
    [Tooltip("The maximum angle of a slope to allow force to be applied")][SerializeField] private float maxSlopeAngle;
    private float moveTime;
    private float slopeAngle;
    private bool sprinting;

    [Header("Dashing")]
    [Tooltip("Amount of force applied to played when dashing")][SerializeField] private float dashForce;
    [Tooltip("Amount of time before player can use dash again once used")][SerializeField] private float dashCooldown;
    [Tooltip("How long the dash slide lasts")][SerializeField] private float dashDuration;
    [Tooltip("How long the player collider is disabled after dashing")][SerializeField] private float noColliderDashTime;
    private bool canDash = true;
    private bool dashing = false;

    [Header("Others")]
    [Tooltip("The acceleratory forec in the downwards direction"), SerializeField] private float gravitationalForce;
    [Tooltip("The maximuim velocity the player can have upwards (to stop flying up after dash)"), SerializeField] private float yUpwardsForceLimit;
    [Tooltip("Amount of time before player can jump again after jumping")][SerializeField] private float jumpCooldown;
    [Tooltip("Amount of force to be applied to the player for jumping")][SerializeField] private float jumpForce;
    public bool grounded = true;
    private bool canJump = true;

    [Tooltip("Tick all layers that are ground")][SerializeField] private LayerMask groundMask;
    [Tooltip("Player input control scheme")][SerializeField] PlayerInputActions playerControls;

    [Header("Drag Modifiers")]
    [Tooltip("The thickness of the atmosphere/air")][SerializeField] private float fluidDensity;
    [Tooltip("Lower = more aerodynamic")][SerializeField] private float dragCoefficient;
    [Tooltip("The size of the object")][SerializeField] private float crossSectionalArea;

    // other 
    [Header("Other")]
    [SerializeField]
    private PlayerInput playerInput;
    private Rigidbody rb;
    private Vector2 direction;
    private Camera mainCamera;
    public Animator playerAnimation;
    public CameraLockOn lockOn;
    private bool landSoundPlayed = true;
    public PlayerAudio targetAudio;
    private Vector3 targetDirection;
    private DashTrail trailScript;
    [SerializeField]
    private CapsuleCollider playerCollider;
    [SerializeField]
    private CapsuleCollider collsionBlocker;
    public bool canMove = true;

    [SerializeField] private Transform groundedCheck;
    private Vector3 floorVector = Vector3.zero;

    private void Awake()
    {
        Instance = this;

        playerInput = GetComponent<PlayerInput>();
        mainCamera = Camera.main;
        lockOn = mainCamera.GetComponent<CameraLockOn>();
        rb = GetComponent<Rigidbody>();
        trailScript = GetComponent<DashTrail>();

        moveForce = walkMoveForce;
        sprinting = false;
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        MovePlayer();
        ApplyFriction();

        // apply gravity
        rb.AddForce(Vector3.down * gravitationalForce, ForceMode.Acceleration);

        // ensure we cannot breach y upwards force limit
        if(rb.velocity.y > yUpwardsForceLimit && dashing)
        {
            rb.velocity = new Vector3(rb.velocity.x, yUpwardsForceLimit, rb.velocity.z);
        }
    }

    private void Update()
    {
        HandleInput();
    }

    /// <summary>
    /// Gets player input and updates input variable values
    /// </summary>
    public void HandleInput()
    {
        // get direction of player input
        //direction = playerControls.Player.Movement.ReadValue<Vector2>();
        direction = playerInput.actions["Movement"].ReadValue<Vector2>();

        if (direction.magnitude == 0)
        {
            moveTime = 0;
        }

        // if jump key pressed, apply jump
        if (playerInput.actions["Jump"].triggered)
        {
            Jump();

        }

        // if dash key pressed, apply dash
        if (playerInput.actions["Dash"].triggered && canDash)
        {
            Dash();
            trailScript.StartTrail();
            targetAudio.PlayDashClip();
        }
        
        // if sprint key pressed, enabled sprint
        if (playerInput.actions["Sprint"].WasPressedThisFrame())
        {
            ToggleSprint(true);
            sprinting = true;
        }
        // if sprint key released, disable sprint
        else if (playerInput.actions["Sprint"].WasReleasedThisFrame())
        {
            if(sprinting)
            {
                ToggleSprint(false);
                sprinting = false;
            }
        }
    }

    /// <summary>
    /// Toggles sprinting depending on the given bool value
    /// </summary>
    /// <param name="enabled">whether sprinting should be enabled or not</param>
    private void ToggleSprint(bool enabled)
    {
        if (enabled)
        {
            moveForce = sprintMoveForce;
        }
        else
        {
            moveForce = walkMoveForce;
        }
    }

    /// <summary>
    /// Applies a force to the player towards a target direction, provided an input is given
    /// </summary>
    private void MovePlayer()
    {
        
        if (canMove)
        {
            ApplyTargetDirection();

            // Is this how this works?
            if (sprinting)
            {
                playerAnimation.SetFloat("speed", rb.velocity.magnitude / 20);            
            }
            else
            {
                playerAnimation.SetFloat("speed", rb.velocity.magnitude / 12);            // why 12? -> Ask Mikolaj
            }
            

            // only rotate if there is a targetDirection
            if (targetDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

            // apply movement force if slope isn't too steep
            if (GetSlopeAngle() <= maxSlopeAngle)
            {
                //rb.AddForce(targetDirection * moveForce * GetCurveValue(moveSpeedCurve, moveTime), ForceMode.Acceleration);

                // if lockon
                // set floorVec to -floorVec
                if (lockOn.IsLockedOn())
                {
                    float targetDirectionFloorVectorDifference = Vector3.Angle(targetDirection.normalized, floorVector.normalized);
                    if(targetDirectionFloorVectorDifference > 100)
                    {
                        floorVector = -floorVector;
                    }
                }
                rb.AddForce((targetDirection + floorVector).normalized * moveForce * GetCurveValue(moveSpeedCurve, moveTime), ForceMode.Acceleration);

                
            }

            // apply change to speed curve when moving
            if (moveTime < 1 && direction.magnitude > 0 || direction.magnitude < 0)
            {
                moveTime += Time.deltaTime;
            }
        }
        else
        {
            playerAnimation.SetFloat("speed", 0);            // why 12? -> Ask Mikolaj
        }
    }

    // Commented it out as it was breaking my lock on and I can't seen any function using it - ASK TOM

    /// <summary>
    /// Rotates the player towards a given direction
    /// </summary>
    /// <param name="toRotation">the direction to look towards</param>
    //public void RotatePlayerTowards(Quaternion toRotation)
    //{
    //    transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    //}

    private void CheckGrounded()
    {
        // spherecast works better than overlapsphere since it's directional and detects ground better
        // But I'm keeping the code in case we find it useful
        // overlapSphere makes it so you can jump on walls at the correct angle
        /*
        if (Physics.OverlapSphere(groundedCheck.position, 0.5f, groundMask) != null)
        {
            playerAnimation.ResetTrigger("jump");
            playerAnimation.SetBool("grounded", true);
            grounded = true;
            if (landSoundPlayed == false)
            {
                targetAudio.PlayLandClip();
                landSoundPlayed = true;
            }
        }
        else
        {
            grounded = false;
            playerAnimation.SetBool("grounded", false);
            landSoundPlayed = false;
        }
        */
        if (Physics.SphereCast(groundedCheck.position, 0.5f, Vector3.down, out RaycastHit groundHit, 1, groundMask))
        {
            playerAnimation.ResetTrigger("jump");
            playerAnimation.SetBool("grounded", true);
            grounded = true;
            if (landSoundPlayed == false)
            {
                targetAudio.PlayLandClip();
                landSoundPlayed = true;
            }
        }
        else
        {
            grounded = false;
            playerAnimation.SetBool("grounded", false);
            landSoundPlayed = false;
        }
    }

    /// <summary>
    /// Gets angle of ground underneath player and sets slopeAngle, also handles grounded state
    /// </summary>
    /// <returns>the angle of the slope beneath the object</returns>
    private float GetSlopeAngle()
    {        
        RaycastHit hit;
        // magic number needs to be replaced with something, but 2 seems to work best. But means player can 'jump' when not on the ground -> need to think of something
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f, groundMask))
        {
            
            // get slope angle and assign
            slopeAngle = Vector3.Angle(transform.forward, hit.normal) - 90;

            floorVector = (transform.forward - Vector3.Dot(transform.forward, -hit.normal) * hit.normal).normalized;
            floorVector.y = -floorVector.y;
        }
        else
        {
            slopeAngle = 0;
            
            floorVector = Vector3.zero;
        }

        return slopeAngle;
    }

    /// <summary>
    /// Gets target direction relative to camera
    /// </summary>
    /// <returns>an input direction relative to camera</returns>
    private Vector3 ApplyTargetDirection()
    {
        targetDirection = new Vector3(direction.x, 0, direction.y);
        targetDirection = mainCamera.transform.TransformDirection(targetDirection);
        targetDirection.y = 0.0f;

        return targetDirection.normalized;
    }

    /// <summary>
    /// Gets the value along an animation curve at a given time
    /// </summary>
    /// <param name="animCurve">the animationCurve to get the value from</param>
    /// <param name="time">the time along this animation curve to get the value from</param>
    /// <returns>the value of the given aniamtion curve at the given time</returns>
    private float GetCurveValue(AnimationCurve animCurve, float time)
    {
        // possible improvement -> if > duration of curve, don't evaluate, just give max curveValue
        float curveValue = animCurve.Evaluate(time);
        return curveValue;
    }

    /// <summary>
    /// Applies a dash force towards input direction
    /// </summary>
    private void Dash()
    {
        if (canDash)
        {
            playerAnimation.SetTrigger("dash");
           
            Actions.OnPlayerDash?.Invoke();
            rb.AddForce(targetDirection.normalized * dashForce, ForceMode.Impulse);
            playerAnimation.SetInteger("DashType", GetDashDirection(direction));
            StartCoroutine(DashCollider());
            StartCoroutine(DashCooldown());
            StartCoroutine(EnableDashSlide());
        }
    }

    /// <summary>
    /// Applies a cooldown to the dash ability
    /// </summary>
    /// <returns>a wait in seconds by dashCooldown</returns>
    private IEnumerator DashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator DashCollider()
    {
        gameObject.layer = 15;
        collsionBlocker.enabled = false;
        yield return new WaitForSeconds(noColliderDashTime);
        gameObject.layer = 9;
        collsionBlocker.enabled = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnableDashSlide()
    {
        dashing = true;
        float originalFluidDesntiy = fluidDensity;
        fluidDensity = .3f;
        yield return new WaitForSeconds(dashDuration);
        fluidDensity = originalFluidDesntiy;
        dashing = false;
    }

    /// <summary>
    /// Applies a cooldown to the jump ability
    /// </summary>
    /// <returns>a wait in seconds by jumpCooldown</returns>
    private IEnumerator JumpCooldown()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    /// <summary>
    /// Makes the player jump, provided they can and are grounded
    /// </summary>
    private void Jump()
    {
        if (grounded && canJump && slopeAngle <= maxSlopeAngle)
        {
            playerAnimation.SetTrigger("jump");
            playerAnimation.SetBool("grounded", false);
            grounded = false;
            targetAudio.PlayJumpClip();
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            StartCoroutine(JumpCooldown());
        }
    }

    /// <summary>
    /// Applies friction to the object
    /// </summary>
    private void ApplyFriction()
    {
        // change for if in air?
        // drag = .5 * fluidDensity * velocity^2 * dragCoefficient * IncidenecArea      -> drag equation

        //Vector3 dragVector = fluidDensity * rb.velocity * 2 * dragCoefficient * crossSectionalArea;     // this wrong? Fix!!
        Vector3 dragVector = .5f * fluidDensity * (rb.velocity * rb.velocity.magnitude) * dragCoefficient * crossSectionalArea;

        dragVector.y = 0;
        rb.AddForce(-dragVector);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            gameObject.transform.parent = other.gameObject.transform;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            gameObject.transform.parent = null;
        }
    }

    /// <summary>
    /// Gets and returns the player's target direction
    /// </summary>
    /// <returns>the direction the player is trying to move towards</returns>
    public Vector3 GetTargetDirection()
    {
        return targetDirection;
    }

    public int GetDashDirection(Vector2 dashDirection)
    {
        if (lockOn.IsLockedOn())
        {
            //forward
            if (dashDirection.y > 0)
            {
                return 1;
            }
            //backward
            else if (dashDirection.y < 0)
            {
                return 2;
            }
            //left
            else if (dashDirection.x < 0)
            {
                return 3;
            }
            //right
            else if (dashDirection.x > 0)
            {
                return 4;
            }
        }
        else
            return 1;
        return 0;
    }
}