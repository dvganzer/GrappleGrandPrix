using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplePull : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform _grapplingHookEndPoint;

    [SerializeField] private Transform _grapplingHook;
    [SerializeField] private Transform _handPos;
    [SerializeField] private Transform _playerBody;

    [SerializeField] private LayerMask _grappleLayer;

    [SerializeField] private float _maxDistance;
    [SerializeField] private float _hookSpeed;

    [SerializeField] private Vector3 _offset;

    private bool isShooting;
    private bool isGrappling;

    public Transform camera;

    private Vector3 hookPoint;

    void Start()
    {
        isShooting = false;
        isGrappling= false;
        lineRenderer.enabled = false;
    }



    public void OnPull(InputAction.CallbackContext context)
    {
        if (_grapplingHook.parent == _handPos)
        {
            _grapplingHook.localPosition = new Vector3(1, 0, 0);
            _grapplingHook.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
        }

        if (context.performed)
        {
            ShootHook();
        }



        if (isGrappling)
        {

            _grapplingHook.position = Vector3.Lerp(_grapplingHook.position, hookPoint, _hookSpeed * Time.deltaTime);
            if (Vector3.Distance(_grapplingHook.position, hookPoint) < 0.5f)
            {
                _playerBody.position = Vector3.Lerp(_playerBody.position, hookPoint - _offset, _hookSpeed * Time.deltaTime);
            }


        }
        if (Vector3.Distance(_playerBody.position, hookPoint) <= 7f)
        {

            isGrappling = false;
            Debug.Log(isGrappling);
            _grapplingHook.SetParent(_handPos);
            _grapplingHook.position = _handPos.position;
            lineRenderer.enabled = false;
        }

    }
    private void LateUpdate()
    {
        if (lineRenderer.enabled)
        {
            lineRenderer.SetPosition(0, _grapplingHookEndPoint.position);
            lineRenderer.SetPosition(1, _handPos.position);
        }
    }
    private void ShootHook()
    {
        if (isShooting || isGrappling) return;
        isShooting = true;
        RaycastHit hit;
        
        if(Physics.Raycast(camera.position,camera.forward,out hit, _maxDistance, _grappleLayer))
        {
            hookPoint = hit.point;
            isGrappling = true;
            _grapplingHook.parent = null;
            _grapplingHook.LookAt(hookPoint);
            print("Hit!)");
            lineRenderer.enabled = true;
            
        }
        isShooting = false;

    }
}
