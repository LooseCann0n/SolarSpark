using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEditor;

/// <summary>
/// Manages lock on for the camera, adding enemy to CM targetGroup
/// </summary>
public class CameraLockOn : MonoBehaviour
{
    #region Variables
    
    public Transform player;
    private Color lockOnLineColour;
    [Tooltip("Reference to InputActionsAsset to allow control inputs")]
    public PlayerInputActions playerControls;
    private CameraController controller;

    #region Cinemachine 
    private CinemachineVirtualCamera virtualCamera;
    private CinemachinePOV cinemachinePOV;
    private CinemachineInputProvider cameraInput;

    #endregion

    [Header("LockOn Variables")]
    [Tooltip("Can only lockon within the distance, will detach current lockOn target if you exceed this variable")]
    public float maxLockOnDistance;
    [Tooltip("Size of the sphere that will find the closest enemy object")]
    public float lockOnRadius;

    public int lockOnAngle;

    [Tooltip("Layer which the enemies are on, so only enemy objects are found")]
    public LayerMask enemyLayer;
    private LayerMask notEnemyLayer;

    [Header("LockOn Targets")]
    public Transform currentLockOnTarget;
    public Transform nearestLockOnTarget;
    public Transform leftLockTarget;
    public Transform rightLockTarget;
    [Tooltip("List storing the targets found in sphere")]
    public List<SimpleEnemy> availableTargets = new List<SimpleEnemy>();
    #endregion

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        controller = GetComponent<CameraController>();
        playerControls = new PlayerInputActions();
        virtualCamera = GameObject.FindGameObjectWithTag("Vcam").GetComponent<CinemachineVirtualCamera>();
        cameraInput = virtualCamera.GetComponent<CinemachineInputProvider>();
        cinemachinePOV = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    private void Start()
    {
        lockOnLineColour = Color.green;
        cinemachinePOV.m_HorizontalRecentering.m_enabled = false;
        notEnemyLayer = 1 << 8;
        notEnemyLayer = ~notEnemyLayer;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }


    private void Update()
    {
        // Player has pressed the LockOn button
        if (playerControls.Player.LockOn.triggered)
        {
            // If there is no lockOn target then
            if (currentLockOnTarget == null)
            {
                HandleLockOn();
                //if (currentLockOnTarget != null)
                //    currentLockOnTarget.parent.GetComponent<MeshRenderer>().material = lockOnColour;
            }
            else // Already lockOn to enemy
            {
                RemoveLockOn();
            }
        }

        if (currentLockOnTarget != null)
        {
            //if (!playerControls.Player.LockOn.IsPressed())
            //{
            //    RemoveLockOn();
            //    return;
            //}
            if (playerControls.Player.LockOnToLeft.triggered)
            {
                ChangeLockOnTarget(Vector3.left); // ChangeLockOn to a left target
            }
            if (playerControls.Player.LockOnToRight.triggered)
            {
                ChangeLockOnTarget(Vector3.right); // ChangeLockOn to a right target
            }
            LockOnBehaviour();
        }
        if (currentLockOnTarget == null)
            controller.noTargets = true;
        else
            controller.noTargets = false;
    }

    /// <summary>
    /// Changes camera to lockOn to the current target
    /// </summary>
    private void LockOnBehaviour()
    {
        // Change camera settings
        cinemachinePOV.m_HorizontalRecentering.m_enabled = true;
        cameraInput.enabled = false;

        // Make player lookAt the lockOnTarget
        Vector3 targetPosition = new Vector3(currentLockOnTarget.position.x, player.position.y, currentLockOnTarget.position.z);
        player.LookAt(targetPosition);
        float distanceFromPlayer = Vector3.Distance(player.position, currentLockOnTarget.position); // Find distanceBetween player and enemy

        // Debug drawline from lockOnBreaking
        float distanceLerp = distanceFromPlayer / maxLockOnDistance;   // Lerp for changing colour based upon distance
        lockOnLineColour = Color.Lerp(Color.green, Color.red, distanceLerp); // Gradient line colour from green to red.

        


        if (distanceFromPlayer > maxLockOnDistance) // If distance exceeds maxLockOnDistance variable
        {
            RemoveLockOn(); // Run RemoveLockOn
        }
    }

