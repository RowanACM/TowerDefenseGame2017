using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyOnDeath : MonoBehaviour {

    public int prizeMoney;

    public void OnDeath()
    {
        Store store = FindObjectOfType<Store>();
        store.ReceiveMoney(prizeMoney);
    }
}
