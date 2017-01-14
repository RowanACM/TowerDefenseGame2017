using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purchaseable : MonoBehaviour {

    private Store store;
    public bool purchased;
    public int price;
	// Use this for initialization
	void Start () {
        store = FindObjectOfType<Store>();
	}

    public int GetPrice()
    {
        return price;
    }

    public bool CanBePurchased()
    {
        return store.CanPurchase(this);
    }

    public void OnPlace()
    {
        if (!purchased)
        {
            purchased = store.Purchase(this);
            SendMessage("OnPurchase");
        }
    }

    public bool WasPurchased()
    {
        return purchased;
    }
}