    /// <summary>
    /// ChangeLockOn target to either left or right of the player
    /// </summary>
    /// <param name="direction">Direction player has flicked stick</param>
    private void ChangeLockOnTarget(Vector3 direction)
    {
        // Run lockOn to find left and right targets
        HandleLockOn();
        if (direction == Vector3.left) // Check left direction
        {
            if (leftLockTarget != null) // If leftLockTarget isn't null
            {
                //currentLockOnTarget.parent.GetComponent<MeshRenderer>().material = standardColour; // Change currentLockTarget back to red
                //leftLockTarget.parent.GetComponent<MeshRenderer>().material = lockOnColour; // Change new lockOnTarget to lockOnColour
                currentLockOnTarget = leftLockTarget; // Change currentLockTarget to leftLockTarget
            }
        }
        if (direction == Vector3.right) // Check right direction
        {
            if (rightLockTarget != null)
            {
                //currentLockOnTarget.parent.GetComponent<MeshRenderer>().material = standardColour; // Change currentLockTarget back to red
                //rightLockTarget.parent.GetComponent<MeshRenderer>().material = lockOnColour; // Change new lockOnTarget to lockOnColour
                currentLockOnTarget = rightLockTarget; // Change currentLockTarget to rightLockTarget
            }
        }
    }

    /// <summary>
    /// Finds the closest enemies and assigns transforms
    /// </summary>
    private void HandleLockOn()
    {
        FindAvailableTargets();
            
        if (availableTargets.Count > 0)
        {
            FindClosestLockTargets();
        }
        else
        {
            if (currentLockOnTarget != null)
            {
                RemoveLockOn();
            }
        } 
    }

