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
    public GameObject grapplePoints;
    public GameObject grapplePointPrefab;

    public float grapplePointShowDistance = 15;
    public float grapplePointActiveDistance = 12;
    private float gpShowDistanceSqr;
    private float gpActiveDistanceSqr;

    private List<GameObject> grapplePointUIList = new List<GameObject>();

    private Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

    [HideInInspector] public GameObject nearestGrapplePointUI = null;
    [HideInInspector] public GameObject prevNearestGrapplePointUI = null;


    void Start()
    {
        gpShowDistanceSqr = grapplePointShowDistance * grapplePointShowDistance;
        gpActiveDistanceSqr = grapplePointActiveDistance * grapplePointActiveDistance;

        player = GameObject.FindWithTag("Player");

        GrapplePoint[] grapplePointsList = grapplePoints.GetComponentsInChildren<GrapplePoint>();

        InitGrapplePointsUI(grapplePointsList);

        StartCoroutine(CheckDistanceAndShowGrapplePoints());
    }

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
            nearestGrapplePointUI = null;

            foreach (GameObject gPointUI in grapplePointUIList)
            {
                // 检测抓钩点是否位于显示距离内
                if ((player.transform.position - gPointUI.transform.position).sqrMagnitude < gpShowDistanceSqr)
                {
                    // 检测抓钩点与相机间是否存在遮挡
                    // 注：显示距离内被遮挡的抓钩点可为active但不参与最近点位的计算
                    if (!Physics.Linecast(Camera.main.transform.position, gPointUI.transform.position))
                    {
                        gPointUI.SetActive(true);
                        gPointUI.transform.forward = Camera.main.transform.forward;

                        // 计算距离屏幕中心位置最近的抓钩点
                        Vector2 uiCenter = Camera.main.WorldToScreenPoint(gPointUI.transform.position);
                        float dist = Vector2.Distance(uiCenter, screenCenter);

                        if (dist < minDist)
                        {
                            minDist = dist;
                            nearestGrapplePointUI = gPointUI;
                        }
                    }
                }
                else
                {
                    gPointUI.SetActive(false);
                }
            }

            if (prevNearestGrapplePointUI != null && prevNearestGrapplePointUI != nearestGrapplePointUI)
            {
                prevNearestGrapplePointUI.GetComponent<Image>().color = Color.white;
            }

            if (nearestGrapplePointUI != null && (player.transform.position - nearestGrapplePointUI.transform.position).sqrMagnitude < gpActiveDistanceSqr)
            {
                nearestGrapplePointUI.GetComponent<Image>().color = Color.green;

                prevNearestGrapplePointUI = nearestGrapplePointUI;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }
}
