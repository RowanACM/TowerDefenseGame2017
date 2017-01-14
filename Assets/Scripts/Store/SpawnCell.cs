using UnityEngine;
using System.Collections;

public class SpawnCell : MonoBehaviour {

	public GameObject item;
	public bool closed;

	public bool IsOpen(){
		return item == null;
	}

	public void SetItem(GameObject item)
	{
        this.item = item;
	}
}