    /// <summary>
    /// Finds all enemies within lockOnRadius, within an angle. Shown by cone gizmo
    /// </summary>
    private void FindAvailableTargets()
    {
        // Sphere centered on the players location, size of lockOnRadius, looking for only enemyLayer
        Collider[] colliders = Physics.OverlapSphere(player.position, lockOnRadius, enemyLayer);
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                SimpleEnemy enemy = colliders[i].GetComponent<SimpleEnemy>();

                if (enemy != null)
                {
                    Vector3 lockOnTargetDirection = enemy.transform.position - player.position;
                    float distanceFromTarget = Vector3.Distance(player.position, enemy.transform.position);
                    float viewableAngle = Vector3.Angle(lockOnTargetDirection, transform.forward);

                    //if (!Physics.Raycast(player.position, lockOnTargetDirection, lockOnRadius, notEnemyLayer))
                    //{
                    //    if (enemy.transform.root != player.transform.root
                    //    && viewableAngle > -lockOnAngle && viewableAngle < lockOnAngle
                    //    && distanceFromTarget <= maxLockOnDistance)
                    //    {
                    //        if (!availableTargets.Contains(enemy))
                    //            availableTargets.Add(enemy);
                    //    }
                    //}

                    if (enemy.transform.root != player.transform.root
                    && viewableAngle > -lockOnAngle && viewableAngle < lockOnAngle
                    && distanceFromTarget <= maxLockOnDistance)
                    {
                        if (!availableTargets.Contains(enemy))
                            availableTargets.Add(enemy);
                    }
                }
            }
        }
        else
            return;
    }

    /// <summary>
    /// Iterate through each enemy in availableTargets and find the closest one to player.
    /// </summary>
    private void FindClosestLockTargets()
    {
        // floats to store shortest distance
        float shortestDistance = Mathf.Infinity;
        float shortestDistanceLeft = Mathf.Infinity;
        float shortestDistanceRight = Mathf.Infinity;

        // Loop through availableTargets
        for (int k = 0; k < availableTargets.Count; k++)
        {
            // Find the distance from player to the enemy
            float distanceFromTarget = Vector3.Distance(player.position, availableTargets[k].transform.position);

            // If distance is less than shortestDistance
            if (distanceFromTarget < shortestDistance)
            {
                // Assign shortestDistance as distanceFromTarget
                shortestDistance = distanceFromTarget;
                nearestLockOnTarget = availableTargets[k].lockOnTransform; // nearestLockOn is target closest to the player
                Transform lockOnLocation = nearestLockOnTarget;
                currentLockOnTarget = lockOnLocation; 
            }

            if (currentLockOnTarget != null) // If currentLockOnTarget is not null
            {
                // Get player position in local space relative to enemy 
                Vector3 relativePlayerPosition = player.InverseTransformPoint(availableTargets[k].transform.position);

                var distanceFromLeftTarget = 1000f; // var to store distance from left target
                var distanceFromRightTarget = 1000f; // var to store distance from right target

                if (relativePlayerPosition.x < 0.00) // If enemy is left relative to the player then
                {
                    distanceFromLeftTarget = Vector3.Distance(currentLockOnTarget.position, availableTargets[k].transform.position); // distanceFromLeft is currentLockOnTarget - enemy position
                }
                else if (relativePlayerPosition.x > 0.00) // Else if enemy is right relative to the player then
                {
                    distanceFromRightTarget = Vector3.Distance(currentLockOnTarget.position, availableTargets[k].transform.position); // distanceFromRight is currentLockOnTarget - enemy position
                }

                if (relativePlayerPosition.x < 0.00 && distanceFromLeftTarget < shortestDistanceLeft) // Players relative position to enemy is LESS than 0 AND distanceFromLeftTarget is LESS than shortestDistanceLeft
                {
                    shortestDistanceLeft = distanceFromLeftTarget; // shortestDistanceLeft is equal to distanceFromLeftTarget
                    leftLockTarget = availableTargets[k].lockOnTransform; // leftLockTarget is equal to closest left target
                }

                if (relativePlayerPosition.x > 0.00 && distanceFromRightTarget < shortestDistanceRight)
                {
                    shortestDistanceRight = distanceFromRightTarget; // shortestDistanceRight is equal to distanceFromRightTarget
                    rightLockTarget = availableTargets[k].lockOnTransform; // rightLockTarget is equal to right left target
                }
            }
        }
    }

    /// <summary>
    /// Resets all target transforms and clears availableTargets list
    /// </summary>
    public void RemoveLockOn()
    {
        cinemachinePOV.m_HorizontalRecentering.m_enabled = false; // Stop recentering camera 
        cameraInput.enabled = true; // Allow camera input movements
        availableTargets.Clear(); // Clear list of availableTargets
        // Reset all lockOn transforms to null
        nearestLockOnTarget = null; 
        currentLockOnTarget = null;
        leftLockTarget = null;
        rightLockTarget = null;
    }
   
    public bool IsLockedOn()
    {
        if (currentLockOnTarget)
            return true;
        else
            return false;
    }
    #region DirFromAngle
    /// <summary>
    /// Used for FOVEditorDisplay 
    /// </summary>
    /// <param name="angleInDegrees"></param>
    /// <param name="angleIsGlobal"></param>
    /// <returns> Vector3 for editor cone </returns>
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    #endregion

    /// <summary>
    /// Shows area for lockOn
    /// </summary>
    #if UNITY_EDITOR == true
    protected void OnDrawGizmos()
    {
        Vector3 viewAngleA = DirFromAngle(-lockOnAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(lockOnAngle / 2, false);

        //Handles.color = Color.red;
        //Handles.DrawWireArc(player.transform.position, Vector3.up, player.transform.forward * -1, 180 - (viewAngle / 2), lockOnRadius);
        //Handles.DrawWireArc(player.transform.position, Vector3.up, player.transform.forward * -1, -180 + (viewAngle / 2), lockOnRadius);

        Handles.color = Color.black;
        Handles.DrawLine(player.transform.position, player.transform.position + viewAngleA * lockOnRadius);
        Handles.DrawLine(player.transform.position, player.transform.position + viewAngleB * lockOnRadius);

        Handles.color = Color.red;
        Handles.DrawWireArc(player.transform.position, Vector3.up, player.transform.forward, (lockOnAngle / 2), lockOnRadius);
        Handles.DrawWireArc(player.transform.position, Vector3.up, player.transform.forward, (-lockOnAngle / 2), lockOnRadius);
    }
    #endif
}