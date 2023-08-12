using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorUIManager : MonoBehaviour
{
    public GameObject indicator;

    public float upperPosRatio = 0.9f;
    public float lowerPosRatio = 0.1f;
    public float leftPosRatio = 0.1f;
    public float rightPosRatio = 0.1f;
    private float upperYPos, lowerYPos;
    private float leftXPos, rightXPos;

    [HideInInspector] public Vector3 targetObjPos;

    private bool isShowing = false;

    void Start()
    {
        upperYPos = upperPosRatio * Screen.height;
        lowerYPos = lowerPosRatio * Screen.height;
        leftXPos = leftPosRatio * Screen.width;
        rightXPos = rightPosRatio * Screen.width;
    }

    void Update()
    {
        if (isShowing)
        {
            // 位于画面上方或下方
            if (0 < targetObjPos.x && targetObjPos.x < 1)
            {
                float posX = Screen.width * targetObjPos.x;
                // y > 1: 在上方
                // y < 0: 在下方
                if (targetObjPos.y > 1)
                {
                    indicator.transform.eulerAngles = new Vector3(0, 0, 180);
                    indicator.transform.position = new Vector3(posX, upperYPos, 0);
                }
                else if (targetObjPos.y < 0)
                {
                    indicator.transform.eulerAngles = new Vector3(0, 0, 0);
                    indicator.transform.position = new Vector3(posX, lowerYPos, 0);
                }

            }
            // 位于画面左边或右边
            else if (0 < targetObjPos.y && targetObjPos.y < 1)
            {
                // x < 0: 在左边
                // x > 1: 在右边
            }
            indicator.SetActive(true);
        }
    }

    public void UpdateIndicatorUI(Vector3 objPos)
    {
        // 位于画面上方或下方
        if (0 < objPos.x && objPos.x < 1)
        {
            float posX = Screen.width * objPos.x;
            // y > 1: 在上方
            // y < 0: 在下方
            if (objPos.y > 1)
            {
                indicator.transform.eulerAngles = new Vector3(0, 0, 180);
                indicator.transform.position = new Vector3(posX, upperYPos, 0);
            }
            else if (objPos.y < 0)
            {
                indicator.transform.eulerAngles = new Vector3(0, 0, 0);
                indicator.transform.position = new Vector3(posX, lowerYPos, 0);
            }

        }
        // 位于画面左边或右边
        else if (0 < objPos.y && objPos.y < 1)
        {
            // x < 0: 在左边
            // x > 1: 在右边
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
