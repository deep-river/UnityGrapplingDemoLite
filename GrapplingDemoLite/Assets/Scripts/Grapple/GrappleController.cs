using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleController : MonoBehaviour
{
    public UIElementManager uiManager;
    public LineRenderer lineRenderer;

    private CharacterMovementController movement;

    public Transform grappleCastPoint;
    private GameObject grappleTargetPoint;
    private float grappleTravelDistance;

    // public float hookLaunchSpeed = 15f;
    public float fixedHookLaunchDelay = 0.5f;
    public float grappleTravelSpeed = 15f;
    public Vector3 grappleTargetOffset = Vector3.zero;

    private bool grappling = false;

    void Start()
    {
        movement = GetComponent<CharacterMovementController>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            if (!grappling) StartGrapple();
        }
    }

    private void LateUpdate()
    {
        if (grappling) lineRenderer.SetPosition(0, grappleCastPoint.transform.position);
    }

    private void StartGrapple()
    {
        grappleTargetPoint = uiManager.nearestGrapplePointUI;

        if (grappleTargetPoint != null && grappleCastPoint != null)
        {
            grappling = true;
            grappleTravelDistance = Vector3.Distance(grappleCastPoint.position, grappleTargetPoint.transform.position);
            // Invoke(nameof(ExecuteGrapple), grappleTravelDistance / hookLaunchSpeed);
            Invoke(nameof(ExecuteGrapple), fixedHookLaunchDelay);

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(1, grappleTargetPoint.transform.position);
        }
    }

    private void ExecuteGrapple()
    {
        movement.freeze = true;

        float travelDuration = grappleTravelDistance / grappleTravelSpeed;

        StartCoroutine(movement.LaunchToPosition(grappleTargetPoint.transform.position, grappleTargetOffset, travelDuration));

        Invoke(nameof(StopGrapple), travelDuration);
    }

    public void StopGrapple()
    {
        grappling = false;
        movement.freeze = false;
        lineRenderer.enabled = false;
        grappleTargetPoint = null;
    }

    public bool IsGrappling()
    {
        return grappling;
    }

    public Vector3 GetGrapplePoint()
    {
        return grappleTargetPoint.transform.position;
    }
}
