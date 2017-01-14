using UnityEngine;
using System.Collections;

public class Armor : MonoBehaviour {

	public int armor;

    public int GetDamageResistance(GameObject source, int damage)
    {
        return armor;
    }
}
