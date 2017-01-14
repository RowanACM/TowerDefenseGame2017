using UnityEngine;
using System.Collections;

public class DestroyVolume : MonoBehaviour {

    private Store store;

	// Use this for initialization
	void Start () {
        store = FindObjectOfType<Store>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider col) {
		print ("triggerred!!!1");
		if(col.gameObject.GetComponent<StoreItemUI>()){
			print ("sell it");
            store.sell(col.gameObject);
		}
	}
}
