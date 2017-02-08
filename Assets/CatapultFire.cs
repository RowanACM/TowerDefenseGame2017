using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultFire : MonoBehaviour {

	public GameObject boulder;
	// Use this for initialization
	void Start () {
		StartCoroutine (FireLoop ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator FireLoop(){
		while (true) {
			Animator animation = GetComponent<Animator> ();

			animation.SetTrigger ("Fire");
			yield return new WaitForSeconds (0.5f / animation.GetFloat ("FireSpeed"));
			boulder.SetActive (false);
			yield return new WaitForSeconds (0.5f / animation.GetFloat ("FireSpeed"));
			yield return new WaitForSeconds (1.0f / animation.GetFloat("ReloadSpeed"));
			boulder.SetActive (true);
		}
	}
}
