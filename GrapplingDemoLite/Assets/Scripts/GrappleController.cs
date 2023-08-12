using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleController : MonoBehaviour
{
    public UIElementManager uiManager;

    public Transform grappleCastPoint;
    private GameObject grappleTargetPoint;

    public float grappleDelayTime;

    public LineRenderer lr;

    private bool grappling = false;

    private CharacterMovementController movement;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<CharacterMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            if (!grappling)
            {
                StartGrapple();
            }
        }
    }

    private void LateUpdate()
    {
        if (grappling)
        {
            lr.SetPosition(0, grappleCastPoint.transform.position);
        }
    }

    private void StartGrapple()
    {
        grappleTargetPoint = uiManager.nearestGrapplePointUI;

        if (grappleTargetPoint != null && grappleCastPoint != null)
        {
            grappling = true;
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);

            lr.enabled = true;
            lr.SetPosition(1, grappleTargetPoint.transform.position);
        }
    }

    private void ExecuteGrapple()
    {
        movement.freeze = true;

        Vector3 dir = grappleTargetPoint.transform.position - grappleCastPoint.position;

        StopGrapple();
    }

    public void StopGrapple()
    {
        grappling = false;
        movement.freeze = false;
        lr.enabled = false;
        grappleTargetPoint = null;
    }
}
