using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractHandler : MonoBehaviour
{
    [HideInInspector] public static InteractHandler Instance;
    [HideInInspector] public bool isInteracting;

    [Tooltip("The distance the player can interact"), SerializeField] private float viewDst;
    [Tooltip("The radius of the interact sphere"), SerializeField] private float interactRadius;
    [Tooltip("The tag assigned to rotators"), SerializeField] private string rotatorTag;
    [Tooltip("Which layers the player should be able to interact with"), SerializeField] private LayerMask mask;
    [HideInInspector] public bool canMove;
    [HideInInspector] public bool canInteract;
    [Tooltip(""), SerializeField] private float minInteractAngle;
    [Tooltip(""), SerializeField] private float maxInteractAngle;
    [SerializeField] private SimpleCombat simpleCombat;

    [Header("UI")]
    [Tooltip("The GameObject holding the interact tip"), SerializeField] private GameObject interactHintUI;

    private PlayerInputActions playerControls;
    private GameObject interactObject = null;

    private Transform playerRoot;
    [HideInInspector] public RotateOnTouch currentRotator;
    
    RaycastHit curHit;

    private void OnEnable()
    {
        playerControls.Enable();
        Actions.ApplyEnemyDamage += TakeDamage;
    }

    private void OnDisable()
    {
        Actions.ApplyEnemyDamage -= TakeDamage;
    }

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        playerRoot = transform.root;
        isInteracting = false;
        Instance = this;   
        canMove = true;
        canInteract = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControls.Player.Interact.WasPressedThisFrame())
        {
            if (isInteracting)
            {
                RemoveFromInteractObject(false);
            }
            else if (interactHintUI.activeSelf)
            {
                ParentToInteract();
            }
        }
        if (Physics.Raycast(transform.position, transform.forward, out curHit, viewDst, mask) && !isInteracting)
        {
            float interactAngle = Vector3.Angle(transform.forward, curHit.collider.transform.forward);
            if (interactAngle > minInteractAngle && interactAngle < maxInteractAngle)
            {
                if (!interactHintUI.activeSelf && !isInteracting)
                {
                    interactHintUI.SetActive(true);
                }                
            }
        }
        else if (curHit.collider != null)
        {
            
            Debug.Log("remove hit");
            curHit = new RaycastHit();
        }
        else
        {
            if (interactHintUI.activeSelf)
            {
                interactHintUI.SetActive(false);
            }
        }
        
    }

    /// <summary>
    /// Fires a raycast and parents to the interact object if found
    /// </summary>
    private void ParentToInteract()
    {
        GameObject curHitObj = curHit.collider.gameObject;

        // if hit object has the given rotator tag, get the hit rotator and set up to allow player to control
        if (curHitObj != null && curHitObj.tag == rotatorTag)
        {
            // get the rotator and apply the player's direction
            RotateOnTouch rotator = curHitObj.GetComponent<RotateOnTouch>();
            rotator.SetDirection();
            currentRotator = rotator;

            // set interact object and ensure we're in the interact state
            interactObject = rotator.pushSection;
            isInteracting = true;

            // ensure player can't move around whilst interacting
            playerRoot.transform.parent = interactObject.transform;

            MasterCameraController.Instance.ChangeCamEnabled(rotator.myCam, true, true);
            simpleCombat.canAttack = false;

            if (interactHintUI.activeSelf)
            {
                interactHintUI.SetActive(false);
            }
        }

    }

    /// <summary>
    /// Clunky way of making the interaction stop when player takes damage
    /// </summary>
    private void TakeDamage(float damage)
    {
        RemoveFromInteractObject(false);
    }

    /// <summary>
    /// Removes player from the interact object
    /// </summary>
    public void RemoveFromInteractObject(bool enableMove)
    {
        playerRoot.transform.parent = null;
        isInteracting = false;
        interactObject = null;
        simpleCombat.canAttack = true;

        if(currentRotator != null)
        {
            MasterCameraController.Instance.ChangeCamEnabled(currentRotator.myCam, false, enableMove);
            currentRotator = null;
        }
    }

}
