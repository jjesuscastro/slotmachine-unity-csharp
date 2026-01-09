using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayoutLines : MonoBehaviour
{
    [SerializeField]
    int payoutLinesCount = 20;
    [SerializeField]
    int[][] payoutLines = new int[20][];

    private void Awake()
    {
        //http://www.onlineslots4u.com/paylines/20/
        payoutLines[0] = new int[] { 1, 1, 1, 1, 1 };
        payoutLines[1] = new int[] { 0, 0, 0, 0, 0 };
        payoutLines[2] = new int[] { 2, 2, 2, 2, 2 };
        payoutLines[3] = new int[] { 0, 1, 2, 1, 0 };
        payoutLines[4] = new int[] { 2, 1, 0, 1, 2 };
        payoutLines[5] = new int[] { 1, 0, 0, 0, 1 };
        payoutLines[6] = new int[] { 1, 2, 2, 2, 1 };
        payoutLines[7] = new int[] { 0, 0, 1, 2, 2 };
        payoutLines[8] = new int[] { 2, 2, 1, 0, 0 };
        payoutLines[9] = new int[] { 1, 2, 1, 0, 1 };
        payoutLines[10] = new int[] { 1, 0, 1, 2, 1 };
        payoutLines[11] = new int[] { 0, 1, 1, 1, 0 };
        payoutLines[12] = new int[] { 2, 1, 1, 1, 2 };
        payoutLines[13] = new int[] { 0, 1, 0, 1, 0 };
        payoutLines[14] = new int[] { 2, 1, 2, 1, 2 };
        payoutLines[15] = new int[] { 1, 1, 0, 1, 1 };
        payoutLines[16] = new int[] { 1, 1, 2, 1, 1 };
        payoutLines[17] = new int[] { 0, 0, 2, 0, 0 };
        payoutLines[18] = new int[] { 2, 2, 0, 2, 2 };
        payoutLines[19] = new int[] { 0, 2, 2, 2, 0 };
    }

    public int[][] GetPayoutLines()
    {
        return payoutLines;
    }

    public int GetPayoutLinesCount()
    {
        return payoutLinesCount;
    }
}
