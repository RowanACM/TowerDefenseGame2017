using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FiringMode : MonoBehaviour {

    public Transform projectileSpawn;

    public abstract void FireWeapon();
}
