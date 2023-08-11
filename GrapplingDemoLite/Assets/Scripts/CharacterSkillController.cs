using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkillController : MonoBehaviour
{
    public UIElementManager uiManager;

    public Transform grappleGunPoint;

    public float grappleDelayTime;

    public LineRenderer lr;

    private bool grappling = false;

    // Start is called before the first frame update
    void Start()
    {
        
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

    private void StartGrapple()
    {
        GameObject grapplePoint = uiManager.nearestUI;
        if (grapplePoint != null && grappleGunPoint != null)
        {
            grappling = true;
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
    }

    private void ExecuteGrapple()
    {


        grappling = false;
    }
}
