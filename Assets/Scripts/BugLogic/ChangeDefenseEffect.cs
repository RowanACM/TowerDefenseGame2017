using UnityEngine;
using System.Collections;

public class ChangeDefenseEffect : Effect {

    protected override void doEffect()
    {
        if (gameObject.GetComponent<Armor>() != null)
        {
            Armor armorComponent = gameObject.GetComponent<Armor>() as Armor;
            armorComponent.armor += magnitude;
        }
    }

	protected override void reverseEffect() {
        if (gameObject.GetComponent<Armor>() != null)
        {
            Armor armorComponent = gameObject.GetComponent<Armor>() as Armor;
            armorComponent.armor -= magnitude;
        }
	}
}
