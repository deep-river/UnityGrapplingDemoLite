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

    public float grapplePointShowMinDistance = 3;
    public float grapplePointShowMaxDistance = 15;
    public float grapplePointActiveDistance = 12;
    private float gpShowMinDistanceSqr;
    private float gpShowMaxDistanceSqr;
    private float gpActiveDistanceSqr;

    private List<GameObject> grapplePointUIList = new List<GameObject>();

    private Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

    [HideInInspector] public GameObject nearestGrapplePointUI = null;
    [HideInInspector] public GameObject prevNearestGrapplePointUI = null;

    public IndicatorUIManager indicatorManager;


    void Start()
    {
        InitDistanceVals();

        player = GameObject.FindWithTag("Player");

        GrapplePoint[] grapplePointsList = grapplePoints.GetComponentsInChildren<GrapplePoint>();

        InitGrapplePointsUI(grapplePointsList);

        StartCoroutine(CheckDistanceAndShowGrapplePoints());
    }

    private void Update()
    {
        UpdateOutOfViewIndicator();
    }

    private void InitDistanceVals()
    {
        gpShowMinDistanceSqr = grapplePointShowMinDistance * grapplePointShowMinDistance;
        gpShowMaxDistanceSqr = grapplePointShowMaxDistance * grapplePointShowMaxDistance;
        gpActiveDistanceSqr = grapplePointActiveDistance * grapplePointActiveDistance;
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

            foreach (GameObject gPointUI in grapplePointUIList)
            {
                // ���ץ�����Ƿ�λ����ʾ������
                float worldDistanceSqr = (player.transform.position - gPointUI.transform.position).sqrMagnitude;
                if (worldDistanceSqr <= gpShowMaxDistanceSqr)
                {
                    gPointUI.SetActive(true);
                    gPointUI.transform.forward = Camera.main.transform.forward;

                    // ���ץ������������Ƿ�����ڵ�
                    // ע����ʾ�����ڱ��ڵ���ץ�����Ϊactive�������������λ�ļ���
                    if (worldDistanceSqr <= gpActiveDistanceSqr && worldDistanceSqr > gpShowMinDistanceSqr &&
                        !Physics.Linecast(Camera.main.transform.position, gPointUI.transform.position))
                    {
                        // ���������Ļ����λ�������ץ����
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

            UpdateNearestGrapplePointUI();

            yield return new WaitForSeconds(0.2f);
        }
    }

    // ��������Ļ���ĵ������ץ������ʾΪ��ɫ
    private void UpdateNearestGrapplePointUI()
    {
        if (prevNearestGrapplePointUI != null)
        {
            float prevNearestGPDistanceSqr = (player.transform.position - prevNearestGrapplePointUI.transform.position).sqrMagnitude;
            if (prevNearestGrapplePointUI != nearestGrapplePointUI || prevNearestGPDistanceSqr > gpActiveDistanceSqr)
            {
                prevNearestGrapplePointUI.GetComponent<Image>().color = Color.white;
            }
        }

        if (nearestGrapplePointUI != null)
        {
            float neareastGPDistanceSqr = (player.transform.position - nearestGrapplePointUI.transform.position).sqrMagnitude;
            if (neareastGPDistanceSqr > gpShowMinDistanceSqr && neareastGPDistanceSqr <= gpActiveDistanceSqr)
            {
                nearestGrapplePointUI.GetComponent<Image>().color = Color.green;

                prevNearestGrapplePointUI = nearestGrapplePointUI;
            }
            else
            {
                nearestGrapplePointUI = null;
            }
        }
    }

    // ��������Ƿ�λ�������Ұ��
    private bool IsInsideView(GameObject checkObj)
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(checkObj.transform.position);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
            return true;
        return false;
    }

    private void UpdateOutOfViewIndicator()
    {
        if (nearestGrapplePointUI != null)
        {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(nearestGrapplePointUI.transform.position);
            if (!(viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0))
            {
                indicatorManager.targetPos = viewPos;
                indicatorManager.ShowIndicator();
            }
            else
            {
                indicatorManager.HideIndicator();
            }
        }
        else
        {
            indicatorManager.HideIndicator();
        }
    }
}
