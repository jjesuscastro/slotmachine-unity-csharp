using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bets : MonoBehaviour
{
    [SerializeField]
    TMP_Text betTotalAmount;

    public void UpdateTotalBetAmount(int totalBet)
    {
        betTotalAmount.text = totalBet.ToString();
    }
}
