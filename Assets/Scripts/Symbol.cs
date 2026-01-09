using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Symbol", menuName = "Double Win Vegas/Symbol")]
public class Symbol : ScriptableObject
{
    public int id;
    new public string name;
    public Sprite sprite;
    public int[] payout;
}
