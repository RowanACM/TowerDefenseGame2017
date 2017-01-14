using UnityEngine;
using System.Collections;

public class SpawnCell : MonoBehaviour {

	public GameObject item;
	public bool closed;

    public void Start()
    {
        GetComponent<Renderer>().enabled = false;
    }

	public bool IsOpen(){
		return item == null;
	}

	public void SetItem(GameObject item)
	{
        this.item = item;
	}
}
