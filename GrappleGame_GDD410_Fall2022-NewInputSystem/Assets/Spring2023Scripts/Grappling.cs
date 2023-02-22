using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grappling : MonoBehaviour
{
   
    [Header("References")]
    public bool freeze = false;
    public Transform cam;

    public LayerMask whatIsPullable;
    public LineRenderer lrs;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;
    private Vector3 grapplePoints;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    Rigidbody rb;



    private bool grapplings;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime;
    }

    public void StartGrapple(InputAction.CallbackContext context)
    {
        if (grapplingCdTimer > 0) return;

        grapplings = true;

        freeze = true;

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsPullable))
        {
            grapplePoints = hit.point;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoints = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lrs.enabled = true;
        lrs.SetPosition(1, grapplePoints);
    }

    private void ExecuteGrapple()
    {
        freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoints.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        JumpToPosition(grapplePoints, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        freeze = false;

        grapplings = false;

        grapplingCdTimer = grapplingCd;

        lrs.enabled = false;
    }

    public bool IsGrapplings()
    {
        return grapplings;
    }

    public Vector3 GetGrapplePoints()
    {
        return grapplePoints;
    }
    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {


        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);


    }
    private Vector3 velocityToSet;
    private void SetVelocity()
    {

        rb.velocity = velocityToSet;

        // cam.DoFov(grappleFov);
    }
    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }
}    
