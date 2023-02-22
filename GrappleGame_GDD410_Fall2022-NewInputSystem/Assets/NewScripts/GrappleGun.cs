using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    
    public Transform gunTip;
    public Transform cameraSpot;
    public Transform player;
    public bool grappling = false;

    [SerializeField] private float maxDistance;
    private SpringJoint joint;

    [HideInInspector]
    public AnimatorHandler animatorHandler;
    public NewMovement newMovement;


    private void Start()
    {
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        animatorHandler.Initialize();
    }
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        newMovement = GetComponent<NewMovement>();
       
    }
    void LateUpdate()
    {
        DrawRope();
    }


    public void OnSwing(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            RaycastHit hit;
            if (Physics.Raycast(cameraSpot.position, cameraSpot.forward, out hit, maxDistance, whatIsGrappleable))
            {
                
                grapplePoint = hit.point;
                grappling = true;
                joint = player.gameObject.AddComponent<SpringJoint>();
                
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = grapplePoint;

                float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);


                joint.maxDistance = distanceFromPoint * 0.8f;
                joint.minDistance = distanceFromPoint * 0.25f;


                joint.spring = 4.5f;
                joint.damper = 7f;
                joint.massScale = 4.5f;

                lr.positionCount = 2;
                currentGrapplePosition = gunTip.position;
            }
        }

           
      
        if (context.canceled)
        {
            lr.positionCount = 0;
            Destroy(joint);
            grappling = false;
        }
       
    }


    private Vector3 currentGrapplePosition;

    void DrawRope()
    {

        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "ToHigh")
        {
            lr.positionCount = 0;
            Destroy(joint);

        }
    }
    #region Pull


    #endregion
}



