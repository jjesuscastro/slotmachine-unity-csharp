using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int coins = 10000000;
    public bool SubtractCoins(int value)
    {
        if (value > coins)
            return false;
        else
            coins -= value;

        return true;

    }

    public void AddCoins(int value)
    {
        coins += value;
    }

    public int GetCoins()
    {
        return coins;
    }
}
