using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Targeter : MonoBehaviour {

    public float awarenessRadius;

	public Transform[] GetNearbyTransforms()
    {
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, awarenessRadius);
        Transform[] options = new Transform[hitObjects.Length];
        for (int i = 0; i < hitObjects.Length; i++)
        {
            options[i] = hitObjects[i].transform;
        }
        return options;
    }

    public abstract Transform GetTargetTransform();

    public abstract void FindTarget();
}
