using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawBeam : MonoBehaviour
{
    [Header("Defaults")]
    [Tooltip("The object the beam will start from"), SerializeField] private Transform beamOrigin;
    [Tooltip("Whether this beam should be on by default")] public bool onByDefault;
    [Tooltip("The tag used for connecting beams"), SerializeField] private string lightRotatorString;
    [Tooltip("The tag used for light locks"), SerializeField] private string lightLockString;

    [Header("Beam Modifiers")]
    [Tooltip("The max distance for the light to travel"), SerializeField] private float lineLength;
    [Tooltip("Moved the light direction up and down"), SerializeField] private float yVarience;
    [Tooltip("Which layers this should be able to interact with"), SerializeField] private LayerMask mask;

    [HideInInspector] public bool isConnected = false;
    [HideInInspector] public DrawBeam connectedBeam;
    [HideInInspector] public DrawBeam mySource;
    [HideInInspector] public LightLock myLightLock;

    private LineRenderer lineRenderer;
    private bool isEnabled = false;

    private void OnValidate()
    {
        Initialize(); 
        CreateBeam();
    }

    // Start is called before the first frame update
    void Start()
    {
        isConnected = false;
        Initialize();   
    }

    private void Initialize()
    {
        // get the line renderer and set it's origin position
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, beamOrigin.position);

        // ensure the beam is set up depending if it's on by default
        if (onByDefault)
        {
            isConnected = true;
            if (lineRenderer.enabled == false)
            {
                lineRenderer.enabled = true;
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isConnected && lineRenderer.enabled == false)
        {
            lineRenderer.enabled = true;
        }

        // enable the beam if connected or on by default
        if (isConnected || onByDefault)
        {
            CreateBeam();
        }
    }

    /// <summary>
    /// Changes whether the beam is enabled or disabled
    /// </summary>
    /// <param name="enabled">whether the beam should be enabled</param>
    /// <param name="newSource">The new source for this beam</param>
    private void IsEnabled(bool enabled, DrawBeam newSource)
    {
        if(enabled == isEnabled)
        {
            return;
        }

        isEnabled = enabled;

        lineRenderer.enabled = enabled;
        isConnected = enabled;
        mySource = newSource;

        // if has a lightlock and not enabled, lock the lightlock
        if(myLightLock != null && !enabled)
        {
            myLightLock.Unlock(false);
            myLightLock = null;
        }

        // if has connectedbeam and not enabled, turn off the connected beam
        if (connectedBeam != null && !enabled)
        {
            connectedBeam.IsEnabled(false, null);
        }
    }

    /// <summary>
    /// Creates and sets the direction of the beam
    /// </summary>
    private void CreateBeam()
    {
        RaycastHit hit;
        // Checks if the ray intersects any objects excluding the player layer
        if (Physics.Raycast(beamOrigin.position, GetBeamDirection(transform.forward, yVarience), out hit, lineLength, mask))
        {
            // if hit, set line end point to hit point
            lineRenderer.SetPosition(1, hit.point);

            // if lightOrigin is hit, enable it
            if (hit.transform.tag == lightRotatorString)
            {
                DrawBeam hitBeam = hit.transform.GetComponentInParent<DrawBeam>();

                // remove all connections and ensure they know they're disconnected
                if (connectedBeam != null && hitBeam != connectedBeam && connectedBeam.mySource == this)
                {
                    connectedBeam.IsEnabled(false, null);
                    connectedBeam = null;
                }

                // ensure to only try and turn on a beam if it isn't already connected to a circuit
                if (!hitBeam.isConnected || !hitBeam.onByDefault && hitBeam.mySource == null)
                {
                    connectedBeam = hitBeam;
                    connectedBeam.IsEnabled(true, this);
                }                
            }
            
            // if lock is hit, unlock it
            if(hit.transform.tag == "LightLock")
            {
                LightLock hitLock;
                hitLock = hit.transform.GetComponent<LightLock>();

                myLightLock = hitLock;
                hitLock.Unlock(true);
            }
            
        }
        else
        {
            // if nothing is hit, set position at max distance
            lineRenderer.SetPosition(1, beamOrigin.position + GetBeamDirection(transform.forward, yVarience) * lineLength);

            // remove all connections and ensure they know they're disconnected
            if(connectedBeam != null && connectedBeam.mySource == this)
            {
                connectedBeam.IsEnabled(false, null);
                connectedBeam = null;
            }
            if (myLightLock != null)
            {
                myLightLock.Unlock(false);
                myLightLock = null;
            }
        }
    }

    /// <summary>
    /// Takes a vector and rotates it along the y axis by a given angle
    /// </summary>
    /// <param name="start">The vector to rotate</param>
    /// <param name="angle">The amount to rotate along the given angle</param>
    /// <returns>A vector rotated around the given angle</returns>
    private Vector3 GetBeamDirection(Vector3 start, float angle)
    {
        if (yVarience == 0)
        {
            return transform.forward;
        }
        start.Normalize();

        // rotate the vector towards the upwards direction
        Vector3 axis = Vector3.Cross(start, Vector3.up);
        if (axis == Vector3.zero) { axis = Vector3.right; }

        // apply the rotation and return it
        return Quaternion.AngleAxis(angle, axis) * start;
    }
}
