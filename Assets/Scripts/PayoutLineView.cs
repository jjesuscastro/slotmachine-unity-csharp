using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PayoutLineView : MonoBehaviour
{
    [SerializeField]
    LineRenderer lineRenderer;

    int startX = -4;
    int startY = 2;

    public int[] pLine;

    public void SetPayoutLine(int[] payoutLine)
    {
        pLine = payoutLine;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(startX + i * 2, startY + payoutLine[i] * -2, 0));

            float r = Random.Range(0f, 1f);
            float g = Random.Range(0f, 1f);
            float b = Random.Range(0f, 1f);
            lineRenderer.startColor = new Color(r, g, b, .75f);
            lineRenderer.endColor = new Color(r, g, b, .75f);
        }
        gameObject.SetActive(false);
    }
}
