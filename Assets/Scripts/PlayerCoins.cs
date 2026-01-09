using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCoins : MonoBehaviour
{
    [SerializeField]
    TMP_Text playerCoins;

    public void UpdatePlayerCoins(int value)
    {
        playerCoins.text = value.ToString();
    }
}
