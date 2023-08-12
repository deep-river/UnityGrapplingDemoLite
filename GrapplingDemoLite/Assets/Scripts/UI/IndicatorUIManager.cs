using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorUIManager : MonoBehaviour
{
    public GameObject indicator;
    private RectTransform indicatorRect;

    public float verticalPosRatio = 0.85f;
    public float horizontalPosRatio = 0.85f;

    private float verticalPos;
    private float horizontalPos;

    [HideInInspector] public Vector3 targetPos;

    private bool isShowing = false;

    void Start()
    {
        indicatorRect = indicator.GetComponent<RectTransform>();

        verticalPos = verticalPosRatio * Screen.height / 2f;
        horizontalPos = horizontalPosRatio * Screen.width / 2f;
    }

    void Update()
    {
        if (isShowing)
        {
            UpdateIndicatorUI();
        }
    }

    public void UpdateIndicatorUI()
    {
        // Debug.Log(targetPos);
        // λ�ڻ����Ϸ����·�
        if (0 < targetPos.x && targetPos.x < 1)
        {
            float posX = Screen.width * targetPos.x - 0.5f * Screen.width;
            // y > 1: ���Ϸ�
            // y < 0: ���·�
            if (targetPos.y > 1)
            {
                indicator.transform.eulerAngles = new Vector3(0, 0, 180);
                indicatorRect.localPosition = new Vector3(posX, verticalPos, 0);
            }
            else if (targetPos.y < 0)
            {
                indicator.transform.eulerAngles = new Vector3(0, 0, 0);
                indicatorRect.localPosition = new Vector3(posX, -verticalPos, 0);
            }

        }
        // λ�ڻ�����߻��ұ�
        else if (0 < targetPos.y && targetPos.y < 1)
        {
            float posY = Screen.height * targetPos.y - 0.5f * Screen.height;
            // x < 0: �����
            // x > 1: ���ұ�
            if (targetPos.x < 0)
            {
                indicator.transform.eulerAngles = new Vector3(0, 0, -90);
                indicatorRect.localPosition = new Vector3(-horizontalPos, posY, 0);
            }
            else if (targetPos.x > 1)
            {
                indicator.transform.eulerAngles = new Vector3(0, 0, 90);
                indicatorRect.localPosition = new Vector3(horizontalPos, posY, 0);
            }
        }
        indicator.SetActive(true);
    }

    public void ShowIndicator()
    {
        if (!isShowing)
        {
            isShowing = true;
            indicator.SetActive(true);
        }
    }

    public void HideIndicator()
    {
        if (isShowing)
        {
            isShowing = false;
            indicator.SetActive(false);
        }
    }
}
