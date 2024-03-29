using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    public static PlayerGold Instance;

    public float Gold { get; private set; } 

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public void AddGold(float gold)
    {
        this.Gold += gold;
    }

    public void RemoveGold(float gold)
    {
        this.Gold -= gold;
    }
}
