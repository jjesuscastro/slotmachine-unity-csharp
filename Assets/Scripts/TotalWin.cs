using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TotalWin : MonoBehaviour
{
    [SerializeField]
    TMP_Text totalWin;

    public void UpdateTotalWin(int value)
    {
        totalWin.text = value.ToString();
    }
}
