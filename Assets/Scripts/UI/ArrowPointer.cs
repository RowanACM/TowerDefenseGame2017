using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPointer : MonoBehaviour {

    public GameObject pointerBase;
    public GameObject pointerArrow;

    public Transform target;

    public float minShowDistance;

	// Use this for initialization
	void Start () {
		
	}

    public void setColor(Color newColor)
    {
        Material baseMat = pointerBase.GetComponent<MeshRenderer>().material;
        Material arrowMat = pointerArrow.GetComponent<MeshRenderer>().material;
        baseMat.EnableKeyword("_EmissionColor");
        baseMat.color = newColor;
        baseMat.SetColor("_EmissionColor", newColor);
        arrowMat.EnableKeyword("_EmissionColor");
        arrowMat.color = newColor;
        arrowMat.SetColor("_EmissionColor", newColor);
    }
	
	// Update is called once per frame
	void Update () {
        if (target)
        {
            this.transform.LookAt(target);
            float dist = this.transform.InverseTransformDirection(target.position - this.transform.position).magnitude;
            if (dist < minShowDistance)
            {
                pointerBase.GetComponent<Renderer>().enabled = false;
                pointerArrow.GetComponent<Renderer>().enabled = false;
            }
            else
            {
                pointerBase.GetComponent<Renderer>().enabled = true;
                pointerArrow.GetComponent<Renderer>().enabled = true;
                float scale = this.transform.localScale.y;
                pointerBase.transform.localScale = new Vector3(pointerBase.transform.localScale.x, ((dist - 2.0f) / (2)), pointerBase.transform.localScale.z);
                pointerArrow.transform.localPosition = new Vector3(0.0f, 0.0f, (dist - 2.0f));
            }
        }
	}
}
