using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ShowArmorResistance : MonoBehaviour {

    public Color armoredColor;
    private int startArmor;
    private Color startColor;
    public Armor target;
    private Material material;
    private Renderer renderer;


	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
        material = renderer.material;
        material.EnableKeyword("_EMISSION");
        startColor = material.GetColor("_EmissionColor");
        startArmor = target.armor;
	}
	
	// Update is called once per frame
	void Update () {
		if(target.armor < startArmor)
        {
            float armorRatio = (float) target.armor / (float) startArmor;
            renderer.material.SetColor("_EmissionColor", Color.Lerp(startColor, armoredColor, armorRatio));
        } else
        {
            renderer.material.SetColor("_EmissionColor", armoredColor);
        }
	}
}
