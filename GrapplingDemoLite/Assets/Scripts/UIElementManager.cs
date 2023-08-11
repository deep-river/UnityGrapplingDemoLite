using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class UIElementManager : MonoBehaviour
{
    private GameObject player;
    public GameObject grapplePointPrefab;

    public float grapplePointShowDistance = 15;
    public float grapplePointActiveDistance = 12;

    public GameObject grapplePoints;
    private List<GameObject> grapplePointUIList = new List<GameObject>();

    Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

    [HideInInspector] public GameObject nearestUI = null;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");

        GrapplePoint[] grapplePointsList = grapplePoints.GetComponentsInChildren<GrapplePoint>();

        InitGrapplePointsUI(grapplePointsList);

        StartCoroutine(CheckDistanceAndShowGrapplePoints());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitGrapplePointsUI(GrapplePoint[] grapplePointsList)
    {
        foreach (GrapplePoint point in grapplePointsList)
        {
            GameObject gPointUI = Instantiate(grapplePointPrefab, this.transform);
            gPointUI.transform.position = point.transform.position;
            grapplePointUIList.Add(gPointUI);
        }
    }

    IEnumerator CheckDistanceAndShowGrapplePoints()
    {

        while (true)
        {
            float minDist = float.MaxValue;
            nearestUI = null;

            foreach (GameObject gPointUI in grapplePointUIList)
            {
                if ((player.transform.position - gPointUI.transform.position).sqrMagnitude < grapplePointShowDistance * grapplePointShowDistance)
                {
                    gPointUI.GetComponent<Image>().color = Color.white;
                    gPointUI.SetActive(true);
                    gPointUI.transform.forward = Camera.main.transform.forward;
                }
                else
                {
                    gPointUI.SetActive(false);
                }

                Vector2 uiCenter = Camera.main.WorldToScreenPoint(gPointUI.transform.position);

                float dist = Vector2.Distance(uiCenter, screenCenter);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearestUI = gPointUI;
                }
            }

            if ((player.transform.position - nearestUI.transform.position).sqrMagnitude < grapplePointActiveDistance * grapplePointActiveDistance)
            {
                nearestUI.GetComponent<Image>().color = Color.green;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }
}
