using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(MeshContainer))]
public class OutlineComponent : MonoBehaviour {

	public float outlineWidth;
	private bool outlineEnabled;
	public bool startEnabled;
	private bool particlesOn;
	public Color color;

	// Use this for initialization
	void Start () {
        setParticles(true);
        if (startEnabled) {
			Enable ();

		} else {
			Disable ();
		}
        

    }
	
	public void setParticles(bool on)
	{
		if(on) {
			if(isOutlineEnabled())
				GetComponent<ParticleSystem> ().Play ();
			particlesOn = true;
			
		} else {
			if(isOutlineEnabled()) {
				GetComponent<ParticleSystem> ().Pause ();
				GetComponent<ParticleSystem> ().Clear ();
			}
			particlesOn = false;
				
		}
			
	}
	
	public void setColor(Color newColor)
	{
		GetComponent<ParticleSystem>().startColor = newColor;
        foreach (MeshRenderer mesh in GetComponent<MeshContainer>().allMeshes)
        {
            Material currMat = mesh.material;
            newColor.a = currMat.GetColor("_OutlineColor").a;
            currMat.SetColor("_OutlineColor", newColor);
        }

        Transform arrow = transform.Find("ArrowPointer");
        if (arrow)
        {
            arrow.gameObject.GetComponent<ArrowPointer>().setColor(newColor);
        }
	}

	public bool isOutlineEnabled()
	{
		return outlineEnabled;
	}
	
	public void makeOpaque()
	{
        foreach (MeshRenderer mesh in GetComponent<MeshContainer>().allMeshes)
        {
            Material currMat = mesh.material;
            Color currColor = currMat.GetColor("_Color");
            currColor.a = 1.0f;
            currMat.SetColor("_Color", currColor);

            Color currOutlineColor = currMat.GetColor("_OutlineColor");
            currOutlineColor.a = 0.2f;
            currMat.SetColor("_OutlineColor", currOutlineColor);
        }
	}

	public void Enable()
	{
		if(particlesOn)
			GetComponent<ParticleSystem> ().Play ();

        foreach(MeshRenderer mesh in GetComponent<MeshContainer>().allMeshes) {
            Material currMat = mesh.material;
            Color currColor = currMat.GetColor("_Color");
            currColor.a = 0.3f;
            currMat.SetColor("_Color", currColor);

            Color currOutlineColor = currMat.GetColor("_OutlineColor");
            currOutlineColor.a = 0.5f;
            currMat.SetColor("_OutlineColor", currOutlineColor);
        }
		
		outlineEnabled = true;
	}


	public void Disable()
	{
		if(particlesOn) {
			GetComponent<ParticleSystem> ().Pause ();
			GetComponent<ParticleSystem> ().Clear ();
		}
        foreach (MeshRenderer mesh in GetComponent<MeshContainer>().allMeshes)
        {
            Material currMat = mesh.material;
            Color currColor = currMat.GetColor("_Color");
            currColor.a = 1.0f;
            currMat.SetColor("_Color", currColor);

            Color currOutlineColor = currMat.GetColor("_OutlineColor");
            currOutlineColor.a = 0.0f;
            currMat.SetColor("_OutlineColor", currOutlineColor);
        }
		
		outlineEnabled = false;
	}
}
